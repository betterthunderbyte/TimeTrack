using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Client
{
    public class ActivityTypeClient : WebClientBase
    {
        public ActivityTypeClient() : base() {}

        public ActivityTypeClient(HttpClient client) : base(client) {}
        public ActivityTypeClient(HttpClientHandler handler, HttpClient client) : base(client, handler){}
        
        public async Task<IEnumerator<ActivityTypeDataTransfer>> GetAllAsync()
        {
            Uri uri = new Uri("/v1/api/activitytype/list");

            var r = await HttpClient.GetAsync(uri);
            if(r.IsSuccessStatusCode)
            {
                var stream = await r.Content.ReadAsStreamAsync();
                var activityTypes = await JsonSerializer.DeserializeAsync<IEnumerator<ActivityTypeDataTransfer>>(stream);
                return activityTypes;
            }

            return null;
        }

        public async Task<ActivityTypeDataTransfer> PutSingleAsync(ActivityTypeDataTransfer activityType)
        {
            Uri uri = new Uri("/v1/api/activitytype");

            MemoryStream memoryStream = new MemoryStream();

            await JsonSerializer.SerializeAsync(memoryStream, activityType);
            
            var r = await HttpClient.PostAsync(uri, new ByteArrayContent(memoryStream.GetBuffer()));
            if(r.IsSuccessStatusCode)
            {
                var stream = await r.Content.ReadAsStreamAsync();
                var activityTypeResult = await JsonSerializer.DeserializeAsync<ActivityTypeDataTransfer>(stream);
                return activityTypeResult;
            }

            return null;
        }
    }
}