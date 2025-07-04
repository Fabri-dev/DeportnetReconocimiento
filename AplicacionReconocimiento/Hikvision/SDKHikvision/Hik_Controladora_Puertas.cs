using Serilog;
using System.Diagnostics;


namespace DeportNetReconocimiento.Hikvision.SDKHikvision
{
    internal class Hik_Controladora_Puertas
    {
        //atributos
        private static Hik_Controladora_Puertas? instancia;

        private Hik_Controladora_Puertas()
        {

        }

        //propiedades
        public static Hik_Controladora_Puertas Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Hik_Controladora_Puertas();
                }
                return instancia;
            }
        }

        //0-close, 1-open, 2-stay open, 3-stay close
        public static Hik_Resultado OperadorPuerta(int operacion)
        {
            Hik_Resultado resultado = new Hik_Resultado();
            int idUsuario = Hik_Controladora_General.Instancia.IdUsuario;
            if (idUsuario == -1)
            {
                resultado.ActualizarResultado(false, "No se ha logueado el usuario.", Hik_SDK.NET_DVR_GetLastError().ToString());
                return resultado;
            }

            switch (operacion)
            {

                case 0:
                    //0-close
                    if (Hik_SDK.NET_DVR_ControlGateway(idUsuario, 1, 0))
                    {
                        resultado.ActualizarResultado(true, "Puerta cerrada con exito", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    else
                    {
                        resultado.ActualizarResultado(false, "Error al cerrar la puerta", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    break;
                case 1:
                    //1 - open
                    if (Hik_SDK.NET_DVR_ControlGateway(idUsuario, 1, 1))
                    {
                        resultado.ActualizarResultado(true, "Puerta abierta con exito", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    else
                    {
                        resultado.ActualizarResultado(false, "Error al abrir la puerta", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    break;
                case 2:
                    //2 - stay open

                    if (Hik_SDK.NET_DVR_ControlGateway(idUsuario, 1, 2))
                    {
                        resultado.ActualizarResultado(true, "Puerta abierta y se mantiene abierta", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    else
                    {
                        resultado.ActualizarResultado(false, "Error al mantener la puerta abierta", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }

                    break;
                case 3:
                    //3 - stay close

                    if (Hik_SDK.NET_DVR_ControlGateway(idUsuario, 1, 3))
                    {
                        resultado.ActualizarResultado(true, "Puerta cerrada y se mantiene cerrada", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    else
                    {
                        resultado.ActualizarResultado(false, "Error al mantener la puerta cerrada", Hik_SDK.NET_DVR_GetLastError().ToString());

                    }
                    break;
                default:
                    resultado.Exito = false;
                    resultado.Mensaje = "Operacion no valida, operaciones del 0 al 3";
                    break;


            }

            return resultado;
        }


        public static void EjecutarExe(string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                Log.Information("Error al ejecutar el exe: La ruta esta vacía");

                return;
            }

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Normal,

                };

                Process proceso = Process.Start(processStartInfo);
                Log.Information("Ejecuto el exe para la apertura.");

            }
            catch
            {
                Log.Error($"Error al procesar el archivo en {ruta}");
            }
        }

    }
}
