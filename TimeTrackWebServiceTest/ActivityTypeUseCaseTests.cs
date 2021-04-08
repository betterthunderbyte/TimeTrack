using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTrack.Core;
using TimeTrack.Db;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.Tools.V1;
using TimeTrack.Web.Service.UseCase.V1;

namespace TimeTrackWebServiceTest
{
    public class ActivityTypeUseCaseTests
    {
        [Test]
        public async Task CreateActivityType()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1", 
                    Description = "Tätigkeitbeschreibung"
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Item.Id);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit1", r1.Item.Title);
            }
        }
        
        [Test]
        public async Task CreateActivityTypeDouble()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1", 
                    Description = "Tätigkeitbeschreibung"
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Item.Id);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit1", r1.Item.Title);
                
                var r2 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1", 
                    Description = "Tätigkeitbeschreibung"
                });
                Assert.Null(r2.Item);
                Assert.Null(r2.Items);
                Assert.NotNull(r2.MessageOutput);
                Assert.False(r2.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r2.ResultType);
            }
        }

        [Test]
        public async Task CreateActivityTypeWithId()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Id = -20, 
                    Title = "Tätigkeit2",
                    Description = "Tätigkeitbeschreibung"
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Item.Id);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit2", r1.Item.Title);
            }
        }

        [Test]
        public async Task CreateActivityTypeEmptyTitleAndDescription()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "", 
                    Description = ""
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }

        [Test]
        public async Task CreateActivityTypeWithLongTitle()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la"
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }

        [Test]
        public async Task CreateActivityTypeWhitespaceTitle1()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "      "
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }
        
        [Test]
        public async Task CreateActivityTypeWhitespaceTitle2()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = " Tätigkeit3 "
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Item.Id);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit3", r1.Item.Title);
            }
        }

        [Test]
        public async Task CreateActivityTypeWithLongDescription()
        {
            using (var context = new TimeTrackDbContext())
            {
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit8",
                    Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea taki"
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }

        [Test]
        public async Task CreateActivityTypeWithEmptyTitleAndDescription()
        {
            using (var context = new TimeTrackDbContext())
            {
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                var r1 = await useCase.CreateSingleAsync(new ActivityTypeEntity());
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }

        [Test]
        public async Task QueryActionTypeAll()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.GetAllAsync();
                Assert.Null(r1.Item);
                Assert.NotNull(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.Positive(r1.Items.Count());
                Assert.AreEqual(3, r1.Items.Count());
            }
        }
        
        [Test]
        public async Task QueryActionTypeWithId()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.GetSingleAsync(2);
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit1", r1.Item.Title);
            }
        }
        
        [Test]
        public async Task QueryActionTypeWrongId()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.GetSingleAsync(-1);
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
            }
        }
        
        [Test]
        public async Task UpdateActivityTypeWithoutDescription()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.UpdateSingleAsync(2, new ActivityTypeEntity()
                {
                    Title = "Tätigkeit3"
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit3", r1.Item.Title);
                Assert.AreEqual(null, r1.Item.Description);
            }
        }
        
        [Test]
        public async Task UpdateActivityTypeTitleAlreadyExists()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r1.ResultType);
            }
        }

        [Test]
        public async Task UpdateActivityTypeEmptyTitle()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity() { Title = "         "});
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                
                Assert.AreEqual("Tätigkeit2", r1.Item.Title);
            }
        }

        [Test]
        public async Task UpdateActivityTypeWithLongTitle()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity()
                {
                    Title = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la"
                });
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.BadRequest, r1.ResultType);
            }
        }

        public async Task UpdateActivityTypesTitleWithWhitespace()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);

                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity()
                {
                    Title = " Tätigkeit5 "
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(3, r1.Item.Id);
                Assert.AreEqual("Tätigkeit5", r1.Item.Title);
                
            }
        }
        
        [Test, Order(3)]
        public async Task UpdateActivityTypesWithWrongId()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });


                {
                    var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity()
                    {
                        Id = 22, Title = "Tätigkeit7"
                    });
                    Assert.NotNull(r1.Item);
                    Assert.Null(r1.Items);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.AreEqual(3, r1.Item.Id);
                    Assert.AreEqual("Tätigkeit7", r1.Item.Title);
                }

            }
        }

        [Test(Description = "Prüft ob ein leerer Titel sowie eine fehlende Beschreibung zu Fehlern führt.")]
        public async Task UpdateActivityTypesWithMissingFields1()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1", 
                    Description = "Tätigkeit1 Beschreibung"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2", 
                    Description = "Tätigkeit2 Beschreibung"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity()
                {
                    Title = ""
                });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(3, r1.Item.Id);
                Assert.AreEqual("Tätigkeit2", r1.Item.Title);
                Assert.AreEqual("Tätigkeit2 Beschreibung", r1.Item.Description);
            }
        }
        
        
        [Test(Description = "Prüft ob keine Daten zu Fehlern führt.")]
        public async Task UpdateActivityTypesWithMissingFields2()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1", 
                    Description = "Tätigkeit1 Beschreibung"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2", 
                    Description = "Tätigkeit2 Beschreibung"
                });
                
                var r1 = await useCase.UpdateSingleAsync(3, new ActivityTypeEntity() { });
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(3, r1.Item.Id);
                Assert.AreEqual("Tätigkeit2", r1.Item.Title);
                Assert.AreEqual("Tätigkeit2 Beschreibung", r1.Item.Description);
            }
        }

        [Test]
        public async Task DeleteActivityTypeSingle()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit1"
                });
                await useCase.CreateSingleAsync(new ActivityTypeEntity()
                {
                    Title = "Tätigkeit2"
                });
                
                var r1 = await useCase.DeleteSingleAsync(2);
                Assert.NotNull(r1.Item);
                Assert.Null(r1.Items);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                Assert.AreEqual(2, r1.Item.Id);
                Assert.AreEqual("Tätigkeit1", r1.Item.Title);

            }
        }

        [Test]
        public async Task DeleteActivityTypeWrongId()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r1 = await useCase.DeleteSingleAsync(5000);
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
            }
        }

        [Test(Description = "Versucht alle zu löschen, was fehlschlagen muss.")]
        public async Task DeleteAllActivityTypes()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new ActivityTypeUseCase(context);
                
                var r4 = await useCase.DeleteSingleAsync(1);
                Assert.Null(r4.Item);
                Assert.Null(r4.Items);
                Assert.NotNull(r4.MessageOutput);
                Assert.False(r4.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r4.ResultType);
            }
        }
    }
}