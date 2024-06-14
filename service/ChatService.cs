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

                _ = ReadGroupInfoAsync(newUser);
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

        Task ReadGroupInfoAsync(User user){
            List<User> tmp;
            while (true)
            {
                Console.WriteLine("ReadGroup Start" + user.ToString());
                string groupInfo = user.Reader.ReadLine()!;
                if(groupInfo != null){
                    Group group = groupService.GetGroup(groupInfo);
                    if(group == null){
                        continue;
                    }
                    if (chatGroups.TryGetValue(group, out tmp!)){
                        chatGroups[group].Add(user);
                        return Task.CompletedTask;
                    }
                    
                    else{
                        chatGroups[group] = new List<User> { user };
                        Thread newGroupControl = new Thread(async () => {
                            await ReadAndSendMessageAsync(group,user);
                        });
                        return Task.CompletedTask;
                    }
                }
            }
        }
    }
}
