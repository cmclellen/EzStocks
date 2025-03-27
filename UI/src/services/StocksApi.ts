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

export interface AddStockTickerRequest {
  ticker: string;
  name: string;
  color: string;
}
async function addStockTicker(req: AddStockTickerRequest): Promise<void> {
  const url = "/stock-tickers";
  const { data } = await AxiosInstance.post(url, req);
  return data;
}

export interface SearchStockTickersResponse {
  stockTickers: { ticker: string; name: string }[];
}

export interface SearchStockTickersRequest {
  searchText: string;
}

async function searchStockTickers({
  searchText,
}: SearchStockTickersRequest): Promise<SearchStockTickersResponse> {
  const url = "/stock-tickers/search";
  const { data } = await AxiosInstance.get<SearchStockTickersResponse>(url, {
    params: { searchText },
  });
  return data;
}

export { getStocksHistory, addStockTicker, searchStockTickers };
