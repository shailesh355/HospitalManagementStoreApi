using BaseClass;
using MySql.Data.MySqlClient;
using System.Net;
using TicketManagementApi.Models.Balayer;
using Newtonsoft.Json;
using System.Data;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlCommon
    {
        readonly DBConnection db = new();
        ReturnClass.ReturnDataTable dt = new();

        public async Task<List<ListValue>> GetStateAsync(LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            dt = await db.ExecuteSelectQueryAsync(@"SELECT  s.stateId as id, s.stateName" + fieldLanguage + @" as name
                                                    FROM state s
                                                    ORDER BY name");
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        /// <summary>
        /// Get List of District
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListValue>> GetDistrictAsync(int stateId, LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.districtId AS id, d.districtName" + fieldLanguage + @" AS name
                             FROM district d
                             WHERE d.stateId = @stateId
                             ORDER BY name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("stateId", MySqlDbType.Int16) { Value= stateId }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        public async Task<List<ListValue>> GetBaseDepartmentAsync(int stateId, LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT b.deptId AS id, b.deptName" + fieldLanguage + @" as name 
                             FROM basedepartment b
                             WHERE b.stateId = @stateId and b.active = @active
                             ORDER BY name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("active", MySqlDbType.Int16) { Value = (int) Active.Yes },
                new MySqlParameter("stateId", MySqlDbType.Int16) { Value = stateId }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        #region Get Common List from DDL Cat List
        /// <summary>
        ///Get Category List from ddlCat
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListValue>> GetCommonListAsync(string category, LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.id AS id, d.name" + fieldLanguage + @" AS name, d.grouping as extraField
                             FROM ddlcatlist d
                             WHERE d.active = @active AND d.category = @category AND d.hideFromPublicAPI = @hideFromPublicAPI AND d.isStateSpecific=@isStateSpecific
                             ORDER BY d.sortOrder, name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("hideFromPublicAPI", MySqlDbType.Int16){ Value=(int) YesNo.No},
                new MySqlParameter("active", MySqlDbType.Int16){ Value = (int) Active.Yes},
                new MySqlParameter("isStateSpecific", MySqlDbType.Int16){ Value= (int)YesNo.No},
                new MySqlParameter("category", MySqlDbType.String) { Value= category }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        /// <summary>
        ///Get Sub category List from ddlCat
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListValue>> GetSubCommonListAsync(string category, string id, LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.id AS id, d.name" + fieldLanguage + @" AS name, d.grouping AS extraField
                             FROM ddlcatlist d
                             WHERE d.active = @active AND d.category = @category AND d.referenceId=@referenceId AND d.hideFromPublicAPI = @hideFromPublicAPI AND 
                                   d.isStateSpecific=@isStateSpecific
                             ORDER BY d.sortOrder, name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("hideFromPublicAPI", MySqlDbType.Int16){ Value=(int) YesNo.No},
                new MySqlParameter("active", MySqlDbType.Int16){ Value = (int) Active.Yes},
                new MySqlParameter("isStateSpecific", MySqlDbType.Int16){ Value= (int)YesNo.No},
                new MySqlParameter("category", MySqlDbType.String) { Value= category }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }

        /// <summary>
        ///Get State Specific Category List from ddlCat
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListValue>> GetCommonListStateAsync(string category, LanguageSupported language, int stateId)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.id AS id, d.name" + fieldLanguage + @" AS name, d.grouping as extraField
                             FROM ddlcatlist d
                             WHERE d.active = @active AND d.category = @category AND d.hideFromPublicAPI = @hideFromPublicAPI AND d.isStateSpecific=@isStateSpecific AND
                                   d.stateId = @stateId
                             ORDER BY d.sortOrder, name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("hideFromPublicAPI", MySqlDbType.Int16){ Value=(int) YesNo.No},
                new MySqlParameter("active", MySqlDbType.Int16){ Value = (int) Active.Yes},
                new MySqlParameter("isStateSpecific", MySqlDbType.Int16){ Value= (int)YesNo.Yes},
                new MySqlParameter("category", MySqlDbType.String) { Value= category },
                new MySqlParameter("stateId", MySqlDbType.Int16) { Value= stateId }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        #endregion
        public async Task<List<ListValue>> GetDesignationAsync(int stateId, LanguageSupported language)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.designationId, designationName" + fieldLanguage + @" AS name,
                             FROM designation d
                             WHERE d.stateId = @stateId
                             ORDER BY name";
            MySqlParameter[] pm = new MySqlParameter[]{
                new MySqlParameter("stateId", MySqlDbType.Int16){ Value = stateId }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
        /// <summary>
        /// Verify captcha 
        /// </summary>
        /// <returns></returns>
        public ReturnClass.ReturnBool VerifyCaptcha(CaptchaReturnType ct, string verification_url)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            Uri url = new Uri(verification_url);
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            string serialisedData = JsonConvert.SerializeObject(ct);
            try
            {
                var response = client.UploadString(url, serialisedData);
                dynamic jsonData = Newtonsoft.Json.Linq.JObject.Parse(response);
                rb.status = jsonData.status;
                rb.message = jsonData.message;
            }
            catch (Exception ex)
            {
                WriteLog.Error("VerifyCaptcha", ex);
            }
            return rb;

        }
        /// <summary>
        /// insert an entry for each login attempt regardless of successfull login or failure.
        /// </summary>
        /// <param name="lt"></param>
        /// <returns></returns>
        public async Task<ReturnClass.ReturnBool> InsertLoginTrail(LoginTrail lt)
        {
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("loginId", MySqlDbType.String) { Value = lt.loginId},
                new MySqlParameter("browserId", MySqlDbType.String){ Value=lt.browserId},
                new MySqlParameter("clientIp", MySqlDbType.String) { Value = lt.clientIp},
                new MySqlParameter("clientOs", MySqlDbType.String) { Value = lt.clientOs},
                new MySqlParameter("clientBrowser", MySqlDbType.String) { Value = lt.clientBrowser},
                new MySqlParameter("userAgent", MySqlDbType.String) { Value = lt.userAgent},
                new MySqlParameter("accessMmode", MySqlDbType.Int16) { Value = lt.siteAccessMode},
                new MySqlParameter("isLoginSuccessful", MySqlDbType.Int16) { Value = lt.isLoginSuccessful},
            };

            string query = @" INSERT INTO logintrail(loginId, browserId, clientIp, clientOs, clientBrowser, userAgent, accessMode, isLoginSuccessful)
                                VALUES(@loginId, @browserId, @clientIp, @clientOs, @clientBrowser, @userAgent, @accessMode, @isLoginSuccessful)";

            ReturnClass.ReturnBool succeded = await db.ExecuteQueryAsync(query, pm, "Logintrail");

            if (succeded.status)
                succeded.message = "Login trail created successfully";
            else
                succeded.message = "Could not create login trail";

            return succeded;
        }
        /// <summary>
        ///get User Detail During Login
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUser(string emailid, string password, bool isSwsUser)
        {
            User user = new User();
            user.message = "Invalid user id or Password";

            YesNo isSingleWindowUser;
            YesNo changePassword;
            YesNo isDisabled;
            try
            {
                MySqlParameter[] pm = new MySqlParameter[]
                {
                    new MySqlParameter("emailid",MySqlDbType.String) { Value = emailid},
                    new MySqlParameter("password",MySqlDbType.String) { Value = password},
                    new MySqlParameter("active",MySqlDbType.Int16) { Value = (int)Active.Yes}
                };
                string where = @"  AND l.password = @password ";
                string query = @" SELECT l.userName, l.userId, l.changePassword, l.isDisabled, l.userRole, l.isSingleWindowUser
                                  FROM userlogin l
                                  WHERE l.emailId=@emailId AND l.active = @active ";
                query = !isSwsUser ? query + where : query;

                dt = await db.ExecuteSelectQueryAsync(query, pm);
                if (dt.table.Rows.Count > 0)
                {
                    DataRow dr = dt.table.Rows[0];
                    user.userId = Convert.ToInt64(dr["userId"]);
                    user.userName = dr["userName"].ToString();
                    user.role = dr["userRole"].ToString();

                    Enum.TryParse(dr["isSingleWindowUser"].ToString(), true, out isSingleWindowUser);
                    user.isSingleWindowUser = isSingleWindowUser;

                    Enum.TryParse(dr["changePassword"].ToString(), true, out changePassword);
                    user.forceChangePassword = changePassword;

                    Enum.TryParse(dr["isDisabled"].ToString(), true, out isDisabled);
                    if (isDisabled == YesNo.Yes)
                        user.message = "Account has been disabled";
                    else
                    {
                        user.isAuthenticated = true;
                        user.message = "Login successfull";
                    }


                }
            }
            catch (Exception ex)
            {
                WriteLog.Error("DlCommon(GetUser) : ", ex);
            }
            return user;
        }

    }
}
