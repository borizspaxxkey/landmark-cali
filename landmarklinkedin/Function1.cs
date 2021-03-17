using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Linq;

namespace landmarklinkedin
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(Constants.COMPUTER_VISION_API_KEY));
            computerVisionClient.Endpoint = Constants.COMPUTER_VISION_ENDPOINT;

            var imageStream = req.Body;

            var result = await computerVisionClient.AnalyzeImageInStreamAsync(imageStream, null, details: new List<Details?>() { Details.Landmarks });
            var landmark = result.Categories.FirstOrDefault(c => c.Detail != null && c.Detail.Landmarks.Any());

            return landmark != null ? (ActionResult)new OkObjectResult($"I think i see the {landmark.Detail.Landmarks.First().Name}")
                : new BadRequestObjectResult("No landmark was found in this image");
        }
    }
}
