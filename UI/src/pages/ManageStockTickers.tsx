import { ReactNode, useState } from "react";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import useQueryStockTickers from "../hooks/useQueryStockTickers";
import Spinner from "../components/Spinner";
import { StockTicker } from "../services/StocksApi";
import Page from "../components/Page";
import FormButton from "../components/FormButton";
import Modal from "../components/Modal";
import AddStockTicker from "../features/AddStockTicker";
import { FaRegEdit } from "react-icons/fa";
import { FaRegTrashCan } from "react-icons/fa6";
import useDeleteStockTicker from "../hooks/useDeleteStockTicker";

interface ActionButtonProp {
  icon: ReactNode;
  onClick?: () => void;
  displayName: string;
}

function ActionButton({ onClick, displayName, icon }: ActionButtonProp) {
  return (
    <button className="flex space-x-1 hover:cursor-pointer" onClick={onClick}>
      <div className="flex items-center">{icon}</div>
      <span className="hidden md:block">{displayName}</span>
    </button>
  );
}

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
          <ActionButton displayName="Edit" icon={<FaRegEdit />} />
        </Modal.Open>
        <Modal.Window name={`add-stock-${props.row.original.ticker}`}>
          <AddStockTicker stockTicker={props.row.original} />
        </Modal.Window>

        <ActionButton
          displayName="Delete"
          icon={<FaRegTrashCan />}
          onClick={() => handleDelete(props.row.original)}
        />
      </div>
    ),
  }),
];

function ManageStockTickers() {
  const [data, _setData] = useState(() => [...defaultData]);
  const { stockTickers, isLoadingStockTickers, error } = useQueryStockTickers();
  const { deleteStockTicker, deleteStockTickerError, isDeletingStockTicker } =
    useDeleteStockTicker();
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
        <FormButton>Add Stock</FormButton>
      </Modal.Open>
      <Modal.Window name={`add-stock`}>
        <AddStockTicker />
      </Modal.Window>
    </div>
  );

  if (error) throw new Error("Failed loading stock tickers");

  if (isLoadingStockTickers) return <Spinner></Spinner>;

  table.options.data = stockTickers!;

  return (
    <Page title="Manage Stock Tickers">
      {buttons}
      <div className="p-2 ">
        <table className="table-auto w-full">
          <thead>
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.map((row) => (
              <tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
          <tfoot>
            {table.getFooterGroups().map((footerGroup) => (
              <tr key={footerGroup.id}>
                {footerGroup.headers.map((header) => (
                  <th key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.footer,
                          header.getContext()
                        )}
                  </th>
                ))}
              </tr>
            ))}
          </tfoot>
        </table>
      </div>
    </Page>
  );
}

export default ManageStockTickers;
