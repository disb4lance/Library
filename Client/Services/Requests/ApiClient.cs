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

namespace Client.Services.Requests
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly string _token;

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5062/api/")
            };
        }

        public ApiClient(string token)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5062/api/")
            };
            _token = token;
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

                    if (result != null && result.TryGetValue("token", out var tokenString))
                    {
                        return ApiResponse.Success(tokenString);
                    }
                    else
                    {
                        return ApiResponse.Failure("Token not found in the response.");
                    }
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return ApiResponse.Failure($"Login error: {error}");
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
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(_token);

                // Извлечение роли из токена
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
                if (roleClaim == null || !string.Equals(roleClaim.Value, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return ApiResponse.Failure("Role not found or not Admin.");
                }

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
