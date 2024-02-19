using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ICompanyService
    {
        Task<CompanyDetailDTO> AddCompanyDetail(CompanyDetailDTO companyDetailDTO);
        Task<CompanyDetailDTO> GetCompanyDetail();
        Task UpdateCompanyDetail(UpdateCompanyDetailDTO updateCompanyDetailDTO);
        Task UpdateCompanyAddress(AddressDTO addressDTO);
        Task UpdateCompanyEFT(CompanyEFTDTO companyEFTDTO);
    }
}
