using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Model.user{
     public class User{
        private int valid;  // 로그인/회원가입 확인용

        private long id;

        private string username;

        private string password;

        private List<long> friends;   // 친구 목록

        private TcpClient tcpClient;
        
        private NetworkStream networkStream;

        private StreamReader reader;

        private StreamWriter writer;

        public TcpClient TCPclient{
            get{return tcpClient;}
            set{tcpClient = value;}
        }
        
        public NetworkStream NetStream{
            get{return networkStream;}
            set {networkStream = value;}
        }
        public StreamReader Reader{
            get { return reader; }
            set {  reader = value; }
        }
        public StreamWriter Writer{
            get { return writer; }
            set { writer = value; }
        }
        
        public User(long id, string username, string password)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.friends = new List<long>();   // 친구 목록 초기화
        }

        public int Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public long Id { 
            get { return id; }
            set { id = value; }
        }
        public string Username {
            get { return username;}
            set { username = value; }
        }
        public string Password { 
            get { return password;}
            set { password = value; }
        }

        public List<long> Friends {
            get { return friends; }
            set { friends = value; }
        }
      
        // 회원가입/로그인 시 사용되는 문자열 반환
        public string ToLoginString()
        {
            return $"{valid},{id},{username},{password}";
        }

        // (친구 목록 포함) 전체 정보 반환
        public override string ToString()
        {
            var friendsStr = string.Join(";", friends);
            return $"{valid},{id},{username},{password},{friendsStr}";
        }

        // 회원가입/로그인 시 사용되는 문자열 파싱
        public static User parseUser(string bytes)
        {
            string[] userInfo = bytes.Split(',');
            return new User(long.Parse(userInfo[1]), userInfo[2], userInfo[3]);
        }

        // 친구 목록 포함한 문자열 파싱
        public static User parseUserWithFriends(string bytes) {
            string[] userInfo = bytes.Split(',');
            var friends = userInfo.Length > 4 ? userInfo[4].Split(';')
                .Select(long.Parse).ToList() : new List<long>();
            return new User(long.Parse(userInfo[1]), userInfo[2], userInfo[3])
            {
                Friends = friends
            };
        }
    }

}
    // UserModel로 Spring Model로 생각하면 됨.
    