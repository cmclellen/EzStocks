import FormButton from "../components/FormButton";
import Modal, { useModal } from "../components/Modal";
import StockGraph from "../components/StockGraph";
import AddStockTicker from "../features/AddStockTicker";

function ViewStock() {
  const { close } = useModal();

  const buttons = (
    <div className="flex justify-end">
      <Modal.Open opensWindowName={`add-stock`}>
        <FormButton>Add Stock</FormButton>
      </Modal.Open>
      <Modal.Window name={`add-stock`}>
        <AddStockTicker />
      </Modal.Window>
    </div>
  );

  return (
    <>
      {buttons}
      <StockGraph />
    </>
  );
}

export default ViewStock;
