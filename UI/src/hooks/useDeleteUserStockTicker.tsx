import { useMutation, useQueryClient } from "@tanstack/react-query";
import QueryKey from "../utils/queryKeys";
import { deleteUserStockTicker as deleteUserStockTickerApi } from "../services/StocksApi";
import toast from "react-hot-toast";

export default function useDeleteUserStockTicker() {
  const queryClient = useQueryClient();
  const {
    mutate: deleteUserStockTicker,
    isPending: isDeletingUserStockTicker,
  } = useMutation({
    mutationKey: [QueryKey.USER],
    mutationFn: deleteUserStockTickerApi,
    onSuccess: () => {
      toast.success("Successfully deleted user stock ticker");
      queryClient.invalidateQueries({ queryKey: [QueryKey.USER] });
    },
    onError: (error) => toast.error(error.message),
  });
  return { deleteUserStockTicker, isDeletingUserStockTicker };
}
