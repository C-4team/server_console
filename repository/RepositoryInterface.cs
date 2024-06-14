namespace Repository{
    public interface RepositoryInterface<I,T>{
        T Get(I id);
        void Insert(T item);
        void Update(I id, T item);
        void Delete(I id);
    }
}


