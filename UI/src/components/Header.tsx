import {
  Disclosure,
  DisclosureButton,
  DisclosurePanel,
  Menu,
  MenuButton,
  MenuItem,
  MenuItems,
} from "@headlessui/react";
import { clsx } from "clsx";
import { NavLink } from "react-router-dom";
import { HiBars3, HiBell, HiXMark } from "react-icons/hi2";
import { AiOutlineStock } from "react-icons/ai";
import {
  AuthenticatedTemplate,
  UnauthenticatedTemplate,
  useIsAuthenticated,
  useMsal,
} from "@azure/msal-react";
import { loginRequest } from "../authConfig";

const navigation = (isAuthenticated: boolean) => {
  return [
    { name: "Dashboard", href: "/view-stock", current: true },
    ...(isAuthenticated
      ? [
          {
            name: "Manage Stock Tickers",
            href: "/manage-stock-tickers",
            current: false,
          },
          {
            name: "Administer Stock Tickers",
            href: "/admin/manage-stock-tickers",
            current: false,
          },
        ]
      : []),
  ];
};

function MobileMenu() {
  return (
    <DisclosureButton className="group relative inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-700 hover:text-white focus:ring-2 focus:ring-white focus:outline-hidden focus:ring-inset">
      <span className="absolute -inset-0.5" />
      <span className="sr-only">Open main menu</span>

      <HiBars3
        aria-hidden="true"
        className="block size-6 group-data-open:hidden"
      />
      <HiXMark
        aria-hidden="true"
        className="hidden size-6 group-data-open:block"
      />
    </DisclosureButton>
  );
}

function Header() {
  const { accounts, instance } = useMsal();
  const isAuthenticated = useIsAuthenticated();

  const handleLoginRedirect = async () => {
    instance.loginRedirect(loginRequest).catch((error) => console.error(error));
  };

  const handleLogout = async () => {
    const logoutRequest = {
      account: instance.getAccountByHomeId(accounts[0].homeAccountId),
      postLogoutRedirectUri: "/",
    };
    instance.logoutRedirect(logoutRequest);
  };

  return (
    <Disclosure as="nav" className="bg-secondary">
      <div className="mx-auto px-2 sm:px-6 lg:px-8 container">
        <div className="relative flex h-16 items-center justify-between ">
          <div className="absolute inset-y-0 left-0 flex items-center sm:hidden">
            <MobileMenu />
          </div>
          <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
            <div className="flex shrink-0 items-center text-on-secondary">
              <AiOutlineStock size={30} />
            </div>
            <div className="hidden sm:ml-6 sm:block">
              <div className="flex space-x-4">
                {navigation(isAuthenticated).map((item) => (
                  <NavLink
                    key={item.name}
                    to={item.href}
                    aria-current={item.current ? "page" : undefined}
                    className={({ isActive }) =>
                      clsx(
                        isActive
                          ? "bg-primary text-on-primary"
                          : "bg-primary/50 text-on-tertiary hover:bg-primary hover:opacity-75 hover:text-white",
                        "rounded-md px-3 py-2 text-sm font-medium"
                      )
                    }
                  >
                    {item.name}
                  </NavLink>
                ))}
              </div>
            </div>
          </div>
          <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
            <UnauthenticatedTemplate>
              <button type="button" onClick={handleLoginRedirect}>
                Sign in
              </button>
            </UnauthenticatedTemplate>
            <AuthenticatedTemplate>
              <span>{accounts[0]?.username}</span>
              <button
                type="button"
                className="relative rounded-full bg-gray-800 p-1 text-gray-400 hover:text-white focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 focus:outline-hidden"
              >
                <span className="absolute -inset-1.5" />
                <span className="sr-only">View notifications</span>
                <HiBell aria-hidden="true" className="size-6" />
              </button>

              {/* Profile dropdown */}
              <Menu as="div" className="relative ml-3">
                <div>
                  <MenuButton className="relative flex rounded-full bg-gray-800 text-sm focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 focus:outline-hidden">
                    <span className="absolute -inset-1.5" />
                    <span className="sr-only">Open user menu</span>
                    <img
                      alt=""
                      src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                      className="size-8 rounded-full"
                    />
                  </MenuButton>
                </div>
                <MenuItems
                  transition
                  className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 ring-1 shadow-lg ring-black/5 transition focus:outline-hidden data-closed:scale-95 data-closed:transform data-closed:opacity-0 data-enter:duration-100 data-enter:ease-out data-leave:duration-75 data-leave:ease-in"
                >
                  <MenuItem>
                    <a
                      href="#"
                      className="block px-4 py-2 text-sm text-gray-700 data-focus:bg-gray-100 data-focus:outline-hidden"
                    >
                      Your Profile
                    </a>
                  </MenuItem>
                  <MenuItem>
                    <a
                      href="#"
                      className="block px-4 py-2 text-sm text-gray-700 data-focus:bg-gray-100 data-focus:outline-hidden"
                    >
                      Settings
                    </a>
                  </MenuItem>
                  <MenuItem>
                    <a
                      onClick={handleLogout}
                      href="#"
                      className="block px-4 py-2 text-sm text-gray-700 data-focus:bg-gray-100 data-focus:outline-hidden"
                    >
                      Sign out
                    </a>
                  </MenuItem>
                </MenuItems>
              </Menu>
            </AuthenticatedTemplate>
          </div>
        </div>
      </div>

      <DisclosurePanel className="sm:hidden">
        <div className="space-y-1 px-2 pt-2 pb-3">
          {navigation(isAuthenticated).map((item) => (
            <DisclosureButton
              key={item.name}
              as="a"
              href={item.href}
              aria-current={item.current ? "page" : undefined}
              className={clsx(
                item.current
                  ? "bg-gray-900 text-white"
                  : "text-gray-300 hover:bg-gray-700 hover:text-white",
                "block rounded-md px-3 py-2 text-base font-medium"
              )}
            >
              {item.name}
            </DisclosureButton>
          ))}
        </div>
      </DisclosurePanel>
    </Disclosure>
  );
}

export default Header;
