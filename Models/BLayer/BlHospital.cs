using BaseClass;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementApi.Models.BLayer
{
    public class BlHospital
    {

        public long? hospitalRegNo { get; set; }
        public string? hospitalNameEnglish { get; set; }
        public string? hospitalNameLocal { get; set; }
        public Int16 stateId { get; set; }
        public Int16 districtId { get; set; }
        public string address { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$", ErrorMessage = "Invalid email address")]
        public string? emailId { get; set; }
        public string? mobileNo { get; set; }
        public YesNo active { get; set; }

        public YesNo isVerified { get; set; }
        public string? verificationDate { get; set; }
        public int verifiedByLoginId { get; set; }
        public RegistrationStatus registrationStatus { get; set; }
        public string? clientIp { get; set; }
        public string? entryDateTime { get; set; }
        public Int64? userId { get; set; }
        public int registrationYear { get; set; }
    }


}
