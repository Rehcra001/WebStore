namespace WebStore.DTO
{
    public class UserRegistrationDTO
    {
        public string EmailAddress { get; set; }

        public string ConfirmEmailAddress { get; set; }

        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<AddressDTO> AddressList { get; set; }
    }
}
