namespace AdoptaYA.Functionalities.AccessRequest.Pages;
public partial class AccessRequest
{
    string subtitle = "Ingresa tus datos de la organizaci�n";

    void OnChangeTab(int index)
    {
        if (index == 0)
        {
            subtitle = "Ingresa tus datos de la organizaci�n";
        }
        if (index == 1)
        {
            subtitle = "Ingresa tu c�digo de empleado";
        }
    }

}