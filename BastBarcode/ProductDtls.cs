using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BastBarcode
{
    public class ProductDtls : INotifyPropertyChanged
    {
        public static ProductDtls Instance;
        public static bool InstaceChanged = false;
        public static ObservableCollection<ProductDtls> Store = new ObservableCollection<ProductDtls>();
        public async static Task Load()
        {
            Windows.Storage.StorageFolder lclFldr = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile saveFle = await lclFldr.CreateFileAsync("save.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            //Windows.Storage.StorageFile saveFle = await lclFldr.CreateFileAsync("save.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            string data = await Windows.Storage.FileIO.ReadTextAsync(saveFle);
            if (!string.IsNullOrEmpty(data))
            {
                ObservableCollection<ProductDtls> list = JsonConvert.DeserializeObject<ObservableCollection<ProductDtls>>(data);
                foreach (var item in list) Store.Add(item);
            }
        }
        public async static Task Save()
        {
            Windows.Storage.StorageFolder lclFldr = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile saveFle = await lclFldr.CreateFileAsync("save.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            //Windows.Storage.StorageFile saveFle = await lclFldr.CreateFileAsync("save.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(saveFle, JsonConvert.SerializeObject(Store));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _BusinessName;
        public string BusinessName { 
            get => _BusinessName;
            set
            {
                if (_BusinessName != value)
                {
                    _BusinessName = value;
                    OnPropertyChanged("BusinessName");
                }
            }
        }
        private string _ProductName;
        public string ProductName
        {
            get => _ProductName;
            set
            {
                if (_ProductName != value)
                {
                    _ProductName = value;
                    OnPropertyChanged("ProductName");
                }
            }
        }
        private string _ProductColor;
        public string ProductColor
        {
            get => _ProductColor;
            set
            {
                if (_ProductColor != value)
                {
                    _ProductColor = value;
                    OnPropertyChanged("ProductColor");
                }
            }
        }
        private List<string> _ProductSizeList = new List<string>();
        [JsonIgnore]
        public List<string> ProductSizeList
        {
            get => _ProductSizeList;
            set
            {
                if (!Enumerable.SequenceEqual<string>(_ProductSizeList, value))
                {
                    _ProductSizeList = value;
                    OnPropertyChanged("ProductSizeList");
                }
            }
        }
        private string _ProductSize;
        public string ProductSize
        {
            get => _ProductSize;
            set
            {
                if (_ProductSize != value)
                {
                    _ProductSize = value;
                    OnPropertyChanged("ProductSize");
                }
            }
        }
        private string _ProductOldPrice;
        public string ProductOldPrice
        {
            get => _ProductOldPrice;
            set
            {
                if (_ProductOldPrice != value)
                {
                    _ProductOldPrice = value;
                    OnPropertyChanged("ProductOldPrice");
                }
            }
        }
        private string _ProductNewPrice;
        public string ProductNewPrice
        {
            get => _ProductNewPrice;
            set
            {
                if (_ProductNewPrice != value)
                {
                    _ProductNewPrice = value;
                    OnPropertyChanged("ProductNewPrice");
                }
            }
        }
        public string PrintingTime { get; set; } // format : {YYYY-MM-DD hh:mm:ss}
    }
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = "";
            if(value != null)
            {
                List<string> array = (List<string>)value;
                for (int i = 0; i < array.Count; i++)
                {
                    result += array.ElementAt(i);
                    if (i != array.Count - 1) result += ",";
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            List<string> array = new List<string>();
            if(value != null)
            {
                string str = (string)value;
                MatchCollection matchCollection = Regex.Matches(str, @"[0-9x]+([0-9x]+)*");
                foreach (Match item in matchCollection)
                {
                    string tmp = item.Value;
                    if(tmp.IndexOf("x") != -1)
                    {
                        string r_value = tmp.Substring(0, tmp.IndexOf("x"));
                        string r_times = tmp.Substring(tmp.IndexOf("x") + 1);
                        int r_int_times = int.Parse(r_times);
                        for (int i = 1; i <= r_int_times; i++) array.Add(r_value);
                    }
                    else if (tmp.IndexOf("x")+1 == tmp.Length)
                    {
                        string r_value = tmp.Substring(0, tmp.IndexOf("x"));
                        array.Add(r_value);
                    }
                    else array.Add(tmp);
                }
            }
            return array;
        }
    }
}
