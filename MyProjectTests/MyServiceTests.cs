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
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockIDataServiceReturnGetData.Id = 9610;
            mockIDataServiceReturnGetData.Name = "L4hzHPbvdm8Kj2JfBD5y, welcome!";
            mockIDataServiceReturnGetData.Description = "UiJhe3bfCFiURD1eIFNe, welcome!";
            Employee expected_results = mockIDataServiceReturnGetData;
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>())).Returns(() => { return mockIDataServiceReturnGetData; });
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessData(ObjmockIDataService_dataService);


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
            mockIDataServices2ReturnGetSummaryData.Id = 7226;
            mockIDataServices2ReturnGetSummaryData.Name = "1rQ8KnSrZynZXtaOXgvh, welcome!";
            mockIDataServices2ReturnGetSummaryData.Description = "8hIwtruyoLnsHDSv0zHm, welcome!";
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
        [TestMethod()]
        public void LogDataTest()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>()));
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            myserviceInstance.LogData(ObjmockIDataService_dataService);


            // Assert

        }
        [TestMethod()]
        public void ProcessData1Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockIDataServiceReturnGetData.Id = 9416;
            mockIDataServiceReturnGetData.Name = "xhfkZzpzBG5GuEbjqKMT, welcome!";
            mockIDataServiceReturnGetData.Description = "wzOC9JVZn7TPUht8AXmD, welcome!";
            Employee expected_results = mockIDataServiceReturnGetData;
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>())).Returns(() => { return mockIDataServiceReturnGetData; });
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessData1(ObjmockIDataService_dataService);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void ProcessDataSummaryData1Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServices2ReturnGetSummaryData = Activator.CreateInstance<Employee>();
            mockIDataServices2ReturnGetSummaryData.Id = 6452;
            mockIDataServices2ReturnGetSummaryData.Name = "Wd5716Tn62e81fAKxqi0, welcome!";
            mockIDataServices2ReturnGetSummaryData.Description = "rhNU4MzMSf2bZgrYn5RR, welcome!";
            Employee expected_results = mockIDataServices2ReturnGetSummaryData;
            mockIDataServices2.Setup(x => x.GetSummaryData(It.IsAny<Int32>(), It.IsAny<String>())).Returns(() => { return mockIDataServices2ReturnGetSummaryData; });
            var ObjmockIDataServices2_dataService = mockIDataServices2.Object;
            var Objmockstring_test = "TestString";

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessDataSummaryData1(ObjmockIDataServices2_dataService, Objmockstring_test);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void LogData1Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>()));
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            myserviceInstance.LogData1(ObjmockIDataService_dataService);


            // Assert

        }
        [TestMethod()]
        public void ProcessData2Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockIDataServiceReturnGetData.Id = 8851;
            mockIDataServiceReturnGetData.Name = "Nz1gYSLHfE2NGtg0uwnz, welcome!";
            mockIDataServiceReturnGetData.Description = "zwq8FBRhRsEnZkSkt3Pf, welcome!";
            Employee expected_results = mockIDataServiceReturnGetData;
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>())).Returns(() => { return mockIDataServiceReturnGetData; });
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessData2(ObjmockIDataService_dataService);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void ProcessDataSummaryData2Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServices2ReturnGetSummaryData = Activator.CreateInstance<Employee>();
            mockIDataServices2ReturnGetSummaryData.Id = 1974;
            mockIDataServices2ReturnGetSummaryData.Name = "a2nAXi4er4AACGzA92oX, welcome!";
            mockIDataServices2ReturnGetSummaryData.Description = "6QWYQzxyvorpemFqDNie, welcome!";
            Employee expected_results = mockIDataServices2ReturnGetSummaryData;
            mockIDataServices2.Setup(x => x.GetSummaryData(It.IsAny<Int32>(), It.IsAny<String>())).Returns(() => { return mockIDataServices2ReturnGetSummaryData; });
            var ObjmockIDataServices2_dataService = mockIDataServices2.Object;
            var Objmockstring_test = "TestString";

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessDataSummaryData2(ObjmockIDataServices2_dataService, Objmockstring_test);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void ProcessData3Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServiceReturnGetData = Activator.CreateInstance<Employee>();
            mockIDataServiceReturnGetData.Id = 6473;
            mockIDataServiceReturnGetData.Name = "9ECknMrsMbbaezW8gJo4, welcome!";
            mockIDataServiceReturnGetData.Description = "3KWFkNciNV1me8FOeLkj, welcome!";
            Employee expected_results = mockIDataServiceReturnGetData;
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>())).Returns(() => { return mockIDataServiceReturnGetData; });
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessData3(ObjmockIDataService_dataService);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void ProcessDataSummaryData3Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            Employee mockIDataServices2ReturnGetSummaryData = Activator.CreateInstance<Employee>();
            mockIDataServices2ReturnGetSummaryData.Id = 8703;
            mockIDataServices2ReturnGetSummaryData.Name = "pryRn5AOdsN0WsCtzae4, welcome!";
            mockIDataServices2ReturnGetSummaryData.Description = "FulO83nePJHYKHbydGSB, welcome!";
            Employee expected_results = mockIDataServices2ReturnGetSummaryData;
            mockIDataServices2.Setup(x => x.GetSummaryData(It.IsAny<Int32>(), It.IsAny<String>())).Returns(() => { return mockIDataServices2ReturnGetSummaryData; });
            var ObjmockIDataServices2_dataService = mockIDataServices2.Object;
            var Objmockstring_test = "TestString";

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            var result = myserviceInstance.ProcessDataSummaryData3(ObjmockIDataServices2_dataService, Objmockstring_test);


            // Assert
            Assert.AreEqual(expected_results, result);
        }
        [TestMethod()]
        public void LogData3Test()
        {
            // Arrange
            Mock<IDataService> mockIDataService = new Mock<IDataService>();
            Mock<IDataServices2> mockIDataServices2 = new Mock<IDataServices2>();
            mockIDataService.Setup(x => x.GetData(It.IsAny<Int32>(), It.IsAny<Employee>()));
            var ObjmockIDataService_dataService = mockIDataService.Object;

            var myserviceInstance = new MyService(mockIDataService.Object, mockIDataServices2.Object);



            // Act
            myserviceInstance.LogData3(ObjmockIDataService_dataService);


            // Assert

        }

    }
}


