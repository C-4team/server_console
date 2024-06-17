using System.Collections.Generic;
using System.Linq;
using Model.user;
using Service.userService;
using Service.groupService;
using System.Net.Sockets;
using server_console.dataset;
using Service.dataSetService;
using System.Data;

namespace Service.conversationservice
{
    public class ConversationService
    {
        private UserService userService;
        private GroupService groupService;
        private DataBase dataBase;

        public ConversationService()
        {
            userService = new UserService();
            groupService = new GroupService();
            dataBase = DataSetService.DB;
        }

        // 사용자의 친구목록, 그룹 정보 문자열로 반환
        public string GetConversationData(long id)
        {
            var friends = userService.GetFriendsAsString(id);
            var groups = GetGroupAsString(id);

            // 친구 목록이나 대화창 없는 경우
            if (string.IsNullOrEmpty(friends) && string.IsNullOrEmpty(groups)) return "4";

            return $"{groups},{friends}";
        }

        // 그룹 대화 정보 문자열로 가져오기
        private string GetGroupAsString(long id)
        {
            var userGroups = GetGroupById(id);

            if (userGroups == null || userGroups.Rows.Count == 0)
                return "4";  // 그룹이 없는 경우
            
            var groupStrings = userGroups.AsEnumerable().Take(3).Select(row =>
            {
                var gid = row.Field<long>("gid");
                var groupName = dataBase.Group.AsEnumerable()
                    .Where(gr => gr.Field<long>("gid") == gid)
                    .Select(gr => gr.Field<string>("name"))
                    .FirstOrDefault();
                var members = GetGroupMembers(gid).AsEnumerable().Take(4)
                    .Select(memberRow => memberRow.Field<string>("username"));

                var messages = GetGroupMessages(gid).AsEnumerable().Take(4)
                    .Select(messageRow => $"{messageRow.Field<string>("message")}({messageRow.Field<DateTime>("datetime")})");
                
                return $"{gid},{groupName},{GetGroupMembers(gid).Rows.Count},{string.Join(",", members)}|{string.Join(",",messages)}";
            });
            
            return $"5,{userGroups.Rows.Count},{string.Join(",", groupStrings)}";
        }

        // 그룹 정보 가져오기
        private DataTable GetGroupById(long id)
        {
            var userInGroups = dataBase.User_in_Group;
            var query = from row in userInGroups.AsEnumerable()
                        where row.Field<long>("uid") == id
                        select row;

            if (query.Any()) return query.CopyToDataTable();
            else return new DataTable();
        }

        // 그룹 멤버 정보 가져오기
        private DataTable GetGroupMembers(long gid)
        {
            var userInGroups = dataBase.User_in_Group;
            var query = from row in userInGroups.AsEnumerable()
                        join user in dataBase.User.AsEnumerable()
                        on row.Field<long>("uid") equals user.Field<long>("uid")
                        where row.Field<long>("gid") == gid
                        select new { Username = user.Field<string>("username")};

            DataTable membersTable = new DataTable();
            membersTable.Columns.Add("username", typeof(string));
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