using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using Model.user;
using Repository.userRepository;
using Service.dataSetService;

namespace Service.userService{
    public class UserService{
        UserRepository userRepository;

        public UserService(){
            userRepository = new UserRepository();   
        }
        
        // 회원가입
        public string Register(User newUser) {
            var existUser = userRepository.Get(newUser.Id);
            if (existUser != null)
                return "0";  // 기존 유저는 회원가입 X
            userRepository.Insert(newUser);
            return "1";
        }

        // 로그인
        public string Login(long id, string password)
        {
            var existUser = userRepository.Get(id);
            if (existUser != null && existUser.Password == password)
                return "3";  // 로그인 성공
            else return "2";  // 로그인 실패
        }

        // 친구추가 (친구 검색)
        public string AddFriend(long id, long friendId)
        {
            var user = userRepository.Get(id);
            var friend = userRepository.Get(friendId);

            if (friend == null || user == null 
                || user.GetFriends().Contains(friendId)) return "6";  // 존재 X 학번

            var friendsTable = DataSetService.DB.Tables["Friend"];
            DataRow newRow = friendsTable.NewRow();
            newRow["uid"] = id;
            newRow["fid"] = friendId;
            friendsTable.Rows.Add(newRow);
            userRepository.SaveCsv();
            return "7";  // 존재하는 학번인 경우 친구 추가 성공
        }

        // 친구 목록 검색
        public List<User> GetFriends(long id)
        {
            var user = userRepository.Get(id);
            if (user == null) return null;

            var friendIds = user.GetFriends();
            var friends = new List<User>();
            foreach(var friendId in friendIds)
            {
                var friend = userRepository.Get(friendId);
                if (friend != null) friends.Add(friend);
            }
            return friends;  // 각 친구 마다 ;로 구분
        }

        // 친구 목록을 문자열로 반환 - 데이터베이스에서 친구 목록 조회
        public string GetFriendsAsString(long id)
        {
            DataSet DB = DataSetService.DB;
            DataTable friendsTable = DB.Tables["Friend"];
            DataTable usersTable = DB.Tables["User"];

            var friendIds = friendsTable.AsEnumerable()
                                        .Where(row => (long)row["uid"] == id)
                                        .Select(row => (long)row["fid"])
                                        .ToList();

            if (friendIds.Count == 0) return null;

            var friends = usersTable.AsEnumerable()
                                    .Where(row => friendIds.Contains((long)row["uid"]))
                                    .Select(row => $"{row["uid"]},{row["name"]}")
                                    .ToList();
            return string.Join(";", friends);
        }

        // 유저 정보 반환 (친구 목록 제외)
        // 친구 목록 제외하고 나머지 형식 통일해서 보내기 위해 만듦
        public string GetUser(long id)
        {
            var user= userRepository.Get(id);
            if (user == null) return null;

            return $"{user.Id},{user.Username},{user.Password}";
        }

        // UserId로 User 객체 반환
        public User GetUserById(long id) {
            return userRepository.Get(id);
        }
    }
}