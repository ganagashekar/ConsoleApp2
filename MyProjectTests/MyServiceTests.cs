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
            Employee dataServiceReturnGetData = Activator.CreateInstance<Employee>();
            dataServiceReturnGetData.Id = 7511;
            dataServiceReturnGetData.Name = "eeHPrX7YyAL2HrSO6WLp, welcome!";
            dataServiceReturnGetData.Description = "YWNb4xDiH7St8BGuLHBD, welcome!";
            Employee expectedProcessData = dataServiceReturnGetData;
            Mock<IDataService> mockdataService = new Mock<IDataService>();
            mockdataService.Setup(x => x.GetData()).Returns(() => { return dataServiceReturnGetData; });
            var dataService = mockdataService.Object;


            // Act
            var result = new MyService(dataService).ProcessData(dataService);


            // Assert
            Assert.AreEqual(expectedProcessData, result);
        }
    }
}