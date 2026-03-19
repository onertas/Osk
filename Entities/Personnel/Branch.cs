using Ardalis.SmartEnum;

namespace OskApi.Entities.Personnel;

public class Branch : Entity
{

    public string Name { get; set; }=string.Empty;

    public Guid TitleId { get; set; }
    public Title Title { get; set; } = null!;

    public int BranchTypeId { get; set; }

    public ICollection<PersonnelBranch>? PersonnelBranches { get; set; }
}


//Ünvan Enum
public class BranchTypeEnum : SmartEnum<BranchTypeEnum>
{
    public static readonly BranchTypeEnum Yok = new BranchTypeEnum(nameof(Yok), 0, "Yok");
    public static readonly BranchTypeEnum AnaDal = new BranchTypeEnum(nameof(AnaDal), 1, "Ana Dal");
    public static readonly BranchTypeEnum YanDal = new BranchTypeEnum(nameof(YanDal), 2, "Yan Dal");


    public string Description { get; }

    private BranchTypeEnum(string name, int value, string description) : base(name, value)
    {
        Description = description;
    }
}

