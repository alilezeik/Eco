namespace Eco.ApiGateway
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;
    using System.Linq;

    public class LowercaseDocumentFilter : IDocumentFilter
    {
        private static string LowercaseEverythingButParameters(string key)
        {
            return string.Join("/", key.Split('/').Select(x => x.Contains("{") ? x : x.ToLower()));
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key), entry => entry.Value);
           
            swaggerDoc.Paths = new OpenApiPaths();
            
            foreach (var value in paths)
            {
                swaggerDoc.Paths.Add(value.Key,value.Value);
            }
            
        }
    }
}
