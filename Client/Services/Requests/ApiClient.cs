using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Classes.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Client.Services.Requests
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5062/api/")
            };
        }


        public async Task<ApiResponse> PostUserAsync(string endpoint, object user)
        {
            try
            {
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse.Success("Registration successful!");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return ApiResponse.Failure($"Registration error: {error}");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.Failure($"Connection error: {ex.Message}");
            }
        }
        public async Task<ApiResponse> LoginAsync(string endpoint, object user)
        {
            try
            {
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {

                        return ApiResponse.Success();
                }
                else
                {
                        return ApiResponse.Failure("Fail");
                }

            }
            catch (Exception ex)
            {
                return ApiResponse.Failure($"Connection error: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SaveBookAsync(Book book)
        {
            try
            {
                

                string json = JsonSerializer.Serialize(book);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправка POST-запроса
                HttpResponseMessage response = await _client.PostAsync("Books/AddBook", content);

                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse.Success("Book saved successfully!");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return ApiResponse.Failure($"Save error: {error}");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.Failure($"Connection error: {ex.Message}");
            }

        }
    }
}
