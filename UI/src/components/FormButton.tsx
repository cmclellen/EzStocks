interface FormButtonProps {
  onClick: () => void;
  type?: "button" | "submit";
  children?: React.ReactNode;
}

function FormButton({ onClick, children, type = "button" }: FormButtonProps) {
  return (
    <button
      className="bg-on-primary px-4 py-2 rounded font-bold hover:bg-blue-700 focus:outline-none focus:shadow-outline"
      type={type}
      onClick={onClick}
    >
      {children}
    </button>
  );
}

export default FormButton;
