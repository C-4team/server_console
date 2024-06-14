using System.Data;
using server_console.dataset;
namespace Service.dataSetService{
    public class DataSetService{
        private static DataBase dataBase;
        private static DataSetService instance;

        private DataSetService(){
            dataBase = new DataBase();
        }
        public static DataBase DB{
            get{
                if(instance == null){
                    instance = new DataSetService();
                }
                return dataBase;
            }
        }
    }
}