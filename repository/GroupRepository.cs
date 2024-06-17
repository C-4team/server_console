using System.Data;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Model.group;
using Model.user;
using Repository.userRepository;
using server_console.dataset;
using Service.dataSetService;

namespace Repository.groupRepository{
    public class GroupRepository : RepositoryInterface<long, Group>
    {
        private DataBase db;
        private static int autogid = 0;
        private UserRepository userRepository;
        public GroupRepository(){
            db = DataSetService.DB;
            userRepository = new UserRepository();
        }
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
        public Group GetGroupByName(string groupName){
            var qurry = 
                from g in db.Tables["Group"].AsEnumerable()
                join t in db.Tables["User_in_Group"].AsEnumerable()
                on (long)g["gid"] equals (long)t["gid"]
                where (string)g["name"] == groupName
                select t;
            Group group = null;
            foreach(var dt in qurry){
                User usr = userRepository.Get((long)dt["uid"])!;
                if(group == null){
                    group = new Group((long)dt["gid"],(string)dt["name"], new List<User>());
                }
                group.AddUser(usr);
            }
            return group;

        }
        public Group Get(long id){
            var qurry = 
                from g in db.Tables["Group"].AsEnumerable()
                join t in db.Tables["User_in_Group"].AsEnumerable()
                on (long)g["gid"] equals (long)t["gid"]
                where (long)g["gid"] == id
                select new List<DataRow>{g, t};

            Group group = null;
            
            foreach(var dt in qurry){
                User usr = userRepository.Get((long)dt[1]["uid"])!;
                if(group == null){
                    group = new Group((long)dt[0]["gid"],(string)dt[0]["name"], new List<User>());
                }
                if(!group.Users.Any(u => u.Id == usr.Id))
                    group.AddUser(usr);
            }
            return group;
        }

        public void Insert(Group item)
        {
            DataTable groups = db.Tables["Group"]!;
            DataRow dataRow = groups.NewRow();
            dataRow["name"] = item.GroupName;
            groups.Rows.Add(dataRow);
            groups.AcceptChanges();
            
            DataTable group_in_user = db.Tables["User_in_Group"];
            DataRow usrRow = null;
            foreach(var usr in item.Users){
                usrRow = group_in_user.NewRow();
                usrRow["gid"] = autogid++;
                usrRow["uid"] = usr.Id;
                group_in_user.Rows.Add(usrRow);
                group_in_user.AcceptChanges();
            }
         
        }

        public List<Group> GetGroupsByUser(User user){
            DataTable group_in_user = db.Tables["User_in_Group"]!;
            var qurry = 
                from groups in group_in_user.AsEnumerable()
                where (long)groups["uid"] == user.Id
                select groups;
            Dictionary<long, Group> avaliableGroups = new Dictionary<long, Group>();
            int count = 0;
            foreach(var g in qurry){
                long groupId = (long)g["gid"];
                long userId = (long)g["uid"];
                if(avaliableGroups.ContainsKey(groupId)){
                    User groupUser = userRepository.Get(userId);
                    // 중복 사용자 체크 후 추가
                    if (!avaliableGroups[groupId].Users.Any(u => u.Id == userId))
                        avaliableGroups[groupId].AddUser(groupUser);   
                }
                else{
                    if(count >= 3){
                        continue;
                    }
                    else{
                        count++;
                        avaliableGroups.Add((long)g["gid"],this.Get((long)g["gid"]));
                    }
                }
                
            }
            return avaliableGroups.Values.ToList();
        }

        public void InviteUser(Group group, User user){
            DataTable group_in_user = db.Tables["User_in_Group"]!;
            DataRow dataRow = group_in_user.NewRow();
            dataRow["gid"] = group.GroupId;
            dataRow["uid"] = user.Id;
            group_in_user.Rows.Add(dataRow);
            group_in_user.AcceptChanges();
            
        }
        public void Update(long id, Group item)
        {
            throw new NotImplementedException();
        }
    }
}

        //const string fileName = "../csv/group.csv";
        // public GroupRepository(){
            
        // }


        // public Group Get(long id){
        //     throw new NotImplementedException();

        // }

        // public void Delete(long id)
        // {
        //     throw new NotImplementedException();
        // }


        // public void Insert(Group item)
        // {
        //     throw new NotImplementedException();
        // }

        // public void Update(long id, Group item)
        // {
        //     throw new NotImplementedException();
        // }