import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
import SearchBox from "../components/SearchBox";
import { addStock } from "../services/StocksApi";

interface AddStockProps {
  onCloseModal?: () => void;
}

function AddStock({ onCloseModal }: AddStockProps) {
  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    addStock({ stock: event.currentTarget.stock.value });
  }

  return (
    <Form onSubmit={handleSubmit}>
      <FormRow label="Stock">
        <SearchBox />
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => onCloseModal?.()}>Cancel</FormButton>
        <FormButton type="submit">Add</FormButton>
      </div>
    </Form>
  );
}

export default AddStock;
