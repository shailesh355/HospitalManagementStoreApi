using BaseClass;
using MySql.Data.MySqlClient;
using System.Net;
using HospitalManagementStoreApi.Models.Balayer;
using Newtonsoft.Json;
using System.Data;
using HospitalManagementStoreApi.Models.AppClass.BLayer;

namespace HospitalManagementStoreApi.Models.DaLayer
{
    public class DlCommon
    {
        readonly DBConnection db = new();
        ReturnClass.ReturnDataTable dt = new();
 

        public async Task<BlDocumentImagesModel> GetDocumentImagesPath_Async(int statecode, DocumentType dp, DocumentImageGroup dg)
        {
            ReturnClass.ReturnDataTable dt = new ReturnClass.ReturnDataTable();
            BlDocumentImagesModel blc = new BlDocumentImagesModel();
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("stateId", MySqlDbType.Int16) { Value = statecode},
                new MySqlParameter("documentType", MySqlDbType.Int16) { Value = (int)dp},
                new MySqlParameter("documentImageGroup", MySqlDbType.Int16) { Value = (int)dg},
            };
            //    string query = @"SELECT t.physicalPath, t.maxFileSizeAllowed, t.fileTypeAllowed, t.addYear, t.dptTableId, t.addFolder
            //FROM documentpathtbl t 
            //WHERE t.stateId=@stateId AND t.documentType=@documentType AND 
            //   t.documentImageGroup=@documentImageGroup";
            string query = @"SELECT t.physicalPath, t.maxFileSizeAllowed, t.fileTypeAllowed, t.addYear, t.dptTableId, t.addFolder
							 FROM documentpathtbl t 
							 WHERE t.stateId=@stateId AND t.documentType=@documentType AND 
								   t.documentImageGroup=@documentImageGroup";

            dt = await db.ExecuteSelectQueryAsync(query, pm);
            if (dt.table.Rows.Count > 0)
            {
                blc.physcialPath = dt.table.Rows[0]["physicalPath"].ToString();
                blc.maxFileSizeAllowed = Convert.ToInt64(dt.table.Rows[0]["maxFileSizeAllowed"].ToString());
                blc.fileType = dt.table.Rows[0]["fileTypeAllowed"].ToString().ToLower().Split(',');
                blc.addYear = dt.table.Rows[0]["addYear"].ToString() == "1" ? true : false; //=====If addYear is 1 then year will be added in physical path
                blc.createFolder = dt.table.Rows[0]["addFolder"].ToString() == "1" ? true : false; //=====If addFolder is 1 then Folder with document id will be created
                blc.status = true;
                blc.dptTableId = Convert.ToInt16(dt.table.Rows[0]["dptTableId"].ToString());
            }
            else
                blc.status = false;
            return blc;
        }

    }
}
