import { useNavigate, useSearchParams } from "react-router-dom";
import TransactionList from "../Components/TransactionList";
import useTitle from "../CustomHooks/useTitle";

const TransactionPage = () => {
  const [searchParams] = useSearchParams();
  const accountId = searchParams.get("accountId");
  const accountName = searchParams.get("accountName");
  useTitle("العمليات المالية");

  const navigate = useNavigate();
  return (
    <div className="bg-gray-900 min-h-screen min-w-[360px] text-gray-100 flex flex-col items-center py-10 relative overflow-hidden">
      <div className="w-full max-w-2xl bg-gray-800 rounded-2xl shadow-lg overflow-hidden border border-gray-700 relative">
        <div className="flex items-center justify-between p-4 border-b border-gray-700 bg-gray-850 relative">
          <h2 className="text-xl font-bold text-white">{accountName}</h2>
          <button
            onClick={() => navigate("/dashboard")}
            className="absolute top-2 right-2 text-gray-400 hover:text-white text-xl font-bold"
          >
            ×
          </button>
        </div>
        <TransactionList accountId={accountId} />
      </div>
    </div>
  );
};
export default TransactionPage;
