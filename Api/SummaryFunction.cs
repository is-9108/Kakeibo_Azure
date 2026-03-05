using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Kakeibo.Api;

public class SummaryFunction
{
    private readonly ILogger<SummaryFunction> _logger;

    public SummaryFunction(ILogger<SummaryFunction> logger)
    {
        _logger = logger;
    }

    [Function("SummaryFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}