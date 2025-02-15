import { useEffect, useState } from "react";
import {
  CartesianGrid,
  Line,
  LineChart,
  ResponsiveContainer,
  XAxis,
  YAxis,
} from "recharts";

function StockGraph() {
  const [data, setData] = useState([]);

  useEffect(() => {
    const opts = {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    };
    console.log("here1");
    fetch("http://localhost:8000/stocks", opts)
      .then((response) => {
        return response.json();
      })
      .then((myjson) => {
        console.log(myjson);
        setData(myjson);
      });
  }, []);

  return (
    <ResponsiveContainer width="100%" height="100%">
      <LineChart data={data}>
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
