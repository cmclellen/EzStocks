import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import {
  PublicClientApplication,
  EventType,
  EventMessage,
  AuthenticationResult,
} from "@azure/msal-browser";
import { msalConfig } from "./authConfig";

async function createMsalInstance() {
  /**
   * MSAL should be instantiated outside of the component tree to prevent it from being re-instantiated on re-renders.
   * For more, visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md
   */
  const msalInstance = new PublicClientApplication(msalConfig);

  await msalInstance.initialize();

  await msalInstance.handleRedirectPromise();

  const activeAccount = msalInstance.getActiveAccount();

  if (!activeAccount) {
    // Account selection
    const accounts = msalInstance.getAllAccounts();
    if (accounts.length > 0) {
      msalInstance.setActiveAccount(accounts[0]);
    }
  }

  //set the account
  msalInstance.addEventCallback((event: EventMessage) => {
    if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
      const authenticationResult = event.payload as AuthenticationResult;
      const account = authenticationResult.account;
      msalInstance.setActiveAccount(account);
    }
  });
}

export const msalInstance = createMsalInstance();

//enable account storage event
// msalInstance.enableAccountStorageEvents();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App instance={msalInstance} />
  </StrictMode>
);
