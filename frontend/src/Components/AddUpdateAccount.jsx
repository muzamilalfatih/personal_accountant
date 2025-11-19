import { useState } from "react";
import { addNewAccount, updateAcount } from "../Services/accountApi";
import { useAuth } from "../CustomHooks/useAuth";

const AddUpdateAccount = ({ onSave, account, onCancel }) => {
  const isEditing = !!account;
  const [error, setError] = useState("");
  const [name, setName] = useState(account ? account.name : "");
  const [isLoading, setIsLoading] = useState(false);
  const { user } = useAuth();

  const handleSaveAcount = async (e) => {
    e.preventDefault();
    if (!name.trim()) return;
    try {
      setError("");
      setIsLoading(true);
      let savedAccount;
      if (isEditing) {
        savedAccount = await updateAcount({
          id: account.id,
          name,
          userId: user.id,
        });
      } else {
        savedAccount = await addNewAccount({ name, userId: user.id });
      }
      onSave(savedAccount, isEditing);
      setName("");
    } catch (error) {
      setError(error?.message || "Failed to save account");
    } finally {
      setIsLoading(false);
    }
  };
  return (
    <div
      className="fixed inset-0 bg-gray-950 bg-opacity-60 backdrop-blur-sm flex items-center justify-center z-50 transition-opacity"
      onClick={onCancel}
    >
      <form
        onSubmit={handleSaveAcount}
        className="bg-gray-800 rounded-xl p-6 w-full max-w-sm shadow-xl border border-gray-700"
        onClick={(e) => e.stopPropagation()} // prevent closing when clicking inside
      >
        <h2 className="text-xl font-bold text-white mb-4 text-center">
          {isEditing ? "تعديل الحساب" : "إنشاء حساب جديد"}
        </h2>
        {error && <div className="text-red-500 text-center py-10">{error}</div>}
        <input
          required
          value={name}
          onChange={(e) => setName(e.target.value)}
          type="text"
          placeholder="اسم الحساب"
          className="w-full p-2 rounded-md bg-gray-700 text-white border border-gray-600 focus:ring-2 focus:ring-green-500 focus:outline-none mb-4"
        />
        <div className="flex justify-end space-x-3">
          {!isLoading && (
            <button
              onClick={onCancel}
              className="px-4 py-2 rounded-md bg-gray-600 hover:bg-gray-500 text-white"
            >
              إلغاء
            </button>
          )}
          <button
            disabled={isLoading}
            className="px-4 py-2 rounded-md bg-green-600 hover:bg-green-500 text-white font-semibold flex items-center justify-center gap-2 disabled:opacity-70 disabled:cursor-not-allowed"
          >
            {isLoading ? "...تحميل" : "حفظ"}
          </button>
        </div>
      </form>
    </div>
  );
};
export default AddUpdateAccount;
