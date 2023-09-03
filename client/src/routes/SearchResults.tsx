import { useEffect, useState } from "react";
import agent from "../api/agent";
import { FlashcardSet } from "../models/flashcardSet";
import SetItem from "../components/SetItem";
import Pagination from "@mui/material/Pagination";
import { Pagination as Paginate } from "../models/pagination";
import { Menu, MenuItem } from "@mui/material";
import React from "react";
import { Link, useParams } from "react-router-dom";
import SetLoadingSkeleton from "../components/Loading/SetLoadingSkeleton";

const SearchResults = () => {
  const { search } = useParams();
  const [sets, setSets] = useState<FlashcardSet[]>([]);
  const [paginated, setPagination] = useState<Paginate>();
  const [page, setPage] = useState<number>(1);
  const [sort, setSort] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);

      const params = new URLSearchParams();
      params.append("pageNumber", page.toString());
      if (search) {
        params.append("search", search);
      }
      if (sort) {
        params.append("orderBy", sort);
      }
      const paginatedData = await agent.Set.list(params);
      setPagination(paginatedData.pagination);
      setSets(paginatedData.data);

      setLoading(false);
    };

    fetchData();
  }, [page, search, sort]);

  const handlePageChange = (
    _event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setPage(value);
  };

  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = (sort: string) => {
    setSort(sort);
    setAnchorEl(null);
  };

  if (loading)
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2">
        <SetLoadingSkeleton />
      </div>
    );

  if (sets.length) {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2">
        <div className="flex justify-between">
          <div className="bg-secondary w-fit px-4 rounded-md flex items-center cursor-pointer">
            <div onClick={handleClick}>
              Sort By: {sort ? <p className="inline">{sort}</p> : "Title"}
            </div>
            <Menu
              id="basic-menu"
              anchorEl={anchorEl}
              open={open}
              onClose={() => handleClose("")}
              MenuListProps={{
                "aria-labelledby": "basic-button",
              }}
            >
              <MenuItem onClick={() => handleClose("Recent")}>Recent</MenuItem>
              <MenuItem onClick={() => handleClose("")}>Title</MenuItem>
            </Menu>
          </div>
        </div>

        <div className="flex flex-col items-center justify-between">
          {sets
            ? sets.map((set: FlashcardSet) => (
                <SetItem key={set.id} flashCardSet={set} />
              ))
            : null}
        </div>
        <div className="bg-secondary rounded-md mt-9 w-fit mx-auto p-2">
          <Pagination
            count={paginated?.totalPages}
            page={page}
            onChange={handlePageChange}
            size="large"
          />
        </div>
      </div>
    );
  } else {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2">
        <div className="mx-auto w-fit flex flex-col items-center bg-accent p-4 rounded-md text-white">
          <h2 className="text-2xl text-bold">No sets found for that search!</h2>
          <p>
            Try another search or create your{" "}
            <Link to="/Create" className="underline">
              own
            </Link>
          </p>
        </div>
      </div>
    );
  }
};

export default SearchResults;
