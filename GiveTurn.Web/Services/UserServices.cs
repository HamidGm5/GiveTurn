using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace GiveTurn.Blazor.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _client;

        public UserServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> DeleteUser(int Userid)
        {
            try
            {
                var Response = await _client.DeleteAsync($"api/User/{Userid}");

                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    var Message = Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is : {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<UserDto> Login(string username, string password)
        {
            try
            {
                var Response = await _client.GetAsync($"api/User/{username}/{password}");

                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }
                    else
                    {
                        return await Response.Content.ReadFromJsonAsync<UserDto>();
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"The Error Message is : {Message} and Status Code is {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<UserDto> SignUp(UserDto newuser)  //Bug It's not take response of controller
        {
            try
            {
                var Response = await _client.PostAsJsonAsync<UserDto>($"api/User", newuser);
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }
                    else
                    {
                        return await Response.Content.ReadFromJsonAsync<UserDto>();
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"You have error and message is : {Message} and status code is : {Response.StatusCode}");
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDto> UpdateUser(UserDto newSpec)
        {
            try
            {
                var JsonRequest = JsonConvert.SerializeObject(newSpec);
                var Content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");
                var Response = await _client.PatchAsync($"api/User/{newSpec.Id}", Content);

                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }
                    else
                    {
                        return await Response.Content.ReadFromJsonAsync<UserDto>();
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }
    }
}
