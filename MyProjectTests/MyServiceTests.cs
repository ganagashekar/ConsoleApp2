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
        public void SumTest()
        {
            // Arrange
            Mock<IDataService> mockdataService = new Mock<IDataService>();
            Mock<IDataServices2> mockdataService2 = new Mock<IDataServices2>();
            var a = 10;
            var b = 10;

            var myserviceInstance = new MyService(mockdataService.Object, mockdataService2.Object);



            // Act
            var result = myserviceInstance.Sum(a, b);


            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod()]
        public void ProcessDataTest()
        {
            // Arrange
            Mock<IDataService> mockdataService = new Mock<IDataService>();
            Mock<IDataServices2> mockdataService2 = new Mock<IDataServices2>();
            Employee mockdataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockdataServiceReturnGetData.Id = 5999;
            mockdataServiceReturnGetData.Name = "vrvnf3OBA1PwOZhY2vDK, welcome!";
            mockdataServiceReturnGetData.Description = "QNvCU8tQpxsYxO68oVrS, welcome!";
            Employee expected_results = mockdataServiceReturnGetData;
            mockdataService.Setup(x => x.GetData(It.IsAny<Int32>())).Returns(() => { return mockdataServiceReturnGetData; });
            var dataService = mockdataService.Object;

            var myserviceInstance = new MyService(mockdataService.Object, mockdataService2.Object);



            // Act
            var result = myserviceInstance.ProcessData(dataService);


            // Assert
            Assert.AreEqual(expected_results, result);
        }

        [TestMethod()]
        public void ProcessDataSummaryDataTest()
        {
            // Arrange
            Mock<IDataService> mockdataService = new Mock<IDataService>();
            Mock<IDataServices2> mockdataService2 = new Mock<IDataServices2>();
            Employee mockdataServiceReturnGetSummaryData = Activator.CreateInstance<Employee>();
            mockdataServiceReturnGetSummaryData.Id = 2443;
            mockdataServiceReturnGetSummaryData.Name = "ud38Jyxuf8sIsSPxyd0w, welcome!";
            mockdataServiceReturnGetSummaryData.Description = "GuTrwNDAP0z0CvplJrRd, welcome!";
            Employee expected_results = mockdataServiceReturnGetSummaryData;
            mockdataService2.Setup(x => x.GetSummaryData(It.IsAny<Int32>())).Returns(() => { return mockdataServiceReturnGetSummaryData; });
            var dataService = mockdataService.Object;

            var myserviceInstance = new MyService(mockdataService.Object, mockdataService2.Object);



            // Act
            var result = myserviceInstance.ProcessDataSummaryData(mockdataService2.Object);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
    }
}

