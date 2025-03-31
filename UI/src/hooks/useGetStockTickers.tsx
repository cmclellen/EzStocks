import { useQuery } from "@tanstack/react-query";
import { getStockTickers } from "../services/StocksApi";
import QueryKey from "../utils/queryKeys";

export default function useGetStockTickers() {
  const {
    error,
    isPending: isLoadingStockTickers,
    data: stockTickers,
  } = useQuery({
    queryKey: [QueryKey.STOCK_TICKERS],
    queryFn: async () => {
      const rsp = await getStockTickers();
      return rsp?.stockTickers;
    },
  });
  return { stockTickers, error, isLoadingStockTickers };
}
