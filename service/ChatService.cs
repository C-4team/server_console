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
        
        
        private Dictionary<Group,List<User>> chatGroups;


        public ChatService() 
        {
            this.chatGroups = new Dictionary<Group, List<User>>();
            
        }
    }
}