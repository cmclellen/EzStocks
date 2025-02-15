import Header from "../components/Header";
import StockGraph from "../components/StockGraph";

function AppLayout() {
  return (
    <div className="h-dvh flex flex-col">
      <Header />
      <div className="grow border border-blue-600 container mx-auto justify-items-center content-center">
        <StockGraph />
      </div>
    </div>
  );
}

export default AppLayout;
