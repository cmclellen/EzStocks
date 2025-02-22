import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import Modal from "../components/Modal";

function AppLayout() {
  return (
    <Modal>
      <div className="h-dvh flex flex-col">
        <Header />
        <main className="grow container mx-auto flex flex-col py-5 spacing-y-5">
          <Outlet />
        </main>
      </div>
    </Modal>
  );
}

export default AppLayout;
