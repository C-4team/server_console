using Model.group;
using Model.message;
using Model.user;
using Repository.groupRepository;
using Repository.messageRepository;
using Repository.userRepository;

namespace Service.messageService{
    public class MessageService{
        private MessageRepository messageRepository;
        private GroupRepository groupRepository;
        private UserRepository userRepository;
        private static Dictionary<long,List<User>> avaliableUserInGroup = new Dictionary<long, List<User>>();

        
        public MessageService(){
            this.messageRepository = new MessageRepository();
            this.groupRepository = new GroupRepository();
            this.userRepository = new UserRepository();

        }
    
        public void EnterNewUser(long gid, User user){
            Group group = groupRepository.Get(gid);
            if(!avaliableUserInGroup.ContainsKey(group.GroupId)){
                avaliableUserInGroup.Add(gid, [user]);
                Console.WriteLine("(" +gid + ")"+ "내부 사람 ");
                foreach(var usr in avaliableUserInGroup[gid]){
                    Console.WriteLine(usr.ToLoginString());
                }
                Console.WriteLine("끝");
            }
            else{
                avaliableUserInGroup[gid].Add(user);
            }
        }
        public string ShowBeforeMessages(User user, string[] splitedInfo){
            List<Message> messages = messageRepository.Get(long.Parse(splitedInfo[1]));
            string result = "8,";
            result += messages.Count.ToString();
            foreach(var msg in messages){
                result += ",";
                User usr = userRepository.Get(msg.Uid);
                result += user.Username + "," + msg.Msg + "," + msg.DateTime.ToString();
            }

            return result;
        }

        // 7, 채팅을 보내고자 하는 그룹 아이디, 메세지, 보낸 시간
        // 11, 채팅이 온 그룹, 채팅을 작성한 유저 이름, 메세지, 보낸 시간 
        public string SendMessageToGroup(User user, string[] splitedInfo){
            Group group = groupRepository.Get(long.Parse(splitedInfo[1]));
            messageRepository.Insert(new Message(long.Parse(splitedInfo[1]),user.Id,splitedInfo[2],DateTime.Parse(splitedInfo[3])));
            string sendToAllUser = "11," + group.GroupId.ToString() + "," + user.Username + "," + splitedInfo[2] + "," + splitedInfo[3];
            foreach(var usr in avaliableUserInGroup[long.Parse(splitedInfo[1])]){
                usr.Writer.WriteLine(sendToAllUser);
            }
            return "";
            
        }

    }
}