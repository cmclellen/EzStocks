import { useQuery } from "@tanstack/react-query";
import { getStocksHistory, StocksHistory } from "../services/StocksApi";

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
