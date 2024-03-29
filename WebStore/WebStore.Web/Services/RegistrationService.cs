﻿using System.Net.Http.Json;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly HttpClient _httpClient;

        public RegistrationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool IsSuccessful, string? Erorrs)> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<UserRegistrationDTO>("api/user/register", userRegistrationDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string serverErrorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                    return (false,serverErrorMessage);
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }
    }
}
