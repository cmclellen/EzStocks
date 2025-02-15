import Header from "../components/Header";
import StockGraph from "../components/StockGraph";

function AppLayout() {
  return (
    <div className="h-dvh flex flex-col">
      <Header />
      <main className="grow border border-blue-600 container mx-auto justify-items-center content-center">
        <StockGraph />
      </main>
    </div>
  );
}

export default AppLayout;
