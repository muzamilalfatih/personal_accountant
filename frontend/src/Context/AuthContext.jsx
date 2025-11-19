import { createContext, useState } from "react";
import { setAuthToken } from "../Services/api";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState("");

  const login = (user, token) => {
    setUser(user);
    setToken(token);
    setAuthToken(token);
  };

  const logout = () => {
    setUser(null);
    setToken("");
    setAuthToken("");
  };
  const refreshUser = (updatedUser) => {
    setUser(updatedUser);
  };
  return (
    <AuthContext.Provider value={{ user, token, login, logout, refreshUser }}>
      {children}
    </AuthContext.Provider>
  );
};
