using System.Net;
using System.Net.Http;
using Moq;

namespace kin_sdk_tests
{
    public class FakeHttpClient
    {

        Mock<FakeHttpMessageHandler> mockFakeHttpMesssageHandler;
        public HttpClient httpClient;

        public FakeHttpClient()
        {
            this.mockFakeHttpMesssageHandler = new Mock<FakeHttpMessageHandler> {CallBase = true};
            this.httpClient = new HttpClient(this.mockFakeHttpMesssageHandler.Object);
        }
        public void SetResponse(string content, HttpStatusCode statusCode=HttpStatusCode.OK)
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            };

            this.mockFakeHttpMesssageHandler.Setup(a => a.Send(It.IsAny<HttpRequestMessage>())).Returns(httpResponseMessage);
        }
    }
}
