using BaseClass;
namespace TicketManagementApi.Models.BLayer
{
    public class BlCommon
    {
        Utilities util = new();
        public BlCommon()
        {

        }
        public enum YesNo
        {
            No = 0,
            Yes = 1
        }

        public enum CRUD
        {
            Create = 1,
            Read = 2,
            Update = 3,
            Delete = 4,
        }


    }
}
