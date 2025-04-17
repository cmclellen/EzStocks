import Page from "../components/Page";
import Spinner from "../components/Spinner";
import StockGraph from "../components/StockGraph";
import StockSelector from "../components/StockSelector";
import useStocksHistory from "../hooks/useStocksHistory";

function ViewStock() {
  const { stocksHistory, error, isLoadingStocksHistory } = useStocksHistory();

  if (isLoadingStocksHistory) return <div>Loading...</div>;
  if (error) throw new Error("Failed loading stock history");

  return (
    <Page title="Dashboard">
      {isLoadingStocksHistory && <Spinner />}
      {!isLoadingStocksHistory && (
        <div className="flex flex-row">
          <StockSelector stocksHistory={stocksHistory} />
          <StockGraph stocksHistory={stocksHistory} />
        </div>
      )}
    </Page>
  );
}

export default ViewStock;
