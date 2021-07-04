using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TimeTrack.Client;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;

using TimeTrack.UseCase;
using Xunit;

namespace TimeTrack.Web.Service.IntegrationTest
{
    public class ProjectIntegrationTest
    {

        private static HttpClient Prepare()
        {
            var server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment("Development")
                    .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
                    .UseStartup<Startup>()
            );

            var db = server.Services.GetService(typeof(TimeTrackTimeTrackDbContext)) as TimeTrackTimeTrackDbContext;
            
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Setup();
            db.CreateDefaultData();
            
            var c = server.CreateClient();
            return c;
        }
        
        [Fact]
        public async Task GetAllProjects()
        {
            var c = Prepare();
            var client = new ProjectClient(c);
            
            var r = await client.GetAllAsync();
            
            r.Response.EnsureSuccessStatusCode();
            Assert.NotEmpty(r.Result);
            Assert.Equal(HttpStatusCode.OK, r.Response.StatusCode);
        }
        
        [Fact]
        public async Task CreateProjectWithSimpleName()
        {
            var c = Prepare();
            var client = new ProjectClient(c);

            var r = await client.PutSingleAsync(new ProjectDataTransfer() {Name = "Projekt1"});
            
            Assert.Equal(HttpStatusCode.OK, r.Response.StatusCode);
        }
        
        [Fact]
        public async Task CreateProjectWithNameIsNull()
        {
            var c = Prepare();
            var client = new ProjectClient(c);
            
            var r = await client.PutSingleAsync(new ProjectDataTransfer() {Name = null});

            Assert.Equal(HttpStatusCode.BadRequest, r.Response.StatusCode);
        }
        
        [Fact]
        public async Task CreateProjectWithEmptyName()
        {
            var c = Prepare();
            var client = new ProjectClient(c);
            
            var r = await client.PutSingleAsync(new ProjectDataTransfer() {Name = ""});

            Assert.Equal(HttpStatusCode.BadRequest, r.Response.StatusCode);
        }
        
        [Fact]
        public async Task CreateProjectWithWhitespaces()
        {
            var c = Prepare();
            var client = new ProjectClient(c);
            
            var r = await client.PutSingleAsync(new ProjectDataTransfer() {Name = "      "});

            Assert.Equal(HttpStatusCode.BadRequest, r.Response.StatusCode);
        }
    }
}