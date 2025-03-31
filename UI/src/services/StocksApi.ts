import AxiosInstance from "../AxiosInstance";

export interface StockTicker {
  ticker: string;
  name: string;
  color: string;
}

interface StockPrice {
  createdDate: Date;
  stocks: { [symbol: string]: number };
}

export interface StocksHistory {
  prices: StockPrice[];
  stockTickers: StockTicker[];
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

async function updateStockTicker(req: AddStockTickerRequest): Promise<void> {
  const url = "/stock-tickers";
  const { data } = await AxiosInstance.put(url, req);
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

export interface GetStockTickersResponse {
  stockTickers: StockTicker[];
}

async function getStockTickers(): Promise<GetStockTickersResponse> {
  const url = "/stock-tickers";
  const { data } = await AxiosInstance.get<GetStockTickersResponse>(url);
  return data;
}

async function deleteStockTicker(ticker: string): Promise<void> {
  const url = "/stock-tickers";
  const { data } = await AxiosInstance.delete(url, {
    params: { ticker },
  });
  return data;
}

export interface GetUserResponse {
  userId: string;
  firstNames: string;
  lastNames: string;
  stockTickers: StockTicker[];
}

async function getUser(userId: string): Promise<GetUserResponse> {
  const url = `/users/${userId}`;
  const { data } = await AxiosInstance.get(url);
  return data;
}

async function addUserStockTicker({
  userId,
  ticker,
}: {
  userId: string;
  ticker: string;
}): Promise<void> {
  const url = `/users/${userId}/stock-tickers`;
  await AxiosInstance.post(url, {
    ticker,
  });
}

async function deleteUserStockTicker({
  userId,
  ticker,
}: {
  userId: string;
  ticker: string;
}): Promise<void> {
  const url = `/users/${userId}/stock-tickers/${ticker}`;
  await AxiosInstance.delete(url);
}

export {
  getStockTickers,
  getStocksHistory,
  addStockTicker,
  searchStockTickers,
  deleteStockTicker,
  updateStockTicker,
  getUser,
  addUserStockTicker,
  deleteUserStockTicker,
};
