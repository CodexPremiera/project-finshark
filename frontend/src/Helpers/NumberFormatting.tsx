export const formatLargeMonetaryNumber: any = (number: number) => {
  return `$${formatLargeNonMonetaryNumber(number)}`;
};

export const formatLargeNonMonetaryNumber: any = (number: number) => {
  if (number < 0) {
    return "-" + formatLargeMonetaryNumber(-1 * number);
  }
  if (number < 1000) {
    return number.toFixed(2);
  }
  if (number < 1_000_000) {
    return (number / 1000).toFixed(2) + "K";
  }
  if (number < 1_000_000_000) {
    return (number / 1_000_000).toFixed(2) + "M";
  }
  if (number < 1_000_000_000_000) {
    return (number / 1_000_000_000).toFixed(2) + "B";
  }
  if (number < 1_000_000_000_000_000) {
    return (number / 1_000_000_000_000).toFixed(2) + "T";
  }
};

export const formatRatio = (ratio: number) => {
  return (Math.round(ratio * 100) / 100).toFixed(2);
};