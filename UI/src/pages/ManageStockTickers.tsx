import {
  createColumnHelper,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import FormButton from "../components/FormButton";
import Grid from "../components/Grid";
import Modal from "../components/Modal";
import Page from "../components/Page";
import AddStockTicker from "../features/AddStockTicker";
import { StockTicker } from "../services/StocksApi";
import { FaRegTrashCan } from "react-icons/fa6";
import useGetUser from "../hooks/useGetUser";
import { useCurrentUser } from "../hooks/useCurrentUser";
import Spinner from "../components/Spinner";

const columnHelper = createColumnHelper<StockTicker>();

const getColumns = (handleDelete: any) => [
  columnHelper.accessor("ticker", {
    header: () => <span>Ticker</span>,
    cell: (info) => <div className="text-center">{info.getValue()}</div>,
  }),
  // columnHelper.accessor((row) => row.name, {
  //   id: "name",
  //   cell: (info) => <i>{info.getValue()}</i>,
  //   header: () => <div className="text-left">Name</div>,
  // }),
  columnHelper.display({
    id: "actions",
    header: () => "Action",
    cell: (props) => (
      <div className="flex space-x-1 md:space-x-4 justify-center">
        <Grid.ActionButton
          displayName="Delete"
          icon={<FaRegTrashCan />}
          onClick={() => handleDelete(props.row.original)}
        />
      </div>
    ),
  }),
];

function ManageStockTickers() {
  const { userId } = useCurrentUser();
  const { user, isLoadingUser } = useGetUser(userId);

  const data = user?.stockTickers ?? [];
  const table = useReactTable({
    data,
    columns: getColumns(handleDelete),
    getCoreRowModel: getCoreRowModel(),
  });

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

  async function handleDelete(item: any) {
    console.log("delete stock ticker");
  }

  if (isLoadingUser) return <Spinner />;

  return (
    <Page title="Manage Stock Tickers">
      {buttons}
      <div className="p-2 ">
        <Grid table={table} />
      </div>
    </Page>
  );
}

export default ManageStockTickers;
