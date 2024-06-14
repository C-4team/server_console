using Service.chatservice;


class Program{
    static void Main(string[] args){
        Console.WriteLine("Server Start");
        ChatService chatService = new ChatService();
        chatService.acceptRequestDeamon();
    }
}