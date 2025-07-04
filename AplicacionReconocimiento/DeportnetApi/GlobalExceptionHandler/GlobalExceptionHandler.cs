﻿using DeportNetReconocimiento.Api.GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;


namespace DeportNetReconocimiento.Api.GlobalExceptionHandler
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Procesa la solicitud normalmente
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Predeterminado
            string message = "Error interno del servidor";
            string result = "F";

            // Manejo específico por tipo de excepción
            switch (exception)
            {
                case HikvisionException:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = exception.Message;
                    break;
                case DispositivoEnUsoException:
                    statusCode = HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "No autorizado";

                    break;
                default:
                    // Registrar la excepción para análisis futuro
                    Console.WriteLine("Global Exception Handler, Excepcion Inesperada: "+ exception);
                    break;
            }




            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(
            new
            {
                result,
                context.Response.StatusCode,
                message

            }.ToString());
        }
    }

}

