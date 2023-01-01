namespace AdaCredit.Logical.Entities {
    public sealed class Employee {
        //caso utilizando DB
        //public int ID { get; init; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public string HashedPassword { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; init; }
        public bool Active { get; set; }
        public Employee() { }

    }
}
