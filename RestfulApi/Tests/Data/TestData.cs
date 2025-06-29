using RestfulApi.Tests.Models;
using System.Reflection.Metadata;

public static class TestData
{


    public const String APPLE_IPHONE_17_PRO = "Apple iPhone 17 Pro";
    public const String APPLE_IPHONE_18_PRO_MAX = "Apple Iphone 18 pro Max";


    public static RequestObject GetValidObject() => new RequestObject
    {
        name = "Apple iPhone 17 Pro",
        data = new Data { color = "Rose Gold", size = "medium" }
    };
}
