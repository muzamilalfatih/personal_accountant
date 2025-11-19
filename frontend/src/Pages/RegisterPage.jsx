import { useState } from "react";
import { addUserAsync } from "../Services/userApi";
import { Link, useNavigate } from "react-router-dom";
import useTitle from "../CustomHooks/useTitle";
import { Eye, EyeOff } from "lucide-react";

export default function RegisterPage() {
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  useTitle("إنشاء حساب جديد");
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });
  const [confirmPassword, setConfirmPassword] = useState("");
  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (user.password != confirmPassword) {
      setError("Passwords do not match");
      return;
    }
    setIsLoading(true);
    try {
      await addUserAsync(user);
      setIsSuccess(true);
    } catch (err) {
      console.log(err);
      setError(err?.message || "An Error happen");
    }
    setIsLoading(false);
  };
  if (isSuccess) {
    return (
      <div className="min-h-screen bg-gray-900 grid place-items-center px-6 py-12 lg:px-8">
        <div className="w-full max-w-md text-center p-6 bg-gray-800 rounded-xl shadow-lg">
          <h2 className="text-3xl font-bold text-white mb-4">
            تأكيد عنوان البريد الإلكتروني
          </h2>
          <p className="text-gray-200">
            تم إنشاء الحساب بنجاح. تم إرسال رسالة إلى{" "}
            <strong>{user.email}</strong> لتأكيد بريدك الإلكتروني.
          </p>
          <p className="mt-4 text-gray-400 text-sm">
            يرجى النقر على الرابط في البريد الإلكتروني لتفعيل حسابك قبل تسجيل
            الدخول.
          </p>
          <button
            onClick={() => navigate("/")}
            className="mt-6 bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded"
          >
            تسجيل الدخول
          </button>
        </div>
      </div>
    );
  }

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
            تسجيل حساب جديد
          </h2>
        </div>
        {/* Error message */}
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
          <div className="grid gap-4 sm:grid-cols-2">
            {/* First Name */}
            <div>
              <label
                htmlFor="first-name"
                className="block text-sm font-medium text-gray-100 text-right"
              >
                الاسم الأول
              </label>
              <input
                onChange={handleChange}
                id="first-name"
                name="firstName"
                type="text"
                required
                className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>

            {/* Last Name */}
            <div>
              <label
                htmlFor="last-name"
                className="block text-sm font-medium text-gray-100 text-right"
              >
                اسم العائلة
              </label>
              <input
                onChange={handleChange}
                id="last-name"
                name="lastName"
                type="text"
                required
                className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>
          </div>

          {/* Email */}
          <div>
            <label
              htmlFor="email"
              className="block text-sm font-medium text-gray-100 text-right"
            >
              عنوان البريد الإلكتروني
            </label>
            <input
              onChange={handleChange}
              id="email"
              name="email"
              type="email"
              required
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          {/* Password */}
          <div className="relative">
            <label
              htmlFor="password"
              className="block text-sm font-medium text-gray-100 text-right "
            >
              كلمة المرور
            </label>
            <input
              onChange={handleChange}
              id="password"
              name="password"
              type={showPassword ? "text" : "password"}
              required
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

          <div className="relative">
            <label
              htmlFor="conformPassword"
              className="block text-sm font-medium text-gray-100 text-right"
            >
              تأكيد كلمة المرور
            </label>
            <input
              onChange={(e) => setConfirmPassword(e.target.value)}
              id="conformPassword"
              name="conformPassword"
              type={showConfirm ? "text" : "password"}
              required
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <button
              type="button"
              onClick={() => setShowConfirm(!showConfirm)}
              className="absolute right-3 top-10 text-gray-400 hover:text-gray-200"
            >
              {showConfirm ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>

          <button
            disabled={isLoading}
            type="submit"
            className="w-full rounded-md bg-indigo-500 px-4 py-2 font-semibold text-white hover:bg-indigo-400 focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:opacity-70 disabled:cursor-not-allowed"
          >
            {isLoading ? "...تحميل" : "انشاء حساب"}
          </button>
        </form>

        {/* Footer link */}
        <p className="mt-4 text-center text-sm text-gray-400">
          هل لديك حساب بالفعل؟{" "}
          <Link
            to={"/"}
            className="font-semibold text-indigo-400 hover:text-indigo-300"
          >
            تسجيل الدخول
          </Link>
        </p>
      </div>
    </div>
  );
}
