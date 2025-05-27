import { ChangeEvent, SyntheticEvent, useState } from "react";
import { Outlet } from "react-router";
import "./App.css";
import "./input.css";
import "./index.css";
import "./output.css";
import Navbar from "./Components/Navbar/Navbar";

function App() {
  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
}

export default App;