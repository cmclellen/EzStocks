import { createContext, ReactNode, useContext, useState } from "react";

interface CurrentUserProps {
  userId: string;
}

const CurrentUserContext = createContext<CurrentUserProps>({
  userId: "",
});

interface CurrentUserProviderProps {
  children: ReactNode;
}

export function CurrentUserProvider({ children }: CurrentUserProviderProps) {
  const [userId] = useState("b8343eaf-6c20-4a40-bada-168ff5ceec89");

  return (
    <CurrentUserContext.Provider value={{ userId }}>
      {children}
    </CurrentUserContext.Provider>
  );
}

export function useCurrentUser() {
  const context = useContext(CurrentUserContext);
  if (context === null)
    throw new Error(
      "CurrentUserContext was used outside of CurrentUserProvider"
    );
  return context;
}
