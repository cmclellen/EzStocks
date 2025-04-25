import CheckBox from "./CheckBox";

export interface StockItem {
  name: string;
  isEnabled: boolean;
}

interface StockSelectorProps {
  stockItems: StockItem[] | undefined;
  onStockItemChanged?: (stockItem: StockItem) => void;
}

function StockSelector({ stockItems, onStockItemChanged }: StockSelectorProps) {
  const handleStockSelected = (ev: { key: string; isChecked: boolean }) => {
    const stockItem = stockItems!.find((si) => si.name === ev.key);
    if (stockItem) {
      stockItem.isEnabled = ev.isChecked;
      onStockItemChanged?.(stockItem);
    }
  };

  return (
    <ul className="">
      {stockItems &&
        stockItems.map((stockItem, index) => (
          <li key={index}>
            <CheckBox
              title={stockItem.name}
              isChecked={stockItem.isEnabled}
              onChange={handleStockSelected}
            ></CheckBox>
          </li>
        ))}
    </ul>
  );
}

export default StockSelector;
