interface ErrorProps {
  children?: React.ReactNode;
}

function Error({ children }: ErrorProps) {
  return <div className="text-red-500">{children}</div>;
}

interface FormRowProps {
  readonly label: string;
  readonly children?: React.ReactNode;
  readonly error?: string;
}

function FormRow({ label, children, error }: FormRowProps) {
  return (
    <div className="mb-4">
      <label className="block test-gray-700 text-sm font-bold" htmlFor="stock">
        {label}
      </label>
      {children}
      {error && <Error>{error}</Error>}
    </div>
  );
}

export default FormRow;
