using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Services
{
    public interface IIBMiSQLApi
    {
        Task<string> ExecuteSQLStatement(string sql);
        Task<List<T>> CallSqlService<T>(string query);
        //Task ExecuteSqlStatement(string statement);
    }

    public class IBMiSQLApi : IIBMiSQLApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IBMiSQLApi> _logger;
        private readonly EndpointConfiguration _config;

        public IBMiSQLApi(IHttpClientFactory httpClientFactory, EndpointConfiguration config, ILogger<IBMiSQLApi> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = config;
            _logger.LogDebug($"Endpoint was configured as {_config.Api}");
        }

        public async Task<string> ExecuteSQLStatement(string sql)
        {
            var resultString = String.Empty;
            _logger.LogDebug($"Sending sql statement ({sql}) to the sql api...");
            try
            {
                var query = new QueryModel
                {
                    Query = sql
                };

                using var client = _httpClientFactory.CreateClient("sqlapi");
                using var request = new HttpRequestMessage(HttpMethod.Post, "");
                client.Timeout = TimeSpan.FromSeconds(20);

                var json = JsonSerializer.Serialize(query);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                var base64EncodedAuthenticationString
                    = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_config.Uname}:{_config.Password}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"SQL Query reguest returns with  {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error calling SQL API: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Calling sql api enpoint timedout: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling sql api enpoint: {ex.Message}", ex);
            }

            return resultString;
        }

        public async Task<List<T>> CallSqlService<T>(string query)
        {
            _logger.LogDebug($"Sending sql statement to sql service: {query}...");
            var sw = new Stopwatch();
            sw.Start();
            var json = await ExecuteSQLStatement(query);
            sw.Stop();
            _logger.LogInformation($"Sending request took {sw.Elapsed.TotalSeconds}!");
            _logger.LogDebug($"Service returned json ${json}!");
            _logger.LogInformation($"Received json with {json.Length} chars!");

            if (string.IsNullOrEmpty(json)) throw new Exception($"Error sending sql string to api! Received json string is empty!");

            _logger.LogDebug("Deserializing json result...");
            var ret = JsonSerializer.Deserialize<SQLServiceResponse<T>>(json);
            _logger.LogDebug($"Json deserialized!");

            return ret.Data;
        }

        //public async Task ExecuteSqlStatement(string statement)
        //{
        //    _logger.LogDebug($"Sending sql statement to sql service: {statement}...");
        //    var sw = new Stopwatch();
        //    sw.Start();
        //    var json = await ExecuteSQLStatement(statement);
        //    sw.Stop();
        //    _logger.LogInformation($"Sending request took {sw.Elapsed.TotalSeconds}!");
        //    _logger.LogDebug($"Service returned json ${json}!");
        //    _logger.LogInformation($"Received json with {json.Length} chars!");
        //}
    }
}
