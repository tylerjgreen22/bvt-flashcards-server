import { Navigate, Outlet } from "react-router-dom";
import { useUserContext } from "../context/UserContext";

// If no logged in user, redirects to log in for protected routes
const RequireAuth = () => {
  const { user } = useUserContext();

  if (!user) {
    return <Navigate to="/Login" />;
  }
  return <Outlet />;
};

export default RequireAuth;
