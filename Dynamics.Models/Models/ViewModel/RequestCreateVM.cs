namespace Dynamics.Models.Models.ViewModel;

public class RequestCreateVM
{
    //User
    public string UserEmail { get; set; }
    public string UserPhoneNumber { get; set; }
    public string UserAddress { get; set; }
    
    //Request
    public string RequestTitle { get; set; }
    public string Content { get; set; }
    public DateOnly? CreationDate { get; set; }
    public string RequestPhoneNumber { get; set; }
    public string RequestEmail { get; set; }
    public string Location { get; set; }
    public int isEmergency { get; set; }
}