using System.Net;

namespace ASW.Models
{
    /// <summary>
    /// Data Structure to return errors to the endpoint
    /// </summary>
    public class ErrorModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}