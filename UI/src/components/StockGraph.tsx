import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  XAxis,
  YAxis,
} from "recharts";
import useStocks from "../hooks/useStocks";
import Spinner from "./Spinner";

function StockGraph() {
  const { stocks, isLoadingStocks } = useStocks();

  if (isLoadingStocks) return <Spinner />;

  return (
    <ResponsiveContainer
      width="100%"
      aspect={4.0 / 2.0}
      className="flex items-center"
    >
      <LineChart data={stocks}>
        <XAxis dataKey="createdDate" />
        <YAxis />
        <Legend />
        <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
        <Line
          name="Apple"
          type="monotone"
          dataKey="stocks.AAPL"
          stroke="#8884d8"
        />
        <Line
          name="Google"
          type="monotone"
          dataKey="stocks.MSFT"
          stroke="#82ca9d"
        />
      </LineChart>
    </ResponsiveContainer>
  );
}

export default StockGraph;
