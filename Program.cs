using Service.chatservice;


class Program{
    static void Main(string[] args){
        ChatService chatService = new ChatService();
        chatService.acceptRequestDeamon();
    }
}