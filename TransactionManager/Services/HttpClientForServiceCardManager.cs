using Data;
using Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TransactionManager.Services
{
    /// <summary>
    /// Http client for service CardManager
    /// </summary>
    public interface IHttpClientForServiceCardManager
    {
        Task<long> GetIdDefaultUserCardAsync(long userId);

        Task<bool> CheckExistUserCardAsync(long cardId);
    }

    public class HttpClientForServiceCardManager : IHttpClientForServiceCardManager
    {
        private readonly HttpClient _client;

        public HttpClientForServiceCardManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> CheckExistUserCardAsync(long cardId) => (await _client.GetFromJsonAsync<ResultApi<bool>>($"/api/v1/CardManager/{cardId}/CheckExistUserCard")).Result;


        public async Task<long> GetIdDefaultUserCardAsync(long userId) => (await _client.GetFromJsonAsync<ResultApi<long>>($"/api/v1/CardManager/{userId}/GetDefaultUserCard")).Result;

    }
}
