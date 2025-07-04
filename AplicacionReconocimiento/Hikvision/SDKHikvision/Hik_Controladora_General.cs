using DeportNetReconocimiento.Utils;
using Serilog;
using System.Runtime.InteropServices;
using System.Xml;



namespace DeportNetReconocimiento.Hikvision.SDKHikvision
{
    public class Hik_Controladora_General
    {
        //atributos

        //patron singleton, instancia de la propia clase
        private static Hik_Controladora_General? instancia;


        private int idUsuario; // solo puede haber solo un user_ID
        private bool soportaFacial;
        private bool soportaHuella;
        private bool soportaTarjeta;
        private static Hik_Controladora_Facial? hik_Controladora_Facial;
        private static Hik_Controladora_Tarjetas? hik_Controladora_Tarjetas;
        private static Hik_Controladora_Eventos? hik_Controladora_Eventos;
        public Hik_SDK.NET_DVR_TIME m_struTimeCfg;
        public Hik_SDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        private int numeroLector = 1;
        //constructores
        private Hik_Controladora_General()
        {
            idUsuario = -1;

            soportaFacial = false;
            soportaHuella = false;
            soportaTarjeta = false;
        }

        //propiedades (getters y setters)

        public static Hik_Controladora_General Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Hik_Controladora_General();
                }
                return instancia;
            }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public bool SoportaFacial
        {
            get { return soportaFacial; }
            set { soportaFacial = value; }
        }

        public bool SoportaHuella
        {
            get { return soportaHuella; }
            set { soportaHuella = value; }
        }

        public bool SoportaTarjeta
        {
            get { return soportaTarjeta; }
            set { soportaTarjeta = value; }
        }


        //metodos
        public static Hik_Resultado InicializarNet_DVR()
        {
            Hik_Resultado resultado = new Hik_Resultado();

            //implementar try catch y si no se puede inicializar no realizar lo demas.
            try
            {
                Hik_SDK.NET_DVR_Init();
                resultado.ActualizarResultado(true, "NET_DVR_Init éxito", Hik_SDK.NET_DVR_GetLastError().ToString());
            }
            catch
            {
                resultado.ActualizarResultado(false, $"Error al inicializar el dispositivo. NET_DVR_Init error", Hik_SDK.NET_DVR_GetLastError().ToString());

                Log.Error($"Error al inicializar el dispositivo. Exito: {resultado.Exito}, Mensaje: {resultado.Mensaje}, Codigo: {resultado.Codigo}");
            }



            return resultado;
        }

        public Hik_Resultado Login(string user, string password, string port, string ip)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            //cerramos la sesion que estaba iniciada anteriormente
            if (IdUsuario >= 0)
            {
                Hik_SDK.NET_DVR_Logout_V30(idUsuario);
                IdUsuario = -1;
            }

            //creamos y cargamos las estructuras de informacion de login y de informacion del dispositivo

            Hik_SDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new Hik_SDK.NET_DVR_USER_LOGIN_INFO();
            Hik_SDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new Hik_SDK.NET_DVR_DEVICEINFO_V40();
            struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[Hik_SDK.SERIALNO_LEN];

            struLoginInfo.sDeviceAddress = ip;
            struLoginInfo.sUserName = user;
            struLoginInfo.sPassword = password;
            ushort.TryParse(port, out struLoginInfo.wPort);

            //struLoginInfo.bUseAsynLogin = false;



            //utilizamos metodo de iniciar sesion
            int auxUserID = -1;
            auxUserID = Hik_SDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);

            if (auxUserID >= 0)
            {
                //si da mayor a 0 signfica exito
                IdUsuario = auxUserID;
                resultado.ActualizarResultado(true, "Se inicio sesión con exito", Hik_SDK.NET_DVR_GetLastError().ToString());
            }
            else
            {
                resultado = ProcesarErrorDeLogin(struDeviceInfoV40);
            }


            return resultado;
        }

        public Hik_Resultado ProcesarErrorDeLogin(Hik_SDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40)
        {
            Hik_Resultado resultado = new Hik_Resultado();
            //sino debemos verificar el tipo de error
            uint nroError = Hik_SDK.NET_DVR_GetLastError();
            string mensajeDeSdk = "";

            switch (nroError)
            {
                case Hik_SDK.NET_DVR_PASSWORD_ERROR:

                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        mensajeDeSdk = string.Format($"Te quedan {struDeviceInfoV40.byRetryLoginTime} intentos para logearte" /*struDeviceInfoV40.byRetryLoginTime*/);
                    }
                    resultado.ActualizarResultado(false, $"Usuario o contraseña invalidos \n {mensajeDeSdk}", Hik_SDK.NET_DVR_GetLastError().ToString());

                    break;
                case Hik_SDK.NET_DVR_USER_LOCKED:

                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        mensajeDeSdk = string.Format($"Usuario bloqueado, el tiempo restante de bloqueo es de {struDeviceInfoV40.dwSurplusLockTime}" /* struDeviceInfoV40.dwSurplusLockTime*/);
                    }
                    resultado.ActualizarResultado(false, mensajeDeSdk, Hik_SDK.NET_DVR_GetLastError().ToString());

                    break;
                case Hik_SDK.NET_DVR_NETWORK_FAIL_CONNECT:

                    resultado.ActualizarResultado(false, "Error IP Incorrecta o Dispositivo no conectado", Hik_SDK.NET_DVR_GetLastError().ToString());
                    break;
                case Hik_SDK.NET_DVR_DVROPRATEFAILED:
                    resultado.ActualizarResultado(false, "Falló la operación del dispositivo", Hik_SDK.NET_DVR_GetLastError().ToString());
                    break;
                default:
                    resultado.ActualizarResultado(false, "Error de Hikvision", Hik_SDK.NET_DVR_GetLastError().ToString());

                    break;
            }

            return resultado;
        }

        public void CerrarYLimpiar()
        {
            if (IdUsuario >= 0)
            {
                Hik_SDK.NET_DVR_Logout_V30(IdUsuario);
                IdUsuario = -1;
            }
            Hik_SDK.NET_DVR_Cleanup();

        }

        //el dwabilityType es el tipo de capacidad que queremos obtener. En este caso esta fijo en ACS_ABILITY
        //obtenemos un xml con TODAS las capacidades del dispositivo
        private XmlDocument? RetornarXmlConLasCapacidadesDelDispositivo()
        {
            XmlDocument? documentoXml = new XmlDocument();

            //solicitamos habilidades de acceso del dispositvo: huella digital, tarjeta y facial
            //! en caso de que surgan errores a la hora de busqeuda del XML hay que tener en cuenta esta parte.
            string xmlRequest = "<AcsAbility version=\"2.0\"><fingerPrintAbility></fingerPrintAbility><cardAbility></cardAbility><faceAbility></faceAbility></AcsAbility>";
            nint pInBuf;  //Request que ira por referencia a la funcion NET_DVR_GetDeviceAbility
            int nSize = xmlRequest.Length;  //Tamaño del string xmlInput
            pInBuf = Marshal.AllocHGlobal(nSize);
            pInBuf = Marshal.StringToHGlobalAnsi(xmlRequest);  //Documento xml que vamos a retornar

            //xml que nos va a devolver la funcion NET_DVR_GetDeviceAbility
            int XML_ABILITY_OUT_LEN = 3 * 1024 * 1024; //esto seria el tamanio del xml que nos va a devolver la funcion NET_DVR_GetDeviceAbility
            nint pOutBuf = Marshal.AllocHGlobal(XML_ABILITY_OUT_LEN);

            try
            {
                //si nos retorna false, significa que hubo un error
                if (Hik_SDK.NET_DVR_GetDeviceAbility(IdUsuario, Hik_SDK.ACS_ABILITY, pInBuf, (uint)nSize, pOutBuf, (uint)XML_ABILITY_OUT_LEN))
                {
                    //si todo salio bien, se crea el xml con el string que nos devolvio la funcion NET_DVR_GetDeviceAbility y lo retornamos
                    string strOutBuf = Marshal.PtrToStringAnsi(pOutBuf, XML_ABILITY_OUT_LEN);
                    documentoXml.LoadXml(strOutBuf);

                    try
                    {
                        // Especifica la ruta donde quieres guardar el archivo XML
                        string filePath = @"capacidadesDispositivo.xml";
                        documentoXml.Save(filePath); // Guarda el XML en el archivo 
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error al guardar el archivo XML en RetornarXmlConLasCapacidadesDelDispositivo: {ex.Message}");
                    }
                }
                else
                {
                    documentoXml = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error al obtener las capacidades del dispositivo: " + ex.Message);
            }
            finally
            {
                //liberamos memoria
                Marshal.FreeHGlobal(pInBuf);
                Marshal.FreeHGlobal(pOutBuf);

            }
            return documentoXml;
        }

        private string GetDescripcionErrorDeviceAbility(uint iErrCode)
        {
            string strDescription = "";
            switch (iErrCode)
            {
                case 1000:
                    strDescription = "No soportado";
                    break;
                case 1001:
                    strDescription = "Memoria insuficiente";
                    break;
                case 1002:
                    strDescription = "No se pudo encontrar el XML local correspondiente";
                    break;
                case 1003:
                    strDescription = "Error al cargar el XML local";
                    break;
                case 1004:
                    strDescription = "El formato de los datos de capacidad del dispositivo es incorrecto";
                    break;
                case 1005:
                    strDescription = "El tipo de capacidad es incorrecto";
                    break;
                case 1006:
                    strDescription = "El formato del XML de capacidad es incorrecto";
                    break;
                case 1007:
                    strDescription = "El valor del XML de capacidad de entrada es incorrecto";
                    break;
                case 1008:
                    strDescription = "La versión del XML no coincide";
                    break;
                default:
                    break;
            }
            return strDescription;
        }

        //obtenemos las capacidades de acceso del dispositivo
        private Hik_Resultado ObtenerTripleCapacidadDelDispositivo()
        {

            Hik_Resultado resultado = new Hik_Resultado();
            //leer el xml pasado por resultado

            XmlDocument? resultadoXML = RetornarXmlConLasCapacidadesDelDispositivo();

            if (resultadoXML == null)
            {
                //AcsAbility no soportado
                resultado.ActualizarResultado(false, GetDescripcionErrorDeviceAbility(1000), "1000");

                Log.Fatal($"Resultado de las capacidades del dispositivo: Exito: {resultado.Exito} Mensaje: {resultado.Mensaje} Codigo: {resultado.Codigo}");
            }
            else
            {
                //leer nodos <FaceParam> <Card> <FingerPrint> del resultadoXML
                SoportaFacial = VerificarCapacidad(resultadoXML, "//FaceParam");
                SoportaHuella = VerificarCapacidad(resultadoXML, "//FingerPrint");
                SoportaTarjeta = VerificarCapacidad(resultadoXML, "//Card");

                // Dar valor a resultado
                resultado.ActualizarResultado(true, $"Soporta reconocimiento facial: {SoportaFacial}. Soporta huella digital: {SoportaHuella}. Soporta tarjeta: {SoportaTarjeta}", Hik_SDK.NET_DVR_GetLastError().ToString());

                Log.Information($"Resultado de las capacidades del dispositivo: Exito: {resultado.Exito} Mensaje: {resultado.Mensaje} Codigo: {resultado.Codigo}");
            }

            return resultado;
        }

        public int ObtenerCapacidadCarasDispositivo()
        {

            int capacidad = -1;
            XmlDocument? resultadoXML = RetornarXmlConLasCapacidadesDelDispositivo();

            if (resultadoXML != null)
            {
                XmlNode valor = resultadoXML.SelectSingleNode("//maxWhiteFaceNum");

                if (valor != null)
                {
                    capacidad = int.Parse(valor.InnerText.Trim());
                }

            }
            else
            {
                Log.Error("El xml es null en ObtenerCapacidadCarasDispositivo");
            }

            if (capacidad == -1)
            {
                Console.WriteLine("No se pudo obtener la capacidad maxima del dispositivo en ObtenerCapacidadCarasDispositivo");
            }

            return capacidad;
        }

        public bool VerificarEstadoDispositivo()
        {
            nint pInBuf;
            int nSize;
            int iLastErr = 17;
            bool conectado = false;
            pInBuf = nint.Zero;
            nSize = 0;

            int XML_ABILITY_OUT_LEN = 3 * 1024 * 1024;
            nint pOutBuf = Marshal.AllocHGlobal(XML_ABILITY_OUT_LEN);

            if (!Hik_SDK.NET_DVR_GetDeviceAbility(Instancia.IdUsuario, 0, pInBuf, (uint)nSize, pOutBuf, (uint)XML_ABILITY_OUT_LEN))
            {
                iLastErr = (int)Hik_SDK.NET_DVR_GetLastError();

                //si perdio conexión
                if (iLastErr == 17)
                {
                    Console.WriteLine("Se perdio la conexion con el dispositivo");
                    return conectado;
                }

            }

            Marshal.FreeHGlobal(pInBuf);
            Marshal.FreeHGlobal(pOutBuf);

            if (iLastErr == 1000)
            {
                // Console.WriteLine("Conectado");
                conectado = true;
            }
            else
            {
                //Console.WriteLine("Desconectado");
            }
            return conectado;
        }

        private bool VerificarCapacidad(XmlDocument resultadoXML, string capacidad)
        {
            bool soporta = false;
            XmlNode? nodoBuscado = resultadoXML.SelectSingleNode(capacidad);

            if (nodoBuscado != null)
            {
                soporta = true;
            }

            return soporta;

        }


        //INICIALIZAMOS TODO
        public Hik_Resultado InicializarPrograma(string user, string password, string port, string ip)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            resultado = InicializarNet_DVR();
            if (!resultado.Exito) return resultado;

            resultado = HacerLogin(user, password, port, ip);
            if (!resultado.Exito) return resultado;

            resultado = ObtenerCapacidades();
            if (!resultado.Exito) return resultado;

            ActualizarCapacidadCarasDispositivo();
            SetTiempoDispositivo(DateTime.Now);
            InicializarInstanciasControladoras();

            return resultado;
        }


        private Hik_Resultado HacerLogin(string user, string password, string port, string ip)
        {
            Hik_Resultado resultado = Login(user, password, port, ip);
            Log.Information($"Resultado login: Exito: {resultado.Exito} Mensaje: {resultado.Mensaje} Codigo: {resultado.Codigo}");
            return resultado;
        }

        private Hik_Resultado ObtenerCapacidades()
        {
            var resultado = ObtenerTripleCapacidadDelDispositivo();
            Log.Information($"Capacidades del dispositivo: Exito: {resultado.Exito} Mensaje: {resultado.Mensaje} Codigo: {resultado.Codigo}");
            if (!resultado.Exito)
            {
                Log.Error($"El dipositivo no soporta reconocimiento facial de Dx");
            }

            return resultado;
        }

        private void ActualizarCapacidadCarasDispositivo()
        {
            ConfiguracionEstilos configuracion = ConfiguracionEstilos.LeerJsonConfiguracion();
            configuracion.ActualizarCapacidadMaxima();

        }

        private void InicializarInstanciasControladoras()
        {
            //setteamos el callback para obtener los ids de los usuarios
            hik_Controladora_Eventos = Hik_Controladora_Eventos.InstanciaControladoraEventos;
            hik_Controladora_Facial = Hik_Controladora_Facial.Instancia;
            hik_Controladora_Tarjetas = Hik_Controladora_Tarjetas.Instancia;
        }

        public DateTime? ObtenerTiempoDispositivo()
        {
            Hik_Resultado resultado = new Hik_Resultado();
            DateTime? tiempoDisp = null;

            //Inicializo los valores necesarios para obtener el tiempo del dispositivo

            UInt32 dwReturn = 0;
            Int32 nSize = Marshal.SizeOf(m_struTimeCfg);
            IntPtr ptrTimeCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(m_struTimeCfg, ptrTimeCfg, false);

            //obtenemos el tiempo del dispositivo
            if (!Hik_SDK.NET_DVR_GetDVRConfig(idUsuario, Hik_SDK.NET_DVR_GET_TIMECFG, -1, ptrTimeCfg, (UInt32)nSize, ref dwReturn))
            {

                resultado.ActualizarResultado(false,
                    "Error al obtener el tiempo del dispositivo",
                    Hik_SDK.NET_DVR_GetLastError().ToString()
                );

                resultado.EscribirResultado("Obtener Tiempo Dispositivo");
            }
            else
            {
                m_struTimeCfg = (Hik_SDK.NET_DVR_TIME)Marshal.PtrToStructure(ptrTimeCfg, typeof(Hik_SDK.NET_DVR_TIME));

                tiempoDisp = new DateTime(
                    m_struTimeCfg.dwYear,
                    m_struTimeCfg.dwMonth,
                    m_struTimeCfg.dwDay,
                    m_struTimeCfg.dwHour,
                    m_struTimeCfg.dwMinute,
                    m_struTimeCfg.dwSecond
                );

            }
            Marshal.FreeHGlobal(ptrTimeCfg);
            return tiempoDisp;
        }

        public void SetTiempoDispositivo(DateTime nuevoTiempo)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            // Creamos el tiempo a settear
            m_struTimeCfg.dwYear = nuevoTiempo.Year;
            m_struTimeCfg.dwMonth = nuevoTiempo.Month;
            m_struTimeCfg.dwDay = nuevoTiempo.Day;
            m_struTimeCfg.dwHour = nuevoTiempo.Hour;
            m_struTimeCfg.dwMinute = nuevoTiempo.Minute;
            m_struTimeCfg.dwSecond = nuevoTiempo.Second;

            // Inicializo los valores necesarios para settear el tiempo del dispositivo
            Int32 nSize = Marshal.SizeOf(m_struTimeCfg);
            IntPtr ptrTimeCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(m_struTimeCfg, ptrTimeCfg, false);

            if (!Hik_SDK.NET_DVR_SetDVRConfig(idUsuario, Hik_SDK.NET_DVR_SET_TIMECFG, -1, ptrTimeCfg, (UInt32)nSize))
            {
                resultado.ActualizarResultado(false,
                    "Error al obtener el tiempo del dispositivo",
                    Hik_SDK.NET_DVR_GetLastError().ToString()
                );

            }
            else
            {
                resultado.ActualizarResultado(true,
                    "El tiempo del dispositivo se asigno con exito",
                    "1"
                );

            }

            Log.Information($"Resultado login: Exito: {resultado.Exito} Mensaje: {resultado.Mensaje} Codigo: {resultado.Codigo}");

            Marshal.FreeHGlobal(ptrTimeCfg);
        }

        public Hik_Resultado AltaCliente(string idCliente, string nombre)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            //valido si existe
            resultado = ObtenerTarjeta(idCliente);
            if (resultado.Exito) return resultado;

            EsperarCooldownDispositivo(1000);

            resultado = CapturarFoto();
            if (!resultado.Exito) return resultado;

            resultado = CrearTarjeta(idCliente, nombre);
            if (!resultado.Exito) return resultado;

            resultado = AsignarCaraATarjeta(idCliente);
            if (!resultado.Exito) return resultado;

            ActualizarCaras(nombre, idCliente);

            return resultado;
        }

        private Hik_Resultado ObtenerTarjeta(string idCliente)
        {
            Hik_Resultado resultado = Hik_Controladora_Tarjetas.Instancia.ObtenerUnaTarjeta(int.Parse(idCliente));

            if (!resultado.Exito)
            {
                resultado.Mensaje = "Error de obtener la tarjeta";
            }

            return resultado;
        }

        private Hik_Resultado CapturarFoto()
        {
            Hik_Resultado resultado = Hik_Controladora_Facial.Instancia.CapturarCara();

            if (!resultado.Exito)
            {
                resultado.Mensaje = "Error de obtener la cara";
            }

            return resultado;
        }

        private Hik_Resultado CrearTarjeta(string idCliente, string nombre)
        {
            Hik_Resultado resultado = Hik_Controladora_Tarjetas.Instancia.EstablecerUnaTarjeta(int.Parse(idCliente), nombre);

            if (!resultado.Exito)
            {
                resultado.Mensaje = "Error de crear una tarjeta";
            }

            return resultado;
        }

        private Hik_Resultado AsignarCaraATarjeta(string idCliente)
        {
            Hik_Resultado resultado = Hik_Controladora_Facial.Instancia.EstablecerUnaCara((uint)numeroLector, idCliente);

            if (!resultado.Exito)
            {
                resultado.Mensaje = "Error de establecer una cara";
            }

            return resultado;
        }

        private void ActualizarCaras(string nombre, string idCliente)
        {
            ConfiguracionEstilos configuracion = ConfiguracionEstilos.LeerJsonConfiguracion();
            ConservarImagenSocio(configuracion, nombre, idCliente);
            ConfiguracionGeneralUtils.SumarRegistroCara();
        }

        public void ConservarImagenSocio(ConfiguracionEstilos configuracion, string nombreCompletoSocio, string idSocio)
        {
            if (!configuracion.AlmacenarFotoSocio)
            {
                return;
            }

            //Obtengo las rutas necesarias
            string rutaOriginal = "captura.jpg";
            string rutaNueva = configuracion.RutaCarpeta;


            if (string.IsNullOrEmpty(rutaNueva))
            {
                return;
            }

            try
            {
                //Si no existe el directorio, lo creo 
                if (!Directory.Exists(rutaNueva))
                {
                    Directory.CreateDirectory(rutaNueva);
                }

                //Configuro el nombre de la foto
                string nuevoNombre = BuscarImagenSocioUtils.CambiarNombreFoto(nombreCompletoSocio, idSocio);
                string rutaDestino = Path.Combine(rutaNueva, nuevoNombre);

                //Hago la copia de un directorio a otro
                File.Copy(rutaOriginal, rutaDestino, overwrite: true);

            }
            catch (Exception ex)
            {
                Log.Error("Error al guardar la imagen del socio: " + ex.ToString());
            }
        }

        public Hik_Resultado BajaCliente(string id)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            resultado = ObtenerTarjeta(id);
            if (!resultado.Exito) return resultado;

            EsperarCooldownDispositivo(1000);

            resultado = EliminarCaraDeTarjeta(id);
            if (!resultado.Exito) return resultado;


            resultado = EliminarTarjeta(id);
            if (!resultado.Exito) return resultado;

            RestarCara();

            return resultado;

        }

        private Hik_Resultado EliminarCaraDeTarjeta(string id)
        {
            var resultado = Hik_Controladora_Facial.Instancia.EliminarCara(numeroLector, id);
            return resultado;
        }

        private Hik_Resultado EliminarTarjeta(string id)
        {
            var resultado = Hik_Controladora_Tarjetas.Instancia.EliminarTarjetaPorId(int.Parse(id));
            return resultado;
        }

        private void RestarCara()
        {
            ConfiguracionGeneralUtils.RestarRegistroCara();
        }

        private void EsperarCooldownDispositivo(int ms)
        {
            Thread.Sleep(ms);
        }


        public Hik_Resultado BajaMasivaClientes(string[] ids)
        {
            Hik_Resultado resultado = new Hik_Resultado();

            try
            {
                foreach (string id in ids)
                {
                    resultado = BajaCliente(id);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la baja masiva de clientes: " + ex.Message);
            }



            return resultado;
        }

    }
}
