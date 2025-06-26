using DeportNetReconocimiento.Api.Data.Domain;
using DeportNetReconocimiento.Api.Data.Dtos.Request;
using DeportNetReconocimiento.Api.Data.Dtos.Response;
using DeportNetReconocimiento.Api.Services.Interfaces;
using DeportNetReconocimiento.Hikvision.SDKHikvision;
using DeportNetReconocimiento.SDK;
using DeportNetReconocimiento.Utils;
using Serilog;

namespace DeportNetReconocimiento.Api.Services
{
    public class ReconocimientoService : IDeportnetReconocimientoService
    {
    
        private static int? idSucursal;
        

        public ReconocimientoService()
        {
            idSucursal = null;
            LeerCredencialesReconocimientoService();

        }

        private void LeerCredencialesReconocimientoService()
        {
            Credenciales credenciales = CredencialesUtils.LeerCredencialesBd();

            if (credenciales == null)
            {
                idSucursal = null;
                return;
            }

            try
            {
                idSucursal = int.Parse(credenciales.BranchId);

            }
            catch (Exception e)
            {
                Log.Error("LeerCredencialesReconocimientoService Excp: " + e.ToString());
                idSucursal = null;
            }

        }


        //Validaciones
        public string ValidarValores(AltaFacialClienteRequest clienteRequest)
        {
            if (idSucursal == null)
            {
                Console.WriteLine("Id sucursal es null en AltaFacial");
                MensajeDeErrorAltaBajaCliente(
                                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                   clienteRequest.IdCliente.ToString(),
                                   "El idSucursal es nulo, debido a que todavia no se ingresaron las credenciales correspondientes y se esta queriendo realizar una accion desde Deportnet.",
                                   "F",
                                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()
                                   ),
                                   true
                               );
                return "F";
            }


            if (Hik_Controladora_General.Instancia.IdUsuario == -1)
            {
                Console.WriteLine("Id usuario dispositivo es -1 en AltaFacial");
                MensajeDeErrorAltaBajaCliente(
                                    new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                    clienteRequest.IdCliente.ToString(),
                                    "El idUsuario del dispositivo de reconocimiento facial es -1. El dispositivo no esta conectado.",
                                    "F",
                                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()

                                    ),
                                   true
                                );
                return "F";
            }



