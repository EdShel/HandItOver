using HandItOver.BackEnd.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Extensions
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (Exception exception)
            {

                var statusCode = exception switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    WrongValueException => (int)HttpStatusCode.BadRequest,
                    RecordAlreadyExistsException => (int)HttpStatusCode.BadRequest,
                    ExpiredException => (int)HttpStatusCode.BadRequest,
                    NoRightsException => (int)HttpStatusCode.Forbidden,
                    NotOwnerException => (int)HttpStatusCode.Forbidden,
                    _ => 500,
                };
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var errorObject = new
                {
                    Code = context.Response.StatusCode,
                    Message = exception.Message
                };
                string jsonError = JsonConvert.SerializeObject(errorObject);

                await context.Response.Body.WriteAsync(
                    Encoding.UTF8.GetBytes(jsonError)
                );
            }
        }
    }

}
