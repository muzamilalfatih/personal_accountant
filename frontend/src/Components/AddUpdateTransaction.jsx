import { useEffect, useState } from "react";
import {
  addNewTransaction,
  updateTransaction,
} from "../Services/transactionApi";

const AddUpdateTransaction = ({
  type,
  onSave,
  onCancel,
  accountId,
  selectedTransaction,
  setType,
}) => {
  const isEditing = !!selectedTransaction;
  const [transaction, setTransaction] = useState({
    amount: 0,
    description: "",
  });

  console.log(selectedTransaction);
  useEffect(() => {
    if (selectedTransaction) {
      setTransaction(selectedTransaction);
    } else {
      setTransaction({ amount: 0, description: "" });
    }
  }, [selectedTransaction]);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const handleChange = (e) => {
    const { name, value } = e.target;
    setTransaction((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!transaction.amount || !transaction.description) return;

    setIsLoading(true);
    setError("");
    try {
      console.log(type);
      let savedTransaction;
      if (isEditing) {
        savedTransaction = await updateTransaction({
          ...transaction,
          amount:
            type == "outgoing"
              ? -Math.abs(transaction.amount)
              : Math.abs(transaction.amount),
          accountId,
        });
        console.log(savedTransaction);
      } else {
        savedTransaction = await addNewTransaction({
          ...transaction,
          amount: type == "outgoing" ? -transaction.amount : transaction.amount,
          accountId,
        });
      }

      onSave(savedTransaction, isEditing);
    } catch (error) {
      console.log(error);
      setError(error?.response?.data?.message || "An Error happen");
    } finally {
      setIsLoading(false);
    }
  };
  return (
    <form
      onSubmit={handleSubmit}
      className="p-4 border-b border-gray-700 bg-gray-850 flex flex-col gap-3"
    >
      <h3
        className={`text-xl font-semibold text-center ${
          type === "income" ? "text-green-400" : "text-red-400"
        }`}
      >
        {type === "income" ? "Add Income" : "Add Outgoing"}
      </h3>
      {error && <div className="text-red-500 text-center py-10">{error}</div>}
      <input
        required
        type="number"
        name="amount"
        placeholder="Amount"
        value={Math.abs(transaction.amount)}
        onChange={handleChange}
        className="p-2 rounded bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
      />
      <input
        required
        type="text"
        name="description"
        placeholder="Description"
        value={transaction.description}
        onChange={handleChange}
        className="p-2 rounded bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-indigo-500"
      />

      <div
        className={`flex  gap-2 ${
          !isLoading ? "justify-between" : "justify-end"
        } `}
      >
        {!isLoading && (
          <div className="flex justify-between gap-2 ">
            <button
              type="button"
              onClick={() => setType("income")}
              className="bg-green-600 hover:bg-green-500 text-white font-semibold px-4 py-2 rounded-lg"
            >
              Income
            </button>
            <button
              type="button"
              onClick={() => setType("outgoing")}
              className="bg-red-600 hover:bg-red-500 text-white font-semibold px-4 py-2 rounded-lg"
            >
              Outgoing
            </button>
          </div>
        )}
        <div className="flex justify-end gap-2">
          {!isLoading && (
            <button
              type="button"
              onClick={onCancel}
              className="bg-gray-600 hover:bg-gray-500 text-white px-4 py-2 rounded-lg"
            >
              إلغاء
            </button>
          )}
          <button
            disabled={isLoading}
            type="submit"
            className={`disabled:opacity-70 disabled:cursor-not-allowed ${
              type === "income"
                ? "bg-green-600 hover:bg-green-500"
                : "bg-red-600 hover:bg-red-500"
            } text-white px-4 py-2 rounded-lg`}
          >
            {isLoading ? "...تحميل" : "حفظ"}
          </button>
        </div>
      </div>
    </form>
  );
};
export default AddUpdateTransaction;
