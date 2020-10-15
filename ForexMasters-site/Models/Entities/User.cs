namespace ForexMasters_site.Models.Entities
{
    public class User
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public bool isActive { get; set; }

        public User()
        {
            this.Balance = 0;
            this.isActive = false;
        }
    }
}
