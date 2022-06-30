using BaseClass;
using MySql.Data.MySqlClient;
using System.Transactions;
using TicketManagementApi.Models.BLayer;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlHod
    {
        readonly DBConnection db = new();
        public async Task<ReturnClass.ReturnBool> RegistorNewHodOffice(BlHod blhod)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckHodOffice(blhod);

            if (!isofficeExists)
            {
                isofficeExists = await CheckEmailExistAsync(blhod.applicantEmailId);
                if (!isofficeExists)
                {
                    string query = @"INSERT INTO hodofficeregistration (hodOfficeId, hodOfficeName,officeCount, baseDeptId, orgType, hodOfficeLevel, hodOfficeStateId, hodOfficeDistrictId, 
                                                hodOfficeDistrictname, hodOfficeIsUrbanRural, hodOfficePinCode, hodOfficeAddress, hodOfficeEmailId, hodOfficePhoneNumber, 
                                                hodOfficeFaxNumber, hodOfficeWebsite, isRegistrationDocumentUploaded, registrationStatus, clientIp, 
                                                applicantName, applicantDesignationCode, applicantMobileNumber, applicantEmailId, isParichayLogin, applicantPassword)
                                        VALUES (@hodOfficeId, @hodOfficeName,@officeCount, @baseDeptId, @orgType, @hodOfficeLevel, @hodOfficeStateId, @hodOfficeDistrictId, 
                                                @hodOfficeDistrictname, @hodOfficeIsUrbanRural, @hodOfficePinCode, @hodOfficeAddress, @hodOfficeEmailId, @hodOfficePhoneNumber, 
                                                @hodOfficeFaxNumber, @hodOfficeWebsite, @isRegistrationDocumentUploaded, @registrationStatus, @clientIp, 
                                                @applicantName, @applicantDesignationCode, @applicantMobileNumber, @applicantEmailId, @isParichayLogin, @applicantPassword)";
                    ReturnClass.ReturnDataTable dt = await GetHodOfficeRegistrationIdAsync(blhod.hodOfficeStateId);
                    if (dt.table.Rows.Count > 0)
                    {
                        blhod.hodOfficeId = Convert.ToInt64(dt.table.Rows[0]["hodOfficeId"].ToString());
                        blhod.officeCount = Convert.ToInt32(dt.table.Rows[0]["officeCount"].ToString());
                    }
                    blhod.isParichayLogin = (int)YesNo.No;
                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = blhod.hodOfficeId });
                    pm.Add(new MySqlParameter("hodOfficeName", MySqlDbType.String) { Value = blhod.hodOfficeName });
                    pm.Add(new MySqlParameter("baseDeptId", MySqlDbType.Int32) { Value = blhod.baseDeptId });
                    pm.Add(new MySqlParameter("orgType", MySqlDbType.Int16) { Value = blhod.orgType });
                    pm.Add(new MySqlParameter("hodOfficeLevel", MySqlDbType.Int16) { Value = blhod.hodOfficeLevel });
                    pm.Add(new MySqlParameter("hodOfficeStateId", MySqlDbType.Int16) { Value = blhod.hodOfficeStateId });
                    pm.Add(new MySqlParameter("officeCount", MySqlDbType.Int32) { Value = blhod.officeCount });
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
                    pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (Int16)RegistrationStatus.Pending });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blhod.clientIp });
                    pm.Add(new MySqlParameter("applicantName", MySqlDbType.VarString) { Value = blhod.applicantName });
                    pm.Add(new MySqlParameter("applicantDesignationCode", MySqlDbType.Int16) { Value = blhod.applicantDesignationCode });
                    pm.Add(new MySqlParameter("applicantMobileNumber", MySqlDbType.Int64) { Value = blhod.applicantMobileNumber });
                    pm.Add(new MySqlParameter("applicantEmailId", MySqlDbType.VarString) { Value = blhod.applicantEmailId });
                    pm.Add(new MySqlParameter("isParichayLogin", MySqlDbType.Int16) { Value = (int)blhod.isParichayLogin });
                    pm.Add(new MySqlParameter("applicantPassword", MySqlDbType.VarString) { Value = blhod.applicantPassword });
                    rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "HodRegistration");
                }
                else
                {
                    rb.message = "Applicant Email-Id has Already Used For Registration!!";
                }
            }
            else
            {
                rb.message = "This Department has Already Applied For Registration!!";
            }
            return rb;
        }
        /// <summary>
        /// Returns 11 digit HodOfficeCode based on StateCode
        /// </summary>
        /// <param name="hodOfficeStateId"></param>
        /// <returns></returns>
        private async Task<ReturnClass.ReturnDataTable> GetHodOfficeRegistrationIdAsync(int hodOfficeStateId)
        {
            string query = @"SELECT LPAD(IFNULL(MAX(h.officeCount),0) + 1, 7, 0) AS SNO,(IFNULL(MAX(h.officeCount),0) + 1) AS  officeCount,
                                '0' AS hodOfficeId
                             FROM hodofficeregistration h
                             WHERE h.hodOfficeStateId = @hodOfficeStateId";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hodOfficeStateId", MySqlDbType.Int16) { Value = hodOfficeStateId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                string officeCount = dt.table.Rows[0]["SNO"].ToString();
                dt.table.Rows[0]["hodOfficeId"] = Convert.ToInt64(DefaultValues.HodOfficePrefix.ToString() + hodOfficeStateId.ToString() + officeCount).ToString();
                return dt;
            }
            else
                return dt;
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
                pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (int)RegistrationStatus.Reject });
                dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());

                if (dt.table.Rows.Count > 0)
                    isAccountExists = true;
            }
            return isAccountExists;
        }
        public async Task<ReturnClass.ReturnDataTable> GetAllHODList(Int16 vid, Int16 rid)
        {
            string query = @"SELECT h.hodOfficeId ,h.officeCount,h.hodOfficeName,h.baseDeptId,h.orgType,h.hodOfficeLevel,
                                     h.hodOfficeStateId,h.hodOfficeDistrictId,h.hodOfficeDistrictname,h.hodOfficeIsUrbanRural,h.hodOfficeAddress,
                                     h.hodOfficePinCode,h.hodOfficeEmailId,h.hodOfficePhoneNumber,h.hodOfficeFaxNumber,h.hodOfficeWebsite,
                                     h.isRegistrationDocumentUploaded,h.isVerified,h.verificationDate,h.registrationStatus,h.registrationDate,
                                    h.applicantName,h.applicantDesignationCode,h.applicantMobileNumber,h.applicantEmailId,h.applicantEmailId,
                                    h.isParichayLogin
                            FROM hodofficeregistration h WHERE h.isVerified=@isVerified AND registrationStatus=@registrationStatus";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("isVerified", MySqlDbType.Int16) { Value = vid });
            pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = rid });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<ReturnClass.ReturnDataTable> GetAllHODListById(Int64 hodOfficeId)
        {
            string query = @"SELECT h.hodOfficeId ,h.hodOfficeName,h.baseDeptId,b.deptNameEnglish,h.orgType,orgeType.nameEnglish AS orgTypeName,h.hodOfficeLevel,
                                     OfficeLevel.nameEnglish AS OfficeLevelName,h.hodOfficeStateId,s.stateNameEnglish AS StateName,h.hodOfficeDistrictId,
									h.hodOfficeDistrictname,h.hodOfficeIsUrbanRural,h.hodOfficeAddress,
                                     h.hodOfficePinCode,h.hodOfficeEmailId,h.hodOfficePhoneNumber,h.hodOfficeFaxNumber,h.hodOfficeWebsite,
                                     h.isRegistrationDocumentUploaded,h.isVerified,h.verificationDate,h.registrationStatus,h.registrationDate,
                                    h.applicantName,h.applicantDesignationCode,ds.designationNameEnglish AS designationName,h.applicantMobileNumber,
									h.applicantEmailId,h.isParichayLogin
                            FROM hodofficeregistration AS  h 
									 JOIN  basedepartment AS b ON b.deptId=h.baseDeptId AND h.hodOfficeStateId=b.stateId
									 JOIN ddlcatlist AS orgeType ON  orgeType.category='organizationType' AND orgeType.id=h.orgType
									 JOIN ddlcatlist AS OfficeLevel ON  OfficeLevel.category='officeLevel' AND OfficeLevel.id=h.hodOfficeLevel
									 JOIN designation AS ds ON  ds.designationId=h.applicantDesignationCode AND ds.stateId=h.hodOfficeStateId
									 JOIN  state AS s ON s.stateId=h.hodOfficeStateId WHERE h.hodOfficeId=@hodOfficeId ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = hodOfficeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<ReturnClass.ReturnBool> VerifyHodOffice(Verification blhod)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            Int32 countData = 0;
            if (blhod.verificationHods.Count != 0)
            {
                foreach (var item in blhod.verificationHods)
                {
                    bool isofficeExists = await CheckVerifyHodOffice(item.hodOfficeId);
                    if (!isofficeExists)
                    {
                        string query = @"UPDATE hodofficeregistration 
                             SET isVerified=@isVerified,clientIp=@clientIp,verificationDate=@verificationDate,
                                verifiedByLoginId=@verifiedByLoginId,registrationStatus=@registrationStatus 
                              WHERE hodOfficeId=@hodOfficeId";
                        if (item.registrationStatus == RegistrationStatus.Approved)
                        {
                            item.isVerified = YesNo.Yes;
                        }
                        else
                        {
                            item.isVerified = YesNo.No;
                        }

                        List<MySqlParameter> pm = new();
                        pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = item.hodOfficeId });
                        pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (int)item.registrationStatus });
                        pm.Add(new MySqlParameter("isVerified", MySqlDbType.Int16) { Value = (int)item.isVerified });
                        pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blhod.clientIp });
                        pm.Add(new MySqlParameter("verifiedByLoginId", MySqlDbType.Int64) { Value = blhod.userId });
                        pm.Add(new MySqlParameter("active", MySqlDbType.Int16) { Value = (int)item.isVerified });
                        pm.Add(new MySqlParameter("officeMappingKey", MySqlDbType.Int32) { Value = 0 });
                        pm.Add(new MySqlParameter("registrationDate", MySqlDbType.String) { Value = blhod.date });
                        pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = DateTime.Now.Date.Year });
                        pm.Add(new MySqlParameter("changePassword", MySqlDbType.Int16) { Value = (int)Active.No });
                        pm.Add(new MySqlParameter("active1", MySqlDbType.Int16) { Value = (int)Active.Yes });
                        pm.Add(new MySqlParameter("isDisabled", MySqlDbType.Int16) { Value = (int)Active.No });
                        pm.Add(new MySqlParameter("userRole", MySqlDbType.Int16) { Value = (int)UserRole.Nodal });
                        pm.Add(new MySqlParameter("isSingleWindowUser", MySqlDbType.Int16) { Value = (int)Active.No });
                        pm.Add(new MySqlParameter("modificationType", MySqlDbType.Int16) { Value = (int)Active.No });
                        pm.Add(new MySqlParameter("userTypeCode", MySqlDbType.Int16) { Value = (int)Active.No });
                        using (TransactionScope ts = new TransactionScope())
                        {
                            rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "VerifyHodOffice");
                            if (rb.status == true && item.registrationStatus == RegistrationStatus.Approved && item.isVerified == YesNo.Yes)
                            {
                                query = @"INSERT INTO userlogin
                                            (userName,userId,emailId,password,changePassword,active,isDisabled,
                                            clientIp,userRole,registrationYear,isSingleWindowUser,modificationType,userTypeCode)
                                        SELECT h.applicantName,h.hodOfficeId,h.applicantEmailId,h.applicantPassword,@changePassword,
                                        @active1,@isDisabled,@clientIp,@userRole,@registrationYear,@isSingleWindowUser,
                                        @modificationType,@userTypeCode
                                            FROM  hodofficeregistration h 
                                          WHERE h.hodOfficeId=@hodOfficeId";
                                rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "InsertUserLogin");
                                if (rb.status)
                                {
                                    query = @"INSERT INTO  hodoffice (hodOfficeId,hodOfficeName,baseDeptId,orgType,hodOfficeLevel,hodOfficeStateId,
                                                hodOfficeDistrictId,hodOfficeDistrictname,hodOfficePinCode,hodOfficeAddress,hodOfficeEmailId,
                                                hodOfficePhoneNumber,hodOfficeFaxNumber,hodOfficeWebsite,currentlyActiveHodMappingKey,
                                                loginId,active,clientIp,registrationDate) 
                              SELECT h.hodOfficeId,h.hodOfficeName,h.baseDeptId,h.orgType,h.hodOfficeLevel,h.hodOfficeStateId,
                                    h.hodOfficeDistrictId,h.hodOfficeDistrictname,h.hodOfficePinCode,h.hodOfficeAddress,h.hodOfficeEmailId,
                                   h.hodOfficePhoneNumber,h.hodOfficeFaxNumber,h.hodOfficeWebsite,@officeMappingKey,h.hodOfficeId,
                                   @active,@clientIp,@registrationDate                        
                               FROM  hodofficeregistration h 
                               WHERE h.hodOfficeId=@hodOfficeId";
                                    rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "InsertHodOffice");
                                    if (rb.status)
                                    {
                                        ts.Complete();
                                        countData = countData + 1;
                                    }
                                    else
                                    {
                                        rb.status = false;
                                    }
                                }

                            }
                            else if (rb.status == true && item.registrationStatus == RegistrationStatus.Reject)
                            {
                                ts.Complete();
                                countData = countData + 1;
                            }


                        }
                    }
                    else
                    {
                        countData = 0;
                        rb.message = "This Department has Already Verified!!";
                    }
                }

                if (blhod.verificationHods.Count == countData)
                {
                    rb.status = true;
                }
                else
                {
                    rb.status = false;
                }
            }
            else
            {
                
                rb.message = "Department Data Empty!!";
            }
            return rb;
        }

        public async Task<bool> CheckVerifyHodOffice(Int64 hodOfficeId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT h.hodOfficeId
                            FROM hodoffice h
                            WHERE h.hodOfficeId = @hodOfficeId  ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = hodOfficeId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }

        public async Task<bool> CheckHodOffice(BlHod blhod)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT h.hodOfficeId
                            FROM hodofficeregistration h
                            WHERE h.baseDeptId = @baseDeptId AND h.orgType=@orgType AND h.hodOfficeLevel=@hodOfficeLevel 
                            AND h.hodOfficeStateId=@hodOfficeStateId AND h.hodOfficeDistrictId=@hodOfficeDistrictId  
                            and h.registrationStatus!=registrationStatus ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("baseDeptId", MySqlDbType.Int32) { Value = blhod.baseDeptId });
            pm.Add(new MySqlParameter("orgType", MySqlDbType.Int16) { Value = blhod.orgType });
            pm.Add(new MySqlParameter("hodOfficeLevel", MySqlDbType.Int16) { Value = blhod.hodOfficeLevel });
            pm.Add(new MySqlParameter("hodOfficeStateId", MySqlDbType.Int16) { Value = blhod.hodOfficeStateId });
            pm.Add(new MySqlParameter("hodOfficeDistrictId", MySqlDbType.Int32) { Value = blhod.hodOfficeDistrictId });
            pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (int)RegistrationStatus.Reject });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }
    }
}
