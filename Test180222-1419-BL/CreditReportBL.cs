using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Test180222_1419_BL.Models;

using static System.Net.Mime.MediaTypeNames;

namespace Test180222_1419_BL
{
    public interface ICreditReportBL
    {
        Task<List<CreditDataModel>> GetData1Async(int applicationId, string source, string bureau);
        Task<Service2Result> GetData2Async(int applicationId, int income);
    }

    public class CreditReportBL : ICreditReportBL
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "CreditReportCacheKey";
        private const string url = "https://raw.githubusercontent.com/StrategicFS/Recruitment/master/creditData.json";
        private const string Source_ABC = "ABC";
        private const string Bureau_EFX = "EFX";
        private const string Type_Unsecured = "UNSECURED";

        public CreditReportBL(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<CreditDataModel>> GetData1Async(int applicationId, string source, string bureau)
        {
            var fullDataSet = await GetAllDataAsync();
            var data = fullDataSet.creditReports.Where(a =>
                a.applicationId.Equals(applicationId)
                && a.source.Equals(source)
                && a.bureau.Equals(bureau)
                )
                .ToList();
            return data;
        }

        public async Task<Service2Result> GetData2Async(int applicationId, int income)
        {
            var fullDataSet = await GetAllDataAsync();
            var data = fullDataSet.creditReports.Where(a =>
                a.source.Equals(Source_ABC)
                && a.bureau.Equals(Bureau_EFX)
                && a.applicationId.Equals(applicationId)
            )
            .ToList();

            var debtTotal = data
                .SelectMany(a => a.tradelines)
                .Where(a => !a.isMortgage)
                .Sum(a => a.monthlyPayment);

            var unsecuredTradeLines = data
                .SelectMany(a => a.tradelines)
                .Where(a => a.type.Equals(Type_Unsecured));

            var unsecuredDebtBalance = unsecuredTradeLines
                .Sum(a => a.balance);

            var result = new Service2Result()
            {
                DTI = debtTotal / income,
                TotalUnsecuredDebtBalance = unsecuredDebtBalance,
                UnsecuredTradeLineCount = unsecuredTradeLines.Count(),
            };
            return result;
        }

        private async Task<CreditDataWrapper> GetAllDataAsync()
        {
            var data = _memoryCache.Get(CacheKey) as CreditDataWrapper;
            if (data != null)
                return data;

            data = await GetCreditReportFromWSAsync();
            _memoryCache.Set(CacheKey, data);
            return data;
        }

        private async Task<CreditDataWrapper> GetCreditReportFromWSAsync()
        {
            var restClient = new RestSharp.RestClient();
            var restRequest = new RestSharp.RestRequest(url);
            var restResult = await restClient.ExecuteAsync(restRequest);
            if (restResult.IsSuccessful && !string.IsNullOrWhiteSpace( restResult?.Content))
            {
                var data = JsonConvert.DeserializeObject<CreditDataWrapper>(restResult?.Content);
                return data;
            }
            throw new Exception($"Failed to get source Credit Report data");
        }
    }
}
