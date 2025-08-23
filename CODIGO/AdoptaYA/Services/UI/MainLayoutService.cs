namespace AdoptaYA.Services.UI;
public class MainLayoutService
{
    // Eventos para el cambio de texto y el estado del sidebar
    public event Action<string> OnHeaderTextChanged = null!;
    public event Action<string> OnHeaderUserNameChanged = null!;
    public event Action<string> OnHeaderEmailChanged = null!;
    public event Action<bool> OnSidebarExpandedChanged = null!;
    public event Action<string> OnSidebarUserNameChanged = null!;
    public event Action<string> OnSidebarRolChanged = null!;

    // Estado del sidebar
    private bool sidebarExpanded = true;

    // Actualizar el texto del header
    public void UpdateHeader(string nuevoTexto)
    {
        OnHeaderTextChanged?.Invoke(nuevoTexto);
    }

    // Actualizar datos del usuario header
    public void UpdateUserInfo(string name, string email)
    {
        OnHeaderUserNameChanged?.Invoke(name);
        OnHeaderEmailChanged?.Invoke(email);
    }

    // Actualizar el estado del sidebar
    public void SetSidebarExpanded(bool expanded)
    {
        if (sidebarExpanded != expanded)
        {
            sidebarExpanded = expanded;
            OnSidebarExpandedChanged?.Invoke(expanded);
        }
    }

    // Obtener el estado actual del sidebar
    public bool GetSidebarExpanded()
    {
        return sidebarExpanded;
    }

    // Actualizar datos del usuario sidebar
    public void UpdateUserInfoSidebar(string name, string rol)
    {
        OnSidebarUserNameChanged?.Invoke(name);
        OnSidebarRolChanged?.Invoke(rol);
    }
}
