import { useEffect, useReducer, useState } from "react";
import { useDebounce } from "@uidotdev/usehooks";
import { searchStock } from "../services/StocksApi";

const DEBOUNCE_INTERVAL = 300;

interface Suggestion {
  ticker: string;
  name: string;
}

interface StockTickerSearchBoxState {
  searchText: string;
  suggestions: Suggestion[];
  showSuggestions: boolean;
  selectedItem?: string;
  status: "ready" | "searching";
}

const initialState: StockTickerSearchBoxState = {
  searchText: "",
  suggestions: [],
  showSuggestions: false,
  selectedItem: undefined,
  status: "ready",
};

function reducer(
  state: StockTickerSearchBoxState,
  action: any
): StockTickerSearchBoxState {
  switch (action.type) {
    case "SET_SUGGESTIONS":
      return {
        ...state,
        status: "ready",
        suggestions: action.payload,
        showSuggestions: true,
      };
    case "SET_SELECTED":
      return {
        ...state,
        selectedItem: action.payload,
        suggestions: [],
        showSuggestions: false,
      };
    case "SET_SEARCH_TEXT":
      return {
        ...state,
        searchText: action.payload,
        status: "searching",
      };
    default:
      throw new Error("Unknown action");
  }
}

interface SuggestionListProps {
  showSuggestions: boolean;
  suggestions: Suggestion[];
  onSuggestionSelected: (selectedItem: Suggestion) => void;
}

function SuggestionList({
  showSuggestions,
  suggestions,
  onSuggestionSelected,
}: SuggestionListProps) {
  if (!showSuggestions) return null;

  if (suggestions.length == 0)
    return (
      <div className="no-suggestions">
        <em>No suggestions available.</em>
      </div>
    );

  return (
    <ul className="border border-gray-300 rounded-md shadow-md">
      {suggestions.map((suggestion) => (
        <li
          className="hover:bg-gray-100 hover:font-bold hover:cursor-pointer p-2 text-gray-700"
          key={suggestion.ticker}
          onClick={(e) => console.log(e.currentTarget.id)} // onSuggestionSelected(e.currentTarget.innerText)
        >
          {suggestion.ticker}{" "}
          <span className="text-sm font-light text-gray-500">
            {suggestion.name}
          </span>
        </li>
      ))}
    </ul>
  );
}

const sortBy = (key: string) => {
  return (a: any, b: any) => (a[key] > b[key] ? 1 : b[key] > a[key] ? -1 : 0);
};

function StockTickerSearchBox() {
  const [searchTerm, setSearchTerm] = useState("");
  const [state, dispatch] = useReducer(reducer, initialState);
  const debouncedSearchTerm = useDebounce(searchTerm, DEBOUNCE_INTERVAL);

  function onSuggestionSelected(selectedItem: Suggestion) {
    dispatch({ type: "SET_SELECTED", payload: selectedItem });
  }

  function onSearchTextChange(e: any) {
    const { value } = e.target;
    setSearchTerm(value);
  }

  useEffect(() => {
    dispatch({
      type: "SET_SEARCH_TEXT",
      payload: debouncedSearchTerm,
    });
    if (!debouncedSearchTerm) return;

    async function searchStocks() {
      const response = await searchStock({ ticker: debouncedSearchTerm });

      dispatch({
        type: "SET_SUGGESTIONS",
        payload: response.stockTickers
          .map((item) => ({ ticker: item.ticker, name: item.name }))
          .concat()
          .sort(sortBy("ticker")),
      });
    }
    searchStocks();
  }, [debouncedSearchTerm]);

  return (
    <>
      <input
        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        id="stock"
        type="text"
        value={state.selectedItem || searchTerm}
        onChange={onSearchTextChange}
        placeholder="Search for your stock..."
      />
      <SuggestionList
        showSuggestions={state.showSuggestions}
        suggestions={state.suggestions}
        onSuggestionSelected={onSuggestionSelected}
      />
    </>
  );
}

export default StockTickerSearchBox;
