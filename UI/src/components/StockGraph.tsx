import {
  CartesianGrid,
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
        <XAxis dataKey="name" />
        <YAxis />
        <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
        <Line type="monotone" dataKey="uv" stroke="#8884d8" />
        <Line type="monotone" dataKey="pv" stroke="#82ca9d" />
      </LineChart>
    </ResponsiveContainer>
  );
}

export default StockGraph;
