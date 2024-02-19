using WebStore.API.Services.Contracts;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<(CompanyDetailModel? CompanyDetailModel, CompanyEFTDetailModel? CompanyEFTDetailModel, AddressModel? CompanyAddressModel)> GetCompanyDetail()
        {
            try
            {
                return await _companyRepository.GetCompanyDetail();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AddressModel?> UpdateCompanyAddress(AddressModel address)
        {
            try
            {
                return await _companyRepository.UpdateCompanyAddress(address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyDetailModel?> UpdateCompanyDetail(CompanyDetailModel companyDetail)
        {
            try
            {
                return await _companyRepository.UpdateCompanyDetail(companyDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyEFTDetailModel?> UpdateCompanyEFT(CompanyEFTDetailModel companyEFTDetail)
        {
            try
            {
                return await _companyRepository.UpdateCompanyEFT(companyEFTDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail, CompanyEFTDetailModel companyEFTDetail, AddressModel companyAddress)
        {
            try
            {
                return await _companyRepository.AddCompanyDetail(companyDetail, companyEFTDetail, companyAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
