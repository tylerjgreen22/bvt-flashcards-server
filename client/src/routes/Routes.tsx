import { Navigate, RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../App";
import Login from "./Login";
import Signup from "./Signup";
import Profile from "./Profile";
import Library from "./Library";
import Study from "./Study";
import SearchResults from "./SearchResults";
import Create from "./Create";
import Edit from "./Edit";
import NotFound from "./NotFound";
import RequireAuth from "./RequireAuth";

// Routes
export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      {
        element: <RequireAuth />,
        children: [
          { path: "/Profile", element: <Profile /> },
          { path: "/Library", element: <Library /> },
          { path: "/Create/:id?", element: <Create /> },
          { path: "/Edit/:id", element: <Edit /> },
        ],
      },
      { path: "/Login", element: <Login /> },
      { path: "/Signup", element: <Signup /> },
      { path: "/Study/:id", element: <Study /> },
      { path: "/Results/:search", element: <SearchResults /> },
      { path: "notFound", element: <NotFound /> },
      { path: "*", element: <Navigate replace to="/notFound" /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
