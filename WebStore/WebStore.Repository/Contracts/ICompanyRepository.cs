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
        Task<(CompanyDetailModel companyDetailModel, CompanyEFTDetailModel companyEFTDetailModel, AddressModel companyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail, CompanyEFTDetailModel companyEFTDetail, AddressModel address);
    }
}
