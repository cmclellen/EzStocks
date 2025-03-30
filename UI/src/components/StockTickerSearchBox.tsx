import { useEffect, useReducer, useState } from "react";
import { useDebounce } from "@uidotdev/usehooks";
import useOutsideClick from "../hooks/useOutsideClick";
import useSearchStockTickerFromService from "../hooks/useSearchStockTickerFromService";
import { ImSpinner9 } from "react-icons/im";

const DEBOUNCE_INTERVAL = 300;

export interface Suggestion {
  ticker: string;
  name: string;
}

interface StockTickerSearchBoxState {
  searchText: string;
  suggestions: Suggestion[];
  showSuggestions: boolean;
  selectedItem: Suggestion | undefined;
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
  const hideSuggestions = {
    suggestions: [],
    showSuggestions: false,
  };

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
        ...hideSuggestions,
      };
    case "HIDE_SUGGESTIONS":
      return {
        ...state,
        ...hideSuggestions,
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
  readonly showSuggestions: boolean;
  readonly suggestions: Suggestion[];
  readonly onSuggestionSelected: (selectedItem?: Suggestion) => void;
}

function SuggestionList({
  showSuggestions,
  suggestions,
  onSuggestionSelected,
}: SuggestionListProps) {
  if (!showSuggestions) return null;

  if (suggestions.length == 0)
    return (
      <div className="bg-white border border-gray-300 rounded-md shadow-md p-2 text-gray-700 z-10">
        <em>No suggestions available</em>
      </div>
    );

  return (
    <ul className="border border-gray-300 rounded-md shadow-md z-10 max-h-[220px] overflow-y-auto scrollbar">
      {suggestions.map((suggestion) => (
        <li
          className="bg-white hover:bg-gray-100 hover:font-bold hover:cursor-pointer p-2 text-gray-700"
          key={suggestion.ticker}
        >
          <button
            className="w-full text-left"
            data-ticker={suggestion.ticker}
            onClick={(e) =>
              onSuggestionSelected(
                suggestions.find(
                  (o) => o.ticker === e.currentTarget.dataset.ticker
                )
              )
            }
          >
            {suggestion.ticker}{" "}
            <span className="text-sm font-light text-gray-500">
              {suggestion.name}
            </span>
          </button>
        </li>
      ))}
    </ul>
  );
}

const sortBy = (key: string) => {
  return (a: any, b: any) => (a[key] > b[key] ? 1 : b[key] > a[key] ? -1 : 0);
};

interface StockTickerSearchBoxProps {
  readonly id: string;
  readonly suggestion?: Suggestion;
  readonly onSelectedSuggestion?: (suggestion: Suggestion | undefined) => void;
}

function StockTickerSearchBox({
  id,
  suggestion,
  onSelectedSuggestion,
}: StockTickerSearchBoxProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [state, dispatch] = useReducer(reducer, {
    ...initialState,
    selectedItem: suggestion,
  });
  const debouncedSearchTerm = useDebounce(searchTerm, DEBOUNCE_INTERVAL);
  const ref = useOutsideClick(handleClickOutside);
  const {
    searchStockTickersResponse,
    searchStockTickers,
    isSearchingStockTickers,
  } = useSearchStockTickerFromService(debouncedSearchTerm);

  function handleClickOutside() {
    dispatch({ type: "HIDE_SUGGESTIONS" });
  }

  function onSuggestionSelected(selectedItem: Suggestion | undefined) {
    dispatch({ type: "SET_SELECTED", payload: selectedItem });
    onSelectedSuggestion?.(selectedItem);
  }

  function onSearchTextChange(e: any) {
    const { value } = e.target;
    if (state.selectedItem) {
      onSuggestionSelected(undefined);
    }
    setSearchTerm(value);
  }

  useEffect(() => {
    dispatch({
      type: "SET_SEARCH_TEXT",
      payload: debouncedSearchTerm,
    });
    if (!debouncedSearchTerm) return;
    async function search() {
      await searchStockTickers();
    }
    search();
  }, [debouncedSearchTerm, searchStockTickers]);

  useEffect(() => {
    const stockTickers = searchStockTickersResponse?.stockTickers;
    if (stockTickers === undefined) return;
    dispatch({
      type: "SET_SUGGESTIONS",
      payload: stockTickers
        ?.map((item) => ({
          ticker: item.ticker,
          name: item.name,
        }))
        .concat()
        .sort(sortBy("ticker")),
    });
  }, [searchStockTickersResponse]);

  return (
    <div ref={ref}>
      <div className="relative">
        <input
          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id={id}
          type="text"
          value={
            state.selectedItem
              ? `${state.selectedItem.ticker} (${state.selectedItem.name})`
              : searchTerm
          }
          onChange={onSearchTextChange}
          autoComplete="off"
          placeholder="Search for your stock..."
        />
        {isSearchingStockTickers && (
          <div className="absolute top-0 right-0 p-3 animate-spin">
            <ImSpinner9 />
          </div>
        )}
      </div>
      <div className="relative w-full">
        <SuggestionList
          showSuggestions={state.showSuggestions}
          suggestions={state.suggestions}
          onSuggestionSelected={onSuggestionSelected}
        />
      </div>
    </div>
  );
}

export default StockTickerSearchBox;
