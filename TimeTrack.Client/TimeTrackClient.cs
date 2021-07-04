using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer;

namespace TimeTrack.Client
{
    public class TimeTrackClient : WebClientBase
    {
        public AccountClient AccountClient { get; set; }
        public TokenClient TokenClient { get; set; }
        public RoleClient RoleClient { get; set; }
        public MemberClient MemberClient { get; set; }
        public CustomerClient CustomerClient { get; set; }
        public ProjectClient ProjectClient { get; set; }

        public ActivityTypeClient ActivityTypeClient
        {
            get
            {
                if (_activityTypeClient == null)
                {
                    _activityTypeClient = new ActivityTypeClient(HttpClientHandler, HttpClient);
                }
                
                return _activityTypeClient;
            }
        }

        public ActivityClient ActivityClient { get; set; }
        
        private AccountClient _accountClient;
        private TokenClient _tokenClient;
        private RoleClient _roleClient;
        private MemberClient _memberClient;
        private CustomerClient _customerClient;
        private ProjectClient _projectClient;
        private ActivityTypeClient _activityTypeClient;
        private ActivityClient _activityClient;
        
        public TimeTrackClient(string baseAddress) : base(baseAddress)
        {
        }

        public TimeTrackClient(HttpClient client) : base(client)
        {
        }

        public TimeTrackClient(HttpClient client, HttpClientHandler handler) : base(client, handler)
        {
        }

        public async Task LoginAsync(string mail, string password)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/v1/api/Account/login");
            
            var httpResponseMessage = await HttpClient.SendAsync(message);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var r = JsonSerializer.Deserialize<NewTokenDataTransfer>(await httpResponseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions(JsonSerializerDefaults.Web));
                if (r != null)
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", r.Token);
                }
            }
        }
    }
}