using Model.group;
namespace Repository.groupRepository{
    public class GroupRepository : RepositoryInterface<long ,Group>{
        const string fileName = "../csv/group.csv";
        public GroupRepository(){
            
        }


        public Group Get(long id){
            throw new NotImplementedException();

        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }


        public void Insert(Group item)
        {
            throw new NotImplementedException();
        }

        public void Update(long id, Group item)
        {
            throw new NotImplementedException();
        }
    }
}