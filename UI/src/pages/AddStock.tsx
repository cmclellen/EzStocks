import FormButton from "../components/FormButton";

function AddStock() {
  function addStock() {
    console.log("Add stock");
  }

  function oncancel() {
    console.log("Cancel");
  }

  return (
    <div className="w-full max-w-md">
      <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div className="mb-4">
          <label
            className="block test-gray-700 text-sm font-bold"
            htmlFor="stock"
          >
            Stock
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="stock"
            type="text"
            placeholder="Stock"
          ></input>
        </div>
        <div className="flex justify-end space-x-2">
          <FormButton onClick={() => oncancel()}>Cancel</FormButton>
          <FormButton onClick={() => addStock()} type="submit">
            Add
          </FormButton>
        </div>
      </form>
    </div>
  );
}

export default AddStock;
