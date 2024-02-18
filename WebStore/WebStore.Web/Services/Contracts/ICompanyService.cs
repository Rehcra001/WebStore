using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ICompanyService
    {
        Task<CompanyDetailDTO> AddCompanyDetail(CompanyDetailDTO companyDetailDTO);
        Task<CompanyDetailDTO> GetCompanyDetail();
    }
}
