namespace AdoptaYA.Functionalities.Permissions.Model;

public class ModuloPermisoViewModel
{
    public int IdModulo { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Icon { get; set; }
    public string Description { get; set; }
    public bool Create { get; set; }
    public bool Read { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public bool IsCreated { get; set; }
}