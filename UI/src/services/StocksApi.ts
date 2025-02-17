async function getStocks() {
  const headers = {
    "Content-Type": "application/json",
    Accept: "application/json",
  };
  const url = "http://localhost:8000/stocks/comparison-summary";
  const response = await fetch(url, { headers });
  return await response.json();
}

async function addStock({ stock }: { stock: string }) {
  const headers = {
    "Content-Type": "application/json",
    Accept: "application/json",
  };
  const opts = {
    headers,
    method: "POST",
    body: JSON.stringify({ stock }),
  };
  const url = "http://localhost:8000/stocks";
  const response = await fetch(url, opts);
  return await response.json();
}

export { getStocks, addStock };
