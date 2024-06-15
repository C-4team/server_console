using Model.group;
using Model.user;
using Repository.groupRepository;

namespace Service.groupService{
    public class GroupService{
        private Dictionary<Group,List<User>> connectedUsersInGroup;
        private GroupRepository groupRepository;

        public GroupService(){
            connectedUsersInGroup = new Dictionary<Group, List<User>>();
            groupRepository = new GroupRepository();   
        }

        
        public Task JoinGroup(string info){
            
            return Task.CompletedTask;
        }
        
        public Task CreateGroup(string info){

            string[] splitedInfo = info.Split(",");
            
            

            return Task.CompletedTask;

        }
        public Task InviteUserInGroup(string info){

            return Task.CompletedTask;
        }
       
    }
}