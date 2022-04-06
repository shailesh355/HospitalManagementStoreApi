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
        /// <summary>
        ///Get Category List from ddlCat
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListValue>> GetCommonListAsync(string category, LanguageSupported language, YesNo hideFromPublicAPI)
        {
            string fieldLanguage = language == LanguageSupported.Hindi ? "Local" : "English";
            string query = @"SELECT d.id AS id, d.name" + fieldLanguage + @" AS name, d.remark as extraField , d.grouping
                             FROM ddlcatlist d
                             WHERE d.active = @active AND d.category = @category AND d.hideFromPublicAPI = @hideFromPublicAPI
                             ORDER BY d.sortOrder, name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("hideFromPublicAPI", MySqlDbType.Int16){ Value=hideFromPublicAPI},
                new MySqlParameter("active", MySqlDbType.Int16){ Value= Active.Yes},
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
                             WHERE d.active = @active AND d.category = @category AND d.referenceId=@referenceId
                             ORDER BY d.sortOrder, name";
            MySqlParameter[] pm = new MySqlParameter[]
            {
                new MySqlParameter("referenceId", MySqlDbType.VarChar){ Value=id},
                new MySqlParameter("active", MySqlDbType.Int16){ Value= Active.Yes},
                new MySqlParameter("category", MySqlDbType.String) { Value= category }
            };
            dt = await db.ExecuteSelectQueryAsync(query, pm);
            List<ListValue> lv = Helper.GetGenericDropdownList(dt.table);
            return lv;
        }
    }
}
