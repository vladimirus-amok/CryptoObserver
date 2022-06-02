using CryptoObserver.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace CryptoObserver.ViewModels
{
    public class CoinViewModel
    {
        private ObservableCollection<Coin> coinsList;
        private ObservableCollection<Coin> topTenCoinsList;
        private Coin coinForDescribing;
        
        public ObservableCollection<Coin> CoinsList => coinsList;
        public ObservableCollection<Coin> TopTenCoinsList => topTenCoinsList;
        public Coin CoinForDescribing => coinForDescribing;

        public CoinViewModel()
        {
            coinsList = new ObservableCollection<Coin>();
            topTenCoinsList = new ObservableCollection<Coin>();
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5),
            };
            DataSource dataSource = new(httpClient);
            dataSource.LoadCoinsList(ref coinsList, ref topTenCoinsList);
        }
    }

    internal class DataSource
    {
        private readonly HttpClient _httpClient;
        public DataSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Coin> GetCoinsListFromApi(CancellationToken cancellationToken = default)
        {
            using var response = _httpClient.GetAsync("http://api.coincap.io/v2/assets", cancellationToken).Result;
            var strResult = response.Content.ReadAsStringAsync().Result;
            JObject coinData = JObject.Parse(strResult);
            IList<JToken> results = coinData["data"].Children().ToList();
            List<Coin> coins = new();
            foreach (JToken res in results)
            {
                Coin coin = res.ToObject<Coin>();
                coins.Add(coin);
            }
            coins.OrderBy(x => x.Rank);
            return coins;
        }
        public void GetCoinsList(ref ObservableCollection<Coin> coinsList, ref ObservableCollection<Coin> topTenCoinsList)
        {
            var tempCoinsList = GetCoinsListFromApi();

            foreach (var coin in tempCoinsList)
            {
                coinsList.Add(coin);
            }

            while (tempCoinsList.Count>10)
                tempCoinsList.RemoveAt(10);

            foreach (var coin in tempCoinsList)
            {
                topTenCoinsList.Add(coin);
            }


        }
        public void LoadCoinsList(ref ObservableCollection<Coin> coinsList, ref ObservableCollection<Coin> topTenCoinsList)
        {
            GetCoinsList(ref coinsList, ref topTenCoinsList);
            //AddTestData(ref coinsList);
        }

        public static void AddTestData(ref ObservableCollection<Coin> coinsList)
        {
            coinsList.Add(new Coin()
            {
                Name = "Bitoc",
                Rank = 1,
                Symbol = "BTC"
            });
            coinsList.Add(new Coin()
            {
                Name = "Ehtetium",
                Rank = 2,
                Symbol = "ETC"
            });
        }
    }
}
