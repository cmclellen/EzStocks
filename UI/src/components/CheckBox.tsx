interface CheckBoxProps {
  title: string;
}

function CheckBox({ title }: CheckBoxProps) {
  return (
    <div className="flex items-center mb-4">
      <input
        checked
        id="checked-checkbox"
        type="checkbox"
        value=""
        className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded-sm focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
      />
      <label
        for="checked-checkbox"
        class="ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
      >
        {title}
      </label>
    </div>
  );
}

export default CheckBox;
