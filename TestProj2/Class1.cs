using System.IO;

namespace TestProj2  // Example namespace
{
    public interface IDataService
    {
        string GetData();
    }

    public class MyService
    {
        private readonly IDataService _dataService;

        public MyService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public string ProcessData()
        {
            return _dataService.GetData().ToUpper();
        }

        public void LogData(IDataService dataService)
        {
            string data = dataService.GetData();
            // ... logging logic
        }
    }
}