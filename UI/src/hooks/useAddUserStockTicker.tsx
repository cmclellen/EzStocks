import { useMutation, useQueryClient } from "@tanstack/react-query";
import QueryKey from "../utils/queryKeys";
import toast from "react-hot-toast";
import { addUserStockTicker as addUserStockTickerApi } from "../services/StocksApi";

export function useAddUserStockTicker() {
  const queryClient = useQueryClient();
  const {
    mutate: addUserStockTicker,
    isPending: isAddingUserStockTicker,
    error: addUserStockTickerError,
  } = useMutation({
    onSuccess: () => {
      toast.success("Successfully added stock ticker");
      queryClient.invalidateQueries({ queryKey: [QueryKey.USER] });
    },
    onError: (error) => toast.error(error.message),
    mutationKey: [QueryKey.USER],
    mutationFn: addUserStockTickerApi,
  });
  return {
    addUserStockTicker,
    isAddingUserStockTicker,
    addUserStockTickerError,
  };
}
