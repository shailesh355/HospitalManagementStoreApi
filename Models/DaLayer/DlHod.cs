using BaseClass;
using MySql.Data.MySqlClient;
using TicketManagementApi.Models.BLayer;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlHod
    {
        readonly DBConnection db = new();
        public async Task<ReturnClass.ReturnBool> RegistorNewHodOffice(BlHod blhod)
        {
            string query = @"INSERT INTO hodofficeregistration (hodOfficeId, hodOfficeName, baseDeptId, orgType, hodOfficeLevel, hodOfficeStateId, hodOfficeDistrictId, 
                                                hodOfficeDistrictname, hodOfficeIsUrbanRural, hodOfficePinCode, hodOfficeAddress, hodOfficeEmailId, hodOfficePhoneNumber, 
                                                hodOfficeFaxNumber, hodOfficeWebsite, isRegistrationDocumentUploaded, registrationStatus, clientIp, 
                                                applicantName, applicantDesignationCode, applicantMobileNumber, applicantEmailId, isParichayLogin, applicantPassword)
                                        VALUES (@hodOfficeId, @hodOfficeName, @baseDeptId, @orgType, @hodOfficeLevel, @hodOfficeStateId, @hodOfficeDistrictId, 
                                                @hodOfficeDistrictname, @hodOfficeIsUrbanRural, @hodOfficePinCode, @hodOfficeAddress, @hodOfficeEmailId, @hodOfficePhoneNumber, 
                                                @hodOfficeFaxNumber, @hodOfficeWebsite, @isRegistrationDocumentUploaded, @registrationStatus, @clientIp, 
                                                @applicantName, @applicantDesignationCode, @applicantMobileNumber, @applicantEmailId, @isParichayLogin, @applicantPassword)";

            blhod.hodOfficeId = await GetHodOfficeRegistrationIdAsync(blhod.hodOfficeStateId);
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = blhod.hodOfficeId });
            pm.Add(new MySqlParameter("hodOfficeName", MySqlDbType.String) { Value = blhod.hodOfficeName });
            pm.Add(new MySqlParameter("baseDeptId", MySqlDbType.Int32) { Value = blhod.baseDeptId });
            pm.Add(new MySqlParameter("orgType", MySqlDbType.Int16) { Value = blhod.orgType });
            pm.Add(new MySqlParameter("hodOfficeLevel", MySqlDbType.Int16) { Value = blhod.hodOfficeLevel });
            pm.Add(new MySqlParameter("hodOfficeStateId", MySqlDbType.Int16) { Value = blhod.hodOfficeStateId });
            pm.Add(new MySqlParameter("hodOfficeDistrictId", MySqlDbType.Int32) { Value = blhod.hodOfficeDistrictId });

            pm.Add(new MySqlParameter("hodOfficeDistrictname", MySqlDbType.VarString) { Value = blhod.hodOfficeDistrictname });
            pm.Add(new MySqlParameter("hodOfficeIsUrbanRural", MySqlDbType.Int16) { Value = (int)blhod.hodOfficeIsUrbanRural });
            pm.Add(new MySqlParameter("hodOfficePinCode", MySqlDbType.Int32) { Value = blhod.hodOfficePinCode });
            pm.Add(new MySqlParameter("hodOfficeAddress", MySqlDbType.VarString) { Value = blhod.hodOfficeAddress });
            pm.Add(new MySqlParameter("hodOfficeEmailId", MySqlDbType.VarString) { Value = blhod.hodOfficeEmailId });
            pm.Add(new MySqlParameter("hodOfficePhoneNumber", MySqlDbType.VarString) { Value = blhod.hodOfficePhoneNumber });

            pm.Add(new MySqlParameter("hodOfficeFaxNumber", MySqlDbType.VarString) { Value = blhod.hodOfficeFaxNumber });
            pm.Add(new MySqlParameter("hodOfficeWebsite", MySqlDbType.VarString) { Value = blhod.hodOfficeWebsite });
            pm.Add(new MySqlParameter("isRegistrationDocumentUploaded", MySqlDbType.Int16) { Value = (int)blhod.isRegistrationDocumentUploaded });
            pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (int)blhod.registrationStatus });
            pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blhod.clientIp });

            pm.Add(new MySqlParameter("applicantName", MySqlDbType.VarString) { Value = blhod.applicantName });
            pm.Add(new MySqlParameter("applicantDesignationCode", MySqlDbType.Int16) { Value = blhod.applicantDesignationCode });
            pm.Add(new MySqlParameter("applicantMobileNumber", MySqlDbType.Int64) { Value = blhod.applicantMobileNumber });
            pm.Add(new MySqlParameter("applicantEmailId", MySqlDbType.VarString) { Value = blhod.applicantEmailId });
            pm.Add(new MySqlParameter("isParichayLogin", MySqlDbType.Int16) { Value = (int)blhod.isParichayLogin });
            pm.Add(new MySqlParameter("applicantPassword", MySqlDbType.VarString) { Value = blhod.applicantPassword });

            ReturnClass.ReturnBool rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "HodRegistration");
            return rb;
        }
        /// <summary>
        /// Returns 11 digit HodOfficeCode based on StateCode
        /// </summary>
        /// <param name="hodOfficeStateId"></param>
        /// <returns></returns>
        private async Task<long> GetHodOfficeRegistrationIdAsync(int hodOfficeStateId)
        {
            string query = @"SELECT LPAD(IFNULL(MAX(h.officeCount),0) + 1, 7, 0) AS officeCount
                             FROM hodofficeregistration h
                             WHERE h.hodOfficeStateId = @hodOfficeStateId";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hodOfficeStateId", MySqlDbType.Int16) { Value = hodOfficeStateId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                string officeCount = dt.table.Rows[0]["officeCount"].ToString();
                long hodOfficeId = Convert.ToInt64(DefaultValues.HodOfficePrefix.ToString() + hodOfficeStateId.ToString() + officeCount);
                return hodOfficeId;
            }
            else
                return 0;
        }

        public async Task<bool> CheckEmailExistAsync(string emailId)
        {
            bool isAccountExists = true;
            string query = @"SELECT u.emailId
                            FROM userlogin u
                            WHERE u.emailId = @emailId ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("emailId", MySqlDbType.VarString) { Value = emailId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count < 1)
            {
                isAccountExists = false;

                query = @"SELECT h.applicantEmailId
                          FROM hodofficeregistration h
                          where h.applicantEmailId = @emailId AND h.registrationStatus != @registrationStatus ";
                pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (int)RegistrationStatus.Rejected });
                dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());

                if (dt.table.Rows.Count > 0)
                    isAccountExists = true;
            }
            return isAccountExists;
        }
    }
}
