using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseClass;
using TicketManagementApi.Models.DaLayer;
using TicketManagementApi.Models.BLayer;

namespace TicketManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        /// <summary>
        /// Insert service registration application
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("savehospitalregistration")]
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> SaveHospital([FromBody] BlHospital appParam)
        {
            DlHospital dl = new DlHospital();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.registrationYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.RegisterNewHospital(appParam);
            if (rb.status)
            {
                rs.message = "Data Saved Successfully";
                rs.status = true;
                rs.value = rb.message;
            }
            else
            {
                //====Failure====
                rs.message = "Failed to save data " + rb.message;
                rs.status = false;
            }
            return rs;
        }
        /// <summary>
        /// Insert service registration application
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("updatehospitalregistration")]
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> updateHospital([FromBody] BlHospital appParam)
        {
            DlHospital dl = new DlHospital();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.registrationYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.UpdateNewHospital(appParam);
            if (rb.status)
            {
                rs.message = "Data Saved Successfully";
                rs.status = true;
                rs.value = rb.message;
            }
            else
            {
                //====Failure====
                rs.message = "Failed to save data " + rb.message;
                rs.status = false;
            }
            return rs;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary>         

        /// <returns></returns>
        [HttpGet("getallhospital")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllHospital()
        {
            DlHospital dl = new DlHospital();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllHospitalList(userId);
            return dt;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary> 
        /// <returns></returns>
        [HttpGet("gethospitalbyid/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GethospitalById(Int64 Id)
        {
            DlHospital dl = new DlHospital();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetHospitalById(Id);
            return dt;
        }
        
    }
}
