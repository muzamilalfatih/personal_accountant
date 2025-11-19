import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import { resetPassword } from "../Services/authentication";
import { changePassword } from "../Services/userApi";

const ResetPassword = ({ id, token }) => {
  const [password, setPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false); // <-- track success
  const [showPassword, setShowPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (newPassword !== confirm) {
      setError("Passwords do not match.");
      return;
    }

    setError("");
    setIsLoading(true);

    try {
      if (token) {
        await resetPassword(token, newPassword);
        setSuccess(true); // <-- show confirmation instead of redirect
      } else {
        await changePassword(id, password, newPassword);
        navigate("/dashboard");
      }
    } catch (error) {
      setError(error?.response?.data?.message || "An error occurred");
    } finally {
      setIsLoading(false);
    }
  };

  if (success) {
    return (
      <div className="p-6 bg-gray-800 rounded-xl text-white text-center">
        <h2 className="text-2xl font-bold mb-4">Password Reset Successful</h2>
        <p>Your password has been reset successfully.</p>
        <button
          onClick={() => navigate("/")}
          className="mt-6 bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded"
        >
          تسجيل الدخول
        </button>
      </div>
    );
  }

  return (
    <form
      className="bg-gray-800 p-6 rounded-xl border border-gray-700 space-y-4 relative"
      onSubmit={handleSubmit}
    >
      <h2 className="text-xl font-semibold text-white">Reset Password</h2>
      {error && <div className="text-red-500 text-sm">{error}</div>}

      {!token && (
        <div className="flex flex-col space-y-2 relative">
          <label className="text-gray-300 text-sm">Current Password</label>
          <input
            required
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="bg-gray-700 text-white p-2 rounded-lg outline-none"
          />
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className="absolute right-3 top-10 text-gray-400 hover:text-gray-200"
          >
            {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
          </button>
        </div>
      )}

      <div className="flex flex-col space-y-2 relative">
        <label className="text-gray-300 text-sm">New Password</label>
        <input
          required
          type={showNewPassword ? "text" : "password"}
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          className="bg-gray-700 text-white p-2 rounded-lg outline-none"
        />
        <button
          type="button"
          onClick={() => setShowNewPassword(!showNewPassword)}
          className="absolute right-3 top-10 text-gray-400 hover:text-gray-200"
        >
          {showNewPassword ? <EyeOff size={18} /> : <Eye size={18} />}
        </button>
      </div>

      <div className="flex flex-col space-y-2 relative">
        <label className="text-gray-300 text-sm">Confirm Password</label>
        <input
          required
          type={showConfirm ? "text" : "password"}
          value={confirm}
          onChange={(e) => setConfirm(e.target.value)}
          className="bg-gray-700 text-white p-2 rounded-lg outline-none"
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
        className="w-full bg-blue-600 hover:bg-blue-500 text-white font-semibold py-2 rounded-lg disabled:opacity-70 disabled:cursor-not-allowed"
      >
        {isLoading ? "Loading..." : "Reset Password"}
      </button>
    </form>
  );
};

export default ResetPassword;
