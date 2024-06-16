using server_console.dataset;
using Service.chatservice;
using Service.dataSetService;


class Program{
    static void Main(string[] args){
        Console.WriteLine("Server Start");
        DataBase db = DataSetService.DB;
        ChatService chatService = new ChatService();
    }
}