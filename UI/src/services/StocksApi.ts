import AxiosInstance from "../AxiosInstance";

async function getStocks() {
  const url = "/stocks/history";
  const { data } = await AxiosInstance.get(url);
  return data;
}

async function addStock({ stock }: { stock: string }) {
  const url = "/stocks";
  const { data } = await AxiosInstance.post(url, { stock });
  return data;
}

export { getStocks, addStock };
