using BaseClass;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementApi.Models.BLayer
{
    public class BlEmployee
    {
        public Int16 stateId { get; set; }
        public long? employeeId { get; set; }
        public string employeeName { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public Int16 workingStatus { get; set; }
        public Int16 recruitmentType { get; set; }        
        public string? clientIp { get; set; }
        public Int32? registrationYear { get; set; }
        public Int64? userId { get; set; }
        public string? entryDateTime { get; set; }
    }
    

}
