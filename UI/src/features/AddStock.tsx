import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
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
        <input
          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="stock"
          type="text"
          placeholder="Stock"
        />
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => onCloseModal?.()}>Cancel</FormButton>
        <FormButton type="submit">Add</FormButton>
      </div>
    </Form>
  );
}

export default AddStock;
