import { useMutation, useQueryClient } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { addStockTicker as addStockTickerApi } from "../services/StocksApi";
import QueryKey from "../utils/queryKeys";

export default function useAddStockTicker() {
  const queryClient = useQueryClient();
  const {
    error,
    isPending: isAddingStockTicker,
    mutate: addStockTicker,
  } = useMutation({
    onSuccess: () => {
      toast.success("Successfully added stock ticker");
      queryClient.invalidateQueries({ queryKey: [QueryKey.STOCK_TICKERS] });
    },
    onError: (error) => toast.error(error.message),
    mutationFn: addStockTickerApi,
  });
  return { addStockTicker, error, isAddingStockTicker };
}
