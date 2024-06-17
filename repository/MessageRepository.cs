using System.Data;
using Model.message;
using server_console.dataset;
using Service.dataSetService;

namespace Repository.messageRepository{
    public class MessageRepository {
        //string = gid, uid, message 
        private DataTable messages;
        public MessageRepository(){
            messages = DataSetService.DB.Tables["Message"]!;
        }
        private bool validInput(string input){
            long tmp;
            string[] splitedInput = input.Split(',');
            if(splitedInput.Length != 3){
                return false;
            }
            if(!long.TryParse(splitedInput[0], out tmp)){
                return false;
            }
            if(!long.TryParse(splitedInput[1], out tmp)){
                return false;
            } 
            return true;
        }
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Message> Get(long gid)
        {
           
            var qurry = 
                from message in messages.AsEnumerable()
                where (long)message["gid"] == gid
                orderby message["datetime"]
                select message;
                
            var select9 = qurry.Take(9);

            List<Message> result = new List<Message>();
            foreach(var dr in qurry){
                result.Add(new Message((long)dr[0],(long)dr[1],(string)dr[2],(DateTime)dr[3]));
            }
            return result;
        }

        public void Insert(Message item)
        {
            
            DataRow newRow = messages.NewRow();
            newRow["gid"] = item.Gid;
            newRow["uid"] = item.Uid;
            newRow["message"] = item.Msg;
            newRow["datetime"] = item.DateTime;
            messages.Rows.Add(newRow);
            messages.AcceptChanges();
        }
    }
}