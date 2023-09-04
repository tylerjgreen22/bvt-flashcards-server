import logo from "../../assets/logo.png";
import "./HomeLoadingSpinner.css";

// Home loading spinner
const HomeLoadingSpinner = () => {
  return (
    <div className="flex flex-col gap-16 mx-auto w-[200px] items-center">
      <h2 className="text-6xl text-bold">Loading...</h2>
      <img src={logo} className="spinner" />
    </div>
  );
};

export default HomeLoadingSpinner;
