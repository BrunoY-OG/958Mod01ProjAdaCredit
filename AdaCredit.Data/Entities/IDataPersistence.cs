namespace AdaCredit.Logical.Entities {
    public interface IDataPersistence {
        public void SaveData<T>(List<T> values);
        public void LoadData<T>(out List<T> values);
    }
}
