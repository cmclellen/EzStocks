import { useQuery } from "@tanstack/react-query";
import { getStocks } from "../services/StocksApi";

export default function useStocks() {
  const {
    data: stocks,
    error,
    isLoading: isLoadingStocks,
  } = useQuery({ queryKey: ["stocks"], queryFn: () => getStocks() });
  return { stocks, error, isLoadingStocks };
}
