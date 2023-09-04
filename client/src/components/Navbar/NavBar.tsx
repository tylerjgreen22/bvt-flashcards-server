import { useState } from "react";
import logo from "../../assets/logo.png";
import SearchIcon from "@mui/icons-material/Search";
import { router } from "../../routes/Routes";
import Avatar from "@mui/material/Avatar";
import { Menu, MenuItem } from "@mui/material";
import React from "react";
import { useUserContext } from "../../context/UserContext";
import { useLocation } from "react-router-dom";

// Nav bar
const NavBar = () => {
  const location = useLocation();
  const { user, setUser } = useUserContext();
  const [search, setSearch] = useState("");
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  // On search, navigate to results page with search in route parameters
  const handleSearch = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && search) {
      router.navigate(`/Results/${search}`);
    }
  };

  // Open modal
  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };

  // Close modal, on close perform navigation or operation based on selected option
  const handleClose = (option: string) => {
    switch (option) {
      case "sets":
        router.navigate("/Library");
        break;
      case "create":
        router.navigate("/Create");
        break;
      case "profile":
        router.navigate("/Profile");
        break;
      case "logout":
        localStorage.removeItem("token");
        setUser(null);
        router.navigate("/");
        break;
      default:
        break;
    }

    setAnchorEl(null);
  };

  // No navbar on log in and sign up pages
  if (location.pathname === "/Login" || location.pathname === "/Signup") {
    return null;
  }

  return (
    <nav className="bg-primary px-5">
      <div className="flex justify-between items-center">
        {/* Logo  */}
        <img
          className="h-20 w-auto cursor-pointer"
          src={logo}
          alt="Quiz Lit"
          onClick={() => router.navigate("/")}
        />

        {/* Search bar  */}
        <div className="hidden md:block relative w-1/3">
          <div className="absolute inset-y-0 left-0 flex items-center pl-3">
            <SearchIcon />
          </div>
          <input
            className="w-full p-2 pl-10 text-md text-white placeholder:text-white bg-accent rounded-lg"
            placeholder="Search all sets..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onKeyUp={handleSearch}
          />
        </div>

        {/* Login / Sign up / User  */}
        {user ? (
          <div>
            {/* User avatar  */}
            {user.image ? (
              <Avatar
                src={user.image}
                className="cursor-pointer"
                onClick={handleClick}
              />
            ) : (
              <Avatar
                sx={{ bgcolor: "#ff5722", width: 45, height: 45 }}
                className="cursor-pointer"
                onClick={handleClick}
              >
                {user.username[0].toUpperCase()}
              </Avatar>
            )}
            {/* User menu  */}
            <Menu
              id="basic-menu"
              anchorEl={anchorEl}
              open={open}
              onClose={() => handleClose("")}
              MenuListProps={{
                "aria-labelledby": "basic-button",
              }}
            >
              <MenuItem onClick={() => handleClose("sets")}>Your Sets</MenuItem>
              <MenuItem onClick={() => handleClose("create")}>
                Create Set
              </MenuItem>
              <MenuItem onClick={() => handleClose("profile")}>
                Your Profile
              </MenuItem>
              <MenuItem onClick={() => handleClose("logout")}>Log out</MenuItem>
            </Menu>
          </div>
        ) : (
          // Log in / Sign up button
          <button
            type="button"
            className="bg-secondary py-2 px-4 rounded-full text-lg"
            onClick={() => router.navigate("/Login")}
          >
            Login / Signup
          </button>
        )}
      </div>
    </nav>
  );
};

export default NavBar;
