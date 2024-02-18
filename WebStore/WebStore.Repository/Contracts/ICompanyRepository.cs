using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface ICompanyRepository
    {
        Task<(CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail, CompanyEFTDetailModel companyEFTDetail, AddressModel address);
        Task<(CompanyDetailModel? CompanyDetailModel, CompanyEFTDetailModel? CompanyEFTDetailModel, AddressModel? CompanyAddressModel)> GetCompanyDetail();
        Task<CompanyDetailModel?> UpdateCompanyDetail(CompanyDetailModel companyDetail);
        Task<AddressModel?> UpdateCompanyAddress(AddressModel address);
        Task<CompanyEFTDetailModel?> UpdateCompanyEFT(CompanyEFTDetailModel companyEFTDetail);
    }
}
