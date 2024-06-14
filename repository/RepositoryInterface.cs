namespace Repository{
    public interface RepositoryInterface<I,T>{
        T Get(int id);
        void Insert(T item);
        void Update(I id, T item);
        void Delete(I id);
    }
}


