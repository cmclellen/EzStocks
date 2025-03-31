import FormButton from "../components/FormButton";
import Modal from "../components/Modal";
import Page from "../components/Page";
import AddStockTicker from "../features/AddStockTicker";

function ManageStockTickers() {
  const buttons = (
    <div className="flex justify-end">
      <Modal.Open opensWindowName={`add-stock`}>
        <FormButton>Add Stock Ticker</FormButton>
      </Modal.Open>
      <Modal.Window name={`add-stock`}>
        <AddStockTicker />
      </Modal.Window>
    </div>
  );

  return (
    <Page title="Manage Stock Tickers">
      {buttons}
      <div className="p-2 ">
        manage
      </div>
    </Page>
  );
}

export default ManageStockTickers;
