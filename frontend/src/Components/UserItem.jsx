import { useState } from "react";
import { deleteUser, updateUserRole } from "../Services/userApi";
import { Users } from "lucide-react";

const UserItem = ({ user, setUsers, setError }) => {
  const [loadingRole, setLoadingRole] = useState(false);
  const [loadingDelete, setLoadingDelete] = useState(false);

  const toggleRole = async () => {
    setLoadingRole(true);
    try {
      setError("");
      const newRole = user.role === "Admin" ? "User" : "Admin";
      await updateUserRole(user.id, newRole);
      setUsers((prev) =>
        prev.map((u) => (u.id === user.id ? { ...u, role: newRole } : u))
      );
    } catch (err) {
      setError(err?.response?.data?.message || "فشل تحديث الدور");
      console.log(err);
    } finally {
      setLoadingRole(false);
    }
  };
  const handleDelete = async (id) => {
    try {
      setError("");
      setLoadingDelete(true);
      await deleteUser(id);
      setUsers((prev) => prev.filter((user) => user.id != id));
    } catch (error) {
      setError(error?.response?.data?.message || "Failed to delete user");
    } finally {
      setLoadingDelete(false);
    }
  };

  return (
    <li className="flex justify-between items-center py-3 px-4 bg-gray-800 rounded mb-2">
      <div>
        <div className="text-white font-semibold mx-5">
          {user.firstName} {user.lastName}
        </div>
        <div className="text-gray-400 text-sm">{user.email}</div>
        <div className="text-gray-300 text-sm">الدور: {user.role}</div>
      </div>
      <div className="flex gap-2">
        {!loadingDelete && (
          <button
            onClick={toggleRole}
            disabled={loadingRole}
            className="px-3 py-1 bg-indigo-600 hover:bg-indigo-500 text-white rounded"
          >
            {loadingRole
              ? "...تحميل"
              : user.role === "Admin"
              ? "تحويل لمستخدم"
              : "تحويل لمسؤول"}
          </button>
        )}
        {!loadingRole && (
          <button
            onClick={() => handleDelete(user.id)}
            disabled={loadingDelete}
            className="px-3 py-1 bg-red-600 hover:bg-red-500 text-white rounded"
          >
            {" "}
            {loadingDelete ? "...تحميل" : " حذف"}
          </button>
        )}
      </div>
    </li>
  );
};
export default UserItem;
