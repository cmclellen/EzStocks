import { useQuery } from "@tanstack/react-query";
import { searchStock, SearchStockResponse } from "../services/StocksApi";

export default function useSearchStocks(symbol: string) {
  const {
    data: searchStockResponse,
    error,
    isLoading: isLoadingSearchingStocks,
  } = useQuery<SearchStockResponse>({
    queryKey: ["stocks"],
    queryFn: () => searchStock({ symbol }),
  });
  return { searchStockResponse, error, isLoadingSearchingStocks };
}
