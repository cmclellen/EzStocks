import { useEffect, useRef } from "react";

export default function useOutsideClick(callback: () => void) {

    const ref = useRef({});

    useEffect(() => {

        const handleClick = () => {
            callback();
        }

        document.addEventListener('click', handleClick);

        return () => document.removeEventListener('click', handleClick);
    }, [])

    return ref;
}