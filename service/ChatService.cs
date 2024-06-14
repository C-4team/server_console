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


        public ChatService(UserRepository userRepository) 
        {
            this.userRepository = userRepository;
            this.chatGroups = new Dictionary<Group, List<User>>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
            listener.Start();
        }
        public void acceptRequestDeamon(){

            while (true){

                TcpClient newClient = listener.AcceptTcpClient();
                NetworkStream newClientStream = newClient.GetStream();
                StreamWriter newClientWriter = new StreamWriter(newClientStream);
                StreamReader newClientStreamReader = new StreamReader(newClientStream);
                string userInfo = newClientStreamReader.ReadToEnd();
                User newUser = User.parseUser(userInfo);
                
                newUser.Writer = newClientWriter;
                newUser.Reader = newClientStreamReader;

                _ = ReadDataAsync(newUser);
            }
        }
        
        private void InterGroupControllerThread(Group group){

            while (chatGroups[group].Count > 0){
                
            }
        }
        
        async Task ReadDataAsync(User user){
            List<User> tmp;
            while (true)
            {
                string groupInfo = await user.Reader.ReadLineAsync();
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
                            InterGroupControllerThread(group);
                        });
                        return;
                    }
                }
            }
            
        }
    }
}
