using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Tests
{
    [TestClass()]
    public class MyServiceTests
    {
        [TestMethod()]
        public void ProcessDataTest()
        {
            // Arrange
            //Employee dataServiceReturnGetData = Activator.CreateInstance<Employee>();
            //dataServiceReturnGetData.Id = 7511;
            //dataServiceReturnGetData.Name = "eeHPrX7YyAL2HrSO6WLp, welcome!";
            //dataServiceReturnGetData.Description = "YWNb4xDiH7St8BGuLHBD, welcome!";
            //Employee expectedProcessData = dataServiceReturnGetData;
            //Mock<IDataService> mockdataService = new Mock<IDataService>();
            //mockdataService.Setup(x => x.GetData()).Returns(() => { return dataServiceReturnGetData; });
            //var dataService = mockdataService.Object;


            //// Act
            //var result = new MyService(dataService).ProcessData(dataService);


            //// Assert
            //Assert.AreEqual(expectedProcessData, result);



            // Arrange
            //var mockdataService = new Mock<IDataService>();
            //Employee dataServiceReturnGetData = Activator.CreateInstance<Employee>();
            //dataServiceReturnGetData.Id = 3790;
            //dataServiceReturnGetData.Name = "QKZsTPY81Y0liiXFKabW, welcome!";
            //dataServiceReturnGetData.Description = "nZxZ4mVYyEM9vhZ9xf8K, welcome!";
            //Employee expectedProcessData = dataServiceReturnGetData;

            //mockdataService.Setup(x => x.GetData()).Returns(() => { return dataServiceReturnGetData; });
            //IDataService dataService = mockdataService.Object;

            //var myserviceInstance = new MyService(dataService);



            //// Act
            //var result = myserviceInstance.ProcessData(dataService);


            //// Assert
            //Assert.AreEqual(expectedProcessData, result);


            // Arrange
            Mock<IDataService> mockdataService = new Mock<IDataService>();
            Mock<IDataServices2> mockdataService2 = new Mock<IDataServices2>();
            Employee mockdataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockdataServiceReturnGetData.Id = 5674;
            mockdataServiceReturnGetData.Name = "ebconfVZxzQUiexq7LSO, welcome!";
            mockdataServiceReturnGetData.Description = "Y07SfNHUUzHzJqaK9W19, welcome!";
            Employee expected_results = mockdataServiceReturnGetData;
            mockdataService.Setup(x => x.GetData(It.IsAny<Int32>())).Returns(() => { return mockdataServiceReturnGetData; });
            var dataService = mockdataService.Object;

            var myserviceInstance = new MyService(mockdataService.Object, mockdataService2.Object);



            // Act
            var result = myserviceInstance.ProcessData(dataService);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
    }
}