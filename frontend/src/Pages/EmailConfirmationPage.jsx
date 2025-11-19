import { useEffect, useRef, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { confirmEmailAsync } from "../Services/userApi";
import useTitle from "../CustomHooks/useTitle";

export default function EmailConfirmationPage() {
  useTitle("تأكيد البريد الإلكتروني");
  const { token } = useParams();
  const navigate = useNavigate();
  const [status, setStatus] = useState("loading");
  const [message, setMessage] = useState("");
  const hasRun = useRef(false);

  useEffect(() => {
    if (hasRun.current) return; // prevent second run
    hasRun.current = true;

    const confirmEmail = async () => {
      try {
        await confirmEmailAsync(token); // call your backend

        setStatus("success");
        setMessage(
          "تم تأكيد البريد الإلكتروني بنجاح. يمكنك الآن تسجيل الدخول."
        );
      } catch (err) {
        console.log(err);
        setStatus("error");
        setMessage("حدث خطأ غير متوقع أثناء تأكيد البريد الإلكتروني.");
      }
    };

    if (token) confirmEmail();
    else {
      setStatus("error");
      setMessage("رابط التأكيد غير صالح.");
    }
  }, [token]);

  return (
    <div className="min-h-screen bg-gray-900 grid place-items-center px-6 py-12 lg:px-8">
      <div
        className="w-full max-w-md text-center p-6 bg-gray-800 rounded-xl shadow-lg"
        dir="rtl"
      >
        {status === "loading" && (
          <p className="text-white text-lg">جاري تأكيد البريد الإلكتروني...</p>
        )}

        {status === "success" && (
          <>
            <h2 className="text-3xl font-bold text-white mb-4">تم التأكيد</h2>
            <p className="text-gray-200">{message}</p>
            <button
              onClick={() => navigate("/")}
              className="mt-6 bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded"
            >
              تسجيل الدخول
            </button>
          </>
        )}

        {status === "error" && (
          <>
            <h2 className="text-3xl font-bold text-red-500 mb-4">
              فشل التأكيد
            </h2>
            <p className="text-gray-200">{message}</p>
            <button
              onClick={() => navigate("/")}
              className="mt-6 bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded"
            >
              العودة لتسجيل الدخول
            </button>
          </>
        )}
      </div>
    </div>
  );
}
