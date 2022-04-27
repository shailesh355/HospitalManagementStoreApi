using BaseClass;
using MySql.Data.MySqlClient;

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
    }
}
