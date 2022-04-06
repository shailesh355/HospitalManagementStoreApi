using BaseClass;
namespace TicketManagementApi.Models.BLayer
{
    public class BlCommon
    {
        Utilities util = new();
        public BlCommon()
        {
            try
            {
                ReturnClass.ReturnBool rb = util.GetAppSettings("AppSettings", "DefaultState");
                if (rb.status)
                    defaultStateId = Convert.ToInt32(rb.message);
            }
            catch
            {
                defaultStateId = 22;    //=====Set To Chhattisgarh if Not Found in App Settings
            }
        }
        public static int defaultStateId { get; set; }
    }
}
