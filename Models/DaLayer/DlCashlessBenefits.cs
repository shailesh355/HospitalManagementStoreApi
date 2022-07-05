using BaseClass;
using MySql.Data.MySqlClient;
using System.Transactions;
using TicketManagementApi.Models.BLayer;
using static TicketManagementApi.Models.BLayer.BlCommon;

namespace TicketManagementApi.Models.DaLayer
{
    public class DlCashlessBenefits
    {
        readonly DBConnection db = new();
        public async Task<ReturnClass.ReturnBool> CUDOperation(BlCashlessBenefits bl)
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
                        new MySqlParameter("cashlessBenefitsId", MySqlDbType.Int32) { Value = item.cashlessBenefitsId },
                        new MySqlParameter("hospitalRegNo", MySqlDbType.Int64) { Value = bl.hospitalRegNo },
                         new MySqlParameter("discountPercent", MySqlDbType.Int16) { Value = item.discountPercent },
                        new MySqlParameter("userId", MySqlDbType.Int64) { Value = bl.userId },
                        new MySqlParameter("entryDateTime", MySqlDbType.String) { Value = bl.entryDateTime }
                    };
                    if (bl.CRUD == (Int16)CRUD.Create)
                    {
                        query = @"INSERT INTO cashlessbenefits (hospitalRegNo,discountPercent,entryDateTime,userId)
                                        VALUES (@hospitalRegNo,@discountPercent,@entryDateTime,@userId)";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "cashlessbenefits");
                    }
                    else if (bl.CRUD == (Int16)CRUD.Update)
                    {
                        query = @"UPDATE cashlessbenefits 
                                        SET discountPercent = @discountPercent
                                WHERE cashlessBenefitsId = @cashlessBenefitsId";
                        rb = await db.ExecuteQueryAsync(query, pm.ToArray(), "cashlessbenefits"); 
                    }
                }
            }
                return rb;
        }
    }
}
