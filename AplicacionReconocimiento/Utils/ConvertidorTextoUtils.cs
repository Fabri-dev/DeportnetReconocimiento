﻿using System.Text.RegularExpressions;

namespace DeportNetReconocimiento.Utils
{
    public class ConvertidorTextoUtils
    {

        public static string LimpiarTextoEnriquecido(string mensajeHTMLCrudo)
        {
            if (string.IsNullOrEmpty(mensajeHTMLCrudo))
            {
                return ConvertirHtmlToRtf("<strong> Bienvenido! </strong>");
            }

            // Preparo el texto para cargarlo como corresponde
            string textoSinCaracteresEscape = LimpiarCaracteresEscape(mensajeHTMLCrudo);
            string textoSinUnicode = SacarFormaToUnicode(textoSinCaracteresEscape);
            string textoRTF = ConvertirHtmlToRtf(textoSinUnicode);

            return textoRTF;
        }


        //El mensaje de acceso concedido trae html y css 
        //Esta función reemplaza todo eso  por saltos de linea para mostrarlo de manera correcta
        public static string LimpiarTextoHtml(string mensajeHTMLCrudo)
        {

            if (string.IsNullOrEmpty(mensajeHTMLCrudo))
            {
                return "";
            }

            string textoHTMLSimplificado = Regex.Replace(mensajeHTMLCrudo, "<.*?>", "\n");

            return textoHTMLSimplificado;

        }

        public static string ConvertirHtmlToRtf(string html)
        {
            // Reemplazar etiquetas HTML por RTF
            html = html.Replace("<strong>", @"\b ").Replace("</strong>", @"\b0 ");
            html = html.Replace("<br>", @"\line ");
            html = html.Replace("<br />", @"\line ");
            html = html.Replace("<div>", @"\line ");
            html = html.Replace("</div>", "");
            html = html.Replace("\n", @"\line ");
            html = Regex.Replace(html, "<.*?>", "");

            // Darle el formato RTF a lo demas 
            string rtfHeader = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Arial;}} ";
            string rtfFooter = "}";

            string rtfCompleto = rtfHeader + html + rtfFooter;

            return rtfCompleto;
        }


        public static string SacarFormaToUnicode(string textoUnicode)
        {
            // Reemplaza las secuencias de escape Unicode con los caracteres correspondientes
            return Regex.Replace(textoUnicode, @"\\u([0-9A-Fa-f]{4})", match =>
            {
                // Convierte el código Unicode en el carácter correspondiente
                return char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber));
            });
        }


        // Método para eliminar caracteres de escape innecesarios
        public static string LimpiarCaracteresEscape(string mensaje)
        {
            // Reemplazar \/ por /
            mensaje = mensaje.Replace(@"\/", "/");

            // Reemplazar \n por un salto de línea
            mensaje = mensaje.Replace(@"\n", "\n");

            // Devolver el texto limpio
            return mensaje;
        }


        public static string PrimerLetraMayuscula(string nombre)
        {
            return char.ToUpper(nombre[0]) + nombre.Substring(1);
        }

    }
}
