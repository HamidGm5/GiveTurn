using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using System.Net.Http.Json;

namespace GiveTurn.Blazor.Services
{
    public class TurnServices : ITurnServices
    {
        private readonly HttpClient _client;

        public TurnServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<TurnDto> DeleteTurn(int TurnId)
        {
            try
            {
                var Response = await _client.DeleteAsync($"api/Turn/{TurnId}");
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return await Response.Content.ReadFromJsonAsync<TurnDto>();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is : {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<TurnDto> AddNewTurn(TurnDto turn, int Userid)
        {
            try
            {
                var Response = await _client.PostAsJsonAsync<TurnDto>($"api/Turn/{Userid}", turn);
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return await Response.Content.ReadFromJsonAsync<TurnDto>();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is : {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<TurnDto> GetUserTurn(int Userid, int TurnId)
        {
            try
            {
                var Response = await _client.GetAsync($"api/Turn/{Userid}/{TurnId}");
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return await Response.Content.ReadFromJsonAsync<TurnDto>();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is : {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<ICollection<TurnDto>> GetUserTurns(int Userid)
        {
            try
            {
                var Response = await _client.GetAsync($"api/Turn/{Userid}");
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }
                    else
                    {
                        return await Response.Content.ReadFromJsonAsync<ICollection<TurnDto>>();

                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"Error Message is : {Message} and Status Code is : {Response.StatusCode}");
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<DateTime> GetTurnDateTime()
        {
            try
            {
                var Response = await _client.GetAsync($"api/Turn");
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return DateTime.MinValue;
                    }
                    else
                    {
                        return await Response.Content.ReadFromJsonAsync<DateTime>();
                    }
                }
                else
                {
                    var Message = await Response.Content.ReadAsStringAsync();
                    throw new Exception($"You have Error with this message : {Message} " +
                        $", and with : {Response.StatusCode} Status Code");
                }
            }

            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
