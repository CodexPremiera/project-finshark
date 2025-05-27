import React, {JSX, SyntheticEvent} from "react";
import "./Card.css";
import { CompanySearch } from "../../company";
import AddPortfolio from "../Portfolio/AddPortfolio/AddPortfolio";

interface Props {
  id: string;
  company: CompanySearch;
  onPortfolioCreate: (e: SyntheticEvent) => void;
}

const Card: React.FC<Props> = ({
                                 id,
                                 company,
                                 onPortfolioCreate,
                               }: Props): JSX.Element => {
  return (
    <div key={id} id={id} className="card">
      <div className="details">
        <h2>
          {company.name} ({company.symbol})
        </h2>
        <p>{company.currency}</p>
      </div>
      <p className="info">
        {company.exchangeShortName} - {company.stockExchange}
      </p>
      <AddPortfolio
        onPortfolioCreate={onPortfolioCreate}
        symbol={company.symbol}
      />
    </div>
  );
};

export default Card;