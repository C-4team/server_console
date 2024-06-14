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
      
        // 여기 부분 수정
        public override string ToString()
        {
            return $"{valid},{id},{username},{password}";
        }

        public static User parseUser(string bytes)
        {
            string[] userInfo = bytes.Split(',');
            return new User(long.Parse(userInfo[1]), userInfo[2], userInfo[3]);
        }
    }

}
    // UserModel로 Spring Model로 생각하면 됨.
    