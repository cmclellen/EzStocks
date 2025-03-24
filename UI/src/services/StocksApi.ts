import AxiosInstance from "../AxiosInstance";

interface StockPrice {
  createdDate: Date;
  stocks: { [symbol: string]: number };
}

export interface StocksHistory {
  prices: StockPrice[];
  tickers: { ticker: string; name: string; color: string }[];
}

async function getStocksHistory(): Promise<StocksHistory> {
  const url = "/stock-prices/history";
  const { data } = await AxiosInstance.get<StocksHistory>(url);
  return data;
}

async function addStock({ stock }: { stock: string }) {
  const url = "/stock-tickers";
  const { data } = await AxiosInstance.post(url, { stock });
  return data;
}

export interface SearchStockTickersResponse {
  stockTickers: { ticker: string; name: string }[];
}

async function searchStock({
  ticker,
}: {
  ticker: string;
}): Promise<SearchStockTickersResponse> {
  const url = "/stock-tickers/search";
  const { data } = await AxiosInstance.get<SearchStockTickersResponse>(url, {
    params: { searchText: ticker },
  });
  return data;
}

export { getStocksHistory, addStock, searchStock };
