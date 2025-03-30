import { useEffect, useState } from "react";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
import StockTickerSearchBox, {
  Suggestion,
} from "../components/StockTickerSearchBox";
import useAddStockTicker from "../hooks/useAddStockTicker";
import { useModal } from "../components/Modal";

interface AddStockTickerProps {
  suggestion?: Suggestion;
}

function AddStockTicker({ suggestion }: AddStockTickerProps) {
  const { close } = useModal();
  const [stockTicker, setStockTicker] = useState<Suggestion | undefined>(
    suggestion
  );
  const { addStockTicker } = useAddStockTicker();

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    addStockTicker(
      { ...stockTicker!, color: "#000" },
      {
        onSuccess: () => {
          close();
        },
      }
    );
  }

  return (
    <Form onSubmit={handleSubmit}>
      <FormRow label="Stock">
        <StockTickerSearchBox
          id="stock"
          suggestion={suggestion}
          onSelectedSuggestion={setStockTicker}
        />
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => close()}>Cancel</FormButton>
        <FormButton type="submit">Add</FormButton>
      </div>
    </Form>
  );
}

export default AddStockTicker;
