using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xamarin.Forms;

namespace ImeStretchSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            buttonNormal.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new NormalPage());
            };

            buttonChatStyle.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new ChatStylePage());
            };
        }
    }
}
