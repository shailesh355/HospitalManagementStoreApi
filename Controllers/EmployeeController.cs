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
    public class Employee : ControllerBase
    {
        /// <summary>
        ///Save Employee Record
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("saveemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> SaveEmployee([FromBody] BlEmployee appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.registrationYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.SaveEmployeeDetail(appParam);
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
        ///Save Employee Record
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("updateemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> UpdateEmployee([FromBody] BlEmployee appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.registrationYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.UpdateEmployeeDetail(appParam);
            if (rb.status)
            {
                rs.message = "Data Updated Successfully";
                rs.status = true;
                rs.value = rb.message;
            }
            else
            {
                //====Failure====
                rs.message = "Failed to Update data " + rb.message;
                rs.status = false;
            }
            return rs;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary>         

        /// <returns></returns>
        [HttpGet("getallemployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllEmployeeList()
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllEmployeeList(userId);
            return dt;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary> 
        /// <returns></returns>
        [HttpGet("getemployeebyid/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllEmployeeById(Int64 Id)
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetEmployeeById(Id);
            return dt;
        }


        /// <summary>
        ///Save Office Record
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("saveoffice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> SaveOffice([FromBody] BlOffice appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            if (appParam.OfficeId == null)
                appParam.OfficeId = 0;
            ReturnClass.ReturnBool rb = await dl.SaveOfficeDetail(appParam);
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
        ///Save Office Record
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("updateoffice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> UpdateOffice([FromBody] BlOffice appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.UpdateOfficeDetail(appParam);
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

        /// <returns></returns>
        [HttpGet("getalloffice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllOfficeList()
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllOfficeList(userId);
            return dt;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary> 
        /// <returns></returns>
        [HttpGet("getofficebyid/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllOfficeById(Int64 Id)
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetOfficeById(Id);
            return dt;
        }

        /// <summary>
        ///Save Employee Office Mapping
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("saveempofficemapping")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> SaveEmployeeOffice([FromBody] BlEmpOffice appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            if (appParam.empOfficeId == null)
                appParam.empOfficeId = 0;
            ReturnClass.ReturnBool rb = await dl.SaveEmpOfficeMapping(appParam);
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
        ///Update Employee Office Mapping
        /// </summary>
        /// <param name="appParam"></param>        
        /// <returns></returns>
        [HttpPost("updateempofficemapping")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnString> UpdateEmployeeOffice([FromBody] BlEmpOffice appParam)
        {
            DlEmployee dl = new DlEmployee();
            ReturnClass.ReturnString rs = new ReturnClass.ReturnString();
            appParam.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            appParam.userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            appParam.entryDateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ReturnClass.ReturnBool rb = await dl.UpdateEmpOfficeMapping(appParam);
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

        /// <returns></returns>
        [HttpGet("getallempofficemapping")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllEmpOfficeList()
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetAllEmployeeOfficeList(userId);
            return dt;
        }
        /// <summary>
        ///Get All HOD List
        /// </summary> 
        /// <returns></returns>
        [HttpGet("getempofficemappingebyid/{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ReturnClass.ReturnDataTable> GetAllEmpOfficeById(Int32 Id)
        {
            DlEmployee dl = new DlEmployee();
            //string clientIP = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            Int64 userId = Convert.ToInt64(User.FindFirst("userId")?.Value);
            ReturnClass.ReturnDataTable dt = await dl.GetEmployeeOfficeById(Id);
            return dt;
        }




    }
}
