import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import "./App.css";
import AppLayout from "./pages/AppLayout.tsx";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { Toaster } from "react-hot-toast";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ViewStock from "./pages/ViewStock.tsx";
import ManageStockTickers from "./pages/ManageStockTickers.tsx";
import AdministerStockTickers from "./pages/AdministerStockTickers.tsx";
import { CurrentUserProvider } from "./hooks/useCurrentUser.tsx";
// import { loginRequest } from "./authConfig";
import {
  MsalProvider,
  // AuthenticatedTemplate,
  // useMsal,
  // UnauthenticatedTemplate,
} from "@azure/msal-react";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,
    },
  },
});

interface AppProps {
  instance: any;
}

function App({ instance }: AppProps) {
  return (
    <MsalProvider instance={instance}>
      <CurrentUserProvider>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <Toaster />
          <BrowserRouter>
            <Routes>
              <Route element={<AppLayout />}>
                <Route index element={<Navigate replace to="view-stock" />} />
                <Route path="view-stock" element={<ViewStock />}></Route>
                <Route
                  path="manage-stock-tickers"
                  element={<ManageStockTickers />}
                ></Route>
                <Route
                  path="admin/manage-stock-tickers"
                  element={<AdministerStockTickers />}
                ></Route>
              </Route>
            </Routes>
            <Toaster />
          </BrowserRouter>
        </QueryClientProvider>
      </CurrentUserProvider>
    </MsalProvider>
  );
}

export default App;
