using Model.group;
using Model.user;
using Repository.groupRepository;
using Repository.userRepository;
using Service.messageService;

namespace Service.groupService{
    public class GroupService{
        private Dictionary<Group,List<User>> connectedUsersInGroup;
        private GroupRepository groupRepository;
        private UserRepository userRepository;

        private MessageService messageService;
        

        public GroupService(){
            connectedUsersInGroup = new Dictionary<Group, List<User>>();
            groupRepository = new GroupRepository();   
            userRepository = new UserRepository();
            messageService = new MessageService();
        }

        public string EnterGroup(User user ,string[] splitedInfo){
            messageService.EnterNewUser(long.Parse(splitedInfo[1]),user);
            return messageService.ShowBeforeMessages(user,splitedInfo);
        }
        
        public string CreateGroup(User user ,string[] splitedInfo){

            
            List<User> users =  [user];
            Group group = new Group(splitedInfo[1],users);
            groupRepository.Insert(group);
            return "9";

        }
        public string InviteUserInGroup(string[] splitedInfo){
           
            User invitedUser = userRepository.Get(long.Parse(splitedInfo[2]));
            Group group = groupRepository.Get(long.Parse(splitedInfo[1]));
            groupRepository.InviteUser(group,invitedUser);
            return "10";
        }
       
    }
}