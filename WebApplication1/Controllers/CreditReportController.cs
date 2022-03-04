using Microsoft.AspNetCore.Mvc;
using Test180222_1419_BL;
using Test180222_1419_BL.Models;

namespace WebApplication1.Controllers
{
    public class CreditReportController : Controller
    {
        public readonly ICreditReportBL _creditReportBL;
        public CreditReportController(ICreditReportBL creditReportBL)
        {
            _creditReportBL = creditReportBL;
        }

        [Route("api/CreditReport/Svc1")]
        [HttpGet("")]
        public async Task<List<CreditDataModel>> Svc1(int applicationId, string source, string bureau)
        {
            var creditReportData = await _creditReportBL.GetData1Async(applicationId, source, bureau);
            return creditReportData;
        }

        [Route("api/CreditReport/Svc2")]
        [HttpGet("")]
        public async Task<Service2Result> Svc2(int applicationId, int income)
        {
            var creditReportData = await _creditReportBL.GetData2Async(applicationId, income);
            return creditReportData;
        }
    }
}
