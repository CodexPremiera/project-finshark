import { Outlet } from "react-router";
import "./App.css";
import "./input.css";
import "./index.css";
import "./output.css";
import Navbar from "./Components/Navbar/Navbar";
import {ToastContainer} from "react-toastify";
import {UserProvider} from "./Context/useAuth";

function App() {
  return (
    <>
      <UserProvider>
        <Navbar />
        <Outlet />
        <ToastContainer />
      </UserProvider>
    </>
  );
}

export default App;