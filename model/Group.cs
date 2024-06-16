using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.user;

namespace Model.group
{
    public class Group
    {
        private long groupId;
        private string groupName;
        private List<User> users;

        public Group(long groupId, string groupName , List<User> users)
        {
            this.groupId = groupId;
            this.users = users;

        }
        public Group(string groupName,List<User> users){
            this.groupName = groupName;
            this.users = users;
        }
        public string GroupName{
            get{return groupName;}
        }
        public long GroupId{
            get{return groupId;}
        }
        public void AddUser(User user){
            users.Add(user);
        }
        public List<User> Users {
            get{
                return users;
            }
        }
        public static long parseGroupId(string info)
        {
            string[] metadata = info.Split(',');
            return long.Parse(metadata[0]);
        }
        //public static Group of(string info)
        

        
    }
}
