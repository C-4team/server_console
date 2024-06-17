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

namespace Service.serverService{
    public class ServerService{
        private TcpListener listener;
        private GroupService groupService;

        private MessageService messageService;

        private UserService userService;

        private GroupRepository groupRepository;

        public ServerService(){
            this.groupService = new GroupService();
            this.messageService = new MessageService();
            this.userService = new UserService();
            this.groupRepository = new GroupRepository();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
            listener.Start();
            acceptRequestDeamon();

        }
        public void dummyDataInitalization(){
            DataBase dataBase = DataSetService.DB;
            DataTable userTable = dataBase.Tables["User"];
            DataTable groupTable = dataBase.Tables["Group"];
            DataTable groupInUser = dataBase.Tables["User_in_Group"];
            User jihoon = new User(2022203078,"강지훈","1234");
            User seayon = new User(2022203019,"최세연","1234");
            userService.Register(jihoon);
            userService.Register(seayon);
            DataRow groupRow = groupTable.NewRow();
            groupRow["name"] = "응소실 팀플";
            groupTable.Rows.Add(groupRow);
            Model.group.Group group = groupRepository.GetGroupByName("응소실 팀플");
            DataRow groupInUserRow = groupInUser.NewRow();
            groupInUserRow["gid"] = group.GroupId;
            groupInUserRow["uid"] = jihoon.Id;
            groupInUser.Rows.Add(groupInUserRow);
            
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
                User newUser = User.parseUser(userInfo!); // -> User Database DataSet 휘발성 
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                Console.WriteLine(newUser.ToString());

                _ = RequestController(newUser);
            }
        }

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

    