using Blazored.LocalStorage;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public CompanyService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<CompanyDetailDTO> AddCompanyDetail(CompanyDetailDTO companyDetailDTO)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<CompanyDetailDTO>("api/company/AddCompanyDetails", companyDetailDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CompanyDetailDTO);
                    }
                    else
                    {
                        companyDetailDTO = await httpResponseMessage.Content.ReadFromJsonAsync<CompanyDetailDTO>();
                        return companyDetailDTO;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CompanyDetailDTO> GetCompanyDetail()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/company/getcompanydetail");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CompanyDetailDTO);
                    }
                    else
                    {
                        CompanyDetailDTO? companyDetailDTO = await httpResponseMessage.Content.ReadFromJsonAsync<CompanyDetailDTO>();
                        return companyDetailDTO;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
