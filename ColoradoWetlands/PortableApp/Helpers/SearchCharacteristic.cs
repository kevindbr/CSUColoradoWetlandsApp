using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public class SearchCharacteristic : ImageButton
    {
        public static readonly BindableProperty QueryProperty = BindableProperty.Create("Query", typeof(bool), typeof(ImageButton), false);
        public static readonly BindableProperty ColumnsProperty = BindableProperty.Create("Columns", typeof(string[]), typeof(ImageButton), null);
        public static readonly BindableProperty SearchStringsProperty = BindableProperty.Create("SearchStrings", typeof(string[]), typeof(ImageButton), null);

        public bool Query
        {
            get { return (bool)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public string[] Columns
        {
            get { return GetValue(ColumnsProperty) as string[]; }
            set { SetValue(ColumnsProperty, value); }
        }

        public string[] SearchStrings
        {
            get { return GetValue(SearchStringsProperty) as string[]; }
            set { SetValue(SearchStringsProperty, value); }
        }

        public SearchCharacteristic()
        {
            TextColor = Color.White;
            BorderColor = Color.White;
            BackgroundColor = Color.Transparent;
        }

    }
}
