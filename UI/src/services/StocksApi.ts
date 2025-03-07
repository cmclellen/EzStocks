import AxiosInstance from "../AxiosInstance";

async function getStocksHistory() {
  const url = "/stocks/history";
  const { data } = await AxiosInstance.get(url);
  return data;
}

async function addStock({ stock }: { stock: string }) {
  const url = "/stocks";
  const { data } = await AxiosInstance.post(url, { stock });
  return data;
}

export { getStocksHistory, addStock };
