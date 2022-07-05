using BaseClass;
using MySql.Data.MySqlClient;
using System.Transactions;
using TicketManagementApi.Models.BLayer;
using static TicketManagementApi.Models.BLayer.BlCommon;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlInfrastructure
    {
        readonly DBConnection db = new();
        public async Task<ReturnClass.ReturnBool> CUDOperation(BlInfrastructure bl)
        {
            ReturnClass.ReturnBool rb = new ReturnClass.ReturnBool();
            MySqlParameter[] pm;
            if (bl.hospitalRegNo == 0)
            {
                rb.status = false;
                rb.message = "Invalid Hospital Registration No !";
                return rb;
            }
            string query = "";
            bool isValidated = true;
            if (isValidated)
            {
                foreach (var item in bl.Bl)
                {
                    pm = new MySqlParameter[]
                    {
                        new MySqlParameter("infrastructureId", MySqlDbType.Int32) { Value = item.infrastructureId },
                    new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = bl.hospitalRegNo },
                    new MySqlParameter("infrastructureFacilitiesId", MySqlDbType.Int16) { Value = item.infrastructureFacilitiesId },
                    new MySqlParameter("infrastructureFacilities", MySqlDbType.VarChar, 50) { Value = item.infrastructureFacilities },
                    new MySqlParameter("remarks", MySqlDbType.VarChar, 100) { Value = item.remarks },
                    new MySqlParameter("userId", MySqlDbType.Int64) { Value = bl.userId },
                    new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = bl.entryDateTime },
                };
                    if (bl.CRUD == (Int16)CRUD.Create)
                    {
                        query = @"INSERT INTO infrastructure (hospitalRegNo,infrastructureFacilitiesId,infrastructureFacilities,remarks,entryDateTime,userId)
                                        VALUES (@hospitalRegNo,@infrastructureFacilitiesId,@infrastructureFacilities,@remarks,@entryDateTime,@userId)";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "infrastructure");
                    }
                    else if (bl.CRUD == (Int16)CRUD.Update)
                    {
                        query = @"UPDATE infrastructure 
                                        SET infrastructureFacilitiesId = @infrastructureFacilitiesId,infrastructureFacilities = @infrastructureFacilities,
                                            remarks=@remarks
                                WHERE infrastructureId = @infrastructureId";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "infrastructure");
                    }
                }
            }
            return rb;
        }
    }
}
