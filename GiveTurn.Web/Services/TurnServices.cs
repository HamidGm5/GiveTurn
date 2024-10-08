﻿using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
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

        public async Task<bool> DeleteTurn(int Userid, int TurnId)
        {
            try
            {
                var Response = await _client.DeleteAsync($"api/Turn/{Userid}/{TurnId}");
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
                return false;
            }
        }

        public async Task<TurnDto> AddNewTurn(AddTurnDto addTurn)
        {
            try
            {
                var Response = await _client.PostAsJsonAsync<AddTurnDto>($"api/Turn", addTurn);
                if (Response.IsSuccessStatusCode)
                {
                    if (Response.StatusCode != System.Net.HttpStatusCode.NoContent)
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
                    if (Response.StatusCode != System.Net.HttpStatusCode.NoContent)
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

        public async Task<bool> DeleteAllUserTurns(int Userid)
        {
            var Response = await _client.DeleteAsync($"api/Turn/{Userid}");
            if (Response.IsSuccessStatusCode)
            {
                if (Response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<TurnDto> UserLastTurn(int Userid)
        {
            try
            {
                var UserTurns = await GetUserTurns(Userid);
                if (UserTurns == null)
                {
                    return default(TurnDto);
                }
                else
                {
                    return UserTurns.LastOrDefault();
                }
            }

            catch
            {
                return null;
            }
        }
    }
}
