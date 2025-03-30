import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import Modal from "../components/Modal";
import { ErrorBoundary } from "react-error-boundary";
import ErrorFallback from "../components/ErrorFallback";

function AppLayout() {
  return (
    <Modal>
      <div className="h-dvh flex flex-col">
        <Header />
        <main className="grow container mx-auto flex flex-col py-5 spacing-y-5">
          <ErrorBoundary FallbackComponent={ErrorFallback}>
            <Outlet />
          </ErrorBoundary>
        </main>
      </div>
    </Modal>
  );
}

export default AppLayout;
