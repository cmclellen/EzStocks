import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  XAxis,
  YAxis,
} from "recharts";
import useStocksHistory from "../hooks/useStocksHistory";
import Spinner from "./Spinner";

function StockGraph() {
  const { stocksHistory, isLoadingStocksHistory } = useStocksHistory();

  if (isLoadingStocksHistory) return <Spinner />;

  return (
    <>
      {/* <pre>{JSON.stringify(stocksHistory, null, 2)}</pre> */}
      <ResponsiveContainer
        width="100%"
        aspect={4.0 / 2.0}
        className="flex items-center"
      >
        <LineChart data={stocksHistory.tickers}>
          <XAxis dataKey="createdDate" />
          <YAxis />
          <Legend />
          <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
          {stocksHistory.symbols.map((symbol: string) => (
            <Line
              name="Apple"
              type="monotone"
              dataKey={`stocks[${symbol}]`}
              stroke="#8884d8"
            />
          ))}
        </LineChart>
      </ResponsiveContainer>
    </>
  );
}

export default StockGraph;

// <Line
//   name="Apple"
//   type="monotone"
//   dataKey="stocks.AAPL"
//   stroke="#8884d8"
// />
// <Line
//   name="Google"
//   type="monotone"
//   dataKey="stocks.MSFT"
//   stroke="#82ca9d"
// />
