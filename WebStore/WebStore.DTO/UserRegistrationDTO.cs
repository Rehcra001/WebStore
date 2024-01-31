namespace WebStore.DTO
{
    public class UserRegistrationDTO
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string ConfirmEmailAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public IEnumerable<AddressDTO> AddressList { get; set; }
    }
}
