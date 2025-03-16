import AxiosInstance from "../AxiosInstance";

interface StockPrice {
  createdDate: Date;
  stocks: { [symbol: string]: number };
}

export interface StocksHistory {
  prices: StockPrice[];
  tickers: { symbol: string; name: string; color: string }[];
}

async function getStocksHistory(): Promise<StocksHistory> {
  const url = "/stocks/history";
  const { data } = await AxiosInstance.get<StocksHistory>(url);
  return data;
}

async function addStock({ stock }: { stock: string }) {
  const url = "/stocks";
  const { data } = await AxiosInstance.post(url, { stock });
  return data;
}

export interface SearchStockResponse {
  items: { symbol: string; name: string }[];
}

async function searchStock({
  symbol,
}: {
  symbol: string;
}): Promise<SearchStockResponse> {
  const url = "/stocks/search";
  const { data } = await AxiosInstance.get<SearchStockResponse>(url, {
    params: { searchText: symbol },
  });
  return data;
}

export { getStocksHistory, addStock, searchStock };
