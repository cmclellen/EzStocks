import Page from "../components/Page";
import StockGraph from "../components/StockGraph";
import StockSelector from "../components/StockSelector";

function ViewStock() {
  return (
    <Page title="Dashboard">
      <StockSelector />
      <StockGraph />
    </Page>
  );
}

export default ViewStock;
