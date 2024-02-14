using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.DTO;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [Authorize]
        [Route("AddCompanyDetails")]
        public async Task<ActionResult<CompanyDetailDTO>> AddCompanyDetails(CompanyDetailDTO companyDetailDTO)
        {
            try
            {
                var companyDetailModels = companyDetailDTO.ConvertToCompanyDetailModel();

                //Validate models
                var companyDetailErrors = ValidationHelper.Validate(companyDetailModels.CompanyDetailModel);
                if (companyDetailErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, companyDetailErrors);
                }

                var companyEFTErrors = ValidationHelper.Validate(companyDetailModels.CompanyEFTDetailModel);
                if (companyEFTErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, companyEFTErrors);
                }

                var companyAddressErrors = ValidationHelper.Validate(companyDetailModels.CompanyAddressModel);
                if (companyAddressErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, companyAddressErrors);
                }

                companyDetailModels = await _companyService.AddCompanyDetail(companyDetailModels.CompanyDetailModel,
                                                                             companyDetailModels.CompanyEFTDetailModel,
                                                                             companyDetailModels.CompanyAddressModel);

                if (companyDetailModels.CompanyDetailModel.CompanyId == default
                    || companyDetailModels.CompanyEFTDetailModel.EFTId == default
                    || companyDetailModels.CompanyAddressModel.AddressId == default)
                {
                    return NoContent();
                }

                else
                {
                    //Add id's
                    companyDetailDTO.CompanyId = companyDetailModels.CompanyDetailModel.CompanyId;
                    companyDetailDTO.AddressId = companyDetailModels.CompanyAddressModel.AddressId;
                    companyDetailDTO.CompanyAddress = companyDetailModels.CompanyAddressModel.ConvertToAddressDTO();
                    companyDetailDTO.EFTId = companyDetailModels.CompanyEFTDetailModel.EFTId;
                    companyDetailDTO.CompanyEFT = companyDetailModels.CompanyEFTDetailModel.ConvertToEFTDTO();

                    return Ok(companyDetailDTO);

                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save company details");
            }

        }
    }
}
