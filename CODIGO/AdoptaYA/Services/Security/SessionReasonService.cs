using AdoptaYA.Shared.Interfaces;
using Microsoft.JSInterop;

namespace AdoptaYA.Services.Security;
public class SessionReasonService : ISessionReasonService
{
    private readonly IJSRuntime _jsRuntime;

    public SessionReasonService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetSessionReasonAsync(string reason)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "SessionReason", reason);
    }
}
