using System.IO;

namespace MyProject  // Example namespace
{
    public interface IDataService
    {
        Employee GetData();
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MyService
    {
        private readonly IDataService _dataService;

        public MyService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public Employee ProcessData(IDataService dataService)
        {
            return _dataService.GetData();
        }

        public void LogData(IDataService dataService)
        {
            var data = dataService.GetData();
            // ... logging logic
        }
    }




}