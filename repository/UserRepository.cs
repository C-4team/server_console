using Repository;
using Model.user;
using server_console.dataset;
using Service.dataSetService;
using System.Data;

namespace Repository.userRepository{
    public class UserRepository : RepositoryInterface<long,User>{
        const string fileName = "./csv/user.csv";
        private DataTable users;
        public UserRepository(){
            DataBase DB = DataSetService.DB;
            users = DB.Tables["User"]!;
        }

        public void Delete(long id) {
        var query =
            from user in users.AsEnumerable()
            where (long)user["uid"] == id
            select user;

        foreach (var dr in query)
        {
            dr.Delete();
        }

        SaveCsv();
        }
       
        public User Get(long id)
        {
            var query = 
                from user in users.AsEnumerable()
                where (long)user["uid"] == id
                select user;

            foreach (var dr in query){
                return new User((long)dr["uid"],(string)dr["name"],(string)dr["password"]);
            }
            return null;  // 사용자 찾지 못한 경우
        }

        public void Insert(User item) {
            DataRow dataRow = users.NewRow();
            dataRow["uid"] = item.Id;
            dataRow["name"]= item.Username;
            dataRow["password"] = item.Password;
            users.Rows.Add(dataRow);
            users.AcceptChanges();
        }

        public void Update(long id, User item) {
            var query = 
                from user in users.AsEnumerable()
                where (long)user["uid"] == id
                select user;

            foreach(var dr in query)
            {
                dr["name"] = item.Username;
                dr["password"] = item.Password;
            }

            SaveCsv();
        }

    public void SaveCsv() {
        using (var writer = new StreamWriter(fileName,false))
        {
            writer.WriteLine(string.Join(",", users.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

            foreach(DataRow row in users.Rows)
            {
                if (row.RowState != DataRowState.Deleted) {
                    writer.WriteLine(string.Join(",", row.ItemArray));
                }
            }
        }
    }
}
}
