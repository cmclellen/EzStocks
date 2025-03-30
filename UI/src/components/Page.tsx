import { ReactNode } from "react";
import PageTitle from "./PageTitle";

interface PageProps {
  title: string;
  children: ReactNode;
}
function Page({ title, children }: PageProps) {
  return (
    <div>
      <PageTitle title={title}></PageTitle>
      <div>{children}</div>
    </div>
  );
}
export default Page;
