import { useQuery } from "@tanstack/react-query";
import { getStocksHistory } from "../services/StocksApi";

interface StockPrice {
  createdDate: Date;
  stocks: { [symbol: string]: number };
}

interface StocksHistory {
  prices: StockPrice[];
  tickers: { symbol: string; name: string; color: string }[];
}

export default function useStocksHistory() {
  const {
    data: stocksHistory,
    error,
    isLoading: isLoadingStocksHistory,
  } = useQuery<StocksHistory>({
    queryKey: ["stocks"],
    queryFn: () => getStocksHistory(),
  });
  return { stocksHistory, error, isLoadingStocksHistory };
}
