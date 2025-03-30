import { useState } from "react";
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

const defaultData: StockTicker[] = [];

const columnHelper = createColumnHelper<StockTicker>();

const columns = [
  columnHelper.accessor("ticker", {
    header: () => <span>Ticker</span>,
    cell: (info) => info.getValue(),
    // footer: (info) => info.column.id,
  }),
  columnHelper.accessor((row) => row.name, {
    id: "name",
    cell: (info) => <i>{info.getValue()}</i>,
    header: () => <span>Name</span>,
    // footer: (info) => info.column.id,
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
    // footer: (info) => info.column.id,
  }),
];

function ManageStockTickers() {
  const [data, _setData] = useState(() => [...defaultData]);
  const { stockTickers, isLoadingStockTickers, error } = useQueryStockTickers();
  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

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
