import { useEffect, useState } from "react";
import Page from "../components/Page";
import Spinner from "../components/Spinner";
import StockGraph, { StockGraphTickerData } from "../components/StockGraph";
import StockSelector, { StockItem } from "../components/StockSelector";
import useStocksHistory from "../hooks/useStocksHistory";
import { StockTicker } from "../services/StocksApi";

function ViewStock() {
  const { stocksHistory, error, isLoadingStocksHistory } = useStocksHistory();
  const [stockItems, setStockItems] = useState<StockItem[]>();
  const [stockTickers, setStockTickers] = useState<StockGraphTickerData[]>();

  useEffect(() => {
    const stockGraphTickerDataList: StockGraphTickerData[] = (
      stocksHistory?.stockTickers ?? []
    ).map((st) => {
      return { ...st, isEnabled: true } as StockGraphTickerData;
    });
    setStockTickers(stockGraphTickerDataList);

    const stockItemNames: Set<string> | undefined = stocksHistory?.prices
      .map((p) => Object.keys(p.stocks))
      .reduce((acc, value) => {
        value.forEach((v) => {
          acc.add(v);
        });
        return acc;
      }, new Set<string>());
    setStockItems(
      Array.from(stockItemNames || []).map((name: string) => {
        return { name, isEnabled: true };
      })
    );
  }, [stocksHistory]);

  if (isLoadingStocksHistory) return <div>Loading...</div>;
  if (error) throw new Error("Failed loading stock history");

  const handleStockItemChanged = (stockItem: StockItem) => {
    setStockTickers((tickers) => {
      const ticker = tickers?.find((t) => t.ticker === stockItem.name);
      ticker!.isEnabled = stockItem.isEnabled;
      return [...(tickers || [])];
    });
  };

  return (
    <Page title="Dashboard">
      {isLoadingStocksHistory && <Spinner />}
      {!isLoadingStocksHistory && (
        <div className="flex flex-row">
          <StockSelector
            stockItems={stockItems}
            onStockItemChanged={handleStockItemChanged}
          />
          <StockGraph
            stockPrices={stocksHistory!.prices!}
            stockTickers={stockTickers!}
          />
        </div>
      )}
    </Page>
  );
}

export default ViewStock;
