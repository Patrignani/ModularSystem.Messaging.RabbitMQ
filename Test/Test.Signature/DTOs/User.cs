namespace Test.Signature.DTOs
{
    public class UserId
    {
        public int Id { get; set; }
    }
    public class UserBasic: UserId
    {
        public string Identification { get; set; }
        public bool Active { get; set; }
    }

    public class UserAddress : UserBasic
    {
       public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
            
    }

    public class UserContact : UserBasic
    {
        public string Email { get; set; }
        public string Telephone { get; set; }
    }

    public class UserUpdate 
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Identification { get; set; }
        public UserContact Contact { get; set; }
    }
    
    public class User : UserAddress
    { 
        public UserContact Contact { get; set;}
    }
}
