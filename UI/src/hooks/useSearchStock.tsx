import { useQuery } from "@tanstack/react-query";
import {
  searchStockTickers,
  SearchStockTickersResponse,
} from "../services/StocksApi";

export default function useSearchStocks(symbol: string) {
  const {
    data: searchStockResponse,
    error,
    isLoading: isLoadingSearchingStocks,
  } = useQuery<SearchStockTickersResponse>({
    queryKey: ["stocks"],
    queryFn: () => searchStockTickers({ searchText: symbol }),
  });
  return { searchStockResponse, error, isLoadingSearchingStocks };
}
