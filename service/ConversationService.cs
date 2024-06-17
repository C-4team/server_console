using System.Collections.Generic;
using System.Linq;
using Model.user;
using Service.userService;
using Service.groupService;
using System.Net.Sockets;
using server_console.dataset;
using Service.dataSetService;
using System.Data;
using Model.group;
using Repository.groupRepository;
using Repository.userRepository;

namespace Service.conversationservice
{
    public class ConversationService
    {
        private UserService userService;
        private GroupService groupService;
        private DataBase dataBase;
        private GroupRepository groupRepository;
        private UserRepository userRepository;

        public ConversationService()
        {
            userService = new UserService();
            groupService = new GroupService();
            groupRepository = new GroupRepository();
            userRepository = new UserRepository();
            dataBase = DataSetService.DB;
        }

        // 사용자의 친구목록, 그룹 정보 문자열로 반환
        public string GetConversationData(User user){
            List<Group> groups = groupRepository.GetGroupsByUser(user);
            List<User> friends = userService.GetFriends(user.Id);
            string result = "5,";
            result += groups.Count.ToString();
            result += ",";
            foreach(var g in groups){
                result += g.GroupId;
                result += ",";
                result += g.GroupName;
                result += ",";
                result += g.Users.Count.ToString();
                result += ",";
                foreach(var u in g.Users){
                    result += u.Username;
                    result += ","; 
                }
            }
            result += friends.Count;
            result += ",";
            foreach(var f in friends){
                result += f.Id;
                result += ",";
                result += f.Username;
                result += ",";
            }
            return result;
        }
        private DataTable GetGroupMembers(long gid)
        {
            var userInGroups = dataBase.User_in_Group;
            var query = from row in userInGroups.AsEnumerable()
                        join user in dataBase.User.AsEnumerable()
                        on row.Field<long>("uid") equals user.Field<long>("uid")
                        where row.Field<long>("gid") == gid
                        select new { Username = user.Field<string>("name")};

            DataTable membersTable = new DataTable();
            membersTable.Columns.Add("name", typeof(string));
            foreach (var member in query)
            {
                membersTable.Rows.Add(member.Username);
            }
            return membersTable;
        }

        // 그룹 메시지 정보 가져오기
        private DataTable GetGroupMessages(long gid) {
            var messages = dataBase.Message;
            var query = from row in messages.AsEnumerable()
                        where row.Field<long>("gid") == gid
                        orderby row.Field<DateTime>("datetime")
                        select row;

            if (query.Any()) return query.CopyToDataTable();
            else return new DataTable();
        }
    }
}