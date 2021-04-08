using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Client
{
    public class ProjectClient : WebClientBase
    {
        public ProjectClient() { }
        public ProjectClient(HttpClient client) : base(client) {}
        public ProjectClient(HttpClientHandler handler, HttpClient client) : base(client, handler) { }

        public async Task<ResponseResult<List<ProjectDataTransfer>, HttpResponseMessage, HttpRequestMessage>> GetAllAsync()
        {
            var list = new List<ProjectDataTransfer>();

            var httpRequestMessage = new HttpRequestMessage(
                new HttpMethod("GET"),
                "v1/api/project/list"
            );

            var httpResponseMessage = await HttpClient.SendAsync(
                httpRequestMessage
                );

            list = await JsonSerializer.DeserializeAsync<List<ProjectDataTransfer>>(await httpResponseMessage.Content.ReadAsStreamAsync());

            return new ResponseResult<List<ProjectDataTransfer>, HttpResponseMessage, HttpRequestMessage>(list, httpResponseMessage, httpRequestMessage);
        }
        
        public async Task<ResponseResult<ProjectDataTransfer, HttpResponseMessage, HttpRequestMessage>> PutSingleAsync(ProjectDataTransfer project)
        {
            var projectsResult = new ProjectDataTransfer();

            var httpRequestMessage = new HttpRequestMessage(
                new HttpMethod("Put"),
                "v1/api/project"
            );
            
            // ToDo Das async machen
            var content = JsonSerializer.Serialize(project);
            httpRequestMessage.Content = new StringContent(content);
            httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            
            var httpResponseMessage = await HttpClient.SendAsync(
                httpRequestMessage
            );
            
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            projectsResult = JsonSerializer.Deserialize<ProjectDataTransfer>(data);
            
            return new ResponseResult<ProjectDataTransfer, HttpResponseMessage, HttpRequestMessage>(projectsResult, httpResponseMessage, httpRequestMessage);
        }
    }
}