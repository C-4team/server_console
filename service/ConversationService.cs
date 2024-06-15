using System.Collections.Generic;
using System.Linq;
using Model.user;
using Service.userService;
using Service.groupService;
using System.Net.Sockets;

namespace Service.conversationservice
{
    public class ConversationService
    {
        private UserService userService;
        private GroupService groupService;

        public ConversationService()
        {
            userService = new UserService();
            groupService = new GroupService();
        }

/*   6/16(일)부터 다시 이어서 하기 ----------------------------------------
        // 친구 목록 및 대화 창 요청
        public string RequestFriendAndConversations(long id)
        {
            var user = userService.GetUser(id);
            if (user == null) return 4;  // 친구 목록 or 대화창 없는 경우

            // 친구 목록 가져오기
            var friends = userService.GetFriends(id);
            var friendsList = "";
            if (friends != null && friends.Any())
            {
                var friendsStringList = new List<string>();
                foreach (var friend in friends)
                {
                    friendsStringList.Add($"{friend.Id},{friend.Username}");
                }
                friendsList = string.Join(",", friendsStringList);
            }

            // 그룹 정보 가져오기
            var groups = groupService.GetGroup(id);
        }
        */
    }
}