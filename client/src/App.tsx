import { Outlet, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./routes/Home";
import NavBar from "./components/Navbar/NavBar";
import MobileNavBar from "./components/Navbar/MobileNavBar";
import { ToastContainer } from "react-toastify";
import { useEffect, useState } from "react";
import agent from "./api/agent";
import { useUserContext } from "./context/UserContext";
import HomeLoadingSpinner from "./components/Loading/HomeLoadingSpinner";

function App() {
  const location = useLocation();
  const { setUser } = useUserContext();
  const [loading, setLoading] = useState(true);
  const token = localStorage.getItem("token");

  // On page load, fetch user if theres a token, set user context
  useEffect(() => {
    if (token) {
      setLoading(true);
      const fetchData = async () => {
        try {
          const userData = await agent.Account.current();

          setUser(userData);
          setLoading(false);
        } catch (error) {
          setLoading(false);
        }
      };

      fetchData();
    } else {
      setLoading(false);
    }
  }, [setUser, token]);

  // If loading, return home loading spinner
  if (loading)
    return (
      <div className="bg-primary body-wrapper flex items-center">
        <HomeLoadingSpinner />
      </div>
    );

  return (
    <>
      {/* Toast container for errors  */}
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      {/* Navbar  */}
      <NavBar />

      {location.pathname === "/" ? (
        <div className="bg-primary body-wrapper">
          {/* Home page  */}
          <Home />
        </div>
      ) : (
        <div
          className={
            location.pathname === "/Create" ||
            location.pathname === "/Profile" ||
            location.pathname === "/Edit"
              ? "bg-mobile lg:bg-primary body-wrapper"
              : "bg-primary body-wrapper"
          }
        >
          {/* Outlet  */}
          <Outlet />
        </div>
      )}

      {/* Mobile nav bar */}
      <MobileNavBar />
    </>
  );
}

export default App;
