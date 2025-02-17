// import FormButton from "../components/FormButton";
// import FormRow from "../components/FormRow";

// interface AddStcockProps {
//   onCloseModal?: () => void;
// }

// function AddStock({ onCloseModal }: AddStcockProps) {
//   function addStock() {
//     console.log("Add stock");
//   }

//   return (
//     <div className="w-full max-w-md">
//       <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
//         <FormRow label="Stock">
//           <input
//             className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
//             id="stock"
//             type="text"
//             placeholder="Stock"
//           />
//         </FormRow>
//         <div className="flex justify-end space-x-2">
//           <FormButton onClick={() => onCloseModal?.()}>Cancel</FormButton>
//           <FormButton onClick={() => addStock()} type="submit">
//             Add
//           </FormButton>
//         </div>
//       </form>
//     </div>
//   );
// }

// export default AddStock;
