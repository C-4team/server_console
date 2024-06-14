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
        private UserRepository userRepository;
        private GroupService groupService;
        private TcpListener listener;
        
        private Dictionary<Group,List<User>> chatGroups;


        public ChatService() 
        {
            this.userRepository = new UserRepository();
            this.chatGroups = new Dictionary<Group, List<User>>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
            listener.Start();
        }
        public void acceptRequestDeamon(){

            while (true){
                TcpClient newClient = listener.AcceptTcpClient();
                NetworkStream newClientStream = newClient.GetStream();
                StreamWriter newClientWriter = new StreamWriter(newClientStream);
                StreamReader newClientStreamReader = new StreamReader(newClientStream);
                string userInfo = newClientStreamReader.ReadToEnd();
                Console.WriteLine(userInfo);
                User newUser = User.parseUser(userInfo);
                newUser.TCPclient = newClient;
                newUser.NetStream = newClientStream;
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;
                Console.WriteLine(newUser.ToString());
                _ = ReadDataAsync(newUser);
            }
        }

        private async Task ReadAndSendMessageAsync(Group group, User user){
            while(chatGroups[group].Count > 0 && user.TCPclient.Connected ){
                string msg =  user.Reader.ReadLine();
                if(msg == null){
                    break;
                }
                foreach(User avaliableUser in chatGroups[group]){
                    avaliableUser.Writer.WriteLine(msg);
                }
            }
        }
        async Task ReadDataAsync(User user){
            List<User> tmp;
            while (true)
            {
                string groupInfo = user.Reader.ReadLine();
                if(groupInfo != null){
                    Group group = groupService.GetGroup(groupInfo);
                    if(group == null){
                        continue;
                    }
                    if (chatGroups.TryGetValue(group,out tmp)){
                        chatGroups[group].Add(user);
                        return;
                    }
                    
                    else{
                        chatGroups[group] = new List<User> { user };
                        Thread newGroupControl = new Thread(() => {
                            ReadAndSendMessageAsync(group,user);
                        });
                        return;
                    }
                }
            }
            
        }
    }
}
