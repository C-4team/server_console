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

namespace Service.chatservice
{
    public class ChatService
    {
        private UserRepository userRepository;
        //private GroupService groupRepository;
        private TcpListener listener;
        
        private Dictionary<Group,List<User>> chatGroups;


        public ChatService(UserRepository userRepository) 
        {
            this.userRepository = userRepository;
            this.chatGroups = new Dictionary<Group, List<User>>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);

            listener.Start();
        }
    
    }
}
