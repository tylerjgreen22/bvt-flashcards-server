import person from "../../assets/person.png";
import plus from "../../assets/plus.png";
import searchpic from "../../assets/search.png";
import SearchIcon from "@mui/icons-material/Search";

import { Link, useLocation } from "react-router-dom";
import { Modal, Box, Typography } from "@mui/material";
import { useState } from "react";
import { router } from "../../routes/Routes";

const style = {
  position: "absolute" as const,
  top: "25%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: "90%",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

// Mobile nav bar
const MobileNavBar = () => {
  const location = useLocation();
  const [search, setSearch] = useState("");
  const [openModal, setOpen] = useState(false);
  const handleOpenModal = () => setOpen(true);
  const handleCloseModal = () => setOpen(false);

  // On search, navigate to results page with search in route parameters
  const handleSearch = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && search) {
      router.navigate(`/Results/${search}`);
    }
  };

  if (
    location.pathname === "/Login" ||
    location.pathname === "/Signup" ||
    location.pathname === "/Create" ||
    location.pathname === "/Edit"
  ) {
    return null;
  }

  return (
    <div className="sticky bottom-0 py-1 bg-primary md:hidden drop-shadow-[0px_2px_5px_rgba(0,0,0,0.75)]">
      <div className="container flex justify-between items-center px-10">
        {/* Search  */}
        <div>
          <button onClick={handleOpenModal}>
            <img src={searchpic} alt="search" className="p-2 mt-2 w-[70px]" />
          </button>

          {/* Modal  */}
          <Modal
            open={openModal}
            onClose={handleCloseModal}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
          >
            <Box sx={style}>
              <Typography id="modal-modal-title" variant="h4" component="h3">
                Search
              </Typography>

              {/* Modal search bar  */}
              <div className="relative mt-2">
                <div className="absolute inset-y-0 left-0 flex items-center pl-3">
                  <SearchIcon sx={{ fontSize: "1.875rem" }} />
                </div>
                <input
                  className="w-full text-2xl p-2 pl-12 text-white placeholder:text-white bg-accent rounded-lg"
                  placeholder="Search all sets..."
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                  onKeyUp={handleSearch}
                />
              </div>
            </Box>
          </Modal>
        </div>

        {/* Create  */}
        <div>
          <Link to="/Create">
            <img
              src={plus}
              alt="note"
              className="p-2 drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] w-[100px] active:translate-y-2 transition-all duration-150"
            />
          </Link>
        </div>

        {/* Profile  */}
        <div>
          <Link to="/Profile">
            <img src={person} alt="person" className="p-2 w-[70px]" />
          </Link>
        </div>
      </div>
    </div>
  );
};

export default MobileNavBar;
