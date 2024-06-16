using System.Data;
using System.Net.Http.Headers;
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
        private UserRepository userRepository;
        public GroupRepository(){
            db = DataSetService.DB;
            userRepository = new UserRepository();
        }
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
        public Group Get(long id){
            var qurry = 
                from g in db.Tables["Group"].AsEnumerable()
                join t in db.Tables["User_in_Group"].AsEnumerable()
                on (long)g["gid"] equals (long)t["gid"]
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

        public void Insert(Group item)
        {
            DataTable groups = db.Tables["Group"]!;
            DataRow dataRow = groups.NewRow();
            dataRow["name"] = item.GroupName;
            groups.Rows.Add(dataRow);
         
        }

        public void InviteUser(Group group, User user){
            DataTable group_in_user = db.Tables["Group_in_User"]!;
            DataRow dataRow = group_in_user.NewRow();
            dataRow["gid"] = group.GroupId;
            dataRow["uid"] = user.Id;
            group_in_user.Rows.Add(dataRow);
            
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