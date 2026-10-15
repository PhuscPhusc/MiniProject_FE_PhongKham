using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class LoginService
{
    public LoginViewModel _login;


    public HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public LoginService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndPhongKham");
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }



    public async Task<bool> Login(LoginViewModel loginInfo)
{
    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Login/Login", loginInfo);
    if (response.IsSuccessStatusCode)
    {
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        var token = loginResponse?.token?.result;

        if (!string.IsNullOrEmpty(token))
        {
            await _localStorage.SetItemAsync("accessToken", token);
            (_authStateProvider as CustomAuthStateProvider)?.MarkUserAsAuthenticated(token);
            setStateHasChange();
            Console.WriteLine($"Token JWT: {token}");
            return true;
        }
        return false;
    }
    else
    {
        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Login thất bại: " + error);
        return false;
    }
}







    public event Action Onchange;

    public void setStateHasChange()
    {
        Onchange?.Invoke();
    }
}