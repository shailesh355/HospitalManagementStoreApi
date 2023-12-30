using BaseClass;
using DmfPortalApi.Models.AppClass;
using HospitalManagementStoreApi.Models.AppClass.BLayer;
using HospitalManagementStoreApi.Models.AppClass.DataLayer;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static BaseClass.ReturnClass;

namespace HospitalManagementStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkDocsController : ControllerBase
    {
        ReturnBool rb = new ReturnBool();
        //private string BASE_URL = "https://localhost:7168/api/WorkDocs/";
        DlDocument DlDocumentObj = new DlDocument();

        [HttpGet("hello")]
        public async Task<string> Hello()
        {
            return "hello";
        }
        /// <summary>
        /// Save Documents related to 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        [HttpPost("uploaddocs")]
        public async Task<ReturnBool> SaveWorkDocumentsAsync([FromBody] BlDocument bl)
        {
            bl.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            rb = await DlDocumentObj.SaveDocumentsAsync(bl);
            return rb;
        }

        /// <summary>
        /// Retrive Work documents
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [HttpGet("getdocs/{documentName}/{documentType}")]
        public async Task<IActionResult> GetDocumentAsyncNew(string documentName, DocumentType documentType)
        {
            ReturnDocumentDetail rs = await DlDocumentObj.GetDocumentAsync(documentName: documentName, documentType: documentType, documentImageGroup: DocumentImageGroup.Hospital);
            if (rs.status)
            {
                if (System.IO.File.Exists(rs.filePath.Replace("D:", "C:")))
                {
                    byte[] documentData = System.IO.File.ReadAllBytes(rs.filePath.Replace("D:", "C:"));
                    return File(documentData, rs.mimeType);
                }
                else
                    return StatusCode(404);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Delete any documents
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        [HttpPost("deleteanydoc")]
        public async Task<ReturnBool> DeleteDocumentsAsync([FromBody] BlDocument bl)
        {
            bl.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            rb = await DlDocumentObj.DeleteApplicationDocumentsAsync(bl);
            return rb;
        }

        /// <summary>
        /// Save Documents related to 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        [HttpPost("uploaddocsmi")]
        public async Task<ReturnBool> SaveWorkDocumentsAsyncMi([FromBody] BlDocument bl)
        {
            bl.clientIp = Utilities.GetRemoteIPAddress(this.HttpContext, true);
            rb = await DlDocumentObj.SaveDocumentsAsyncMi(bl);
            return rb;
        }
    }
}
