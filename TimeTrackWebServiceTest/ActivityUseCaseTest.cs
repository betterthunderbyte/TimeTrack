using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TimeTrack.Core;
using TimeTrack.Core.Model;
using TimeTrack.Db;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrackWebServiceTest
{
    public class ActivityUseCaseTest
    {
        [Test]
        public async Task ReplaceActivitiesTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);

                await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    OwnerFk = 1,
                    ProjectFk = 1,
                    Begin = new DateTime(2020, 1, 1, 12, 0, 0),
                    Duration = new TimeSpan(0, 2, 0, 0)
                });

                await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    OwnerFk = 1,
                    ProjectFk = 1,
                    Begin = new DateTime(2020, 1, 1, 12, 0, 0),
                    Duration = new TimeSpan(0, 1, 0, 0)
                });


                var r = await activityUseCase.GetSingleAsync(1);
                Assert.AreEqual(r.Value.Begin, new DateTime(2020, 1, 1, 13, 0, 0));
            }
        }
        
        [Test, Order(1)]
        public async Task CreateActivitiesTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                
                var activityUseCase = new ActivityUseCase(context);

                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });

                    Assert.NotNull(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                    Assert.Positive(r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                    Assert.AreEqual(1, r1.Value.CustomerFk);
                    Assert.AreEqual(1, r1.Value.ProjectFk);
                    Assert.AreEqual(new DateTime(2020, 1, 1, 6, 0, 0), r1.Value.Begin);
                    Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);
                }

                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 2,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }

                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 2,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 2,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {

                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity());
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity(){
                        Begin = new DateTime(2020, 2, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity(){
                        Begin = new DateTime(2020, 2, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity(){
                        Begin = new DateTime(2020, 2, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        OwnerFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity(){
                        Begin = new DateTime(2020, 2, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1
                    });
                
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
            }
        }

        [Test]
        public async Task CreateActivityWithoutToDateTime()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);
                
                var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    ProjectFk = 1,
                    OwnerFk = 1
                });

                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                Assert.Positive(r1.Value.Id);
                Assert.AreEqual(1, r1.Value.Id);
                Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                Assert.AreEqual(1, r1.Value.CustomerFk);
                Assert.AreEqual(1, r1.Value.ProjectFk);
                Assert.AreEqual(new DateTime(2020, 1, 1, 6, 0, 0), r1.Value.Begin);
                Assert.AreEqual(new TimeSpan(0, 0, 0, 0), r1.Value.Duration);
            }
        }

        [Test]
        public async Task CreateActivityWithFromDateTimeBiggerThanToDateTime()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);
                
                var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    Begin = new DateTime(2020, 2, 1, 6, 0, 0),
                    Duration = new TimeSpan(0, 1, 0, 0),
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    ProjectFk = 1,
                    OwnerFk = 1
                });
                
                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                Assert.Positive(r1.Value.Id);
                Assert.AreEqual(1, r1.Value.Id);
                Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                Assert.AreEqual(1, r1.Value.CustomerFk);
                Assert.AreEqual(1, r1.Value.ProjectFk);
                Assert.AreEqual(new DateTime(2020, 2, 1, 6, 0, 0), r1.Value.Begin);
                Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);
            }
        }

        [Test]
        public async Task CreateActivityWithoutFromDateTime()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);
                
                var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity(){
                    Duration = new TimeSpan(0, 1, 0, 0),
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    ProjectFk = 1,
                    OwnerFk = 1
                });
                
                Assert.NotNull(r1.Value);
                Assert.Null(r1.Values);
                Assert.Null(r1.MessageOutput);
                Assert.True(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                Assert.Positive(r1.Value.Id);
                Assert.AreEqual(1, r1.Value.Id);
                Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                Assert.AreEqual(1, r1.Value.CustomerFk);
                Assert.AreEqual(1, r1.Value.ProjectFk);
                Assert.AreEqual(new DateTime(), r1.Value.Begin);
                Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);

            }
        }
        
        [Test, Order(2)]
        public async Task QueryActivitiesTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);

                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 3, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }


                {
                    var r1 = await activityUseCase.GetAllAsync();
                    
                    Assert.Null(r1.Value);
                    Assert.NotNull(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.AreEqual(3, r1.Values.Count());
                    
                }
                
                
                {
                    var r1 = await activityUseCase.GetSingleAsync(1);
                    
                    Assert.NotNull(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                    Assert.Positive(r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                    Assert.AreEqual(1, r1.Value.CustomerFk);
                    Assert.AreEqual(1, r1.Value.ProjectFk);
                    Assert.AreEqual(new DateTime(2020, 1, 1, 6, 0, 0), r1.Value.Begin);
                    Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);
                }

                {
                    var r1 = await activityUseCase.GetSingleAsync(0);
                    
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
            }
        }
        
        [Test, Order(3)]
        public async Task PatchActivitiesTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 3, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }

                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                    
                    Assert.NotNull(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                    Assert.Positive(r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                    Assert.AreEqual(1, r1.Value.CustomerFk);
                    Assert.AreEqual(1, r1.Value.ProjectFk);
                    Assert.AreEqual(new DateTime(2020, 1, 2, 6, 0, 0), r1.Value.Begin);
                    Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);
                }
                
                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                    
                    Assert.NotNull(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                    Assert.Positive(r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                    Assert.AreEqual(1, r1.Value.CustomerFk);
                    Assert.AreEqual(1, r1.Value.ProjectFk);
                    Assert.AreEqual(new DateTimeOffset(2020, 1, 2, 6, 0, 0, TimeSpan.Zero), r1.Value.Begin);
                    Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);

                }
                
                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 2,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                    
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 2,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                    
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 2,
                        OwnerFk = 1
                    });
                    
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.UpdateSingleAsync(1, new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 2
                    });
                    
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
            }
        }

        [Test, Order(1)]
        public async Task DeleteActivitiesTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);

                var activityUseCase = new ActivityUseCase(context);
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 1, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 2, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }
                
                {
                    var r1 = await activityUseCase.CreateSingleAsync(new ActivityEntity()
                    {
                        Begin = new DateTime(2020, 1, 3, 6, 0, 0),
                        Duration = new TimeSpan(0, 1, 0, 0),
                        ActivityTypeFk = 1,
                        CustomerFk = 1,
                        ProjectFk = 1,
                        OwnerFk = 1
                    });
                }

                {
                    var r1 = await activityUseCase.DeleteSingleAsync(0);
                    Assert.Null(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.NotNull(r1.MessageOutput);
                    Assert.False(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r1.ResultType);
                }
                
                {
                    var r1 = await activityUseCase.DeleteSingleAsync(1);
                    Assert.NotNull(r1.Value);
                    Assert.Null(r1.Values);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    
                    Assert.Positive(r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.Id);
                    Assert.AreEqual(1, r1.Value.ActivityTypeFk);
                    Assert.AreEqual(1, r1.Value.CustomerFk);
                    Assert.AreEqual(1, r1.Value.ProjectFk);
                    Assert.AreEqual(new DateTime(2020, 1, 1, 6, 0, 0), r1.Value.Begin);
                    Assert.AreEqual(new TimeSpan(0, 1, 0, 0), r1.Value.Duration);
                }
            }
        }
    }
}