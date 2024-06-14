using Repository;
using Model.user;

namespace Repository.userRepository{
    public class UserRepository : RepositoryInterface<long,User>
{
    const string fileName = "../csv/user.csv";

    // 여기부터 추가 - 5/28(화)
    public UserRepository()
    {
        // 파일이 없으면 생성 & 헤더 추가
        if (!File.Exists(fileName))
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                // CSV 파일의 헤더 작성
                writer.WriteLine("id,username,password");
            }
        }
    }

    public void Delete(long id)
    {
        // 모든 사용자 정보 읽어올 리스트 초기화
        var users = new List<User>();

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
            writer.WriteLine("id,username,password");
            foreach(var user in users)
            {
                writer.WriteLine(user.ToString());
            }
        }
    }

    public User Get(int id)
    {
        using (var reader = new StreamReader(fileName))
        {
            string line;
            reader.ReadLine();  // 헤더 건너뛰기
            while ((line=reader.ReadLine()) != null)
            {
                var user = User.parseUser(line);
                if (user.Id == id) return user;
            }
        }
        return null;  // 사용자 찾지 못한 경우
    }

    public void Insert(User item)
    {
        using (var writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(item.ToString());
        }
    }

    public void Update(long id, User item)
    {
        var users = new List<User>();

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
            writer.WriteLine("id,username,password");
            foreach (var user in users)
            {
                writer.WriteLine(user.ToString());
            }
        }
    }
}
}
