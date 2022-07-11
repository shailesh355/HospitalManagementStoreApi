using BaseClass;
using MySql.Data.MySqlClient;
using System.Transactions;
using TicketManagementApi.Models.BLayer;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlHospital
    {
        readonly DBConnection db = new();

        public async Task<ReturnClass.ReturnBool> RegisterNewHospital(BlHospital blHospital)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            if (blHospital.hospitalRegNo == null)
                blHospital.hospitalRegNo = 0;
            bool isExists = await CheckMobileExistAsync(blHospital.mobileNo, "INSERT", (Int64)blHospital.hospitalRegNo);

            if (!isExists)
            {
                isExists = await CheckEmailExistAsync(blHospital.emailId, "INSERT", (Int64)blHospital.hospitalRegNo);
                if (!isExists)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        string query = @"INSERT INTO hospitalregistration (hospitalRegNo,hospitalNameEnglish,hospitalNameLocal,stateId,districtId,address,mobileNo,
                                                 emailId,active,isVerified,verificationDate,verifiedByLoginId,registrationStatus,userId, 
                                                entryDateTime, clientIp)
                                        VALUES (@hospitalRegNo,@hospitalNameEnglish,@hospitalNameLocal,@stateId, @districtId, @address, @mobileNo,
                                                 @emailId,@active, @isVerified,@verificationDate,@verifiedByLoginId,@registrationStatus,@userId, 
                                                @entryDateTime,@clientIp)";
                        blHospital.hospitalRegNo = await GetHospitalId(blHospital.registrationYear);


                        List<MySqlParameter> pm = new();
                        pm.Add(new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = blHospital.hospitalRegNo });
                        pm.Add(new MySqlParameter("hospitalNameEnglish", MySqlDbType.String) { Value = blHospital.hospitalNameEnglish });
                        pm.Add(new MySqlParameter("hospitalNameLocal", MySqlDbType.String) { Value = blHospital.hospitalNameLocal });
                        pm.Add(new MySqlParameter("stateId", MySqlDbType.Int16) { Value = blHospital.stateId });
                        pm.Add(new MySqlParameter("districtId", MySqlDbType.Int16) { Value = blHospital.districtId });
                        pm.Add(new MySqlParameter("address", MySqlDbType.String) { Value = blHospital.address });
                        pm.Add(new MySqlParameter("mobileNo", MySqlDbType.String) { Value = blHospital.mobileNo });
                        pm.Add(new MySqlParameter("emailId", MySqlDbType.String) { Value = blHospital.emailId });
                        pm.Add(new MySqlParameter("active", MySqlDbType.Int16) { Value = blHospital.active });
                        pm.Add(new MySqlParameter("isVerified", MySqlDbType.Int16) { Value = (int)blHospital.isVerified });
                        pm.Add(new MySqlParameter("verifiedByLoginId", MySqlDbType.Int64) { Value = blHospital.userId });
                        pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (Int16)RegistrationStatus.Approved });
                        pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = blHospital.registrationYear });
                        pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blHospital.clientIp });
                        pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blHospital.userId });
                        pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blHospital.entryDateTime });

                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "HospitalRegistration");

                        query = @"INSERT INTO userlogin (userName,userId,emailId,password,active,isDisabled,userTypeCode,userRole,entryDateTime)
                                        VALUES (@hospitalNameEnglish,@hospitalRegNo,@emailId,@password, 1, 0, 1,1,@entryDateTime)";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "userlogin");
                        ts.Complete();

                    }
                }
                else
                {
                    rb.message = " Email-Id has Already Used For Registration!!";
                }
            }
            else
            {
                rb.message = " Mobile no. has Already Used For Registration!!";
            }
            return rb;
        }

        public async Task<ReturnClass.ReturnBool> UpdateNewHospital(BlHospital blHospital)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            bool isExists = await CheckMobileExistAsync(blHospital.mobileNo, "UPDATE", (Int64)blHospital.hospitalRegNo);

            if (!isExists)
            {
                isExists = await CheckEmailExistAsync(blHospital.emailId, "UPDATE", (Int64)blHospital.hospitalRegNo);
                if (!isExists)
                {
                    List<MySqlParameter> pm = new();
                    pm.Add(new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = blHospital.hospitalRegNo });
                    pm.Add(new MySqlParameter("hospitalNameEnglish", MySqlDbType.String) { Value = blHospital.hospitalNameEnglish });
                    pm.Add(new MySqlParameter("hospitalNameLocal", MySqlDbType.String) { Value = blHospital.hospitalNameLocal });
                    pm.Add(new MySqlParameter("stateId", MySqlDbType.Int16) { Value = blHospital.stateId });
                    pm.Add(new MySqlParameter("districtId", MySqlDbType.Int16) { Value = blHospital.districtId });
                    pm.Add(new MySqlParameter("address", MySqlDbType.String) { Value = blHospital.address });
                    pm.Add(new MySqlParameter("mobileNo", MySqlDbType.String) { Value = blHospital.mobileNo });
                    pm.Add(new MySqlParameter("emailId", MySqlDbType.String) { Value = blHospital.emailId });
                    pm.Add(new MySqlParameter("active", MySqlDbType.Int16) { Value = blHospital.active });
                    pm.Add(new MySqlParameter("isVerified", MySqlDbType.Int16) { Value = (int)blHospital.isVerified });
                    pm.Add(new MySqlParameter("verifiedByLoginId", MySqlDbType.Int64) { Value = blHospital.userId });
                    pm.Add(new MySqlParameter("registrationStatus", MySqlDbType.Int16) { Value = (Int16)RegistrationStatus.Approved });
                    pm.Add(new MySqlParameter("clientIp", MySqlDbType.VarString) { Value = blHospital.clientIp });
                    pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = blHospital.userId });
                    pm.Add(new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = blHospital.entryDateTime });
                    string query = @"INSERT INTO hospitalregistrationlog
                                     SELECT * FROM  hospitalregistration h
                                       WHERE h.hospitalRegNo=@hospitalRegNo";
                    using (TransactionScope ts = new TransactionScope())
                    {
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "Hospitallog");
                        query = @"UPDATE hospitalregistration 
                                                 SET hospitalNameEnglish=@hospitalNameEnglish,hospitalNameLocal=@hospitalNameLocal,stateId=@stateId,
                                                districtId=@districtId,address=@address,mobileNo=@mobileNo,emailId=@emailId,active=@active,isVerified=@isVerified,
                                                verifiedByLoginId=@verifiedByLoginId,registrationStatus=@registrationStatus,userId=@userId, 
                                                entryDateTime=@entryDateTime,clientIp=@clientIp WHERE  hospitalRegNo=@hospitalRegNo";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "HospitalUpdate");
                        if (rb.status == true)
                        {
                            ts.Complete();
                        }
                    }
                }
                else
                {
                    rb.message = " Email-Id has Already Used For Registration!!";
                }
            }
            else
            {
                rb.message = " Mobile no. has Already Used For Registration!!";
            }
            return rb;
        }
        /// <summary>
        /// Returns 12 digit hospitalId id based on year
        /// </summary>
        /// <param name="registrationYear"></param>
        /// <returns></returns>
        public async Task<Int64> GetHospitalId(int registrationYear)
        {
            string hospitalId = "0";
            try
            {
                string qr = @"SELECT IFNULL(MAX(SUBSTRING(ur.hospitalRegNo,6,12)),0) + 1 AS hospitalId
								FROM hospitalregistration ur 
							WHERE ur.registrationYear = @registrationYear";

                List<MySqlParameter> pm = new();
                pm.Add(new MySqlParameter("registrationYear", MySqlDbType.Int32) { Value = registrationYear });

                ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(qr, pm.ToArray());
                if (dt.table.Rows.Count > 0)
                {
                    hospitalId = dt.table.Rows[0]["hospitalId"].ToString();
                    hospitalId = ((int)idPrefix.employeeId).ToString() + registrationYear.ToString() + hospitalId.PadLeft(7, '0');
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Convert.ToInt64(hospitalId);
        }
        public async Task<bool> CheckEmailExistAsync(string emailId, string transType, Int64 hospitalRegNo)
        {
            bool isAccountExists = false;
            string query = @"SELECT u.emailId
                            FROM hospitalregistration u
                            WHERE u.emailId = @emailId ";
            if (transType == "UPDATE")
            {
                query = query + " AND u.hospitalRegNo!=@hospitalRegNo ";
            }
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("emailId", MySqlDbType.VarString) { Value = emailId });
            pm.Add(new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = hospitalRegNo });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
                isAccountExists = true;
            return isAccountExists;
        }
        public async Task<bool> CheckMobileExistAsync(string mobileNo, string transType, Int64 hospitalRegNo)
        {
            bool isAccountExists = false;
            string query = @"SELECT u.mobileNo
                            FROM hospitalregistration u
                            WHERE u.mobileNo = @mobileNo ";
            if (transType == "UPDATE")
            {
                query = query + " AND u.hospitalRegNo!=@hospitalRegNo ";
            }
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("mobileNo", MySqlDbType.VarString) { Value = mobileNo });
            pm.Add(new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = hospitalRegNo });
            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            if (dt.table.Rows.Count > 0)
                isAccountExists = true;
            return isAccountExists;
        }
        public async Task<ReturnClass.ReturnDataTable> GetAllHospitalList(Int64 userId)
        {
            string query = @"SELECT  h.hospitalRegNo,h.hospitalNameEnglish,h.hospitalNameLocal,h.stateId,h.districtId,h.address,h.mobileNo,
                                     h.emailId,h.active,s.stateNameEnglish AS stateName,d.districtNameEnglish AS districtName
                                FROM hospitalregistration h
                                 JOIN state s ON s.stateId=h.stateId
				                 JOIN district d ON d.districtId=h.districtId
                            WHERE h.userId=@userId";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("userId", MySqlDbType.Int64) { Value = userId });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }
        public async Task<ReturnClass.ReturnDataTable> GetHospitalById(Int64 hospitalRegNo)
        {
            string query = @"SELECT  h.hospitalRegNo,h.hospitalNameEnglish,h.hospitalNameLocal,h.stateId,h.districtId,h.address,h.mobileNo,
                                     h.emailId,h.active,s.stateNameEnglish AS stateName,d.districtNameEnglish AS districtName
                                FROM hospitalregistration h
                                 JOIN state s ON s.stateId=h.stateId
				                 JOIN district d ON d.districtId=h.districtId
                            WHERE h.hospitalRegNo=@hospitalRegNo ";
            List<MySqlParameter> pm = new();
            pm.Add(new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = hospitalRegNo });

            ReturnClass.ReturnDataTable dt = await db.ExecuteSelectQueryAsync(query, pm.ToArray());
            return dt;
        }



    }
}
