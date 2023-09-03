import { Outlet, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./routes/Home";
import NavBar from "./components/NavBar";
import MobileNavBar from "./components/MobileNavBar";
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

  if (loading)
    return (
      <div className="bg-primary body-wrapper flex justify-center items-center">
        <HomeLoadingSpinner />
      </div>
    );

  if (
    location.pathname === "/Create" ||
    location.pathname === "/Profile" ||
    location.pathname === "/Study" ||
    location.pathname === "/Edit"
  ) {
    return (
      <>
        <ToastContainer
          position="bottom-right"
          hideProgressBar
          theme="colored"
        />
        <div className="max-lg:bg-mobile bg-primary body-wrapper">
          <NavBar />
          <Outlet />
        </div>
        <MobileNavBar />
      </>
    );
  } else {
    return (
      <>
        <ToastContainer
          position="bottom-right"
          hideProgressBar
          theme="colored"
        />
        {location.pathname === "/" ? (
          <>
            <div className="bg-primary body-wrapper">
              <NavBar />
              <Home />
            </div>

            <MobileNavBar />
          </>
        ) : (
          <div className="bg-primary body-wrapper">
            <NavBar />
            <Outlet />
          </div>
        )}
      </>
    );
  }
}

export default App;
