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
    public class HodManagmentController : ControllerBase
    {
        /// <summary>
        /// Insert service registration application
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("savehodregistration")]
      // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> SaveHODRegistration([FromBody] BlHod appParam)
        {
            DlHod dl = new DlHod();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.registrationYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            appParam.userId = 0;//Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnBool rb = await dl.RegistorNewHodOffice(appParam);
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
        [HttpGet("getallhodlist/{vid?}/{rid?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllHODList(Int16 vid=0, Int16 rid = 0)
        {
            DlHod dl = new DlHod();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllHODList(vid,rid);
            return dt;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary> 
        /// <returns></returns>
        [HttpGet("getallhodlistbyid/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllHODListById(Int64 Id)
        {
            DlHod dl = new DlHod();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllHODListById(Id);
            return dt;
        }
        /// <summary>
        /// Verification of HOD registration application
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("verifyhodregistration")]
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> VerifyHODRegistration([FromBody] Verification appParam)
        {
            DlHod dl = new DlHod();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);            
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.date = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.VerifyHodOffice(appParam);
            if (rb.status)
            {
                rs.message = "Successfully Verified";
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
    }
}
