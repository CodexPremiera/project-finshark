import React, {useEffect, useState} from "react";
import {useParams} from "react-router";
import {CompanyProfile} from "../../company";
import {getCompanyProfile} from "../../api";
import Sidebar from "../../Components/Sidebar/Sidebar";
import Tile from "../../Components/Tile/Tile";
import CompanyDashboard from "../../Components/CompanyDashboard/CompanyDashboard";

const CompanyPage = () => {
  let { ticker } = useParams();
  const [company, setCompany] = useState<CompanyProfile>();

  useEffect(() => {
    const getProfileInit = async () => {
      try {
        const result = await getCompanyProfile(ticker!);
        setCompany(result?.data[0]);
      } catch (error) {
        console.error("Failed to fetch company profile:", error);
      }
    };
    getProfileInit();
  }, [])


  return (
    <>
      {company ? (
        <div className="w-full relative flex ct-docs-disable-sidebar-content overflow-x-hidden">
          <Sidebar />
          <CompanyDashboard ticker={ticker!}>
            <Tile title="Company Name" subTitle={company.companyName} />
          </CompanyDashboard>
        </div>
      ) : (
        <div>Company Not Found!</div>
      )}
    </>
  );
};

export default CompanyPage;