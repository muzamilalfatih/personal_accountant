import { useState } from "react";
import { NavLink } from "react-router-dom";
import { useAuth } from "../CustomHooks/useAuth";
import { Menu, X } from "lucide-react";

export default function Navbar() {
  const { logout, user } = useAuth();
  const [menuOpen, setMenuOpen] = useState(false);

  const handleToggle = () => setMenuOpen((prev) => !prev);
  const handleLogout = () => {
    setMenuOpen(false);
    logout();
  };

  return (
    <nav className="bg-gray-900 border-b border-gray-800 w-full min-w-[360px] relative z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Left side */}
          <div className="flex items-center space-x-3">
            <img
              src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
              alt="Logo"
              className="h-8 w-8"
            />

            <NavLink
              to="/dashboard"
              className={({ isActive }) =>
                `text-xl font-bold truncate max-w-[150px] ${
                  isActive ? "text-indigo-400" : "text-white"
                } hover:text-indigo-400`
              }
            >
              {user?.firstName || "Dashboard"}
            </NavLink>
          </div>

          {/* Right side */}
          <div className="flex items-center space-x-3">
            <button
              onClick={handleToggle}
              className="text-gray-300 hover:text-white focus:outline-none"
            >
              {menuOpen ? <X size={24} /> : <Menu size={24} />}
            </button>
          </div>
        </div>
      </div>

      {/* Dropdown menu */}
      {menuOpen && (
        <div className="absolute right-4 top-16 bg-gray-800 border border-gray-700 rounded-lg shadow-lg w-48 animate-slideDownFade">
          <div className="flex flex-col py-2">
            <NavLink
              to="/profile"
              onClick={() => setMenuOpen(false)}
              className={({ isActive }) =>
                `px-4 py-2 hover:bg-gray-700 ${
                  isActive ? "text-indigo-400 font-semibold" : "text-gray-200"
                }`
              }
            >
              الملف الشخصي
            </NavLink>

            <NavLink
              to="/settings"
              onClick={() => setMenuOpen(false)}
              className={({ isActive }) =>
                `px-4 py-2 hover:bg-gray-700 ${
                  isActive ? "text-indigo-400 font-semibold" : "text-gray-200"
                }`
              }
            >
              الإعدادات
            </NavLink>

            {user?.role === "Admin" && (
              <NavLink
                to="/manage-users"
                onClick={() => setMenuOpen(false)}
                className={({ isActive }) =>
                  `px-4 py-2 hover:bg-gray-700 ${
                    isActive ? "text-indigo-400 font-semibold" : "text-gray-200"
                  }`
                }
              >
                إدارة المستخدمين
              </NavLink>
            )}

            <NavLink
              to={`/users/${user.id}/change-password`}
              onClick={() => setMenuOpen(false)}
              className={({ isActive }) =>
                `px-4 py-2 hover:bg-gray-700 ${
                  isActive ? "text-indigo-400 font-semibold" : "text-gray-200"
                }`
              }
            >
              إعادة تعيين كلمة المرور
            </NavLink>

            <button
              onClick={handleLogout}
              className="w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700"
            >
              تسجيل الخروج
            </button>
          </div>
        </div>
      )}
    </nav>
  );
}
