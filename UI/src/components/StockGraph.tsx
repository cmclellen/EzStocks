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
import { ReactNode } from "react";
import { StockPrice } from "../services/StocksApi";

function CustomTooltip({ payload }: { payload?: StockPrice[] }) {
  const prices = [...new Set(payload!.map((i: any) => i.payload.stocks))][0];

  const tickers = prices ? Object.keys(prices!) : [];

  return (
    <div className="w-[150px] border bg-blue-100 rounded-lg">
      <ul>
        {tickers &&
          tickers.map(
            (ticker: string): ReactNode => (
              <li key={ticker} className="flex">
                <div className="flex-1 text-right mr-1 font-bold">{ticker}</div>
                <div className="flex-1 ">{prices[ticker]}</div>
              </li>
            )
          )}
      </ul>
    </div>
  );
}

function StockGraph({stocksHistory}) {
  // const { stocksHistory, error, isLoadingStocksHistory } = useStocksHistory();

  // if (isLoadingStocksHistory) return <Spinner />;

  // if (error) throw new Error("Failed loading stock history");

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
          <Tooltip content={<CustomTooltip></CustomTooltip>} />
          <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
          {stocksHistory!.stockTickers.map((ticker) => (
            <Line
              key={ticker.ticker}
              name={`${ticker.name} (${ticker.ticker})`}
              type="monotone"
              dataKey={`pricePercentages[${ticker.ticker}]`}
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
