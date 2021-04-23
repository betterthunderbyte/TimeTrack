using System;
using System.Threading.Tasks;
using TimeTrack.Core.Model;
using TimeTrack.UseCase;
using Xunit;

namespace TimeTrack.Web.Service.UnitTest
{
    public class ActivityUseCaseTest
    {
        [Fact]
        public async Task TestActivityCreate()
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
                    Begin = new DateTime(2020, 1, 1, 12, 0, 33),
                    Duration = new TimeSpan(0, 2, 0, 22)
                });

                var r = await activityUseCase.GetSingleAsync(1);
                
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 12, 0, 0, new TimeSpan(1, 0, 0)), r.Value.Begin);
                Assert.Equal(new TimeSpan(2, 0, 0), r.Value.Duration);
            }
        }

        [Fact]
        public async Task TestActivityOverwriteTimeBegin()
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
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 13, 0, 0, new TimeSpan(1, 0, 0)), r.Value.Begin);
                Assert.Equal(new TimeSpan(1, 0, 0), r.Value.Duration);
            }
        }
     
        [Fact]
        public async Task TestActivityOverwriteTimeCenter()
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
                    Duration = new TimeSpan(0, 8, 0, 0)
                });

                await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    OwnerFk = 1,
                    ProjectFk = 1,
                    Begin = new DateTime(2020, 1, 1, 13, 0, 0),
                    Duration = new TimeSpan(0, 6, 0, 0)
                });


                var r1 = await activityUseCase.GetSingleAsync(1);
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 12, 0, 0, new TimeSpan(1, 0, 0)), r1.Value.Begin);
                Assert.Equal(new TimeSpan(1, 0, 0), r1.Value.Duration);
                
                var r2 = await activityUseCase.GetSingleAsync(3);
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 13, 0, 0, new TimeSpan(1, 0, 0)), r2.Value.Begin);
                Assert.Equal(new TimeSpan(6, 0, 0), r2.Value.Duration);
                
                var r3 = await activityUseCase.GetSingleAsync(2);
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 19, 0, 0, new TimeSpan(1, 0, 0)), r3.Value.Begin);
                Assert.Equal(new TimeSpan(1, 0, 0), r3.Value.Duration);
            }
        }
        
        [Fact]
        public async Task TestActivityOverwriteTimeEnd()
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
                    Duration = new TimeSpan(0, 8, 0, 0)
                });

                await activityUseCase.CreateSingleAsync(new ActivityEntity()
                {
                    ActivityTypeFk = 1,
                    CustomerFk = 1,
                    OwnerFk = 1,
                    ProjectFk = 1,
                    Begin = new DateTime(2020, 1, 1, 18, 0, 0),
                    Duration = new TimeSpan(0, 4, 0, 0)
                });


                var r1 = await activityUseCase.GetSingleAsync(1);
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 12, 0, 0, new TimeSpan(1, 0, 0)), r1.Value.Begin);
                Assert.Equal(new TimeSpan(6, 0, 0), r1.Value.Duration);

                var r3 = await activityUseCase.GetSingleAsync(2);
                Assert.Equal(new DateTimeOffset(2020, 1, 1, 18, 0, 0, new TimeSpan(1, 0, 0)), r3.Value.Begin);
                Assert.Equal(new TimeSpan(4, 0, 0), r3.Value.Duration);
            }
        }
    }
}