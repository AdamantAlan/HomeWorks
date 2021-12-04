using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Middleware.Filters.Schemas
{
    /// <summary>
    /// Schema filter for swagger.
    /// </summary>
    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            var desc = Enum.GetNames(context.Type)
                .Aggregate("", (acc, c) => acc + $"{Convert.ToInt64(Enum.Parse(context.Type, c))} - {c} ");

            schema.Description = $"{schema.Description} : {desc}";
        }
    }
}
