
namespace RestfulApi.Tests.Models
{

    public class ResponseData
    {
        public String color { get; set; }
        public String size { get; set; }
    }
    public class ResponseObject
    {
        public String id { get; set; }
        public String name { get; set; }
        public ResponseData data { get; set; }
        public String message { get; set; }
    }
}

