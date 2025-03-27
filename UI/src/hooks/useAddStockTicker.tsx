import { useMutation, useQuery } from "@tanstack/react-query";
import { getStocksHistory, StocksHistory } from "../services/StocksApi";
import { addStock } from "../services/StocksApi";
import AddStock from "../features/AddStock";

export default function useAddStockTicker() {
  const {
    error,
    isPending,
    mutateAsync
  } = useMutation({    
    onSuccess: (data) => console.log(data),
    mutationFn: addStock,
  });
  return { mutateAsync, error, isPending };
}
