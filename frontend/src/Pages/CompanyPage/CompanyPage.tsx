import React, {useEffect, useState} from "react";
import {useParams} from "react-router";
import {CompanyProfile} from "../../company";
import {getCompanyProfile} from "../../api";

interface Props {}

const CompanyPage = (props: Props) => {
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
        <div className="company-profile-container">{company.companyName}</div>
      ) : (
        <div>Company Not Found!</div>
      )}
    </>
  );
};

export default CompanyPage;