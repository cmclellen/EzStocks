import {
  createColumnHelper,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { ReactNode, useState } from "react";
import { FaRegEdit } from "react-icons/fa";
import { FaRegTrashCan } from "react-icons/fa6";
import FormButton from "../components/FormButton";
import Grid from "../components/Grid";
import Modal from "../components/Modal";
import Page from "../components/Page";
import Spinner from "../components/Spinner";
import AddStockTicker from "../features/AddStockTicker";
import useDeleteStockTicker from "../hooks/useDeleteStockTicker";
import useQueryStockTickers from "../hooks/useQueryStockTickers";
import { StockTicker } from "../services/StocksApi";

const defaultData: StockTicker[] = [];

const columnHelper = createColumnHelper<StockTicker>();

const getColumns = (handleEdit: any, handleDelete: any) => [
  columnHelper.accessor("ticker", {
    header: () => <span>Ticker</span>,
    cell: (info) => <div className="text-center">{info.getValue()}</div>,
  }),
  columnHelper.accessor((row) => row.name, {
    id: "name",
    cell: (info) => <i>{info.getValue()}</i>,
    header: () => <div className="text-left">Name</div>,
  }),
  columnHelper.accessor("color", {
    header: () => "Color",
    cell: (info) => (
      <div className="text-center">
        <div
          className="rounded w-5 h-5 inline-block"
          style={{ backgroundColor: info.getValue() }}
        >
          &nbsp;
        </div>
      </div>
    ),
  }),
  columnHelper.display({
    id: "actions",
    header: () => "Action",
    cell: (props) => (
      <div className="flex space-x-1 md:space-x-4 justify-center">
        <Modal.Open opensWindowName={`add-stock-${props.row.original.ticker}`}>
          <Grid.ActionButton displayName="Edit" icon={<FaRegEdit />} />
        </Modal.Open>
        <Modal.Window name={`add-stock-${props.row.original.ticker}`}>
          <AddStockTicker stockTicker={props.row.original} />
        </Modal.Window>

        <Grid.ActionButton
          displayName="Delete"
          icon={<FaRegTrashCan />}
          onClick={() => handleDelete(props.row.original)}
        />
      </div>
    ),
  }),
];

function AdministerStockTickers() {
  const [data, _setData] = useState(() => defaultData);
  const { stockTickers, isLoadingStockTickers, error } = useQueryStockTickers();
  const { deleteStockTicker, isDeletingStockTicker } = useDeleteStockTicker();
  const table = useReactTable({
    data,
    columns: getColumns(handleEdit, handleDelete),
    getCoreRowModel: getCoreRowModel(),
  });

  function handleEdit(item: any) {
    console.log(item);
  }
  async function handleDelete(item: any) {
    await deleteStockTicker(item.ticker);
  }

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

  if (error) throw new Error("Failed loading stock tickers");

  if (isLoadingStockTickers || isDeletingStockTicker)
    return <Spinner></Spinner>;

  table.options.data = stockTickers!;

  return (
    <Page title="Administer Stock Tickers">
      {buttons}
      <div className="p-2 ">
        <Grid table={table} />
      </div>
    </Page>
  );
}

export default AdministerStockTickers;
