import { useMutation, useQueryClient } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { updateStockTicker as updateStockTickerApi } from "../services/StocksApi";
import QueryKey from "../utils/queryKeys";

export default function useUpdateStockTicker() {
  const queryClient = useQueryClient();
  const {
    error: errorUpdatingStockTicker,
    isPending: isUpdatingStockTicker,
    mutate: updateStockTicker,
  } = useMutation({
    onSuccess: () => {
      toast.success("Successfully updated stock ticker");
      queryClient.invalidateQueries({ queryKey: [QueryKey.STOCK_TICKERS] });
    },
    onError: (error) => toast.error(error.message),
    mutationFn: updateStockTickerApi,
  });
  return { updateStockTicker, errorUpdatingStockTicker, isUpdatingStockTicker };
}
