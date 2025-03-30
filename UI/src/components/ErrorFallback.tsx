import { FallbackProps } from "react-error-boundary";
import FormButton from "./FormButton";

function ErrorFallback({ error, resetErrorBoundary }: FallbackProps) {
  return (
    <div className="w-full text-center">
      <div className="border py-2 px-4 rounded-lg text-center font-semibold inline-block">
        <div className="text-xl font-bold">
          An unexpected error has occurred
        </div>
        <div className="p-2">{error.message}</div>
        <FormButton className="m-3" onClick={resetErrorBoundary}>
          Try again
        </FormButton>
      </div>
    </div>
  );
}

export default ErrorFallback;
