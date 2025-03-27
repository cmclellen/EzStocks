import {
  cloneElement,
  createContext,
  ReactElement,
  ReactNode,
  useContext,
  useState,
} from "react";
import { createPortal } from "react-dom";

interface ModalContextType {
  openName: string;
  close: () => void;
  open: (name: string) => void;
}

const ModalContext = createContext<ModalContextType>({
  openName: "",
  close: () => {},
  open: (_name: string) => {},
});

export function useModal() {
  const context = useContext(ModalContext);
  if (context === null)
    throw new Error("ModalContext was used outside of ModalProvider");
  return context;
}

interface ModalProps {
  children: ReactNode;
}

function Modal({ children }: ModalProps) {
  const [openName, setOpenName] = useState("");

  const close = () => setOpenName("");
  const open = setOpenName;

  return (
    <ModalContext.Provider value={{ openName, close, open }}>
      {children}
    </ModalContext.Provider>
  );
}

interface OpenProps {
  children: ReactElement;
  opensWindowName: string;
}

function Open({ children, opensWindowName }: OpenProps) {
  const { open } = useContext(ModalContext);

  return cloneElement(children, { onClick: () => open(opensWindowName) });
}

interface WindowProps {
  name: string;
  children: React.ReactNode;
}

function Window({ children, name }: WindowProps) {
  const { openName } = useContext(ModalContext);
  if (openName !== name) return null;
  return createPortal(
    <div className="relative z-10">
      <div
        className="fixed inset-0 bg-gray-500/75 transition-opacity"
        aria-hidden="true"
      ></div>
      <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
        <div className="flex min-h-full justify-center text-center items-center p-0">
          <div className="relative transform overflow-hidden rounded-lg bg-background text-left shadow-xl transition-all my-8 w-full max-w-xl p-4">
            {children}
          </div>
        </div>
      </div>
    </div>,
    document.body
  );
}
Modal.Window = Window;
Modal.Open = Open;
export default Modal;
