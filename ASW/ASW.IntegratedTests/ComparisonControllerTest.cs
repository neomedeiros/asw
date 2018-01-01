using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ASW.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace ASW.IntegratedTests
{
    public class ComparisonControllerTest
    {
        public ComparisonControllerTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        private readonly TestServer _server;
        private readonly HttpClient _client;

        [Fact]
        public async Task Diff_SHOULD_Return_Bad_Request_WHEN_Request_is_Incomplete_missing_left()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var rightBase64 = "\"IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            var response = await _client.PostAsync("/v1/diff/2/right",
                new StringContent(rightBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync("/v1/diff/2");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Diff_SHOULD_Return_Bad_Request_WHEN_Request_is_Incomplete_missing_right()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var leftBase64 = "\"eyAibmFtZSI6IkpvaG4iLCAiYWdlIjozMCwgImNhciI6bnVsbCB9\"";

            var response = await _client.PostAsync("/v1/diff/1/left",
                new StringContent(leftBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync("/v1/diff/1");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Diff_SHOULD_Return_Bad_Request_WHEN_Request_not_exists()
        {
            var response = await _client.GetAsync("/v1/diff/10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Diff_SHOULD_Return_Ok_and_equal()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var jsonBase64 = "\"IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            var response = await _client.PostAsync("/v1/diff/3/left",
                new StringContent(jsonBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.PostAsync("/v1/diff/3/right",
                new StringContent(jsonBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync("/v1/diff/3");
            var result = JsonConvert.DeserializeObject<DiffResultModel>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public async Task Diff_SHOULD_Return_Ok_different_size()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var johnBase64 = "\"IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            // { "name":"Peter Jackson", "age":30, "car":null }
            var peterJacksonBase64 = "\"IHsgIm5hbWUiOiJQZXRlciBKYWNrc29uIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            var response = await _client.PostAsync("/v1/diff/5/left",
                new StringContent(johnBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.PostAsync("/v1/diff/5/right",
                new StringContent(peterJacksonBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync("/v1/diff/5");
            var result = JsonConvert.DeserializeObject<DiffResultModel>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.AreEqual.Should().BeFalse();
            result.HaveSameSize.Should().BeFalse();
            result.DiffInsights.Count.Should().Be(0);
        }

        [Fact]
        public async Task Diff_SHOULD_Return_Ok_same_size_and_1_insight()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var johnBase64 = "\"IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            //Base64 of { "name":"Pete", "age":30, "car":null }
            var peteBase64 = "\"IHsgIm5hbWUiOiJQZXRlIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==\"";

            var response = await _client.PostAsync("/v1/diff/4/left",
                new StringContent(johnBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.PostAsync("/v1/diff/4/right",
                new StringContent(peteBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync("/v1/diff/4");
            var result = JsonConvert.DeserializeObject<DiffResultModel>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.AreEqual.Should().BeFalse();
            result.HaveSameSize.Should().BeTrue();
            result.DiffInsights.Count.Should().Be(1);
        }

        [Fact]
        public async Task PostDiffEntry_SHOULD_Return_Ok_left()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var leftBase64 = "\"eyAibmFtZSI6IkpvaG4iLCAiYWdlIjozMCwgImNhciI6bnVsbCB9\"";

            var response = await _client.PostAsync("/v1/diff/6/left",
                new StringContent(leftBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostDiffEntry_SHOULD_Return_Ok_right()
        {
            //Base64 of { "name":"John", "age":30, "car":null }
            var rightBase64 = "\"eyAibmFtZSI6IkpvaG4iLCAiYWdlIjozMCwgImNhciI6bnVsbCB9\"";

            var response = await _client.PostAsync("/v1/diff/6/right",
                new StringContent(rightBase64, Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}