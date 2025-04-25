import CheckBox from "./CheckBox";

export interface StockItem {
  name: string;
  isEnabled: boolean;
}

interface StockSelectorProps {
  stockItems: StockItem[];
}

function StockSelector({ stockItems }: StockSelectorProps) {
  const stockTickers = stockItems.map((si) => si.name);

  const handleStockSelected = (ev: { key: string; isChecked: boolean }) => {
    console.log("isChecked", ev.key, ev.isChecked);
  };

  return (
    <ul className="">
      {stockTickers &&
        stockTickers.map((name, index) => (
          <li key={index}>
            <CheckBox
              title={name}
              isChecked={false}
              onChange={handleStockSelected}
            ></CheckBox>
          </li>
        ))}
    </ul>
  );
}

export default StockSelector;
