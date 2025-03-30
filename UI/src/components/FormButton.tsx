import clsx from "clsx";

interface FormButtonProps {
  onClick?: () => void;
  type?: "button" | "submit";
  children?: React.ReactNode;
  className?: string;
}

function FormButton({
  onClick,
  children,
  type = "button",
  className,
}: FormButtonProps) {
  return (
    <button
      className={clsx(
        "bg-primary text-on-primary px-4 py-2 rounded font-bold hover:bg-primary/75 focus:outline-none focus:shadow-outline",
        className
      )}
      type={type}
      onClick={onClick}
    >
      {children}
    </button>
  );
}

export default FormButton;
