import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { formatDate } from "../Services/utility";
const TransactionItem = ({ transaction, handleDelete, handleUpdate }) => {
  return (
    <li className="flex justify-between items-center p-4">
      <div className="text-lg font-semibold">{transaction.description}</div>
      <div className="text-sm text-gray-400">
        {formatDate(transaction.date)}
      </div>
      <div
        className={`text-xl font-bold ${
          transaction.amount < 0 ? "text-red-500" : "text-green-500"
        }`}
      >
        {Math.abs(transaction.amount).toLocaleString()}
      </div>

      <div>
        <PencilIcon
        
          onClick={() => handleUpdate(transaction)}
          className="h-5 w-5 m-1 text-indigo-400 cursor-pointer hover:text-indigo-300"
        />
        <TrashIcon
          onClick={() => handleDelete(transaction.id)}
          className="h-5 w-5 text-red-500 cursor-pointer hover:text-red-400"
        />
      </div>
    </li>
  );
};
export default TransactionItem;
