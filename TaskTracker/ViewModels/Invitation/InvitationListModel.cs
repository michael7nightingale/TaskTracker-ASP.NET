namespace TaskTracker.ViewModels.Invitation;

public class InvitationListModel
{
    public List<Models.Invitation> OutcomingInvitations { get; set; }= new();
    public List<Models.Invitation> IncomingInvitations { get; set; } = new();
    
}