namespace Model.message{
    public class Message{
        private long uid;
        private long gid;
        private string message;
        private DateTime dateTime;

        public Message(long gid, long uid, string message, DateTime dateTime){
            this.uid = uid;
            this.gid = gid;
            this.message = message;
            this.dateTime = DateTime.Now;
        }
        public long Gid{
            get{return gid;}
        }
        public long Uid{
            get{return uid;}
        }
        public string Msg{
            get{return message;}
        }
        public DateTime DateTime{
            get{return dateTime;}
        }
    }
}