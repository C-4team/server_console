using server_console.dataset;
using Service.dataSetService;
using Service.serverService;


class Program{
    static void Main(string[] args){
        Console.WriteLine("Server Start");
        DataBase db = DataSetService.DB;
        ServerService serverService = new ServerService();
    }
}