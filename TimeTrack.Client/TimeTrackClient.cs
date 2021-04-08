using System.Net.Http;

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
        
        
    }
}