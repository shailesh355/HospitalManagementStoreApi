#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// Don't Add any additional enumerators in this code file. For Project Specific enumerator use CommonEnumerator.cs file
/// </summary>
namespace BaseClass
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    public enum HashingAlgorithmSupported
    {
        Md5,
        Sha256,
        Sha512
    }
    public enum LanguageSupported
    {
        Hindi = 1,
        English = 2,
    }
    public enum DBConnectionList
    {
        TransactionDb = 1,
        ReportingDb = 2
    }
    public enum Active
    {
        No = 0,
        Yes = 1
    }
    public enum YesNo
    {
        No = 0,
        Yes = 1
    }
    public enum UserRole
    {
        
        Admin = 1,
        Nodal = 2,
        Receiver = 3,
        User = 4
    }
    public enum idPrefix
    {
        registrationId = 3,
        serviceRegistrationId = 2,
        employeeId = 2,
        officeId = 1,
        empOfficeMappingId = 2,
        liftServiceId = 4,
    }
}