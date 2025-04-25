import { useState } from "react";

interface CheckBoxProps {
  title: string;
  isChecked: boolean;
  onChange: (checked: boolean) => void;
}

function CheckBox(props: CheckBoxProps) {
  const [isChecked, setIsChecked] = useState(props.isChecked || false);

  console.log("isChecked", isChecked);

  const handleCheckboxChanged = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const checked = event.target.checked;
    setIsChecked(checked);
    props.onChange(checked);
  };

  return (
    <div className="flex items-center my-4 gap-2">
      <input
        checked={isChecked}
        id="checked-checkbox"
        type="checkbox"
        onChange={handleCheckboxChanged}
        className="relative peer shrink-0 appearance-none w-6 h-6 border-2 border-primary-500 rounded-sm bg-secondary checked:bg-secondary checked:border-2"
      />
      <label
        htmlFor="checked-checkbox"
        className="text-md font-medium text-gray-900 dark:text-gray-300"
      >
        {props.title}
      </label>
      <svg
        className="
      absolute 
      w-6 h-6 mt-1
      hidden peer-checked:block pointer-events-none"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        stroke-width="4"
        stroke-linecap="round"
        stroke-linejoin="round"
      >
        <polyline points="20 6 9 17 4 12"></polyline>
      </svg>
    </div>
  );
}

export default CheckBox;
