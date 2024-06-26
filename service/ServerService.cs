using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Model.user;
using Repository.groupRepository;
using Repository.userRepository;
using Model.group;
using server_console.dataset;
using Service.dataSetService;
using Service.groupService;
using Service.messageService;
using Service.userService;
using Service.conversationservice;

namespace Service.serverService{
    public class ServerService{
        private TcpListener listener;
        private GroupService groupService;
        private MessageService messageService;
        private UserService userService;

        private  ConversationService conversationService;
        private GroupRepository groupRepository;
        public ServerService(){
            this.groupService = new GroupService();
            this.messageService = new MessageService();
            this.userService = new UserService();
            this.groupRepository = new GroupRepository();
            this.conversationService = new ConversationService();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
            listener.Start();
            dummyDataInitalization();
            acceptRequestDeamon();

        }
        public void dummyDataInitalization(){
            DataBase dataBase = DataSetService.DB;
            DataTable userTable = dataBase.Tables["User"];
            DataTable groupTable = dataBase.Tables["Group"];
            DataTable groupInUser = dataBase.Tables["User_in_Group"];
            User jihoon = new User(2022203078,"강지훈","1234");
            User heanho = new User(2020203080,"김현호","1234");
            User imseayon =  new User(2022203085,"임서연","1234");
            User choiseayon = new User(2022203019,"최세연","1234");
            userService.Register(jihoon);
            userService.Register(choiseayon);
            userService.Register(imseayon);
            userService.Register(heanho);
            groupService.CreateGroup(jihoon,["5","응소실 4조"]);
            groupService.InviteUserInGroup(["6","0","2020203080"]);
            messageService.SendMessageToGroup(heanho,["7","0","오늘 6시 회의~",DateTime.Now.ToString()]);
            messageService.SendMessageToGroup(jihoon,["7","0","밥은 먹었나요 다들",DateTime.Now.ToString()]);
            userService.AddFriend(2022203078, 2022203019);
            
            //userService.AddFriend(2022203078,2022203019);
            Console.WriteLine(dataBase.GetXml());
            
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

                User newUser = null;
                

                string[] requestData = userInfo.Split(',');
                int requestType = int.Parse(requestData[0]);
                string response = string.Empty;

                switch (requestType) {
                    case 0 :  // 회원가입
                        if (requestData.Length == 4)
                        {
                            User tmp = new User(long.Parse(requestData[1]),requestData[2],requestData[3]);
                            
                            
                            newUser = userService.Register(tmp);
                            if(newUser == null){
                                response = "0";
                            }
                            else{
                                response = "1";
                                newUser.TCPclient = newClient;
                                newUser.NetStream = newClientStream;
                                newUser.Writer = newClientWriter;
                                newUser.Reader = newClientStreamReader;
                                Console.WriteLine(newUser.ToString());

                            }
                        }
                        
                        break;
                    case 1:  // 로그인
                        if (requestData.Length == 3) {
                            long id = long.Parse(requestData[1]);
                            string password = requestData[2];
                            newUser = userService.Login(id, password);
                            if(newUser == null){
                                response = "2";
                            }
                            else{
                                response = "3";
                                newUser.TCPclient = newClient;
                                newUser.NetStream = newClientStream;
                                newUser.Writer = newClientWriter;
                                newUser.Reader = newClientStreamReader;
                                response += "," +newUser.Username;
                                Console.WriteLine(newUser.ToString());

                            }
                        }
                        
                        break;
                    
                }

                newClientWriter.WriteLine(response);
                
                if(newUser!= null){
                    Thread ts = new Thread(() => RequestController(newUser));
                    ts.Start();
                    continue;
                }
            
                /*
                newClientWriter.AutoFlush = true;
                Console.WriteLine(userInfo);
                User newUser = User.parseUser(userInfo!); // -> User Database DataSet 휘발성 
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                Console.WriteLine(newUser.ToString());
                */
            }
        }

        void RequestController(User user){
            while (user.TCPclient.Connected){
                string request = user.Reader.ReadLine();
                if(request == "-1"){
                    user.TCPclient.Close();
                    return;
                }
                if(!string.IsNullOrEmpty(request) && !string.IsNullOrWhiteSpace(request)){
                    Console.WriteLine("Requester : " + user.Username);
                    Console.WriteLine("RequestController : " + request);

                    string[] splitedRequest = request.Split(',');
                
                    RequestMatcher(user, splitedRequest);
                    Console.WriteLine(DataSetService.DB.GetXml());

                }
                else{
                    Console.WriteLine("Null");
                }
                
            }

        }
        private void RequestMatcher(User user ,string[] splitedRequest){
            int reqType = int.Parse(splitedRequest[0]);
            Console.WriteLine("reqType : " + reqType);
            string result = "";
            switch(reqType){
                
                case 2:
                    result = conversationService.GetConversationData(user);
                    user.Writer.WriteLine(result);
                    break;
                case 3:
                    long friendId = long.Parse(splitedRequest[1]);
                    result = userService.AddFriend(user.Id, friendId);
                    user.Writer.WriteLine(result);
                    break;
                case 4:
                    result = groupService.EnterGroup(user,splitedRequest);
                    Console.WriteLine(result);
                    user.Writer.WriteLine(result);
                    break;
                case 5:
                    Console.WriteLine("그룹 생성 요청");
                    result = groupService.CreateGroup(user,splitedRequest);

                    user.Writer.WriteLine(result);

                    break;
                
                case 6:
                    result = groupService.InviteUserInGroup(splitedRequest);
                  
                    user.Writer.WriteLine(result);
                    
                    break;
                
                case 7:
                    if(splitedRequest.Length == 2){
                        string nextStrings = user.Reader.ReadLine()!;
                        splitedRequest.Concat(nextStrings.Split(","));
                    }
                    messageService.SendMessageToGroup(user, splitedRequest);
                    break;
                case -1:
                    
                    break;
            }
        }
    }
}

    