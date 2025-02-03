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
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServices2ReturnGetSummaryData = Activator.CreateInstance<Employee>();
            mockIDataServices2ReturnGetSummaryData.Id = 1712;
            mockIDataServices2ReturnGetSummaryData.Name = "qvg19Z9l5EGIki2651HL, welcome!";
            mockIDataServices2ReturnGetSummaryData.Description = "2gsl1RRMB6klK2vAoLVq, welcome!";
            Employee expected_results = mockIDataServices2ReturnGetSummaryData;
            mockIDataServices2.Setup(x => x.GetSummaryData(It.IsAny<Int32>(), It.IsAny<String>())).Returns(() => { return mockIDataServices2ReturnGetSummaryData; });
            var ObjmockIDataServices2_dataService = mockIDataServices2.Object;
            var Objmockstring_test = "TestString";

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessDataSummaryData(ObjmockIDataServices2_dataService, Objmockstring_test);


            // Assert
            Assert.AreEqual(expected_results, result);
        }


    }
}

