using Strateq.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace HB.SmartSD.Integrator
{
    public class ServiceHelper
    {
        protected HttpClient httpclient = new();
        //private readonly IMMSRequestLogRepository _mmsRequestLogRepository;
        private static string smartSDUrl = Environment.GetEnvironmentVariable("SMARTSD_API_URL") ?? throw new Exception("SMARTSD API URL not found in ENV");
        private string smartSDToken = string.Empty;
        private readonly ISystemLogService _logger;

        public ServiceHelper(ISystemLogService logger)
        {
            _logger = logger;
        }

        public string GetSmartSDToken()
        {
            return "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5IiwianRpIjoiMmJjZTFlMjk1NmRjNWNjMDliNDNiZTk3OTU2NmM2ZmIyMTE4M2IwYWJjMzZmYzI4ZmZiZjk0YWIwNzY1ZWZiNzc5ZTU4NzViMjg1N2E3MGEiLCJpYXQiOjE2NzgxNDY3NDEuNjQ4NzMsIm5iZiI6MTY3ODE0Njc0MS42NDg3MzUsImV4cCI6MTgwNDM3NzE0MS41OTEwOTcsInN1YiI6IjU0MCIsInNjb3BlcyI6WyIqIl19.1G3PWnjvjwN_L-V3eVkIvGzO8VRuhmKDEZ014tYi7TwtluY_jsdruNvVLiow_pV3yd5-2VplTjgt6OHuiznJEwBF_K2oZCaOfwD1UDlwy1NP77E32JqkuPvHmPgnY_FliXWF8P33xrlckfoN8uolWa_h8JULOfJVMGHH0eVlzmO70xwIMYbeH9QSHxV7MPq--lQ1hLFDvuqBAenKLK-gW-aN4o3PamBux2kAOMk98UyknNmkQFm5gLnkUivJli1_hzjKLngicYaYLcCc6PubDHzg1_dGwsDyEcLsZtL6a531eBk9TeVuv3nM3GAMygSbWx9ynkmWPU4LmObZfN0uN0nRVIM5B93hJyrs9EJzG39Q9ECIj125GWNppGWsFb3nLaq3_prnn-6RMcZCkfhtTObS1QtNYsrfgMRvTuF1_x2SBf-Wf9UwEOiMuI_3iWf8hz5QXNo-AEwz0BnJsxRcAJ4tkviheOywtFWX1Qy9ThUFqkdoXoR738eifjMOvnEegO4kWARgkrltkJ2v-KWoUI7h03OslEUFRHNn15LGFwT9R1_mYLCkZh8HXdTleZ1jG4np_0nDg1k2mvHd_6szAmY6n2wuT9-eNAFAZ-i7-_YfIpDA9Fyr6yDpSj2lOI0E3NMsMkxiNbeJMKgRw9VTAkLHqwxReZtOOVUOl9Qyf3U";
        }

        public async Task<CreateIncidentsResponseModel> CreateIncidentRecord(CreateIncidentsRequestModel model)
        {
            StringBuilder logger = new();
            smartSDToken = GetSmartSDToken();
            var uri = new Uri($"{smartSDUrl}incidents");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                logger?.AppendLine($"Url: {uri}");
                string sContent = JsonSerializer.Serialize(model);
                logger?.AppendLine($"Content: {sContent}");

                request.Content = new StringContent(sContent, Encoding.UTF8, "application/json");

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", smartSDToken);
                logger?.AppendLine($"Add Request Header ACCESS_TOKEN {smartSDToken}");

                logger?.AppendLine($"[{DateTime.UtcNow}] Start sending the CreateIncidentRecord API");
                HttpResponseMessage response = await httpclient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                logger?.AppendLine($"[{DateTime.UtcNow}] End sending the CreateIncidentRecord API");

                string responseString = await response.Content.ReadAsStringAsync();
                logger?.AppendLine($"ResponseCode: {response.StatusCode}: {responseString}");
                //mmsLog.ResponseCode = Convert.ToInt16((int)response.StatusCode);
                logger?.AppendLine($"SmartSD CreateIncidentRecord Response: {responseString}");
                await _logger.LogInformation(logger?.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new SmartSDException($"SmartSD Create Incident Record Failed - Error Code {response.StatusCode}");
                }

                CreateIncidentsResponseModel result = JsonSerializer.Deserialize<CreateIncidentsResponseModel>(responseString) ?? throw new SmartSDException("Invalid Deserialize modal");
                if (result == null) { throw new SmartSDException($"SmartSD Failed - Error Code {response.StatusCode}"); }
                return result;
            }
        }


        public async Task<GetIncidentsResponseModel> GetIncidentDetails(string incidentId)
        {
            StringBuilder logger = new();
            smartSDToken = GetSmartSDToken();
            var uri = new Uri($"{smartSDUrl}incidents/{incidentId}");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                logger?.AppendLine($"Url: {uri}");

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", smartSDToken);
                logger?.AppendLine($"Add Request Header ACCESS_TOKEN {smartSDToken}");

                logger?.AppendLine($"[{DateTime.UtcNow}] Start sending the GetIncidentDetails API");
                HttpResponseMessage response = await httpclient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                logger?.AppendLine($"[{DateTime.UtcNow}] End sending the GetIncidentDetails API");

                string responseString = await response.Content.ReadAsStringAsync();
                logger?.AppendLine($"ResponseCode: {response.StatusCode}: {responseString}");
                logger?.AppendLine($"SmartSD GetIncidentDetails Response: {responseString}");
                await _logger.LogInformation(logger?.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    throw new SmartSDException($"SmartSD Get Incident Details Failed - Error Code {response.StatusCode}");
                }

                GetIncidentsResponseModel result = JsonSerializer.Deserialize<GetIncidentsResponseModel>(responseString) ?? throw new SmartSDException("Invalid Deserialize modal");
                if (result == null) { throw new SmartSDException($"SmartSD Failed - Error Code {response.StatusCode}"); }
                return result;

            }
        }

        public async Task<CreateSiteResponseModel> CreateSiteRecord(CreateSiteRequestModel model)
        {
            StringBuilder logger = new();
            smartSDToken = GetSmartSDToken();
            var uri = new Uri($"{smartSDUrl}sites");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                logger?.AppendLine($"Url: {uri}");
                string sContent = JsonSerializer.Serialize(model);
                logger?.AppendLine($"Content: {sContent}");

                request.Content = new StringContent(sContent, Encoding.UTF8, "application/json");

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", smartSDToken);
                logger?.AppendLine($"Add Request Header ACCESS_TOKEN {smartSDToken}");

                logger?.AppendLine($"[{DateTime.UtcNow}] Start sending the CreateSiteRecord API");
                HttpResponseMessage response = await httpclient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                logger?.AppendLine($"[{DateTime.UtcNow}] End sending the CreateSiteRecord API");

                string responseString = await response.Content.ReadAsStringAsync();
                logger?.AppendLine($"ResponseCode: {response.StatusCode}: {responseString}");
                //mmsLog.ResponseCode = Convert.ToInt16((int)response.StatusCode);
                logger?.AppendLine($"SmartSD CreateSiteRecord Response: {responseString}");
                await _logger.LogInformation(logger?.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    throw new SmartSDException($"SmartSD Create Site Record Failed - Error Code {response.StatusCode}");
                }

                CreateSiteResponseModel result = JsonSerializer.Deserialize<CreateSiteResponseModel>(responseString) ?? throw new SmartSDException("Invalid Deserialize modal");
                if (result == null) { throw new SmartSDException($"SmartSD Failed - Error Code {response.StatusCode}"); }
                return result;
            }
        }
    }


}
