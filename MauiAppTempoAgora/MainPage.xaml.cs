using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Threading.Tasks;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
               
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";
                                                                                                                
                        dados_previsao = $"Latitude: {t.lat}\n" +
                                         $"Longitude: {t.lon}\n" +
                                         $"Descrição: {t.description}\n" +
                                         $"Vel. do Vento: {t.speed}\n" +
                                         $"Visibilidadde: {t.visibility}\n" +
                                         $"Nascer do Sol: {t.sunrise}\n" +
                                         $"Pôr do Sol: {t.sunset}\n" +
                                         $"Temp Min: {t.temp_min}°C\n" +
                                         $"Temp Máx: {t.temp_max}°C\n" +
                                         $"Sensação Térmica: {t.feels_like}°C\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão.";
                    }
                }
                else
                {
                    await DisplayAlert("Atenção", "Preencha o nome da cidade.", "OK");
                    lbl_res.Text = "";
                }
            }
            catch (Exception ex) 
            {
                await DisplayAlert("Ops", ex.Message, "OK");
                lbl_res.Text = "";
            }
        }
    }
}
