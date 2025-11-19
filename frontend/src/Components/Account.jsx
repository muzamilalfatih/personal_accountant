import { PencilIcon, TrashIcon } from "@heroicons/react/24/solid";
import { formatDate } from "../Services/utility";

const Account = ({ account, handleDelete, onEdit, handleShowTransactions }) => {
  return (
    <li
      onClick={() => handleShowTransactions(account)}
      className="flex items-center justify-between px-5 py-3 hover:bg-gray-750 transitions "
    >
      <div className="flex flex-col items-start text-right">
        <span className="text-lg ">{account.name}</span>
        <span className="text-sm font-semibold text-gray-400">
          {formatDate(account.date)}
        </span>
      </div>
      <span
        className={`text-lg font-bold ${
          account.totalBalance >= 0 ? "text-green-400" : "text-red-400"
        }`}
      >
        {account.totalBalance}
      </span>

      <div>
        <PencilIcon
          onClick={(e) => {
            e.stopPropagation();
            onEdit(account);
          }}
          className="h-5 w-5 m-1 text-indigo-400 cursor-pointer hover:text-indigo-300"
        />
        <TrashIcon
          className="h-5 w-5 text-red-500 cursor-pointer hover:text-red-400"
          onClick={(e) => {
            e.stopPropagation();
            handleDelete(account.id);
          }}
        />
      </div>
    </li>
  );
};
export default Account;
