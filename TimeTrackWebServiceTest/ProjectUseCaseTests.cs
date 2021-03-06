using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TimeTrack.Core;
using TimeTrack.Core.Model;
using TimeTrack.Db;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrackWebServiceTest
{
    public class ProjectUseCaseTests
    {
        [Test]
        public async Task CreateProjectSucessfull()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);
                
                
                var r1 = await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt1"});
                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Value.Id);
                Assert.AreEqual(2, r1.Value.Id);
            }
        }
        
        [Test, Order(1)]
        public async Task CreateProjectsTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);

                var r2 = await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt1"});
                Assert.Null(r2.Value);
                Assert.Null(r2.Values);
                Assert.NotNull(r2.MessageOutput);
                Assert.False(r2.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r2.ResultType);
            
                var r3 = await useCase.CreateSingleAsync(new ProjectEntity() {Id = -20, Name = "Projekt2"});
                Assert.NotNull(r3.Value);
                Assert.Null(r3.Values);
                Assert.Null(r3.MessageOutput);
                Assert.True(r3.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r3.ResultType);
                Assert.Positive(r3.Value.Id);
                Assert.AreEqual(3, r3.Value.Id);
            
                var r4 = await useCase.CreateSingleAsync(new ProjectEntity() { Name = "" });
                Assert.Null(r4.Value);
                Assert.Null(r4.Values);
                Assert.NotNull(r4.MessageOutput);
                Assert.False(r4.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r4.ResultType);
            
                var r5 = await useCase.CreateSingleAsync(new ProjectEntity()
                {
                    Name = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la"
                });
                Assert.Null(r5.Value);
                Assert.Null(r5.Values);
                Assert.NotNull(r5.MessageOutput);
                Assert.False(r5.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r5.ResultType);
                
                var r6 = await useCase.CreateSingleAsync(new ProjectEntity() { Name = "      " });
                Assert.Null(r6.Value);
                Assert.Null(r6.Values);
                Assert.NotNull(r6.MessageOutput);
                Assert.False(r6.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r6.ResultType);
                
                var r7 = await useCase.CreateSingleAsync(new ProjectEntity() { Name = " Projekt3 " });
                Assert.NotNull(r7.Value);
                Assert.Null(r7.Values);
                Assert.Null(r7.MessageOutput);
                Assert.True(r7.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r7.ResultType);
                Assert.Positive(r7.Value.Id);
                Assert.AreEqual(4, r7.Value.Id);
                Assert.AreEqual("Projekt3", r7.Value.Name);
            }
        }
        
        [Test, Order(2)]
        public async Task QueryProjectsTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);

                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt1"});
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt2"});
                
                var r1 = await useCase.GetAllAsync();
                Assert.Null(r1.Value);
                Assert.NotNull(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Values.Count());
                Assert.AreEqual(3, r1.Values.Count());

                var r2 = await useCase.GetSingleAsync(2);
                Assert.NotNull(r2.Value);
                Assert.Null(r2.Values);
                Assert.Null(r2.MessageOutput);
                Assert.True(r2.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r2.ResultType);
                Assert.AreEqual(2, r2.Value.Id);
                Assert.AreEqual("Projekt1", r2.Value.Name);
                
                var r3 = await useCase.GetSingleAsync(-1);
                Assert.Null(r3.Value);
                Assert.Null(r3.Values);
                Assert.NotNull(r3.MessageOutput);
                Assert.False(r3.Successful);
                Assert.AreEqual(UseCaseResultType.NotFound, r3.ResultType);

            }
        }

        [Test]
        public async Task UpdateProjectWithDoubleName()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt2"});
                
                var r2 = await useCase.UpdateSingleAsync(2, new ProjectEntity() { Name = "Projekt2" });
                Assert.Null(r2.Value);
                Assert.Null(r2.Values);
                Assert.NotNull(r2.MessageOutput);
                Assert.False(r2.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r2.ResultType);
            }
        }
        
        [Test, Order(3)]
        public async Task UpdateProjectsTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);
                
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt1"});
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt2"});

                var r1 = await useCase.UpdateSingleAsync(2, new ProjectEntity() { Name = "Project3"});
                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(2, r1.Value.Id);
                Assert.AreEqual("Project3", r1.Value.Name);
                
                var r3 = await useCase.UpdateSingleAsync(3, new ProjectEntity() { Name = ""});
                Assert.Null(r3.Value);
                Assert.Null(r3.Values);
                Assert.NotNull(r3.MessageOutput);
                Assert.False(r3.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r3.ResultType);
                
                var r4 = await useCase.UpdateSingleAsync(3, new ProjectEntity() { Name = "         "});
                Assert.Null(r4.Value);
                Assert.Null(r4.Values);
                Assert.NotNull(r4.MessageOutput);
                Assert.False(r4.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r4.ResultType);
                
                var r5 = await useCase.UpdateSingleAsync(3, new ProjectEntity() { Name = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la"});
                Assert.Null(r5.Value);
                Assert.Null(r5.Values);
                Assert.NotNull(r5.MessageOutput);
                Assert.False(r5.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r5.ResultType);
                
                var r6 = await useCase.UpdateSingleAsync(3, new ProjectEntity() { Name = " Project5 "});
                Assert.NotNull(r6.Value);
                Assert.Null(r6.Values);
                Assert.Null(r6.MessageOutput);
                Assert.True(r6.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r6.ResultType);
                Assert.AreEqual(3, r6.Value.Id);
                Assert.AreEqual("Project5", r6.Value.Name);
                
                var r7 = await useCase.UpdateSingleAsync(3, new ProjectEntity() { Id = 22, Name = "Project7"});
                Assert.NotNull(r7.Value);
                Assert.Null(r7.Values);
                Assert.Null(r7.MessageOutput);
                Assert.True(r7.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r7.ResultType);
                Assert.AreEqual(3, r7.Value.Id);
                Assert.AreEqual("Project7", r7.Value.Name);
            }
        }
        
        [Test, Order(4)]
        public async Task DeleteProjectsTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);
                
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt1"});
                await useCase.CreateSingleAsync(new ProjectEntity() {Name = "Projekt2"});
                
                var r1 = await useCase.DeleteSingleAsync(2);
                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(2, r1.Value.Id);
                Assert.AreEqual("Projekt1", r1.Value.Name);
                
                var r2 = await useCase.DeleteSingleAsync(5000);
                Assert.Null(r2.Value);
                Assert.Null(r2.Values);
                Assert.NotNull(r2.MessageOutput);
                Assert.False(r2.Successful);
                Assert.AreEqual(UseCaseResultType.NotFound, r2.ResultType);

                var r3 = await useCase.DeleteSingleAsync(3);
                Assert.NotNull(r3.Value);
                Assert.Null(r3.Values);
                Assert.Null(r3.MessageOutput);
                Assert.True(r3.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r3.ResultType);
                Assert.AreEqual(3, r3.Value.Id);
                Assert.AreEqual("Projekt2", r3.Value.Name);

            }
        }

        [Test]
        public async Task DeleteLastProject()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ProjectUseCase(context);
                
                var r1 = await useCase.DeleteSingleAsync(1);
                Assert.Null(r1.Value);
                Assert.Null(r1.Values);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r1.ResultType);
            }
        }

    }
}