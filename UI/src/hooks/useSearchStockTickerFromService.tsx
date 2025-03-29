import { useQuery } from "@tanstack/react-query";
import {
  searchStockTickers as searchStockTickersApi,
  SearchStockTickersResponse,
} from "../services/StocksApi";
import QueryKey from "../utils/queryKeys";

export default function useSearchStockTickerFromService(ticker: string) {
  const {
    data: searchStockTickersResponse,
    error: searchStockTickerError,
    isFetching: isSearchingStockTickers,
    refetch: searchStockTickers,
  } = useQuery<SearchStockTickersResponse>({
    queryKey: [QueryKey.STOCK_TICKERS_SEARCHED],
    enabled: false,
    queryFn: () => searchStockTickersApi({ searchText: ticker }),
  });
  return {
    searchStockTickersResponse,
    searchStockTickerError,
    isSearchingStockTickers,
    searchStockTickers,
  };
}
