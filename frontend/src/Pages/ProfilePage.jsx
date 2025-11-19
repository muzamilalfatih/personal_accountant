import { useState } from "react";
import { useAuth } from "../CustomHooks/useAuth";
import { Link, useNavigate } from "react-router-dom";
import { updateUser } from "../Services/userApi";
import useTitle from "../CustomHooks/useTitle";

const ProfilePage = () => {
  useTitle("الملف الشخصي");
  const { user, refreshUser } = useAuth(); // assuming you have an updateUser() method in your hook
  const [formData, setFormData] = useState({
    id: user?.id,
    firstName: user?.firstName || "",
    lastName: user?.lastName || "",
    email: user?.email || "",
    password: "",
    role: user.role,
  });
  const [confirmPassword, setConfirmPasswrod] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const handleChange = (e) => {
    console.log(e.target);
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    console.log(formData);
    if (formData.password && formData.password != confirmPassword) {
      setError("Passwords do not match");
      return;
    }
    setError("");
    setIsLoading(true);
    try {
      const newUser = await updateUser({
        ...formData,
      });
      console.log(newUser);
      refreshUser(newUser);
      navigate("/accounts");
    } catch (err) {
      console.log(err.response);
      setError(err?.response?.date || "Failed to update profile.");
    } finally {
      setIsLoading(false);
    }
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
            Profile
          </h2>
        </div>

        {/* Error / Success message */}
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
                className="block text-sm font-medium text-gray-100"
              >
                First Name
              </label>
              <input
                onChange={handleChange}
                id="first-name"
                name="firstName"
                type="text"
                required
                value={formData.firstName}
                className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>

            {/* Last Name */}
            <div>
              <label
                htmlFor="last-name"
                className="block text-sm font-medium text-gray-100"
              >
                Last Name
              </label>
              <input
                onChange={handleChange}
                id="last-name"
                name="lastName"
                type="text"
                required
                value={formData.lastName}
                className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>
          </div>

          {/* Email */}
          <div>
            <label
              htmlFor="email"
              className="block text-sm font-medium text-gray-100"
            >
              Email Address
            </label>
            <input
              value={formData.email}
              disabled
              id="email"
              name="email"
              type="email"
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500 opacity-70"
            />
          </div>

          {/* Password */}
          <div>
            <label
              htmlFor="password"
              className="block text-sm font-medium text-gray-100"
            >
              New Password
            </label>
            <input
              onChange={handleChange}
              id="password"
              name="password"
              type="password"
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          {/* Confirm Password */}
          <div>
            <label
              htmlFor="confirmPassword"
              className="block text-sm font-medium text-gray-100"
            >
              Confirm Password
            </label>
            <input
              onChange={(e) => setConfirmPasswrod(e.target.value)}
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              className="mt-1 block w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          <button
            disabled={isLoading}
            type="submit"
            className="w-full rounded-md bg-indigo-500 px-4 py-2 font-semibold text-white hover:bg-indigo-400 focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:opacity-70 disabled:cursor-not-allowed"
          >
            {isLoading ? "Saving..." : "Save Changes"}
          </button>
        </form>

        <p className="mt-4 text-center text-sm text-gray-400">
          <Link
            to={"/dashboard"}
            className="font-semibold text-indigo-400 hover:text-indigo-300"
          >
            Back to Dashboard
          </Link>
        </p>
      </div>
    </div>
  );
};
export default ProfilePage;
