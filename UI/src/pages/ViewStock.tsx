import Page from "../components/Page";
import Spinner from "../components/Spinner";
import StockGraph from "../components/StockGraph";
import StockSelector, { StockItem } from "../components/StockSelector";
import useStocksHistory from "../hooks/useStocksHistory";

function ViewStock() {
  const { stocksHistory, error, isLoadingStocksHistory } = useStocksHistory();

  if (isLoadingStocksHistory) return <div>Loading...</div>;
  if (error) throw new Error("Failed loading stock history");

  const stockItemNames = stocksHistory?.prices
    .map((p) => Object.keys(p.stocks))
    .reduce((acc, value) => {
      value.forEach((v) => {
        acc.add(v);
      });
      return acc;
    }, new Set<string>());

  const stockItems: StockItem[] = [...stockItemNames!].map((name) => {
    return { name, isEnabled: false };
  });

  const stockTickers = stocksHistory!.stockTickers;

  return (
    <Page title="Dashboard">
      {isLoadingStocksHistory && <Spinner />}
      {!isLoadingStocksHistory && (
        <div className="flex flex-row">
          <StockSelector stockItems={[...stockItems!]} />
          <StockGraph
            stockPrices={stocksHistory!.prices!}
            stockTickers={stockTickers}
          />
        </div>
      )}
    </Page>
  );
}

export default ViewStock;
