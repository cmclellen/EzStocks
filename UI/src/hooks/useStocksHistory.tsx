import { useQuery } from "@tanstack/react-query";
import { getStocksHistory } from "../services/StocksApi";

export default function useStocksHistory() {
  const {
    data: stocksHistory,
    error,
    isLoading: isLoadingStocksHistory,
  } = useQuery({ queryKey: ["stocks"], queryFn: () => getStocksHistory() });
  return { stocksHistory, error, isLoadingStocksHistory };
}