            if (idSucursal != clienteRequest.IdSucursal)
            {
                Console.WriteLine("El IdSucursal recibido no es igual al local en AltaFacial");
                MensajeDeErrorAltaBajaCliente(
                                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                   clienteRequest.IdCliente.ToString(),
                                   "El idSucursal del dispositivo no coincide con el idSucursal del cliente.",
                                   "F",
                                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                                   true
                               );
                return "F";
            }

            
            if (!DispositivoEnUsoUtils.EstaLibre())
            {
                Console.WriteLine("El dispositivo esta en uso en AltaFacial");
                MensajeDeErrorAltaBajaCliente(
                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                   clienteRequest.IdCliente.ToString(),
                   "El dispositivo ya está en uso.",
                   "F",
                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    true
               );

                return "F";
            }
            
           
            return "T";
        }
        public string ValidarValores(BajaFacialClienteRequest clienteRequest)
        {
            if (idSucursal == null)
            {
                Console.WriteLine("Id sucursal es null en BajaFacial");
                MensajeDeErrorAltaBajaCliente(
                                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                   clienteRequest.IdCliente.ToString(),
                                   "El idSucursal es nulo, debido a que todavia no se ingresaron las credenciales correspondientes y se esta queriendo realizar una accion desde Deportnet.",
                                   "F",
                                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                                   true
                               );
                return "F";
            }


            if (Hik_Controladora_General.Instancia.IdUsuario == -1)
            {
                Console.WriteLine("Id usuario dispositivo es -1 en BajaFacial");
                MensajeDeErrorAltaBajaCliente(
                                    new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                    clienteRequest.IdCliente.ToString(),
                                    "El idUsuario del dispositivo de reconocimiento facial es -1. El dispositivo no esta conectado.",
                                    "F", 
                                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                                   true
                                );
                return "F";
            }



            if (idSucursal != clienteRequest.IdSucursal)
            {
                Console.WriteLine("El IdSucursal recibido no es igual al local en BajaFacial");
                MensajeDeErrorAltaBajaCliente(
                                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                                   clienteRequest.IdCliente.ToString(),
                                   "El idSucursal del dispositivo no coincide con el idSucursal del cliente.",
                                   "F",
                                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                                   true
                               );
                return "F";
            }

            if (!DispositivoEnUsoUtils.EstaLibre())
            {
                Console.WriteLine("El dispositivo esta en uso en BajaFacial");

                MensajeDeErrorAltaBajaCliente(
                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                   clienteRequest.IdCliente.ToString(),
                   "El dispositivo ya está en uso.",
                   "F",
                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    true
               );

                return "F";
            }

            return "T";
        }


        public string AltaFacialCliente(AltaFacialClienteRequest clienteRequest) 
        {
            string resultado = ValidarValores(clienteRequest);

            if(resultado == "T")
            {
                DispositivoEnUsoUtils.Ocupar("Alta facial cliente");
                //asincronico no se espera
                _ = AltaClienteDeportnet(clienteRequest);
            }

            return resultado;

        }

        private static int TiempoRetrasoLuegoDeUnAlta;
        public static bool EstaEsperandoLuegoDeUnAlta;

        public void IniciarTiempoEspera()
        {
            TiempoRetrasoLuegoDeUnAlta = ConfiguracionEstilos.LeerJsonConfiguracion().TiempoDeRetrasoAltaCliente;

            EstaEsperandoLuegoDeUnAlta = true;
            Thread.Sleep(TiempoRetrasoLuegoDeUnAlta * 1000);
            EstaEsperandoLuegoDeUnAlta = false;

        }

        public async Task AltaClienteDeportnet(AltaFacialClienteRequest altaFacialClienteRequest)
        {
            //Console.WriteLine("Ocupo el dispositivo en alta facial");
            //DispositivoEnUsoUtils.Ocupar("Alta cliente Deportnet");

            Hik_Resultado resAlta = Hik_Controladora_General.Instancia.AltaCliente(altaFacialClienteRequest.IdCliente.ToString(), altaFacialClienteRequest.NombreCliente);

            //si no hubo exito
            if (!resAlta.Exito)
            {
                MensajeDeErrorAltaBajaCliente(
                    new RespuestaAltaBajaCliente(altaFacialClienteRequest.IdSucursal.ToString(),
                    altaFacialClienteRequest.IdCliente.ToString(),
                    resAlta.Mensaje,
                    "F",
                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    true
                );

                Console.WriteLine("Hubo un Error en alta facial: " + resAlta.Mensaje);
                DispositivoEnUsoUtils.Desocupar();

                return;
            }

            //si hubo exito
            RespuestaAltaBajaCliente respuestaAlta = new RespuestaAltaBajaCliente(
                altaFacialClienteRequest.IdSucursal.ToString(),
                altaFacialClienteRequest.IdCliente.ToString(),
                "Alta facial cliente exitosa",
                "T",
                ConfiguracionGeneralUtils.ObtenerLectorActual()
            );

            Console.WriteLine($"Hago el aviso a DX de que es exitoso o falliido, en este caso {resAlta.Exito} / {respuestaAlta.IsSuccessful}");

            string mensaje = await WebServicesDeportnet.AltaFacialClienteDeportnet(respuestaAlta);

                
            Console.WriteLine("Se ha dado de alta el cliente facial con id: " + altaFacialClienteRequest.IdCliente + " y nombre: " + altaFacialClienteRequest.NombreCliente);
            IniciarTiempoEspera();

            Console.WriteLine("Desocupo este desocupar osea que aca ya anduvo todo bien");
            DispositivoEnUsoUtils.Desocupar();
        }

        public string BajaFacialCliente(BajaFacialClienteRequest clienteRequest)
        {

            string resultado = ValidarValores(clienteRequest);

            if(resultado == "T")
            {
                //asincronico no se espera
                _ = BajaClienteDeportnet(clienteRequest);
                 return resultado;
            }

            if (Hik_Controladora_General.Instancia.IdUsuario == -1)
            {
                MensajeDeErrorAltaBajaCliente(
                    new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                    clienteRequest.IdCliente.ToString(),
                    "El idUsuario del dispositivo de reconocimiento facial es -1. El dispositivo no esta conectado.",
                    "F",
                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    false
                );

                return "F";
            }

            if (idSucursal != clienteRequest.IdSucursal)
            {
                MensajeDeErrorAltaBajaCliente(
                    new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                    clienteRequest.IdCliente.ToString(),
                    "El idSucursal del dispositivo no coincide con el idSucursal del cliente.",
                    "F",
                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    false
                );

               
                return "F";
            }

            if (!DispositivoEnUsoUtils.EstaLibre())
            {
                MensajeDeErrorAltaBajaCliente(
                   new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                   clienteRequest.IdCliente.ToString(),
                   "El dispositivo ya está en uso.",
                   "F",
                   lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                   false
               );

             
                return "F";
               
            }

            //asincronico no se espera
            _ = BajaClienteDeportnet(clienteRequest);
            return "T";

        }

        private async Task BajaClienteDeportnet(BajaFacialClienteRequest clienteRequest)
        {
            DispositivoEnUsoUtils.Ocupar("Baja cliente deportnet");
            Hik_Resultado resBaja = Hik_Controladora_General.Instancia.BajaCliente(clienteRequest.IdCliente.ToString());

            if (!resBaja.Exito)
            {

                MensajeDeErrorAltaBajaCliente(
                    new RespuestaAltaBajaCliente(clienteRequest.IdSucursal.ToString(),
                    clienteRequest.IdCliente.ToString(),
                    resBaja.Mensaje,
                    "F",
                    lector: ConfiguracionGeneralUtils.ObtenerLectorActual()),
                    false
                );
                Console.WriteLine("Hubo un Error en Baja facial: " + resBaja.Mensaje);
                return;

            }
            else
            {
                RespuestaAltaBajaCliente respuestaAlta = new RespuestaAltaBajaCliente(
                    clienteRequest.IdSucursal.ToString(),
                    clienteRequest.IdCliente.ToString(),
                    "Baja facial cliente exitosa",
                    "T",
                    ConfiguracionGeneralUtils.ObtenerLectorActual()
                );
                string mensaje = await WebServicesDeportnet.BajaFacialClienteDeportnet(respuestaAlta);

                Console.WriteLine("Se ha dado de baja el cliente facial con id: " + clienteRequest.IdCliente);

            }
            DispositivoEnUsoUtils.Desocupar();
        }

        private void MensajeDeErrorAltaBajaCliente(RespuestaAltaBajaCliente rta, bool isAlta)
        {

            if (isAlta)
            {
                _ = WebServicesDeportnet.AltaFacialClienteDeportnet(rta);
            }
            else
            {
                _ = WebServicesDeportnet.BajaFacialClienteDeportnet(rta);
            }

        }

        //public string BajaMasivaFacialCliente(BajaFacialClienteRequest clienteRequest)
        //{
        //    string[] arregloResultados = [];

        //    if (enUso)
        //    {
        //        return "F";
        //    }

        //    for (int i=0; i < clienteRequest.ArregloIdClientes.Length; i++)
        //    {
        //        arregloResultados[i] = BajaFacialCliente(clienteRequest);
        //    }

        //    return "T";
        //}
    }
}
