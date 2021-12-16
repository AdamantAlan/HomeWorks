using Microsoft.AspNetCore.Mvc;

namespace Filters
{
    /// <summary>
    /// Result server work with code 500;
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            base.StatusCode = 500;
        }
    }
}
