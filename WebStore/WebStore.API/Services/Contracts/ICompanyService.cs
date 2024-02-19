using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICompanyService
    {
        Task<(CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail,
                                                                                                                                                      CompanyEFTDetailModel companyEFTDetail,
                                                                                                                                                      AddressModel companyAddress);
        Task<(CompanyDetailModel? CompanyDetailModel, CompanyEFTDetailModel? CompanyEFTDetailModel, AddressModel? CompanyAddressModel)> GetCompanyDetail();
        Task<CompanyDetailModel?> UpdateCompanyDetail(CompanyDetailModel companyDetail);
        Task<AddressModel?> UpdateCompanyAddress(AddressModel address);
        Task<CompanyEFTDetailModel?> UpdateCompanyEFT(CompanyEFTDetailModel companyEFTDetail);
    }
}
