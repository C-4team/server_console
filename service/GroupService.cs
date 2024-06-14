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

        public Group GetGroup(string groupInfo){
            string[] info = groupInfo.Split(',');
            long gid;
            if(long.TryParse(info[0], out gid)){
                return groupRepository.Get(gid);
            }
            else{
                return null;
            }
        }
        public Task JoinGroup(long gid, User user){
            
            return Task.CompletedTask;
        }
        
        
       
    }
}