interface ErrorProps {
  children?: React.ReactNode;
}

function Error({ children }: ErrorProps) {
  return <div className="text-red-500">{children}</div>;
}

export default Error;
