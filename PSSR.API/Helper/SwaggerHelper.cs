using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSSR.API.Helper
{
    public class SwaggerHelper
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition("oauth2", new ApiKeyScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = "header",
                Name = "Authorization",
                Type = "Authorization"
            });

            var webApiAssembly = Assembly.GetEntryAssembly();
            AddSwaggerDocPerVersion(swaggerGenOptions, webApiAssembly);
            ApplyDocInclusions(swaggerGenOptions);
            //IncludeXmlComments(swaggerGenOptions);
        }

        private static void AddSwaggerDocPerVersion(SwaggerGenOptions swaggerGenOptions, Assembly webApiAssembly)
        {
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                string title = "Amin Sahranavard Oil Api";
                switch (apiVersion)
                {
                    case "1.0":
                        title += " - Manage Profile";
                        break;
                }

                swaggerGenOptions.SwaggerDoc($"v{apiVersion}",
                    new Info
                    {
                        Title = title,
                        Version = $"v{apiVersion}"
                    });
            }
        }

        private static void ApplyDocInclusions(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.DocInclusionPredicate((docName, apiDesc) =>
            {
                var versions = apiDesc.ControllerAttributes()
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v.ToString()}" == docName);
            });
        }

        private static IEnumerable<string> GetApiVersions(Assembly webApiAssembly)
        {
            var apiVersion = webApiAssembly.DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)) && x.GetCustomAttributes<ApiVersionAttribute>().Any())
                .Select(y => y.GetCustomAttribute<ApiVersionAttribute>())
                .SelectMany(v => v.Versions)
                .Distinct()
                .OrderBy(x => x);

            return apiVersion.Select(x => x.ToString());
        }

        public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            //swaggerOptions.RouteTemplate = "api-docs/v1-Building/swagger.json";
        }

        public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                string title = apiVersion;
                switch (apiVersion)
                {
                    case "1.0":
                        title = "1.0 - Manage Profile";
                        break;
                }
                swaggerUIOptions.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", $"V{title} Docs");
            }
            swaggerUIOptions.RoutePrefix = "api-docs";
        }
    }
}
