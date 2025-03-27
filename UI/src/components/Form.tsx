interface FormProps {
  readonly children?: React.ReactNode;
  readonly onSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
}

function Form({ children, onSubmit }: FormProps) {
  return <form onSubmit={onSubmit}>{children}</form>;
}

export default Form;
