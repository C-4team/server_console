using System.Net;
using System.Net.Sockets;
using System.Text;
using Model.user;
using Service.groupService;
using Service.messageService;
using Service.userService;

namespace Service.serverService{
    public class ServerService{
        private TcpListener listener;
        private GroupService groupService;

        private MessageService messageService;

        private UserService userService;


        public ServerService(){
            this.groupService = new GroupService();
            this.messageService = new MessageService();
            this.userService = new UserService();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
            listener.Start();
            acceptRequestDeamon();

        }
        public void acceptRequestDeamon(){

            while (true){
                TcpClient newClient = listener.AcceptTcpClient();
                NetworkStream newClientStream = newClient.GetStream();
                StreamWriter newClientWriter = new StreamWriter(newClientStream,Encoding.UTF8){AutoFlush = true};
                StreamReader newClientStreamReader = new StreamReader(newClientStream,Encoding.UTF8);
                string userInfo = newClientStreamReader.ReadLine()!;
                
                newClientWriter.AutoFlush = true;
                Console.WriteLine(userInfo);
                User newUser = User.parseUser(userInfo!);
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                Console.WriteLine(newUser.ToString());

                _ = RequestController(newUser);
            }
        }

        // private Task ReadAndSendMessageAsync(Group group, User user)
        // {
        //     while (chatGroups[group].Count > 0 && user.TCPclient.Connected ){
        //         string msg =  user.Reader.ReadLine()!;
        //         if(msg == null){
        //             break;
        //         }
        //         foreach(User avaliableUser in chatGroups[group]){
        //             avaliableUser.Writer.WriteLine(msg);
        //         }
        //     }

        //     return Task.CompletedTask;
        // }
        

        Task RequestController(User user){
            while (user.TCPclient.Connected){
                string request = user.Reader.ReadLine()!;
                string[] splitedRequest = request.Split(',');
                
                _ = RequestMatcher(user, splitedRequest);
                
            }
            return Task.CompletedTask;

        }
        Task RequestMatcher(User user ,string[] splitedRequest){
            int reqType = int.Parse(splitedRequest[0]);
            string result = "";
            switch(reqType){
                
                case 2:
                    
                    break;
                case 3:

                    break;
                case 4:
                    result = groupService.EnterGroup(user,splitedRequest);
                    user.Writer.WriteLine(result);
                    break;
                case 5:
                    result = groupService.CreateGroup(user,splitedRequest);

                    user.Writer.WriteLine(result);

                    break;
                
                case 6:
                    result = groupService.InviteUserInGroup(splitedRequest);
                  
                    user.Writer.WriteLine(result);
                    
                    break;
                
                case 7:
                    messageService.SendMessageToGroup(user, splitedRequest);
                    break;
                case -1:

                    break;
            }
            return Task.CompletedTask;
        }
    }
}

    