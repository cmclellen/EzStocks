import { useState } from "react";
import { HexColorPicker } from "react-colorful";
import ControlSpinner from "../components/ControlSpinner";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
import { useModal } from "../components/Modal";
import StockTickerSearchBox, {
  Suggestion,
} from "../components/StockTickerSearchBox";
import useAddStockTicker from "../hooks/useAddStockTicker";
import useUpdateStockTicker from "../hooks/useUpdateStockTicker";
import { StockTicker } from "../services/StocksApi";

interface AddStockTickerProps {
  stockTicker?: StockTicker;
}

function AddStockTicker(props: AddStockTickerProps) {
  const { close } = useModal();
  const [stockTicker, setStockTicker] = useState<Suggestion | undefined>(
    props.stockTicker
  );
  const isCreating = !props.stockTicker;
  const { addStockTicker, isAddingStockTicker } = useAddStockTicker();
  const { updateStockTicker, isUpdatingStockTicker } = useUpdateStockTicker();
  const [color, setColor] = useState(
    props.stockTicker ? props.stockTicker.color : "#aabbcc"
  );

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (isCreating) {
      addStockTicker(
        { ...stockTicker!, color: color },
        {
          onSuccess: () => {
            close();
          },
        }
      );
    } else {
      updateStockTicker(
        { ...stockTicker!, color: color },
        {
          onSuccess: () => {
            close();
          },
        }
      );
    }
  }

  const isLoading = isAddingStockTicker || isUpdatingStockTicker;

  return (
    <Form onSubmit={handleSubmit}>
      <FormRow label="Stock">
        <StockTickerSearchBox
          id="stock"
          suggestion={props.stockTicker}
          onSelectedSuggestion={setStockTicker}
        />        
      </FormRow>
      <FormRow label="Color">
        <HexColorPicker color={color} onChange={setColor} />
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => close()}>Cancel</FormButton>
        <FormButton type="submit">
          <div className="flex space-x-1">
            <div>Add</div>
            {isLoading && <ControlSpinner />}
          </div>
        </FormButton>
      </div>
    </Form>
  );
}

export default AddStockTicker;
