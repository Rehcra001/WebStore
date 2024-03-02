using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.CustomerProfile
{
    public partial class CustomerDetail
    {
        [Inject]
        public ICustomerService CustomerService { get; set; }

        [Inject]
        public IManageCustomerDetailLocalStorage ManageCustomerDetailLocalStorage { get; set; }

        [Parameter]
        public CustomerDTO Customer { get; set; } = new CustomerDTO();

        private List<AddressDTO> Addresses = new List<AddressDTO>();

        private int count = 0;

        protected override void OnInitialized()
        {
            Addresses = Customer.AddressList.ToList();
        }

        private async Task SaveCustomerDetail()
        {
            UpdateCustomerDetailDTO updateCustomerDetailDTO = new UpdateCustomerDetailDTO
            {
                CustomerId = Customer.CustomerId,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                PhoneNumber = Customer.PhoneNumber,
                EmailAddress = Customer.EmailAddress
            };

            await CustomerService.UpdateCustomerDetailAsync(updateCustomerDetailDTO);

            await ManageCustomerDetailLocalStorage.SaveCustomerDetail(Customer);
        }

        private async Task SaveAddress(int index)
        {
            //Save address
            await CustomerService.UpdateCustomerAddressAsync(Addresses[index]);

            Customer.AddressList = Addresses;

            await ManageCustomerDetailLocalStorage.SaveCustomerDetail(Customer);
        }

        private async Task AddNewAddress()
        {
            throw new NotImplementedException();
        }
    }
}
