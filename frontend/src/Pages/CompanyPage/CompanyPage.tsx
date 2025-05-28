import React, {useEffect, useState} from "react";
import {useParams} from "react-router";
import {CompanyProfile} from "../../company";
import {getCompanyProfile} from "../../api";
import Sidebar from "../../Components/Sidebar/Sidebar";
import Tile from "../../Components/Tile/Tile";
import CompanyDashboard from "../../Components/CompanyDashboard/CompanyDashboard";
import Spinner from "../../Components/Spinner/Spinner";
import CompFinder from "../../Components/CompFinder/CompFinder";

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
            <Tile title="Price" subTitle={company.price.toString()} />
            <Tile title="Sector" subTitle={company.sector} />
            <Tile title="Market Cap" subTitle={company.mktCap.toString()} />

            <CompFinder ticker={company.symbol} />

            <p className="bg-white shadow rounded text-medium text-gray-900 p-3 m-4">{company.description}</p>
          </CompanyDashboard>
        </div>
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default CompanyPage;