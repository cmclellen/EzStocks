import {
  CartesianGrid,
  Line,
  LineChart,
  ResponsiveContainer,
  XAxis,
  YAxis,
} from "recharts";
import useStocks from "../hooks/useStocks";

function StockGraph() {
  const { stocks } = useStocks();

  return (
    <ResponsiveContainer width="100%" aspect={4.0 / 2.0} className="py-15">
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
