async function getStocks() {
  const opts = {
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
  };
  const url = "http://localhost:8000/stocks";
  const response = await fetch(url, opts);
  return await response.json();
}

export { getStocks };
