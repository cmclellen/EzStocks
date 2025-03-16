import { useEffect, useReducer, useState } from "react";
import { useDebounce } from "@uidotdev/usehooks";
import useSearchStocks from "../hooks/useSearchStock";
import { searchStock } from "../services/StocksApi";

interface SearchBoxState {
  searchText: string;
  suggestions: string[];
  showSuggestions: boolean;
  selectedItem?: string;
  status: "loading" | "searching";
}

const initialState: SearchBoxState = {
  searchText: "",
  suggestions: ["here"],
  showSuggestions: false,
  selectedItem: undefined,
  status: "loading",
};

function reducer(state: SearchBoxState, action: any): SearchBoxState {
  switch (action.type) {
    case "SET_SUGGESTIONS":
      return { ...state, suggestions: action.payload, showSuggestions: true };
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
  suggestions: string[];
  onSuggestionSelected: (selectedItem: string) => void;
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
    <ul className="suggestions">
      {suggestions.map((suggestion) => (
        <li
          key={suggestion}
          onClick={(e) => onSuggestionSelected(e.currentTarget.innerText)}
        >
          {suggestion}
        </li>
      ))}
    </ul>
  );
}

function SearchBox() {
  const [searchTerm, setSearchTerm] = useState("");
  const [state, dispatch] = useReducer(reducer, initialState);
  const debouncedSearchTerm = useDebounce(searchTerm, 500);
  // const {searchStockResponse} = useSearchStocks(searchTerm);

  function onSuggestionSelected(selectedItem: string) {
    dispatch({ type: "SET_SELECTED", payload: selectedItem });
  }

  function onSearchTextChange(e: any) {
    setSearchTerm((i) => i + e.target.value);
  }

  useEffect(() => {
    dispatch({
      type: "SET_SEARCH_TEXT",
      payload: debouncedSearchTerm,
    });
    if (!debouncedSearchTerm) return;

    async function searchStocks() {
      const response = await searchStock({ symbol: debouncedSearchTerm });
      console.log(response);
    }
    searchStocks();
  }, [debouncedSearchTerm]);

  return (
    <>
      <input
        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        id="stock"
        type="text"
        value={state.selectedItem || ""}
        onChange={onSearchTextChange}
        placeholder="Search for your stock..."
      />
      <div>{state.status}</div>
      <SuggestionList
        showSuggestions={state.showSuggestions}
        suggestions={state.suggestions}
        onSuggestionSelected={onSuggestionSelected}
      />
    </>
  );
}

export default SearchBox;
