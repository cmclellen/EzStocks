import { useEffect, useState } from "react";
import { StocksHistory } from "../services/StocksApi";
import CheckBox from "./CheckBox";

interface StockSelectorProps {
  stocksHistory?: StocksHistory;
}

function StockSelector({ stocksHistory }: StockSelectorProps) {
  const [stockTickers, setStockTickers] = useState<string[]>([]);

  useEffect(() => {
    const items = stocksHistory?.prices
      .map((p) => Object.keys(p.stocks))
      .reduce((acc, value) => {
        value.forEach((v) => acc.add(v));
        return acc;
      }, new Set<string>());
    console.log(items);
    setStockTickers([...items!]);
  }, [stocksHistory]);

  return (
    <ul className="">
      {stockTickers &&
        stockTickers.map((name, index) => (
          <li key={index}>
            <CheckBox title={name}></CheckBox>
          </li>
        ))}
    </ul>
  );
}

export default StockSelector;
