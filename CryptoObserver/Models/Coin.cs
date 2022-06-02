using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptoObserver.Models
{
    public class Coin: INotifyPropertyChanged
    {
        private string id;
        private int rank;
        private string symbol;
        private string name;
        private float priceUsd;

        private List<string> markets;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                OnPropertyChanged("Symbol");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public float PriceUsd
        {
            get { return priceUsd; }
            set
            {
                priceUsd = value;
                OnPropertyChanged("PriceUsd");
            }
        }
        public List<string> Markets
        {
            get { return markets; }
            set
            {
                markets = value;
                OnPropertyChanged("Markets");
            }
        }
     
        public event PropertyChangedEventHandler PropertyChanged;              
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private object coinsList;

        public object CoinsList { get => coinsList; set => SetProperty(ref coinsList, value); }

        private object topTenCoinsList;

        public object TopTenCoinsList { get => topTenCoinsList; set => SetProperty(ref topTenCoinsList, value); }


    }


}
