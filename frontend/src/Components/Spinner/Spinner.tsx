import React from "react";
import "./Spinner.css";
import { ClipLoader } from "react-spinners";

type props = {
  isLoading?: boolean;
}

function Spinner({isLoading = true}: props) {
  return (
    <div id="loading-spinner">
      <ClipLoader
        color="#36d7b7"
        loading={isLoading}
        size={35}
        aria-label="Loading Spinner"
        data-testid="loader"
      />
    </div>
  );
}

export default Spinner;