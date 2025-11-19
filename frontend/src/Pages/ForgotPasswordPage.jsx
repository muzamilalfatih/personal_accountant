import { useState } from "react";
import { forgetPassword } from "../Services/authentication";

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState("");
  const [isLoading, setIsloading] = useState(false);
  const [error, setError] = useState("");
  const [isSuccess, setIsSuccess] = useState(false); // <-- track success

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("استعادة كلمة المرور");
    setIsloading(true);
    try {
      await forgetPassword(email);
    } catch (error) {
      // setError(error?.response?.data?.message || "An error occurred");
      console.log(error);
    } finally {
      setIsloading(false);
      setIsSuccess(true); // <-- show confirmation
    }
  };

  if (isSuccess) {
    return (
      <div className="p-6 bg-gray-800 rounded-xl text-white text-center">
        <h2 className="text-3xl font-bold tracking-tight mb-4">
          Check Your Email
        </h2>
        <p>
          If an account with <strong>{email}</strong> exists, you will receive a
          password reset link shortly.
        </p>
      </div>
    );
  }

  return (
    <div className="p-6 bg-gray-800 rounded-xl">
      <h2 className="mt-6 text-3xl font-bold tracking-tight text-white">
        Reset Password
      </h2>
      {error && <div className="text-red-500 text-center py-2">{error}</div>}
      <form onSubmit={handleSubmit} className="m-3">
        <label className="text-white">Enter your email address</label>
        <input
          type="email"
          className="bg-gray-700 text-white p-2 rounded mt-1 w-full"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <button
          type="submit"
          className="bg-indigo-500 text-white mt-4 p-2 rounded w-full"
        >
          {isLoading ? "Sending..." : "Send Reset Link"}
        </button>
      </form>
    </div>
  );
};

export default ForgotPasswordPage;
