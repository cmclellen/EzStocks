import type { Meta, StoryObj } from "@storybook/react";
import { http, HttpResponse, delay } from "msw";
import StockTickerSearchBox, { Suggestion } from "./StockTickerSearchBox";
import { SearchStockTickersResponse } from "../services/StocksApi";
import { useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const testData: SearchStockTickersResponse = {
  stockTickers: [
    {
      name: "Apple Inc.",
      ticker: "AAPL",
    },
    {
      name: "Microsoft Corporation",
      ticker: "MSFT",
    },
    {
      name: "Amazon.com Inc.",
      ticker: "AMZN",
    },
    {
      name: "Meta Platforms Inc.",
      ticker: "META",
    },
    {
      name: "Alphabet Inc.",
      ticker: "GOOGL",
    },
    {
      name: "Tesla Inc.",
      ticker: "TSLA",
    },
    {
      ticker: "MSA",
      name: "Mine Safety Incorporated",
    },
    {
      ticker: "MSAI",
      name: "MultiSensor AI Holdings, Inc. Common Stock",
    },
    {
      ticker: "MSB",
      name: "Mesabi Trust",
    },
    {
      ticker: "MSBI",
      name: "Midland States Bancorp, Inc. Common Stock",
    },
    {
      ticker: "MSCI",
      name: "MSCI, Inc.",
    },
    {
      ticker: "MSDL",
      name: "Morgan Stanley Direct Lending Fund",
    },
  ],
};

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,
    },
  },
});

function filterByTicker(searchText: string) {
  const result = structuredClone(testData);
  result.stockTickers = result.stockTickers.filter((i) =>
    i.ticker.startsWith(searchText!.toUpperCase())
  );
  return result;
}

//👇 This default export determines where your story goes in the story list
const meta: Meta<typeof StockTickerSearchBox> = {
  component: StockTickerSearchBox,
};

export default meta;
type Story = StoryObj<typeof StockTickerSearchBox>;

export const Default: Story = {
  render: () => {
    const [selectedSuggestion, setSelectedSuggestion] = useState<Suggestion>();

    function onSelectedSuggestion(selected?: Suggestion) {
      setSelectedSuggestion(selected);
    }
    return (
      <>
        <QueryClientProvider client={queryClient}>
          <StockTickerSearchBox onSelectedSuggestion={onSelectedSuggestion} />
          {selectedSuggestion && (
            <div>
              Selection: {selectedSuggestion?.ticker} -{" "}
              {selectedSuggestion?.name}
            </div>
          )}
          {!selectedSuggestion && <div>No selection</div>}
        </QueryClientProvider>
      </>
    );
  },
  args: {
    //👇 The args you need here will depend on your component
  },
  parameters: {
    msw: {
      handlers: [
        http.get("/api/stock-tickers/search", async ({ request }) => {
          await delay(1500);
          const url = new URL(request.url);
          const searchText = url.searchParams.get("searchText");
          return HttpResponse.json(filterByTicker(searchText!));
        }),
      ],
    },
  },
};
