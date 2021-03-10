using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace Todo.IntegrationTests.Common
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }

    public class IntegrationTestsFixture
    {
        private HttpClient HttpClient;
        public static string TodoUserAccessToken { get; set; }
        public static string TesterUserAccessToken { get; set; }

        public IntegrationTestsFixture()
        {
            HttpClient = new CustomWebAppFactory().CreateClient();
        }

        public void StoreTodoUserAccessToken(string todoUserAccessToken)
        {
            TodoUserAccessToken = todoUserAccessToken;
        }

        public void StoreTesterUserAccessToken(string testerUserAccessToken)
        {
            TesterUserAccessToken = testerUserAccessToken;
        }

        public HttpClient ClientWithTodoUserAuthorizationHeader()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TodoUserAccessToken);
            return HttpClient;
        }

        public HttpClient ClientWithTesterUserAuthorizationHeader()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TesterUserAccessToken);
            return HttpClient;
        }

        public HttpClient ClientWithoutAuthorizationHeader()
        {
            HttpClient.DefaultRequestHeaders.Clear();
            return HttpClient;
        }
    }
}
