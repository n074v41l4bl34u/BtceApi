using System;
using System.Linq;
using System.Diagnostics.Contracts;

namespace BtcE.Data
{
  public static class BtcePairHelper
  {
    [Obsolete("Functionality replaced by BtcePairHelper.GetInfo(BtcePair)", true)]
    private static readonly Tuple<BtcePair, int, int>[] precisions = new[]
    {
        Tuple.Create(BtcePair.btc_usd, 2, 3),
        Tuple.Create(BtcePair.ltc_btc, 1, 5),
        Tuple.Create(BtcePair.ltc_usd, 1, 6),
        Tuple.Create(BtcePair.nmc_btc, 1, 5),
        Tuple.Create(BtcePair.ppc_btc, 1, 5),
        Tuple.Create(BtcePair.trc_btc, 1, 5),
    };

    public static BtcePair FromString(string s)
    {
      Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(s));

      var ret = BtcePair.Unknown;
      Enum.TryParse(s.ToLowerInvariant(), out ret);
      return ret;
    }

    /// <summary>
    ///     Returns decimal precision for Amount and Price
    /// </summary>
    /// <param name="btcPair">Btc-E Pair</param>
    /// <returns>Tuple of int,int where Item1 is Amount's precision and Item2 is Price's precision</returns>

    [Obsolete("Functionality replaced by BtcePairHelper.GetInfo(BtcePair)", true)]
    public static Tuple<int, int> GetPrecision(BtcePair btcPair)
    {
      throw new NotSupportedException("Method is no longer supported. Use BtcePairHelper.GetInfo(BtcePair) instead.");

      Tuple<BtcePair, int, int> pairPrecision = precisions.SingleOrDefault(x => x.Item1 == btcPair);
      if (pairPrecision == default(Tuple<BtcePair, int, int>))
      {
        return Tuple.Create(4, 8); // ex. 12.3456 USD for 0.12345678 BTC:
      }
      else
      {
        return Tuple.Create(pairPrecision.Item2, pairPrecision.Item3);
      }
    }

    static DateTime _apiInfoLastFetch = DateTime.MinValue;
    static ApiInfo _apiInfoCache = null;
    static readonly TimeSpan _apiInfoCacheMaxAge = new TimeSpan(24, 0, 0);

    public static BtcePairInfo GetInfo(BtcePair pair, bool forceFetch = false)
    {
      UpdateExpiredApiInfo(forceFetch);
      return _apiInfoCache.Pairs[pair];
    }

    public static BtcePairInfo GetInfo(string pair, bool forceFetch = false)
    {
      UpdateExpiredApiInfo(forceFetch);
      return _apiInfoCache.UnsupportedPairs[pair];
    }

    private static void UpdateExpiredApiInfo(bool forceFetch)
    {
      if (forceFetch || _apiInfoCache == null || DateTime.UtcNow - _apiInfoLastFetch >= _apiInfoCacheMaxAge)
      {
        _apiInfoCache = BtceApiV3.GetApiInfo();
        _apiInfoLastFetch = DateTime.UtcNow;
      }
    }

    public static Tuple<BtceCurrency, BtceCurrency> ToCurrencies(BtcePair pair)
    {
      return BtceCurrencyHelper.FromBtcePair2(pair);
    }

    public static string ToString(BtcePair v)
    {
      return Enum.GetName(typeof(BtcePair), v).ToLowerInvariant();
    }
  }
}