import { useEffect, useState } from "react";
import TransactionItem from "./TransactionItem";
import {
  deleteTransaction,
  getAllTransactions,
} from "../Services/transactionApi";
import Spinner from "../Components/Spinner";
import AddUpdateTransaction from "../Components/AddUpdateTransaction";
const TransactionList = ({ accountId }) => {
  const [transactions, setTransactions] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [showForm, setShowForm] = useState();
  const [formType, setFormType] = useState("");
  const [selecteTransaction, setSelectedTransaction] = useState(null);

  const totalBalance = transactions.reduce((sum, t) => sum + t.amount, 0);

  const onSave = (savedTransaction, isEditing) => {
    setError("");
    console.log(isEditing);
    console.log(savedTransaction);
    setShowForm(false);
    if (isEditing) {
      setTransactions(
        transactions.map((transaction) =>
          transaction.id == savedTransaction.id ? savedTransaction : transaction
        )
      );
      setSelectedTransaction(null);
    } else {
      setTransactions([...transactions, savedTransaction]);
    }
  };
  const handleDelete = async (id) => {
    try {
      setIsLoading(true);
      setError("");

      await deleteTransaction(id);
      setTransactions(
        transactions.filter((transaction) => transaction.id !== id)
      );
    } catch (error) {
      setError(error?.response?.date?.message || "Failed to delete account");
    } finally {
      setIsLoading(false);
    }
  };
  const handleUpdate = (tran) => {
    console.log(tran);
    setSelectedTransaction(tran);
    setShowForm(true);
    setFormType(tran.amount < 0 ? "outgoing" : "income");
  };
  const handleCancel = () => {
    setSelectedTransaction(null);
    setShowForm(false);
  };
  useEffect(() => {
    const loadData = async () => {
      try {
        setError("");
        const transactions = await getAllTransactions(accountId);
        console.log(transactions);
        setTransactions(transactions);
      } catch (err) {
        const message =
          err?.response?.data?.message || "Failed to load transactions";
        console.log(message);
        setError(message);
      } finally {
        setIsLoading(false);
      }
    };

    loadData();
  }, [accountId]);

  const handleShowForm = (formType) => {
    setFormType(formType);
    setShowForm(true);
  };
  if (isLoading) return <Spinner />;
  return (
    <>
      {/* Smooth overlay form */}
      {showForm && (
        <div
          className="fixed inset-0 bg-black/70 backdrop-blur-sm flex items-center justify-center 
          z-50 transition-all duration-300 ease-in-out animate-fadeIn"
        >
          <div className="w-full max-w-md transform transition-all duration-300 scale-100">
            <AddUpdateTransaction
              type={formType}
              onSave={onSave}
              onCancel={handleCancel}
              accountId={accountId}
              selectedTransaction={selecteTransaction}
              setType={setFormType}
            />
          </div>
        </div>
      )}

      <div
        style={{ backgroundColor: "#b4cec5" }}
        className="p-6 text-center border-b border-gray-700 flex items-center justify-between"
      >
        <div className="text-gray-900 text-lg font-medium">Balance</div>
        <div
          className={`text-3xl font-bold ${
            totalBalance < 0 ? "text-red-500" : "text-green-600"
          }`}
        >
          {totalBalance.toLocaleString()}
        </div>
      </div>
      <div className="flex justify-between gap-4 px-3 py-3 border-b border-gray-700">
        <button
          onClick={() => handleShowForm("income")}
          className="bg-green-600 hover:bg-green-500 text-white font-semibold px-6 py-2 rounded-lg"
        >
          Income
        </button>
        <button
          onClick={() => handleShowForm("outgoing")}
          className="bg-red-600 hover:bg-red-500 text-white font-semibold px-6 py-2 rounded-lg"
        >
          Outgoing
        </button>
      </div>
      {error && <div className="text-red-500 text-center">{error}</div>}
      <ul className="divide-y divide-gray-700">
        {transactions.map((transaction) => (
          <TransactionItem
            key={transaction.id}
            handleUpdate={handleUpdate}
            handleDelete={handleDelete}
            transaction={transaction}
          />
        ))}
      </ul>
    </>
  );
};
export default TransactionList;
