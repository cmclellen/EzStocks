import { RefObject, useEffect, useRef } from "react";

export default function useOutsideClick(callback: () => void): RefObject<any> {
  const ref = useRef({});

  useEffect(() => {
    const handleClick = () => {
      callback();
    };

    document.addEventListener("click", handleClick);

    return () => document.removeEventListener("click", handleClick);
  }, [callback]);

  return ref;
}
