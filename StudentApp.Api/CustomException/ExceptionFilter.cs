using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Filters;

namespace code.CustomException
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //if none condition matches this gets excecuted .otherwise ststuscode wil be ok
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errMsg = string.Empty;
            var exceptionType = actionExecutedContext.Exception.GetType();
            if (exceptionType == typeof(FormatException))
            {
                errMsg = "input string not in correct format";
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NullReferenceException))
            {
                errMsg = "Data not found";
                statusCode = HttpStatusCode.NotFound;
            }
            else
            {
                errMsg = "Internal error";
                statusCode = HttpStatusCode.InternalServerError;
            }
            //constructing a message to send to client
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(errMsg),
                ReasonPhrase = "from exception filter"
            };
            //  passing msg to client
            actionExecutedContext.Response = response;
            //calling base clss to implemment additional logic
            base.OnException(actionExecutedContext);
        }
    }
}