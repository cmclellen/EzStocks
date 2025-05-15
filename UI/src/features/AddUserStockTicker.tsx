import { useState } from "react";
import ControlSpinner from "../components/ControlSpinner";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormRow from "../components/FormRow";
import { useModal } from "../components/Modal";
import Spinner from "../components/Spinner";
import { useAddUserStockTicker } from "../hooks/useAddUserStockTicker";
import { useCurrentUser } from "../hooks/useCurrentUser";
import useGetStockTickers from "../hooks/useGetStockTickers";
import { useMsal } from "@azure/msal-react";

function AddUserStockTicker() {
  const { close } = useModal();
  const [ticker, setTicker] = useState<string>();
  const { userId } = useCurrentUser();
  const { stockTickers, isLoadingStockTickers } = useGetStockTickers();
  const { addUserStockTicker } = useAddUserStockTicker();
  const isAdding = false;

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    await addUserStockTicker(
      { userId, ticker: ticker! },
      {
        onSuccess: close,
      }
    );
  }

  if (isLoadingStockTickers) return <Spinner></Spinner>;

  return (
    <Form onSubmit={handleSubmit}>
      <FormRow label="Stock Ticker">
        <select
          className="border w-full rounded-md p-2"
          onChange={(e) => setTicker(e.currentTarget.value)}
        >
          {stockTickers &&
            stockTickers.map((st) => (
              <option value={st.ticker}>{st.ticker}</option>
            ))}
        </select>
      </FormRow>
      <div className="flex justify-end space-x-2">
        <FormButton onClick={() => close()}>Cancel</FormButton>
        <FormButton type="submit">
          <div className="flex space-x-1">
            <div>Add</div>
            {isAdding && <ControlSpinner />}
          </div>
        </FormButton>
      </div>
    </Form>
  );
}

export default AddUserStockTicker;
