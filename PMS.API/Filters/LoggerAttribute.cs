using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;

namespace PMS.API.Filters;

public class LoggerAttribute : ActionFilterAttribute
{
    private readonly ILoggerFactory _loggerFactory;

    public LoggerAttribute(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = _loggerFactory.CreateLogger(context.Controller.GetType());

        string actionName = context.ActionDescriptor.DisplayName;
        var reqUrl = context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host + context.HttpContext.Request.Path.Value;
        var username = ((context.Controller as ControllerBase)?.User?.Identity as ClaimsIdentity)?.Name ?? "";

        logger.LogInformation(string.Format("REQUEST--->LoggedIn Username:{0};URL:{1};Body:{2};", username, reqUrl, JsonConvert.SerializeObject(context.ActionArguments)));
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = _loggerFactory.CreateLogger(context.Controller.GetType());

        string actionName = context.ActionDescriptor.DisplayName;

        string statusCode = "";
        string resBody = "";

        var result = context.Result;

        if (result is JsonResult json)
        {
            var x = json.Value;
            statusCode = json.StatusCode.ToString();
            resBody = JsonConvert.SerializeObject(x);
        }

        if (result is ObjectResult _json)
        {
            var x = _json.Value;
            statusCode = context.HttpContext.Response.StatusCode.ToString();
            resBody = JsonConvert.SerializeObject(x);
        }
        if (result is ViewResult view)
        {
            // I think it's better to log ViewData instead of the finally rendered template string
            statusCode = view.StatusCode.ToString();
            var x = view.ViewData;
            var name = view.ViewName;
            resBody = JsonConvert.SerializeObject(x);
        }
        else
        {
            // if result is of something else type
            statusCode = context.HttpContext.Response.StatusCode.ToString();
        }

        logger.LogInformation(string.Format("RESPONSE---> (" + statusCode + ")--->Body:{0}", resBody));

        base.OnActionExecuted(context);
    }


}
