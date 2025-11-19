import { Link, useNavigate } from "react-router-dom";
import useTitle from "../CustomHooks/useTitle";

const ForbiddenPage = () => {
  const navigate = useNavigate();
  useTitle("صفحة محظورة");

  return (
    <div className="min-h-screen bg-gray-900 grid place-items-center px-6 py-12 lg:px-8">
      <div className="w-full max-w-md space-y-8 text-center">
        {/* Logo and heading */}
        <div>
          <img
            alt="Logo"
            src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
            className="mx-auto h-10 w-auto"
          />
          <h2 className="mt-6 text-3xl font-bold tracking-tight text-white">
            403 - Forbidden
          </h2>
          <p className="mt-2 text-gray-400">
            You don’t have permission to access this page.
          </p>
        </div>

        {/* Back Home / Login buttons */}
        <div className="space-y-4">
          <button
            onClick={() => navigate("/dashboard")}
            className="block w-full rounded-md bg-indigo-500 px-4 py-2 font-semibold text-white hover:bg-indigo-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          >
            Go To Dashboard
          </button>
        </div>

        {/* Footer */}
        <p className="mt-4 text-sm text-gray-500">
          Need help?{" "}
          <a
            href="mailto:support@example.com"
            className="text-indigo-400 hover:text-indigo-300"
          >
            Contact Support
          </a>
        </p>
      </div>
    </div>
  );
};
export default ForbiddenPage;
