using System.Net.Http.Json;
using System.Windows.Input;

namespace LoginPage
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://testwork.cloud39.ru/BonusWebApi/"),
        };
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var request = new AuthRequest() { Login = login.Text, Password = password.Text };

            var httpResponse = await _httpClient.PostAsJsonAsync("Account/Login", request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    await DisplayAlert("Ошибка авторизации", "Пользователь с таким логином или паролем не найден", "OK");
                return;
            }

            try
            {
                var authResponse = await httpResponse.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse.Token != null)
                {
                    await DisplayAlert("FullName", authResponse.Client.FullName, "OK");
                    return;
                }
                    await DisplayAlert("token", authResponse.Token, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                throw;
            }

        }
    }

}
