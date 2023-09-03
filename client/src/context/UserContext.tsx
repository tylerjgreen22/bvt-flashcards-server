import { ReactNode, createContext, useContext, useState } from "react";
import { User } from "../models/user";

interface ContextProps {
  user: User | null;
  setUser: (user: User | null) => void;
}

interface UserContextProps {
  children: ReactNode;
}

export const UserContext = createContext<ContextProps>({
  user: null,
  setUser: () => {},
});

export const useUserContext = () => {
  return useContext(UserContext);
};

export const UserProvider = ({ children }: UserContextProps) => {
  const [user, setUser] = useState<User | null>(null);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};
