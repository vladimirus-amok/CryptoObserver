using CryptoObserver.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CryptoObserver.ViewModels
{
    public class CoinViewModel
    {
        private ObservableCollection<Coin> coinsList;
        public ObservableCollection<Coin> CoinsList { get { return this.coinsList; } }
        public CoinViewModel()
        {
            this.coinsList = new ObservableCollection<Coin>();

            HttpClient httpClient=new HttpClient();
            DataSource dataSource = new DataSource(httpClient);
            dataSource.LoadCoinsList(ref this.coinsList);
        }
    }

    internal class DataSource
    {
        private readonly HttpClient _httpClient;

        public DataSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Coin>> GetCoinsListFromApiAsync(CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync("http://api.coincap.io/v2/assets", cancellationToken);
            var strResult = await response.Content.ReadAsStringAsync();
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

        public void GetCoinsList(ref ObservableCollection<Coin> coinsList)
        {
            var tempCoinList = GetCoinsListFromApiAsync();
            foreach (var coin in tempCoinList.Result)
            {
                coinsList.Add(coin);
            }
        }

        public void LoadCoinsList(ref ObservableCollection<Coin> coinsList)
        {
            GetCoinsList(ref coinsList);
        }
    }
}
