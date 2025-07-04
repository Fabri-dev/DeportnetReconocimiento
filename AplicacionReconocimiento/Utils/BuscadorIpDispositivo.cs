using DeportNetReconocimiento.Hikvision.SDKHikvision;
using Serilog;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace DeportNetReconocimiento.Utils
{
    public class BuscadorIpDispositivo
    {
        public static async Task<Hik_Resultado> ObtenerIpDispositivo(string port, string username, string password)
        {


            Hik_Resultado resultadoLogin = new Hik_Resultado();

            List<string> activeIps = GetActiveIPs();

            foreach (var ip in activeIps)
            {

                //disminuimos el tiempo de ejecucion descartando las ips que no empiezan con esta secuencia
                if (ip.StartsWith("192.168."))
                {
                    Log.Information($"Probando con ip: {ip}");
                    resultadoLogin = Hik_Controladora_General.Instancia.InicializarPrograma(username, password, port, ip);

                    if (resultadoLogin.Exito)
                    {
                        Log.Information($"Dispositivo Hikvision encontrado en: {ip}");
                        resultadoLogin.Mensaje = ip;
                        break; // Sale del bucle al encontrar el primer dispositivo
                    }

                }

            }

            if (!resultadoLogin.Exito)
            {
                resultadoLogin.Mensaje = "No se encontró la ip del dispositivo, verifique si esta conectado.";
            }


            return resultadoLogin;
        }

        private static List<string> GetActiveIPs()
        {
            List<string> ips = new List<string>();

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c arp -a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Expresión regular para extraer direcciones IP del comando `arp -a`
            MatchCollection matches = Regex.Matches(output, @"\d+\.\d+\.\d+\.\d+");

            foreach (Match match in matches)
            {
                ips.Add(match.Value);
            }

            return ips;
        }

    }
}
