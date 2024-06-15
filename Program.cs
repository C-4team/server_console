using System.ComponentModel.DataAnnotations.Schema;
using server_console.dataset;
using Service.chatservice;
using Service.dataSetService;

class Program{
    static void Main(string[] args){
        Console.WriteLine("Server Start");
        
        ChatService chatService = new ChatService();
    }
}