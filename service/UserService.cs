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
    }
}