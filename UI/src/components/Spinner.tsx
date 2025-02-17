import { BiLoaderAlt } from "react-icons/bi";

function Spinner() {
  return (
    <div className="flex items-center justify-center space-x-2 grow">
      <span className="animate-spin">
        <BiLoaderAlt size={50} />
      </span>
      <span className="font-semibold text-2xl">Loading...</span>
    </div>
  );
}

export default Spinner;
