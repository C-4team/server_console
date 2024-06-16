using Model.message;
using Model.user;
using Repository.groupRepository;
using Repository.messageRepository;

namespace Service.messageService{
    public class MessageService{
        private MessageRepository messageRepository;
        private GroupRepository groupRepository;
        public MessageService(){
            this.messageRepository = new MessageRepository();
            this.groupRepository = new GroupRepository();
        }
        public string sendBeforeMessages(User user, string msgInfo){
            string[] splitedMsgInfo = msgInfo.Split(',');
            List<Message> messages = messageRepository.Get(long.Parse(splitedMsgInfo[1]));
            

        
            return "";
        }
    }
}