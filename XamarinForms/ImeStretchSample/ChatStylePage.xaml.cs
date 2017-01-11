using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ImeStretchSample
{
    public partial class ChatStylePage : ContentPage
    {
        public ChatStylePage()
        {
            InitializeComponent();

            var list = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(i);
            }

            listView.ItemsSource = list;
        }
    }
}
