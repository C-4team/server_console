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
                return "4";  // 그룹이 없는 경우  -- 이거 물어보기
            
            var groupStrings = userGroups.AsEnumerable().Take(3).Select(row =>
            {
                var gid = row.Field<long>("gid");
                var members = GetGroupMembers(gid).AsEnumerable().Take(4)
                    .Select(memberRow => memberRow.Field<string>("Username"));
                return $"{gid},{GetGroupMembers(gid).Rows.Count},{string.Join(",", members)}";
            });
            
            return $"5,{userGroups.Rows.Count},{string.Join(",", groupStrings)}";
        }

        // 그룹 정보 가져오기
        private DataTable GetGroupById(long id)
        {
            var userInGroups = dataBase.User_in_Group;
            var query = from row in userInGroups.AsEnumerable()
                        where row.Field<long>("Uid") == id
                        select row;

            if (query.Any()) return query.CopyToDataTable();
            else return new DataTable();
        }

        // 그룹 멤버 정보 가져오기
        private DataTable GetGroupMembers(long gid)
        {
            var userInGroups = dataBase.User_in_Group;
            var query = from row in userInGroups.AsEnumerable()
                        where row.Field<long>("Gid") == gid
                        select row;
            if (query.Any()) return query.CopyToDataTable();
            else return new DataTable();
        }
    }
}