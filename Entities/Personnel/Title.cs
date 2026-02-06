namespace OskApi.Entities.Personnel;

    public class Title:Entity
    {
    public string Name { get; set; }= string.Empty;
    public bool IsDoctor { get; set; }=false;
}

