using Model.group;
using Model.user;
using Repository.groupRepository;
using Repository.userRepository;

namespace Service.groupService{
    public class GroupService{
        private Dictionary<Group,List<User>> connectedUsersInGroup;
        private GroupRepository groupRepository;
        private UserRepository userRepository;

        public GroupService(){
            connectedUsersInGroup = new Dictionary<Group, List<User>>();
            groupRepository = new GroupRepository();   
            userRepository = new UserRepository();
        }

        
        public Task CreateGroup(User user ,string info){

            string[] splitedInfo = info.Split(",");
            List<User> users =  [user];
            Group group = new Group(splitedInfo[1],users);
            groupRepository.Insert(group);
            return Task.CompletedTask;

        }
        public Task InviteUserInGroup(string info){
            string[] splitedInfo = info.Split(",");
            User invitedUser = userRepository.Get(long.Parse(splitedInfo[2]));
            Group group = groupRepository.Get(long.Parse(splitedInfo[1]));
            groupRepository.InviteUser(group,invitedUser);
            return Task.CompletedTask;
        }
       
    }
}