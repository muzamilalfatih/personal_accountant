import Navbar from "../Components/NavBar";
import { Outlet } from "react-router-dom";

const MainLayout = () => {
  return (
    <>
      <Navbar />
      <Outlet /> {/* renders child routes here */}
    </>
  );
};
export default MainLayout;
