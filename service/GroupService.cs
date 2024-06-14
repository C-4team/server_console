using Model.group;
using Repository.groupRepository;

namespace Service.groupService{
    public class GroupService{
        GroupRepository groupRepository;

        public GroupService(){
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

    }
}