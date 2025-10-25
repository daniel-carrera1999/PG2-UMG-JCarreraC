namespace AdoptaYA.Functionalities.AccessRequest.Pages;
public partial class AccessRequest
{
    string subtitle = "Ingresa tus datos de la organización";

    void OnChangeTab(int index)
    {
        if (index == 0)
        {
            subtitle = "Ingresa tus datos de la organización";
        }
        if (index == 1)
        {
            subtitle = "Ingresa tu código de empleado";
        }
    }

}