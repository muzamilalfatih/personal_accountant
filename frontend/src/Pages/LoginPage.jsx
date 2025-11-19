import { useState } from "react";
import { logIn } from "../Services/userApi";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../CustomHooks/useAuth";
import useTitle from "../CustomHooks/useTitle";
import { Eye, EyeOff } from "lucide-react";

export default function LoginPage() {
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();
  useTitle("سجيل الدخول");

  const [loginModel, setLoginModel] = useState({
    email: "",
    password: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");
    try {
      setIsLoading(true);
      const data = await logIn(loginModel);
      login(data.user, data.accessToken);
      navigate("/accounts");
    } catch (error) {
      setError(error?.message || "An error happen!");
    }
    {
      setIsLoading(false);
    }
  };
  const handleChange = (e) => {
    const { name, value } = e.target;
    setLoginModel((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  return (
    <div className="min-h-screen bg-gray-900 grid place-items-center px-6 py-12 lg:px-8">
      <div className="w-full max-w-md space-y-8">
        {/* Logo and heading */}
        <div className="text-center">
          <img
            alt="Your Company"
            src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
            className="mx-auto h-10 w-auto"
          />
          <h2 className="mt-6 text-3xl font-bold tracking-tight text-white">
            تسجيل الدخول إلى حسابك
          </h2>
        </div>

        {error && (
          <div className="bg-red-600 text-white px-4 py-2 rounded-md text-center">
            {error}
          </div>
        )}

        {/* Form */}
        <form
          onSubmit={handleSubmit}
          className="space-y-6 bg-gray-800 p-8 rounded-xl shadow-lg"
        >
          {/* Email */}
          <div>
            <label
              htmlFor="email"
              className="block text-sm font-medium text-gray-100 text-right"
            >
              البريد الإلكتروني
            </label>
            <input
              onChange={handleChange}
              id="email"
              name="email"
              type="email"
              required
              autoComplete="email"
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          {/* Password */}
          <div className="relative">
            <div className="flex items-center justify-between">
              <div className="text-sm">
                <Link
                  to={"/forget-password"}
                  className="font-semibold text-indigo-400 hover:text-indigo-300"
                >
                  هل نسيت كلمة المرور؟
                </Link>
              </div>
              <label
                htmlFor="password"
                className="block text-sm font-medium text-gray-100"
              >
                كلمة المرور
              </label>
            </div>
            <input
              onChange={handleChange}
              id="password"
              name="password"
              type={showPassword ? "text" : "password"}
              required
              autoComplete="current-password"
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-3 top-10 text-gray-400 hover:text-gray-200"
            >
              {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>

          {/* Submit button */}

          <button
            disabled={isLoading}
            type="submit"
            className="disabled:opacity-70 disabled:cursor-not-allowed w-full rounded-md bg-indigo-500 px-4 py-2 font-semibold text-white hover:bg-indigo-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          >
            {isLoading ? "...تحميل" : "تسجيل الدخول"}
          </button>
        </form>

        {/* Footer link */}
        <p className="mt-4 text-center text-sm text-gray-400">
          ليس لديك حساب؟{" "}
          <Link
            to={"/register"}
            className="font-semibold text-indigo-400 hover:text-indigo-300"
          >
            سجل الآن
          </Link>
        </p>
      </div>
    </div>
  );
}
