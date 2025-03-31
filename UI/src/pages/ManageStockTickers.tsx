import {
  createColumnHelper,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { FaRegTrashCan } from "react-icons/fa6";
import FormButton from "../components/FormButton";
import Grid from "../components/Grid";
import Modal from "../components/Modal";
import Page from "../components/Page";
import Spinner from "../components/Spinner";
import AddUserStockTicker from "../features/AddUserStockTicker";
import { useCurrentUser } from "../hooks/useCurrentUser";
import useGetUser from "../hooks/useGetUser";
import { StockTicker } from "../services/StocksApi";

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
  // useDeleteUserStockTicker();

  const data = user?.stockTickers ?? [];
  const table = useReactTable({
    data,
    columns: getColumns(handleDelete),
    getCoreRowModel: getCoreRowModel(),
  });

  const buttons = (
    <div className="flex justify-end">
      <Modal.Open opensWindowName={`add-user-stock`}>
        <FormButton>Add Stock Ticker</FormButton>
      </Modal.Open>
      <Modal.Window name={`add-user-stock`}>
        <AddUserStockTicker />
      </Modal.Window>
    </div>
  );

  async function handleDelete(item: any) {
    console.log("delete stock ticker", item);
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
