using System.Runtime.InteropServices;
using Model.user;
using Repository.userRepository;

namespace Service.userService{
    public class UserService{
        UserRepository userRepository;

        public UserService(){
            userRepository = new UserRepository();   
        }
        
        // 회원가입
        public int Register(User newUser) {
            var existUser = userRepository.Get(newUser.Id);
            if (existUser != null && existUser.Valid == 1)
                return 0;  // 기존 유저는 회원가입 X
            newUser.Valid = 1;   // 신규유저 -> 기존유저
            userRepository.Insert(newUser);
            return 1;
        }

        // 로그인
        public int Login(long id, string password)
        {
            var existUser = userRepository.Get(id);
            if (existUser != null && existUser.Valid == 1
                && existUser.Password == password)
                return 3;  // 로그인 성공
            else return 2;  // 로그인 실패
        }

        // 친구추가 (친구 검색)
        public int AddFriend(long id, long friendId)
        {
            var user = userRepository.Get(id);
            var friend = userRepository.Get(friendId);

            if (friend == null || user == null 
                || user.Friends.Contains(friendId)) return 6;  // 존재 X 학번

            user.Friends.Add(friendId);
            userRepository.Update(id, user);
            return 7;  // 존재하는 학번인 경우 친구 추가 성공
        }

        // 친구 목록 검색
        public List<User> GetFriends(long id)
        {
            var user = userRepository.Get(id);
            if (user == null) return null;

            var friends = new List<User>();
            foreach(var friendId in user.Friends)
            {
                var friend = userRepository.Get(friendId);
                if (friend != null) friends.Add(friend);
            }
            return friends;  // 각 친구 마다 ;로 구분
        }

        // 친구 목록을 문자열로 반환
        public string GetFriendsAsString(long id)
        {
            var friends = GetFriends(id);
            if (friends == null) return null;

            var friendsString = friends.Select(friend => $"{friend.Id},{friend.Username}");
            return string.Join(";", friendsString);
        }

        // 유저 정보 반환 (친구 목록 제외)
        // 친구 목록 제외하고 나머지 형식 통일해서 보내기 위해 만듦
        public string GetUser(long id)
        {
            var user= userRepository.Get(id);
            if (user == null) return null;

            return $"{user.Valid},{user.Id},{user.Username},{user.Password}";
        }
    }
}