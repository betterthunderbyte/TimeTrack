using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TimeTrack.Core;
using TimeTrack.Db;
using TimeTrack.Models.V1;
using TimeTrack.Web.Service.UseCase.V1;

namespace TimeTrackWebServiceTest
{
    public class CustomerUseCaseTests
    {
        [Test]
        public async Task CreateCustomerWithDoubleName()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new CustomerUseCase(context);
                
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                var r1 = await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                Assert.Null(r1.Item);
                Assert.Null(r1.Items);
                Assert.NotNull(r1.MessageOutput);
                Assert.False(r1.Successful);
                Assert.AreEqual(UseCaseResultType.Conflict, r1.ResultType);
            }
        }
        
        [Test, Order(1)]
        public async Task CreateCustomersTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new CustomerUseCase(context);
                {
                    var r1 = await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                    Assert.NotNull(r1.Item);
                    Assert.Null(r1.Items);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.Positive(r1.Item.Id);
                    Assert.AreEqual(2, r1.Item.Id);
                    Assert.AreEqual("Kunde1", r1.Item.Name);
                }

                {
                    var r3 = await useCase.CreateSingleAsync(new CustomerEntity() {Id = -20, Name = "Kunde2"});
                    Assert.NotNull(r3.Item);
                    Assert.Null(r3.Items);
                    Assert.Null(r3.MessageOutput);
                    Assert.True(r3.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r3.ResultType);
                    Assert.Positive(r3.Item.Id);
                    Assert.AreEqual(3, r3.Item.Id);
                }

                {
                    var r4 = await useCase.CreateSingleAsync(new CustomerEntity() { Name = "" });
                    Assert.Null(r4.Item);
                    Assert.Null(r4.Items);
                    Assert.NotNull(r4.MessageOutput);
                    Assert.False(r4.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r4.ResultType);
                }

                {
                    var r5 = await useCase.CreateSingleAsync(new CustomerEntity() { Name = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la" });
                    Assert.Null(r5.Item);
                    Assert.Null(r5.Items);
                    Assert.NotNull(r5.MessageOutput);
                    Assert.False(r5.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r5.ResultType);
                }

                {
                    var r6 = await useCase.CreateSingleAsync(new CustomerEntity() { Name = "      " });
                    Assert.Null(r6.Item);
                    Assert.Null(r6.Items);
                    Assert.NotNull(r6.MessageOutput);
                    Assert.False(r6.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r6.ResultType);
                }

                {
                    var r7 = await useCase.CreateSingleAsync(new CustomerEntity() { Name = " Kunde3 " });
                    Assert.NotNull(r7.Item);
                    Assert.Null(r7.Items);
                    Assert.Null(r7.MessageOutput);
                    Assert.True(r7.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r7.ResultType);
                    Assert.Positive(r7.Item.Id);
                    Assert.AreEqual(4, r7.Item.Id);
                    Assert.AreEqual("Kunde3", r7.Item.Name);
                }

            }
        }
        
        [Test, Order(2)]
        public async Task QueryCustomersTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new CustomerUseCase(context);

                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde2"});

                {
                    var r1 = await useCase.GetAllAsync();
                    Assert.Null(r1.Item);
                    Assert.NotNull(r1.Items);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.Positive(r1.Items.Count());
                    Assert.AreEqual(3, r1.Items.Count());
                }

                {
                    var r2 = await useCase.GetSingleAsync(2);
                    Assert.NotNull(r2.Item);
                    Assert.Null(r2.Items);
                    Assert.Null(r2.MessageOutput);
                    Assert.True(r2.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r2.ResultType);
                    Assert.AreEqual(2, r2.Item.Id);
                    Assert.AreEqual("Kunde1", r2.Item.Name); 
                }

                {
                    var r3 = await useCase.GetSingleAsync(-1);
                    Assert.Null(r3.Item);
                    Assert.Null(r3.Items);
                    Assert.NotNull(r3.MessageOutput);
                    Assert.False(r3.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r3.ResultType);
                }
            }
        }
        
        [Test, Order(3)]
        public async Task UpdateCustomersTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new CustomerUseCase(context);
                
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde2"});

                {
                    var r1 = await useCase.UpdateSingleAsync(2, new CustomerEntity() { Name = "Kunde3"});
                    Assert.NotNull(r1.Item);
                    Assert.Null(r1.Items);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.AreEqual(2, r1.Item.Id);
                    Assert.AreEqual("Kunde3", r1.Item.Name);
                }

                {
                    var r2 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Name = "Kunde2"});
                    Assert.Null(r2.Item);
                    Assert.Null(r2.Items);
                    Assert.NotNull(r2.MessageOutput);
                    Assert.False(r2.Successful);
                    Assert.AreEqual(UseCaseResultType.Conflict, r2.ResultType);
                }

                {
                    var r3 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Name = ""});
                    Assert.Null(r3.Item);
                    Assert.Null(r3.Items);
                    Assert.NotNull(r3.MessageOutput);
                    Assert.False(r3.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r3.ResultType);
                }

                {
                    var r4 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Name = "         "});
                    Assert.Null(r4.Item);
                    Assert.Null(r4.Items);
                    Assert.NotNull(r4.MessageOutput);
                    Assert.False(r4.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r4.ResultType);
                }

                {
                    var r5 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Name = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut la"});
                    Assert.Null(r5.Item);
                    Assert.Null(r5.Items);
                    Assert.NotNull(r5.MessageOutput);
                    Assert.False(r5.Successful);
                    Assert.AreEqual(UseCaseResultType.BadRequest, r5.ResultType);
                }

                {
                    var r6 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Name = " Kunde5 "});
                    Assert.NotNull(r6.Item);
                    Assert.Null(r6.Items);
                    Assert.Null(r6.MessageOutput);
                    Assert.True(r6.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r6.ResultType);
                    Assert.AreEqual(3, r6.Item.Id);
                    Assert.AreEqual("Kunde5", r6.Item.Name);
                }

                {
                    var r7 = await useCase.UpdateSingleAsync(3, new CustomerEntity() { Id = 22, Name = "Kunde7"});
                    Assert.NotNull(r7.Item);
                    Assert.Null(r7.Items);
                    Assert.Null(r7.MessageOutput);
                    Assert.True(r7.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r7.ResultType);
                    Assert.AreEqual(3, r7.Item.Id);
                    Assert.AreEqual("Kunde7", r7.Item.Name);
                }
            }
        }
        
        [Test, Order(4)]
        public async Task DeleteCustomersTest()
        {
            using (var context = new TimeTrackDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                context.Setup(true);
                var useCase = new CustomerUseCase(context);
                
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde1"});
                await useCase.CreateSingleAsync(new CustomerEntity() {Name = "Kunde2"});
                
                {
                    var r1 = await useCase.DeleteSingleAsync(2);
                    Assert.NotNull(r1.Item);
                    Assert.Null(r1.Items);
                    Assert.Null(r1.MessageOutput);
                    Assert.True(r1.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r1.ResultType);
                    Assert.AreEqual(2, r1.Item.Id);
                    Assert.AreEqual("Kunde1", r1.Item.Name);
                }
                
                {
                    var r2 = await useCase.DeleteSingleAsync(5000);
                    Assert.Null(r2.Item);
                    Assert.Null(r2.Items);
                    Assert.NotNull(r2.MessageOutput);
                    Assert.False(r2.Successful);
                    Assert.AreEqual(UseCaseResultType.NotFound, r2.ResultType);
                }

                {
                    var r3 = await useCase.DeleteSingleAsync(3);
                    Assert.NotNull(r3.Item);
                    Assert.Null(r3.Items);
                    Assert.Null(r3.MessageOutput);
                    Assert.True(r3.Successful);
                    Assert.AreEqual(UseCaseResultType.Ok, r3.ResultType);
                    Assert.AreEqual(3, r3.Item.Id);
                    Assert.AreEqual("Kunde2", r3.Item.Name);
                }

                {
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
}