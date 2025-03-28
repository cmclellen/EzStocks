import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import useStocksHistory from "../hooks/useStocksHistory";
import Spinner from "./Spinner";

function StockGraph() {
  const { stocksHistory, error, isLoadingStocksHistory } = useStocksHistory();

  if (isLoadingStocksHistory) return <Spinner />;

  if (error)
    return (
      <div className="w-full text-center">
        <span className="border py-2 px-4 rounded-lg text-center font-semibold">
          An unexpected error has occurred while loading the graph
        </span>
      </div>
    );

  return (
    <>
      {/* <pre>{JSON.stringify(stocksHistory, null, 2)}</pre> */}
      <ResponsiveContainer
        width="100%"
        aspect={4.0 / 2.0}
        className="flex items-center"
      >
        <LineChart data={stocksHistory!.prices}>
          <XAxis dataKey="createdDate" />
          <YAxis />
          <Legend />
          <Tooltip />
          <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
          {stocksHistory!.tickers.map((ticker) => (
            <Line
              key={ticker.ticker}
              name={`${ticker.name} (${ticker.ticker})`}
              type="monotone"
              dataKey={`stocks[${ticker.ticker}]`}
              stroke={ticker.color}
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
