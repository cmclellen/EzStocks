import { flexRender, Table } from "@tanstack/react-table";
import { StockTicker } from "../services/StocksApi";
import { ReactNode } from "react";

interface GridBodyProps {
  table: Table<StockTicker>;
}

function GridBody({ table }: GridBodyProps) {
  return (
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
  );
}

interface GridHeaderProps {
  readonly table: Table<StockTicker>;
}

function GridHeader({ table }: GridHeaderProps) {
  return (
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
  );
}

interface GridFooterProps {
  readonly table: Table<StockTicker>;
}

function GridFooter({ table }: GridFooterProps) {
  return (
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
  );
}

interface GridProps {
  readonly table: Table<StockTicker>;
}

function Grid({ table }: GridProps) {
  return (
    <table className="table-auto w-full">
      <GridHeader table={table} />
      <GridBody table={table} />
      <GridFooter table={table} />
    </table>
  );
}

interface GridActionButtonProp {
  readonly icon: ReactNode;
  readonly onClick?: () => void;
  readonly displayName: string;
}

function _GridActionButton({
  onClick,
  displayName,
  icon,
}: GridActionButtonProp) {
  return (
    <button className="flex space-x-1 hover:cursor-pointer" onClick={onClick}>
      <div className="flex items-center">{icon}</div>
      <span className="hidden md:block">{displayName}</span>
    </button>
  );
}

Grid.ActionButton = _GridActionButton;

export default Grid;
