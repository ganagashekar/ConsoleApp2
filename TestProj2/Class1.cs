using System.IO;

namespace MyProject  // Example namespace
{
    public interface IDataService
    {
        Employee GetData(int summarycount,Employee employee);
    }
    public interface IDataServices2
    {
        Employee GetSummaryData(int summarycount,string teststring );
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
        private readonly IDataServices2 _dataService2;

        public MyService(IDataService dataService, IDataServices2 dataService2)
        {
            _dataService = dataService;
            _dataService2 = dataService2;
        }

        public Employee ProcessData(IDataService dataService)
        {
            return _dataService.GetData(10, new Employee());
        }

        public Employee ProcessDataSummaryData(IDataServices2 dataService,string test)
        {
            return _dataService2.GetSummaryData(10, test);
        }

        public void LogData(IDataService dataService)
        {
            var data = dataService.GetData(20,new Employee());
            // ... logging logic
        }


        public Employee ProcessData1(IDataService dataService)
        {
            return _dataService.GetData(10, new Employee());
        }

        public Employee ProcessDataSummaryData1(IDataServices2 dataService, string test)
        {
            return _dataService2.GetSummaryData(10, test);
        }

        public void LogData1(IDataService dataService)
        {
            var data = dataService.GetData(20, new Employee());
            // ... logging logic
        }




        public Employee ProcessData2(IDataService dataService)
        {
            return _dataService.GetData(10, new Employee());
        }

        public Employee ProcessDataSummaryData2(IDataServices2 dataService, string test)
        {
            return _dataService2.GetSummaryData(10, test);
        }

        //public void LogData(IDataService dataService)
        //{
        //    var data = dataService.GetData(20, new Employee());
        //    // ... logging logic
        //}





        public Employee ProcessData3(IDataService dataService)
        {
            return _dataService.GetData(10, new Employee());
        }

        public Employee ProcessDataSummaryData3(IDataServices2 dataService, string test)
        {
            return _dataService2.GetSummaryData(10, test);
        }

        public void LogData3(IDataService dataService)
        {
            var data = dataService.GetData(20, new Employee());
            // ... logging logic
        }

        //public int Sum(int a, int b)
        //{
        //    return a + b;
        //}
    }




}