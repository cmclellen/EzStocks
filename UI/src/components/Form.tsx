interface FormProps {
  children?: React.ReactNode;
  onSubmit: () => void;
}

function Form({ children, onSubmit }: FormProps) {
  return <form onSubmit={onSubmit}>{children}</form>;
}

export default Form;
