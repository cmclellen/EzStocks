import FormButton from "../components/FormButton";
import Modal, { useModal } from "../components/Modal";
import StockGraph from "../components/StockGraph";

function ViewStock() {
  const { close } = useModal();
  return (
    <>
      <div className="flex justify-end">
        <Modal.Open opensWindowName={`add-stock`}>
          <FormButton>Add Stock</FormButton>
        </Modal.Open>
        <Modal.Window name={`add-stock`}>
          <div className="p-4">
            here <button onClick={() => close()}>here</button>
          </div>
        </Modal.Window>
      </div>
      <StockGraph />
    </>
  );
}

export default ViewStock;
