import { useMutation, useQueryClient } from "@tanstack/react-query";
import QueryKey from "../utils/queryKeys";
import toast from "react-hot-toast";
import { deleteStockTicker as deleteStockTickerApi } from "../services/StocksApi";

export default function useDeleteStockTicker() {
  const queryClient = useQueryClient();
  const {
    mutate: deleteStockTicker,
    error: deleteStockTickerError,
    isPending: isDeletingStockTicker,
  } = useMutation({
    mutationKey: [QueryKey.STOCK_TICKERS],
    mutationFn: deleteStockTickerApi,
    onSuccess: () => {
      toast.success("Successfully deleted stock ticker");
      queryClient.invalidateQueries({ queryKey: [QueryKey.STOCK_TICKERS] });
    },
    onError: (error) => toast.error(error.message),
  });
  return { deleteStockTicker, deleteStockTickerError, isDeletingStockTicker };
}
