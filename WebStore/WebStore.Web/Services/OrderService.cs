﻿using Blazored.LocalStorage;
using System.Net.Http.Headers;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace WebStore.WEB.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public OrderService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/order/getorderbyid/{id}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(OrderDTO);
                    }
                    else
                    {
                        OrderDTO orderDTO = await httpResponseMessage.Content.ReadFromJsonAsync<OrderDTO>();
                        return orderDTO;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersToBeShipped()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/order/GetOrdersToBeShipped");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<OrderDTO>();
                    }
                    else
                    {
                        IEnumerable<OrderDTO> orders = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<OrderDTO>>();
                        return orders;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersWithOutstandingPayment()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/order/GetOrdersWithOutstandingPayment");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<OrderDTO>();
                    }
                    else
                    {
                        IEnumerable<OrderDTO> orders = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<OrderDTO>>();
                        return orders;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateOrderPayment(int orderId, bool payed)
        {
            try
            {
                PaymentConfirmationDTO paymentConfirmation = new PaymentConfirmationDTO { OrderId = orderId, Payed = payed };

                HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsJsonAsync($"api/order/UpdateOrderPaymentConfirmation/{orderId}", paymentConfirmation);

                if (httpResponseMessage.IsSuccessStatusCode == false)
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateOrderShipped(int orderId, bool shipped)
        {
            try
            {
                ShippingConfirmationDTO shippingConfirmation = new ShippingConfirmationDTO { OrderId = orderId, Shipped = shipped };

                HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsJsonAsync($"api/order/UpdateOrderShippedConfirmation/{orderId}", shippingConfirmation);

                if (httpResponseMessage.IsSuccessStatusCode == false)
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
