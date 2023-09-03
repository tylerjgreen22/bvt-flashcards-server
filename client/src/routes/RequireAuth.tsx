import { Navigate, Outlet } from "react-router-dom";
import { useUserContext } from "../context/UserContext";

const RequireAuth = () => {
  const { user } = useUserContext();

  if (!user) {
    return <Navigate to="/Login" />;
  }
  return <Outlet />;
};

export default RequireAuth;
