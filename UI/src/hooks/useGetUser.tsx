import { useQuery } from "@tanstack/react-query";
import QueryKey from "../utils/queryKeys";
import { getUser as getUserApi } from "../services/StocksApi";

export default function useGetUser(userId: string) {
  const { data: user, isPending: isLoadingUser } = useQuery({
    queryKey: [QueryKey.USER],
    queryFn: () => getUserApi(userId),
  });
  return { user, isLoadingUser };
}
