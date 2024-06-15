using Repository;
using Model.user;
using System;
using System.Data;
using System.IO;
using System.Linq;
using server_console.dataset;
using Service.dataSetService;
using System.Collections.Generic;


namespace Repository.userRepository{
    public class UserRepository : RepositoryInterface<long,User>
{
    const string fileName = "./csv/user.csv";
    private DataTable users;

    public UserRepository()
    {
        DataBase DB = DataSetService.DB;   // 여기 dataset 파일이 없어서 오류남
        users = DB.Tables["User"];
    }
        // 파일이 없으면 생성 & 헤더 추가
    /*    if (!File.Exists(fileName))
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                // CSV 파일의 헤더 작성
                writer.WriteLine("id,username,password");
            }
        }*/

    public void Delete(long id)
    {
        // 모든 사용자 정보 읽어올 리스트 초기화
        /*var users = new List<User>();

        // 파일에서 데이터 읽음
        using (var reader = new StreamReader(fileName))
        {
            string line;
            reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                var user = User.parseUser(line);
                // 삭제할 사용자가 아닌 경우 리스트에 추가
                if (user.Id != id) users.Add(user);
            }
        }

        // 변경된 사용자 정보 파일에 씀
        using (var writer = new StreamWriter(fileName, false))
        {
            writer.WriteLine("valid,id,username,password");
            foreach(var user in users)
            {
                writer.WriteLine(user.ToString());
            }
        }*/ 
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
        /*using (var reader = new StreamReader(fileName))
        {
            string line;
            reader.ReadLine();  // 헤더 건너뛰기
            while ((line=reader.ReadLine()) != null)
            {
                var user = User.parseUser(line);
                if (user.Id == id) return user;
            }
        }
        return null;  // 사용자 찾지 못한 경우*/

        var query =
            from user in users.AsEnumerable()
            where (long)user["uid"] == id
            select user;

        foreach (var dr in query) {
            var user = new User((long)dr["uid"],(string)dr["name"],(string)dr["password"])
            {
                Friends = dr["friends"].ToString().Split(';').Select(long.Parse).ToList()
            };
            return user;
        }
        return null;
    }

    public void Insert(User item)
    {
        /*using (var writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(item.ToString());
        }*/
        DataRow dataRow = users.NewRow();
        dataRow["valid"] = item.Valid;
        dataRow["uid"] = item.Id;
        dataRow["name"]= item.Username;
        dataRow["password"] = item.Password;
        dataRow["friends"] = string.Join(";", item.Friends);
        users.Rows.Add(dataRow);
    }

    public void Update(long id, User item)
    {
        /*var users = new List<User>();

        // 파일에서 데이터 읽음
        using (var reader = new StreamReader(fileName))
        {
            string line;
            reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                var user = User.parseUser(line);
                if (user.Id == id) user = item;
                users.Add(user);
            }
        }

        // 변경된 사용자 정보 파일에 씀
        using (var writer = new StreamWriter(fileName, false))
        {
            writer.WriteLine("valid,id,username,password");
            foreach (var user in users)
            {
                writer.WriteLine(user.ToString());
            }
        }*/
        var query = 
            from user in users.AsEnumerable()
            where (long)user["uid"] == id
            select user;

        foreach(var dr in query)
        {
            dr["valid"] = item.Valid;
            dr["name"] = item.Username;
            dr["password"] = item.Password;
            dr["friends"] = string.Join(";", item.Friends);
        }

        SaveCsv();
    }

    private void SaveCsv() {
        using (var writer = new StreamWriter(fileName,false))
        {
            writer.WriteLine(string.Join(",", users.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

            foreach(DataRow row in users.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    writer.WriteLine(string.Join(",", row.ItemArray));
                }
            }
        }
    }
}
}
