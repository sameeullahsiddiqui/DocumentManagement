//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using DocumentManagement.API.Controllers;
//using DocumentManagement.API.ViewModels;
//using DocumentManagement.Core.Models;
//using DocumentManagement.Infrastructure.Interfaces;

//using Moq;
//using System.Linq;
//using System.Web.Http.Results;

//namespace DocumentManagement.Tests
//{
//    [TestClass]
//    public class RackControllerTest
//    {
//        private Mock<IRackRepository> _memberRepositoryMock;
//        private Mock<IUnitOfWork> _iUnitOfWorkMock;
//        private RacksController objController;
//        private List<Rack> testRacks;
//        private RackPagingViewModel pagingmodel;

//        [TestInitialize]
//        public void Initialize()
//        {
//            _memberRepositoryMock = new Mock<IRackRepository>();
//            _iUnitOfWorkMock = new Mock<IUnitOfWork>();

//            objController = new RacksController(_iUnitOfWorkMock.Object, _memberRepositoryMock.Object);
//            testRacks = new List<Rack>() {

//                        new Rack() { Id = Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D1E"), Name = "Samee" },
//                        new Rack() { Id = Guid.Parse("2DFCF83C-B675-4C2C-BB60-0E9AD20E6DC6"), Name = "Rashmi" },
//                        new Rack() { Id = Guid.Parse("4E72E22C-3369-4B28-AED8-4102177B4346"), Name = "Shweta" }
//                    };

//            pagingmodel = new RackPagingViewModel { pageNumber = 1, pageSize = 20};
//        }



//        [TestMethod]
//        public async Task GetAllRacks_ShouldReturnAllRacks()
//        {
//            //Arrange
//            _memberRepositoryMock.Setup(x => x.GetAll()).Returns(testRacks.AsQueryable());

//            //Act
//            var result = await objController.GetRacksAsync(pagingmodel) as OkNegotiatedContentResult<List<Rack>>;

//            //Assert
//            Assert.AreEqual(result.Content.Count, 3);
//            Assert.AreEqual("Rashmi", result.Content[0].Name);
//            Assert.AreEqual("Samee", result.Content[1].Name);
//            Assert.AreEqual("Shweta", result.Content[2].Name);

//        }

//        [TestMethod]
//        public async Task GetAllRacks_ShouldSearchReturnCorrentRacks()
//        {
//            //Arrange
//            _memberRepositoryMock.Setup(x => x.GetAll()).Returns(testRacks.AsQueryable());
//            pagingmodel.Name = "Samee";

//            //Act
//            var result = await objController.GetRacksAsync(pagingmodel) as OkNegotiatedContentResult<List<Rack>>;

//            //Assert
//            Assert.AreEqual(result.Content.Count, 1);
//            Assert.AreEqual("Samee", result.Content[0].Name);
//        }

//        [TestMethod]
//        public async Task GetRack_ShouldReturnCorrectRack()
//        {
//            //Arrange
//            _memberRepositoryMock.Setup(x => x.GetByIdAsync(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D1E"))).Returns(Task.FromResult(testRacks.Where(x=>x.Id == Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D1E")).FirstOrDefault<Rack>()));

//            //Act
//            var result = await objController.GetRackAsync(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D1E")) as OkNegotiatedContentResult<Rack>;

//            //Assert
//            Assert.IsNotNull(result.Content);
//            Assert.AreEqual("Samee", result.Content.Name);
//        }

//        [TestMethod]
//        public async Task GetRack_ShouldNotFindRack()
//        {

//            //Arrange
//            //_memberRepositoryMock.Setup(x => x.GetByIdAsync(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D16"))).Returns((Rack e) =>
//            //{
//            //    e.Id = Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D16");
//            //    return Task.FromResult(e);
//            //});

//            _memberRepositoryMock.Setup(x => x.GetByIdAsync(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D1E"))).Returns(Task.FromResult<Rack>(null));

//            //Act
//            var result = await objController.GetRackAsync(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D16"));

//            //Assert
//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//        }

//        [TestMethod]
//        public async Task Valid_Rack_Create()
//        {
//            //Arrange
//            Rack c = new Rack() { Name = "test1" };
//            _memberRepositoryMock.Setup(m => m.Add(c)).Returns((Rack e) =>
//            {
//                e.Id = Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D16");
//                return e;
//            });

//            //Act
//            var result = await objController.PostRackAsync(c) as OkNegotiatedContentResult<Rack>;

//            //Assert
//            Assert.AreEqual(Guid.Parse("A39C3AB6-46AB-4CE6-810E-01954CC32D16"), c.Id);
//            //_memberRepositoryMock.Verify(m => m.Commit(), Times.Once);

//        }

//        [TestMethod]
//        public async Task Invalid_Rack_Create()
//        {
//            // Arrange
//            Rack c = new Rack() { Name = "" };
//            objController.ModelState.AddModelError("Error", "Something went wrong");

//            //Act
//            var result = await objController.PostRackAsync(c) as OkNegotiatedContentResult<Rack>;

//            //Assert
//            _memberRepositoryMock.Verify(m => m.Add(c), Times.Never);
//            Assert.AreEqual(null, result);
//        }

//    }
//}
