using AdoptaYA.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace AdoptaYA.Shared.Components;
public partial class ViewErrorsModal
{
    [Parameter] public ApiResponseNotAcceptable? responseNotAcceptable { get; set; }

    public string strErrores = "";

    protected override void OnInitialized()
    {
        strErrores += $"{responseNotAcceptable!.Detail}\n";

        if (responseNotAcceptable?.Errors != null && responseNotAcceptable.Errors.Any())
        {
            foreach (var campoError in responseNotAcceptable.Errors)
            {
                var campo = campoError.Key;
                var mensajes = campoError.Value;
                foreach (var mensaje in mensajes)
                {
                    strErrores += $"- {mensaje}\n";
                }
            }
        }
    }
}