using BaseClass;
using MySql.Data.MySqlClient;
using System.Transactions;
using TicketManagementApi.Models.BLayer;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlEmployee
    {
        readonly DBConnection db = new();
        public async Task<ReturnClass.ReturnBool> SaveEmployeeDetail(BlEmployee blEmployee)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckEmployeeMobileExists(blEmployee.mobileNo, "INSERT", (Int64)blEmployee.employeeId);

            if (!isofficeExists)
            {
                isofficeExists = await CheckEmployeeMobileExists(blEmployee.emailId, "INSERT", (Int64)blEmployee.employeeId);
                if (!isofficeExists)
                {
                    string query = @"INSERT INTO employeemaster (stateId,employeeId,employeeName,mobileNo,emailId,workingStatus,
                                                recruitmentType,userId,clientIp,entryDateTime, registrationYear)
                                        VALUES (@stateId,@employeeId,@employeeName,@mobileNo,@emailId,@workingStatus,
                                                @recruitmentType,@userId,@clientIp,@entryDateTime,@registrationYear)";

                    blEmployee.employeeId = await GetEmployeeId((int)blEmployee.registrationYear);

                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = blEmployee.employeeId });
                    pm.Add(new MySqlParameter("stateId", MySqlDbType.Int16) { Value = blEmployee.stateId });
                    pm.Add(new MySqlParameter("employeeName", MySqlDbType.String) { Value = blEmployee.employeeName });
                    pm.Add(new MySqlParameter("mobileNo", MySqlDbType.String) { Value = blEmployee.mobileNo });
                    pm.Add(new MySqlParameter("emailId", MySqlDbType.String) { Value = blEmployee.emailId });
                    pm.Add(new MySqlParameter("workingStatus", MySqlDbType.Int16) { Value = blEmployee.workingStatus });
                    pm.Add(new MySqlParameter("recruitmentType", MySqlDbType.Int16) { Value = blEmployee.recruitmentType });
                    pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blEmployee.userId });
                    pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blEmployee.entryDateTime });
                    pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = (int)blEmployee.registrationYear });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blEmployee.clientIp });

                    rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "employeeDetail");
                }
                else
                {
                    rb.message = "Employee Email-Id has Already Used!!";
                }
            }
            else
            {
                rb.message = "Employee Mobile number has Already Used!!";
            }
            return rb;
        }

        public async Task<ReturnClass.ReturnBool> UpdateEmployeeDetail(BlEmployee blEmployee)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckEmployeeMobileExists(blEmployee.mobileNo, "UPDATE", (Int64)blEmployee.employeeId);

            if (!isofficeExists)
            {
                isofficeExists = await CheckEmployeeMobileExists(blEmployee.emailId, "UPDATE", (Int64)blEmployee.employeeId);
                if (!isofficeExists)
                {
                    string query = @"INSERT INTO employeemasterlog
                                    SELECT * FROM employeemaster e
                                        WHERE e.employeeId=@employeeId";
                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = blEmployee.employeeId });
                    pm.Add(new MySqlParameter("stateId", MySqlDbType.Int16) { Value = blEmployee.stateId });
                    pm.Add(new MySqlParameter("employeeName", MySqlDbType.String) { Value = blEmployee.employeeName });
                    pm.Add(new MySqlParameter("mobileNo", MySqlDbType.String) { Value = blEmployee.mobileNo });
                    pm.Add(new MySqlParameter("emailId", MySqlDbType.String) { Value = blEmployee.emailId });
                    pm.Add(new MySqlParameter("workingStatus", MySqlDbType.Int16) { Value = blEmployee.workingStatus });
                    pm.Add(new MySqlParameter("recruitmentType", MySqlDbType.Int16) { Value = blEmployee.recruitmentType });
                    pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blEmployee.userId });
                    pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blEmployee.entryDateTime });
                    pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = (int)blEmployee.registrationYear });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blEmployee.clientIp });

                    using (TransactionScope ts = new TransactionScope())
                    {
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "employeelog");
                        if (rb.status == true)
                        {
                            query = @"UPDATE employeemaster 
                               SET stateId=@stateId,employeeName=@employeeName,mobileNo=@mobileNo,emailId=@emailId,
                               workingStatus=@workingStatus,recruitmentType=@recruitmentType,userId=@userId,clientIp=@clientIp,
                                entryDateTime=@entryDateTime 
                                WHERE employeeId=@employeeId";
                            rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "employeeUpdate");
                            if (rb.status == true)
                            {
                                ts.Complete();
                            }
                        }
                        else
                        {
                            rb.message = "Record Not Updated!!";
                        }
                    }
                }
                else
                {
                    rb.message = "Employee Email-Id has Already Used!!";
                }
            }
            else
            {
                rb.message = "Employee Mobile number has Already Used!!";
            }
            return rb;
        }

        /// <summary>
        /// for EmployeeId generation 12 digit
        /// </summary>
        /// <returns></returns>
        public async Task<Int64> GetEmployeeId(int registrationYear)
        {
            string employeeId = "0";
            try
            {
                string qr = @"SELECT IFNULL(MAX(SUBSTRING(ur.employeeId,6,12)),0) + 1 AS employeeId
								FROM employeemaster ur 
							WHERE ur.registrationYear = @registrationYear";

                List<MySqlParameter> pm = new();
                pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = registrationYear });

                ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(qr, pm.ToArray());
                if (dt.table.Rows.Count > 0)
                {
                    employeeId = dt.table.Rows[0]["employeeId"].ToString();
                    employeeId = ((int)idPrefix.employeeId).ToString() + registrationYear.ToString() + employeeId.PadLeft(7, '0');
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Convert.ToInt64(employeeId);
        }

        public async Task<ReturnClass.ReturnDataTable> GetAllEmployeeList(Int64 userId )
        {
            string query = @"SELECT e.stateId,e.employeeId,e.employeeName,e.mobileNo,e.emailId,e.workingStatus,
                                        e.recruitmentType,e.registrationYear,
									workingStatus.nameEnglish AS workingStatusName,recruitmentType.nameEnglish AS recruitmentTypeName
                            FROM employeemaster e
                            JOIN ddlcatlist AS workingStatus ON  workingStatus.category='workingStatus' AND workingStatus.id=e.workingStatus
							JOIN ddlcatlist AS recruitmentType ON  recruitmentType.category='recruitmentType' 
                            AND recruitmentType.id=e.recruitmentType 
                            WHERE e.userId=@userId";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = userId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<ReturnClass.ReturnDataTable> GetEmployeeById(Int64 employeeId)
        {
            string query = @"SELECT e.stateId,e.employeeId,e.employeeName,e.mobileNo,e.emailId,e.workingStatus,
                                        e.recruitmentType,e.userId,e.clientIp,e.entryDateTime,e.registrationYear,
									workingStatus.nameEnglish AS workingStatusName,recruitmentType.nameEnglish AS recruitmentTypeName
                            FROM employeemaster e
                            JOIN ddlcatlist AS workingStatus ON  workingStatus.category='workingStatus' AND workingStatus.id=e.workingStatus
							JOIN ddlcatlist AS recruitmentType ON  recruitmentType.category='recruitmentType' AND recruitmentType.id=e.recruitmentType
                    WHERE e.employeeId=@employeeId ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = employeeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<bool> CheckEmployeeMobileExists(string mobileNo, string transType, Int64 employeeId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT e.employeeId
                            FROM employeemaster e
                            WHERE e.mobileNo=@mobileNo ";
            if (transType == "UPDATE")
            {
                query = query + " AND e.employeeId!=@employeeId ";
            }
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("mobileNo", MySqlDbType.VarChar) { Value = mobileNo });
            pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = employeeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }
        public async Task<bool> CheckEmployeeEmailExists(string emailId, string transType, Int64 employeeId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT e.employeeId
                            FROM employeemaster e
                            WHERE e.emailId=@emailId";
            if (transType == "UPDATE")
            {
                query = query + " AND e.employeeId!=@employeeId ";
            }
            List<MySqlParameter> pm = new();

            pm.Add(new MySqlParameter("emailId", MySqlDbType.VarChar) { Value = emailId });
            pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = employeeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }

        public async Task<ReturnClass.ReturnBool> SaveOfficeDetail(BlOffice blOffice)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckOfficeMobileExists(blOffice.officePhoneNumber, "INSERT", (Int64)blOffice.hodOfficeId, (Int64)blOffice.OfficeId);

            if (!isofficeExists)
            {
                isofficeExists = await CheckOfficeMobileExists(blOffice.officeEmailId, "INSERT", (Int64)blOffice.hodOfficeId, (Int64)blOffice.OfficeId);
                if (!isofficeExists)
                {
                    string query = @"INSERT INTO office (hodOfficeId,officeId,officeName,baseDeptId,officeLevel,districtId,
                                                districtname,urbanRural,pinCode,officeAddress, officeEmailId,officePhoneNumber,userId,
                                            entryDateTime,clientIp)
                                        VALUES (@hodOfficeId,@officeId,@officeName,@baseDeptId,@officeLevel,@districtId,
                                                @districtname,@urbanRural,@pinCode,@officeAddress,@officeEmailId,@officePhoneNumber,@userId,
                                            @entryDateTime,@clientIp)";

                    blOffice.OfficeId = await GetOfficeId((Int64)blOffice.hodOfficeId);

                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = blOffice.hodOfficeId });
                    pm.Add(new MySqlParameter("officeId", MySqlDbType.Int16) { Value = blOffice.OfficeId });
                    pm.Add(new MySqlParameter("officeName", MySqlDbType.String) { Value = blOffice.OfficeName });
                    pm.Add(new MySqlParameter("baseDeptId", MySqlDbType.String) { Value = blOffice.baseDeptId });
                    pm.Add(new MySqlParameter("officeLevel", MySqlDbType.String) { Value = blOffice.officeLevel });
                    pm.Add(new MySqlParameter("districtId", MySqlDbType.Int16) { Value = blOffice.districtId });
                    pm.Add(new MySqlParameter("districtname", MySqlDbType.String) { Value = blOffice.districtname });
                    pm.Add(new MySqlParameter("urbanRural", MySqlDbType.Int16) { Value = blOffice.urbanRural });
                    pm.Add(new MySqlParameter("officeAddress", MySqlDbType.String) { Value = blOffice.officeAddress });
                    pm.Add(new MySqlParameter("officeEmailId", MySqlDbType.String) { Value = blOffice.officeEmailId });
                    pm.Add(new MySqlParameter("officePhoneNumber", MySqlDbType.String) { Value = blOffice.officePhoneNumber });
                    pm.Add(new MySqlParameter("officePhoneNumber", MySqlDbType.Int16) { Value = blOffice.officePhoneNumber });
                    pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blOffice.userId });
                    pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blOffice.entryDateTime });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blOffice.clientIp });

                    rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "OfficeEntry");
                }
                else
                {
                    rb.message = "Office Email-Id has Already Used!!";
                }
            }
            else
            {
                rb.message = "Office Mobile number has Already Used!!";
            }
            return rb;
        }
        public async Task<ReturnClass.ReturnBool> UpdateOfficeDetail(BlOffice blOffice)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckOfficeMobileExists(blOffice.officePhoneNumber, "UPDATE", (Int64)blOffice.hodOfficeId, (Int64)blOffice.OfficeId);

            if (!isofficeExists)
            {
                isofficeExists = await CheckOfficeMobileExists(blOffice.officeEmailId, "UPDATE", (Int64)blOffice.hodOfficeId, (Int64)blOffice.OfficeId);
                if (!isofficeExists)
                {

                    string query = @"INSERT INTO officelog
                                       SELECT *
                                        FROM office                                             
                                        WHERE officeId=@officeId;";
                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = blOffice.hodOfficeId });
                    pm.Add(new MySqlParameter("officeId", MySqlDbType.Int16) { Value = blOffice.OfficeId });
                    pm.Add(new MySqlParameter("officeName", MySqlDbType.String) { Value = blOffice.OfficeName });
                    pm.Add(new MySqlParameter("baseDeptId", MySqlDbType.String) { Value = blOffice.baseDeptId });
                    pm.Add(new MySqlParameter("officeLevel", MySqlDbType.String) { Value = blOffice.officeLevel });
                    pm.Add(new MySqlParameter("districtId", MySqlDbType.Int16) { Value = blOffice.districtId });
                    pm.Add(new MySqlParameter("districtname", MySqlDbType.String) { Value = blOffice.districtname });
                    pm.Add(new MySqlParameter("urbanRural", MySqlDbType.Int16) { Value = blOffice.urbanRural });
                    pm.Add(new MySqlParameter("officeAddress", MySqlDbType.String) { Value = blOffice.officeAddress });
                    pm.Add(new MySqlParameter("officeEmailId", MySqlDbType.String) { Value = blOffice.officeEmailId });
                    pm.Add(new MySqlParameter("officePhoneNumber", MySqlDbType.String) { Value = blOffice.officePhoneNumber });
                    pm.Add(new MySqlParameter("officePhoneNumber", MySqlDbType.Int16) { Value = blOffice.officePhoneNumber });
                    pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blOffice.userId });
                    pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blOffice.entryDateTime });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blOffice.clientIp });

                    using (TransactionScope ts = new TransactionScope())
                    {
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "Officelog");
                        if (rb.status == true)
                        {
                            query = @"UPDATE office 
                                            set hodOfficeId=@hodOfficeId,officeName=@officeName,baseDeptId=@baseDeptId,
                                            officeLevel=@officeLevel,districtId=@districtId,districtname=@districtname,urbanRural=@urbanRural,
                                            pinCode=@pinCode,officeAddress=@officeAddress,officeEmailId=@officeEmailId,
                                            officePhoneNumber=@officePhoneNumber,userId=@userId,entryDateTime=@entryDateTime,
                                            clientIp=@clientIp 
                                            WHERE officeId=@officeId;";



                            rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "OfficeUpdate");
                            if (rb.status == true)
                            {
                                ts.Complete();
                            }
                        }
                        else
                        {
                            rb.message = "Office data Failed To Update!";
                        }
                    }
                }
                else
                {
                    rb.message = "Office Email-Id has Already Used!!";
                }
            }
            else
            {
                rb.message = "Office Mobile number has Already Used!!";
            }
            return rb;
        }
        public async Task<Int64> GetOfficeId(Int64 hodOfficeId)
        {
            string OfficeId = "0";
            try
            {
                string qr = @"SELECT IFNULL(MAX(SUBSTRING(o.office,12,13)),0) + 1 AS OfficeId
								FROM office o 
							WHERE o.hodOfficeId = @hodOfficeId";

                List<MySqlParameter> pm = new();
                pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = hodOfficeId });

                ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(qr, pm.ToArray());
                if (dt.table.Rows.Count > 0)
                {
                    OfficeId = dt.table.Rows[0]["OfficeId"].ToString();
                    OfficeId = hodOfficeId.ToString() + OfficeId.PadLeft(2, '0');
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Convert.ToInt64(OfficeId);
        }
        public async Task<bool> CheckOfficeMobileExists(string mobileNo, string transType, Int64 hodOfficeId, Int64 officeId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT o.officeId
                            FROM office o
                            WHERE o.hodOfficeId=@hodOfficeId AND o.officePhoneNumber=@mobileNo ";
            if (transType == "UPDATE")
            {
                query = query + " AND o.officeId!=@officeId ";
            }
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("mobileNo", MySqlDbType.VarChar) { Value = mobileNo });
            pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = hodOfficeId });
            pm.Add(new MySqlParameter("officeId", MySqlDbType.Int64) { Value = officeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }
        public async Task<bool> CheckOfficeEmailExists(string emailId, string transType, Int64 hodOfficeId, Int64 officeId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT o.officeId
                            FROM office o
                            WHERE o.hodOfficeId=@hodOfficeId AND o.emailId=@emailId ";
            if (transType == "UPDATE")
            {
                query = query + " AND o.officeId!=@officeId ";
            }
            List<MySqlParameter> pm = new();

            pm.Add(new MySqlParameter("emailId", MySqlDbType.VarChar) { Value = emailId });
            pm.Add(new MySqlParameter("hodOfficeId", MySqlDbType.Int64) { Value = hodOfficeId });
            pm.Add(new MySqlParameter("officeId", MySqlDbType.Int64) { Value = officeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }

        public async Task<ReturnClass.ReturnDataTable> GetAllOfficeList(Int64 userId)
        {
            string query = @"SELECT h.hodOfficeId ,h.officeCount,h.hodOfficeName,h.baseDeptId,h.orgType,h.hodOfficeLevel,
                                     h.hodOfficeStateId,h.hodOfficeDistrictId,h.hodOfficeDistrictname,h.hodOfficeIsUrbanRural,h.hodOfficeAddress,
                                     h.hodOfficePinCode,h.hodOfficeEmailId,h.hodOfficePhoneNumber,h.hodOfficeFaxNumber,h.hodOfficeWebsite,
                                     h.isRegistrationDocumentUploaded,h.isVerified,h.verificationDate,h.registrationStatus,h.registrationDate,
                                    h.applicantName,h.applicantDesignationCode,h.applicantMobileNumber,h.applicantEmailId,h.applicantEmailId,
                                    h.isParichayLogin
                            FROM hodofficeregistration h WHERE h.isVerified=@isVerified AND registrationStatus=@registrationStatus 
                            WHERE e.userId=@userId";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = userId });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<ReturnClass.ReturnDataTable> GetOfficeById(Int64 employeeId)
        {
            string query = @"SELECT e.stateId,e.employeeId,e.employeeName,e.mobileNo,e.emailId,e.workingStatus,
                                        e.recruitmentType,e.userId,e.clientIp,e.entryDateTime,e.registrationYear,
									workingStatus.nameEnglish AS workingStatusName,recruitmentType.nameEnglish AS recruitmentTypeName
                            FROM employeemaster e
                            JOIN ddlcatlist AS workingStatus ON  workingStatus.category='workingStatus' AND workingStatus.id=e.workingStatus
							JOIN ddlcatlist AS recruitmentType ON  recruitmentType.category='recruitmentType' AND recruitmentType.id=e.recruitmentType
                    WHERE e.employeeId=@employeeId ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("employeeId", MySqlDbType.Int64) { Value = employeeId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
    }
}
