import { useReducer, useState } from "react";

const initialState = {
  suggestions: ["here"],
  showSuggestions: false,
  selectedItem: undefined,
  status: "loading",
};

function reducer(state: any, action: any) {
  switch (action.type) {
    case "SET_SUGGESTIONS":
      return { ...state, suggestions: action.payload, showSuggestions: true };
    case "SET_SELECTED":
      return {
        ...state,
        selected: action.payload,
        suggestions: [],
        showSuggestions: false,
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
  const [state, dispatch] = useReducer(reducer, initialState);

  function onSuggestionSelected(selectedItem: string) {
    dispatch({ type: "SET_SELECTED", payload: selectedItem });
  }

  function onSearchTextChange(e) {
    const userInput = e.currentTarget.value;
    console.log(userInput);
  }

  return (
    <>
      <input
        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        id="stock"
        type="text"
        value={state.selected}
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

export default SearchBox;
