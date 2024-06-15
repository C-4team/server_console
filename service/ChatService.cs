using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Model.user;
using Model.group;
using Repository.userRepository;
using Service.groupService;

namespace Service.chatservice
{
    public class ChatService
    {
        private GroupService groupService;
        private TcpListener listener;
        
        private Dictionary<Group,List<User>> chatGroups;


        public ChatService() 
        {
            this.chatGroups = new Dictionary<Group, List<User>>();
            
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

        private Task ReadAndSendMessageAsync(Group group, User user)
        {
            while (chatGroups[group].Count > 0 && user.TCPclient.Connected ){
                string msg =  user.Reader.ReadLine()!;
                if(msg == null){
                    break;
                }
                foreach(User avaliableUser in chatGroups[group]){
                    avaliableUser.Writer.WriteLine(msg);
                }
            }

            return Task.CompletedTask;
        }
        

        Task RequestController(User user){
            while (user.TCPclient.Connected){
                string request = user.Reader.ReadLine()!;
                string[] splitedRequest = request.Split(',');
                
                RequestMatcher(splitedRequest);
                
            }
            return Task.CompletedTask;

        }
        void RequestMatcher(string[] splitedRequest){
            int reqType = int.Parse(splitedRequest[0]);
            switch(reqType){
                
                case 2:
                    
                    break;
                case 3:

                    break;
                case 4:
                
                    break;
                case 5:
                
                    break;
                case 6:

                    break;
                case -1:

                    break;
            }
        }
    }
}
