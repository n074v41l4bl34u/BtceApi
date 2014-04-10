using System;
using System.Diagnostics.Contracts;

namespace BtcE.Data
{
  public static class TradeTypeHelper
  {
    public static TradeType FromString(string s)
    {
      Contract.Requires<ArgumentNullException>(s != null);

      s = s.ToLower();
      switch (s)
      {
        case "sell":
          return TradeType.Sell;
        case "buy":
          return TradeType.Buy;
        default:
          throw new ArgumentException();
      }
    }

    public static string ToString(TradeType v)
    {
      return Enum.GetName(typeof(TradeType), v).ToLowerInvariant();
    }
  }
}