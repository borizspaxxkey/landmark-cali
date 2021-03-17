using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ExploreCalifornia
{
    public partial class MainPage : ContentPage
    {
        private MediaFile _pickedImage;
        private HttpClient _httpClient = new HttpClient();
        private readonly string AZURE_FUNCTION_URL = "https://landmarklinkedin.azurewebsites.net/api/function1";

        public MainPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _pickedImage = await CrossMedia.Current.PickPhotoAsync();
            pickedImage.Source = ImageSource.FromFile(_pickedImage.Path);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(_pickedImage != null)
            {
                var postResult =await _httpClient.PostAsync(AZURE_FUNCTION_URL, 
                                        new StreamContent(_pickedImage.GetStreamWithImageRotatedForExternalStorage()));

                var stringResult = await postResult.Content.ReadAsStringAsync();

                if (postResult.IsSuccessStatusCode)
                {
                    await DisplayAlert("Here is What you See", stringResult, "Ok");
                }
                else
                {
                    await DisplayAlert("Error", stringResult, "Ok");

                }
            }
        }
    }
}
