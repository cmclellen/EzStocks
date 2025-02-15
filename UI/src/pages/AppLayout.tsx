import { Outlet } from "react-router-dom";
import Header from "../components/Header";

function AppLayout() {
  return (
    <div className="h-dvh flex flex-col">
      <Header />
      <main className="grow container mx-auto justify-items-center content-center">
        <Outlet />
      </main>
    </div>
  );
}

export default AppLayout;
