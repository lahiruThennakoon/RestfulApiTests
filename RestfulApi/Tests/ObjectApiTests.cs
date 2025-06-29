using FluentAssertions;
using Newtonsoft.Json;
using RestfulApi.Tests.Config;
using RestfulApi.Tests.Models;
using RestSharp;
using System.Net;

namespace RestfulApi.Tests
{
    [TestCaseOrderer("RestfulApi.Tests.Config.PriorityOrderer", "RestfulApi")]
    public class ObjectApiTests
    {
        private readonly RestClient _client = new("https://api.restful-api.dev");
        private static string? ObjectId;
        private async Task<ResponseObject> CreateObjectAsync(RequestObject requestObject)
        {
            var request = new RestRequest("/objects", Method.Post);
            request.AddJsonBody(requestObject);
            var response = await _client.PostAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseObject = JsonConvert.DeserializeObject<ResponseObject>(response.Content);
            ObjectId = responseObject.id;
            return responseObject;
        }

        [Fact(DisplayName = "Get list of all objects"), TestPriority(1)]
        public async Task GetAllObjects_ShouldReturnListOfAllObjects()
        {
            var request = new RestRequest("/objects", Method.Get);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var objectList = JsonConvert.DeserializeObject<List<ResponseObject>>(response.Content);
            objectList.Count.Should().BeGreaterThan(1);

        }

        [Fact(DisplayName = "Add an Object"), TestPriority(2)]
        public async Task AddObject_ShouldCreateObjectSuccessfully()
        {
            RequestObject requestObject = TestData.GetValidObject();
            var created = await CreateObjectAsync(requestObject);
            created.id.Should().NotBeNullOrEmpty();
            created.name.Should().Be(TestData.APPLE_IPHONE_17_PRO);

        }

        [Fact(DisplayName = "Get Single Object Using Created Id"), TestPriority(3)]
        public async Task GetObjectById_ShouldReturnCorrectObject()
        {
            var request = new RestRequest($"/objects/{ObjectId}", Method.Get);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty("Response body should not be empty");
            response.ContentType.Should().Contain("application/json", "Should return JSON");
            var responseObject = JsonConvert.DeserializeObject<ResponseObject>(response.Content);
            responseObject.id.Should().Be(ObjectId);
            responseObject.name.Should().Be(TestData.APPLE_IPHONE_17_PRO);

        }
        [Fact(DisplayName = "Get Single Object Using invalid Id"), TestPriority(4)]
        public async Task GetObjectByInvalidId_ShouldReturnNotFound()
        {
            var request = new RestRequest($"/objects/{ObjectId+"invalid"}", Method.Get);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            

        }
        [Fact(DisplayName = "Update Existing Object Using Valid Id"), TestPriority(5)]
        public async Task UpdateObject_ShouldUpdateSuccessfully()
        {
            var request = new RestRequest($"/objects/{ObjectId}", Method.Put);
            RequestObject updatedObject =new RequestObject();
            updatedObject.name = TestData.APPLE_IPHONE_18_PRO_MAX;
            updatedObject.data = new Data { color = "Graphite", size = "large" };
            request.AddJsonBody(updatedObject);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty("Response body should not be empty");
            response.ContentType.Should().Contain("application/json", "Should return JSON");
            var responseObject = JsonConvert.DeserializeObject<ResponseObject>(response.Content);
            responseObject.id.Should().Be(ObjectId);
            responseObject.name.Should().Be(TestData.APPLE_IPHONE_18_PRO_MAX);

        }
        [Fact(DisplayName = "Update Non-Existing Object"), TestPriority(6)]
        public async Task UpdateNonExistentObject_ShouldReturnNotFound()
        {
            var request = new RestRequest($"/objects/{ObjectId+"invalid"}", Method.Put);
            RequestObject updatedObject = new RequestObject();
            updatedObject.name = TestData.APPLE_IPHONE_18_PRO_MAX;
            updatedObject.data = new Data { color = "Graphite", size = "large" };
            request.AddJsonBody(updatedObject);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
        [Fact(DisplayName = "Delete Existing Object Using Id"), TestPriority(7)]
        public async Task DeleteObject_ShouldDeleteSuccessfully()
        {
            var request = new RestRequest($"/objects/{ObjectId}", Method.Delete);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty("Response body should not be empty");
            response.ContentType.Should().Contain("application/json", "Should return JSON");
            var responseObject = JsonConvert.DeserializeObject<ResponseObject>(response.Content);
            responseObject.message.Should().Be($"Object with id = {ObjectId} has been deleted.");

        }
        [Fact(DisplayName = "Get Already Deleted Object"), TestPriority(8)]
        public async Task GetObject_AlreadyDeletedShouldReturnNotFound()
        {
            var request = new RestRequest($"/objects/{ObjectId}", Method.Get);
            var response = await _client.ExecuteAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
    }
}
