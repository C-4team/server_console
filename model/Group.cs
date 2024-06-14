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
        private string groupedName;
        private int activeBit;
        private List<User> users;

        public Group(long groupId, List<User> users)
        {
            this.groupId = groupId;
            this.users = users;
            this.activeBit = 1;

        }
        public long getGroupId()
        {
            return groupId;
        }
        public int getActiveBit(){
            return activeBit;
        }

        public List<User> getUsers() 
        {
            return users;
        }
        public static long parseGroupId(string info)
        {
            string[] metadata = info.Split(',');
            return long.Parse(metadata[0]);
        }
        //public static Group of(string info)
        

        
    }
}
