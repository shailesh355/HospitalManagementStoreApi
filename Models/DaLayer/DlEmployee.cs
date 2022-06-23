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
            bool isofficeExists = await CheckEmployeeMobileExists(blEmployee.mobileNo);

            if (!isofficeExists)
            {
                isofficeExists = await CheckEmployeeMobileExists(blEmployee.emailId);
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
                    rb.message = "Applicant Email-Id has Already Used For Registration!!";
                }
            }
            else
            {
                rb.message = "This Department has Already Applied For Registration!!";
            }
            return rb;
        }

        public async Task<ReturnClass.ReturnBool> UpdateEmployeeDetail(BlEmployee blEmployee)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isofficeExists = await CheckEmployeeMobileExists(blEmployee.mobileNo);

            if (!isofficeExists)
            {
                isofficeExists = await CheckEmployeeMobileExists(blEmployee.emailId);
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

        public async Task<ReturnClass.ReturnDataTable> GetAllEmployeeList()
        {
            string query = @"SELECT e.stateId,e.employeeId,e.employeeName,e.mobileNo,e.emailId,e.workingStatus,
                                        e.recruitmentType,e.userId,e.clientIp,e.entryDateTime,e.registrationYear,
									workingStatus.nameEnglish AS workingStatusName,recruitmentType.nameEnglish AS recruitmentTypeName
                            FROM employeemaster e
                            JOIN ddlcatlist AS workingStatus ON  workingStatus.category='workingStatus' AND workingStatus.id=e.workingStatus
							JOIN ddlcatlist AS recruitmentType ON  recruitmentType.category='recruitmentType' AND recruitmentType.id=e.recruitmentType";

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query);
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
        public async Task<bool> CheckEmployeeMobileExists(string mobileNo)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT e.employeeId
                            FROM employeemaster e
                            WHERE e.mobileNo=@mobileNo ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("mobileNo", MySqlDbType.VarChar) { Value = mobileNo });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }
        public async Task<bool> CheckEmployeeEmailExists(string emailId)
        {
            bool isHodOfficeExists = false;
            string query = @"SELECT e.employeeId
                            FROM employeemaster e
                            WHERE e.emailId=@emailId";
            List<MySqlParameter> pm = new();

            pm.Add(new MySqlParameter("emailId", MySqlDbType.VarChar) { Value = emailId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
            {
                isHodOfficeExists = true;
            }
            return isHodOfficeExists;
        }
    }
}
