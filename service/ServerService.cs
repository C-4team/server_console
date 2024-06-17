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
            UserService userService = new UserService();

            while (true){
                TcpClient newClient = listener.AcceptTcpClient();
                NetworkStream newClientStream = newClient.GetStream();
                StreamWriter newClientWriter = new StreamWriter(newClientStream,Encoding.UTF8){AutoFlush = true};
                StreamReader newClientStreamReader = new StreamReader(newClientStream,Encoding.UTF8);
                string userInfo = newClientStreamReader.ReadLine()!;
                Console.WriteLine(userInfo);

                User newUser = User.parseUser(userInfo!);
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                Console.WriteLine(newUser.ToString());

                string[] requestData = userInfo.Split(',');
                int requestType = int.Parse(requestData[0]);
                string response = string.Empty;

                switch (requestType) {
                    case 0 :  // 회원가입
                        if (requestData.Length == 4)
                        {
                            newUser.Id = long.Parse(requestData[1]);
                            newUser.Username = requestData[2];
                            newUser.Password = requestData[3];

                            response = userService.Register(newUser);
                        }
                        else {
                            response = "형식에 맞지 않는 요청입니다.";
                        }
                        break;
                    case 1:  // 로그인
                        if (requestData.Length == 3) {
                            long id = long.Parse(requestData[1]);
                            string password = requestData[2];
                            response = userService.Login(id, password);
                        }
                        else {
                            response = "Login 형식과 불일치하는 request입니다.";
                        }
                        break;
                    default :
                        response = "잘못된 request type입니다.";
                        break;
                }

                newClientWriter.WriteLine(response);

                if (requestType == 1 && response == "3")
                    _ = RequestController(newUser);
                /*
                newClientWriter.AutoFlush = true;
                Console.WriteLine(userInfo);
                User newUser = User.parseUser(userInfo!);
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                Console.WriteLine(newUser.ToString());
                */
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

    