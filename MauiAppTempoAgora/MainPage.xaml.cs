using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Diagnostics;
using System.Globalization;
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
               
        private async void Button_Clicked_Previsao(object sender, EventArgs e)
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

                        string mapa = $"https://embed.windy.com/embed.html?" +
                            $"type=map&location=coordinates&metricRain=mm&metricTemp=°C&" +
                            $"metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface&" +
                            $"lat={t.lat.ToString("F6", CultureInfo.InvariantCulture)}&lon={t.lon.ToString("F6", CultureInfo.InvariantCulture)}&message=true";

                        wv_mapa.Source = mapa;

                        Debug.WriteLine(mapa);
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

        private async void Button_Clicked_localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = 
                    new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10)
                        );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude}";

                    lbl_coords.Text = local_disp;

                    //pega o nome da cidade que está nas coordenadas.
                    GetCidade(local.Latitude, local.Longitude);
                }
                else 
                {
                    lbl_coords.Text = "Nenhuma localização";
                }

            }
            catch(FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");
            }
            catch(FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilita", fneEx.Message, "OK");
            }
            catch(PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão da Localização", pEx.Message, "OK");
            }
            catch(Exception ex) 
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Erro: Obtenção do nome da Cidade", ex.Message, "OK");
            }
            
        }

    }
}
