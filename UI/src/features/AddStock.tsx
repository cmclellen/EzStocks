import { useState } from "react";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
import StockTickerSearchBox, { Suggestion } from "../components/StockTickerSearchBox";
import { addStock } from "../services/StocksApi";
import useAddStockTicker from "../hooks/useAddStockTicker";

interface AddStockProps {
  readonly onCloseModal?: () => void;
}

function AddStock({ onCloseModal }: AddStockProps) {
  const [ticker, setTicker] = useState<Suggestion|undefined>();
  const {mutate} = useAddStockTicker();

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const ticker = event.currentTarget.stock.value;
    // addStock({ stock: ticker });

    mutate(ticker)
  }

  return (
    <Form onSubmit={handleSubmit}>
      <FormRow label="Stock">
        <StockTickerSearchBox id="stock" onSelectedSuggestion={setTicker} />
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => onCloseModal?.()}>Cancel</FormButton>
        <FormButton type="submit">Add</FormButton>
      </div>
    </Form>
  );
}

export default AddStock;
