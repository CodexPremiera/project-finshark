import axios from "axios";
import {
  CompanyBalanceSheet,
  CompanyCashFlow,
  CompanyCompData,
  CompanyIncomeStatement,
  CompanyKeyMetrics,
  CompanyProfile,
  CompanySearch,
  CompanyTenK,
} from "./company";

export interface SearchResponse {
  data: CompanySearch[];
}

export const searchCompanies = async (query: string) => {
  try {
    return await axios.get<SearchResponse>(
      `https://financialmodelingprep.com/api/v3/search?query=${query}&limit=10&exchange=NASDAQ&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.log("error message: ", error.message);
      return error.message;
    } else {
      console.log("unexpected error: ", error);
      return "An expected error has occurred.";
    }
  }
};

export const getCompanyProfile = async (query: string) => {
  try {
    return await axios.get<CompanyProfile[]>(
      `https://financialmodelingprep.com/api/v3/profile/${query}?apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
};

export const getKeyMetrics = async (query: string) => {
  try {
    return await axios.get<CompanyKeyMetrics[]>(
      `https://financialmodelingprep.com/api/v3/key-metrics-ttm/${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
}

export const getIncomeStatement = async (query: string) => {
  try {
    return await axios.get<CompanyIncomeStatement[]>(
      `https://financialmodelingprep.com/api/v3/income-statement/${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
}

export const getBalanceSheet = async (query: string) => {
  try {
    return await axios.get<CompanyBalanceSheet[]>(
      `https://financialmodelingprep.com/api/v3/balance-sheet-statement/${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
}

export const getCashFlow = async (query: string) => {
  try {
    return await axios.get<CompanyCashFlow[]>(
      `https://financialmodelingprep.com/api/v3/cash-flow-statement/${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
}

export const getCompData = async (query: string) => {
  try {
    return await axios.get<CompanyCompData[]>(
      `https://financialmodelingprep.com/api/v3/stock_peers?symbol=${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
}


export const getTenK = async (query: string) => {
  try {
    return await axios.get<CompanyTenK[]>(
        `https://financialmodelingprep.com/api/v3/sec_filings/${query}?limit=40&apikey=${process.env.REACT_APP_API_KEY}`
    );
  } catch (error: any) {
    console.log("error message: ", error.message);
  }
};