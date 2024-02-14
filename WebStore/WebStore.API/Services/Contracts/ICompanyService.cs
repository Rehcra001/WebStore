using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICompanyService
    {
        Task<(CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail,
                                                                                                                                                      CompanyEFTDetailModel companyEFTDetail,
                                                                                                                                                      AddressModel companyAddress);
    }
}
