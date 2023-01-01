namespace AdaCredit.Logical.Entities {
    public class Client {
        //caso mais de uma conta por cliente, ou utilizando DB
        //public int ID { get; init; }
        public string Name { get; set; }
        public long Document { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Account Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }

    }
}
