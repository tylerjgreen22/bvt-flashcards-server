import { ReactNode, createContext, useContext, useState } from "react";
import { User } from "../models/user";

interface ContextProps {
  user: User | null;
  setUser: (user: User | null) => void;
}

interface UserContextProps {
  children: ReactNode;
}

// User context for tracking the user throughout the whole app
export const UserContext = createContext<ContextProps>({
  user: null,
  setUser: () => {},
});

// Hook that calls useContext with the user context passed in
export const useUserContext = () => {
  return useContext(UserContext);
};

// Provider that provides the user context to all children
export const UserProvider = ({ children }: UserContextProps) => {
  const [user, setUser] = useState<User | null>(null);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};
