namespace OskApi.Entities.Personnel;

public class PmType : Entity
{
    public string Name { get; set; }=default!;
    public string Description { get; set; }=string.Empty;   
    public int Order { get; set; }
    public bool IsUsingStaff { get; set; }
    public bool IsBeforeStartStaff { get; set; }
    public bool IsManager { get; set; }
    public bool IsFaaliyet2Control { get; set; }
    public bool IsOnlyOneStatu { get; set; }
    public int StatusQuota { get; set; }

}


