import React, {JSX, SyntheticEvent} from "react";
import { CompanySearch } from "../../company";
import AddPortfolio from "../Portfolio/AddPortfolio";

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
    <div
      className="flex flex-col items-center justify-between w-full p-6 bg-slate-100 rounded-lg md:flex-row"
      key={id}
      id={id}
    >
      <h2 className="font-bold text-center text-veryDarkViolet md:text-left">
        {company.name} ({company.symbol})
      </h2>
      <p className="text-veryDarkBlue">{company.currency}</p>
      <p className="font-bold text-veryDarkBlue">
        {company.exchangeShortName} - {company.stockExchange}
      </p>
      <div className="flex">
        <AddPortfolio
          onPortfolioCreate={onPortfolioCreate}
          symbol={company.symbol}
        />
      </div>

    </div>
  );
};

export default Card;