using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using server_console.dataset;
using Service.dataSetService;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Model.user{
     public class User{
        private long id;

        private string username;

        private string password;

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

        public List<long> GetFriends() {
            var friends = new List<long>();
            DataSet DB = DataSetService.DB;
            DataTable friendsTable = DB.Tables["Friend"];
            foreach (DataRow row in friendsTable.Rows) {
                if ((long)row["uid"] == this.id)
                    friends.Add((long)row["fid"]);
            }
            return friends;
        }
      
        // 회원가입/로그인 시 사용되는 문자열 반환
        public string ToLoginString()
        {
            return $"{id},{username},{password}";
        }

        // (친구 목록 포함) 전체 정보 반환
        public override string ToString()
        {
            var friendsStr = string.Join(";", GetFriends());
            return $"{id},{username},{password},{friendsStr}";
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
            var user = new User(long.Parse(userInfo[1]), userInfo[2], userInfo[3]);
            var frineds = user.GetFriends();
            return user;
        }
    }

}
    // UserModel로 Spring Model로 생각하면 됨.
    