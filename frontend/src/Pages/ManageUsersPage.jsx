import { useEffect, useState } from "react";
import UsersList from "../Components/UsersList";
import Spinner from "../Components/Spinner";
import { getAllUsers } from "../Services/userApi"; // implement API call
import { useAuth } from "../CustomHooks/useAuth";
import useTitle from "../CustomHooks/useTitle";

const ManageUsersPage = () => {
  const { user } = useAuth();
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  useTitle("إدارة المستخدمين");

  console.log(user);
  useEffect(() => {
    const loadUsers = async () => {
      setIsLoading(true);
      try {
        const data = await getAllUsers(user.id);
        setUsers(data);
      } catch (err) {
        setError(err?.response?.data?.message || "Failed to load users.");
      } finally {
        setIsLoading(false);
      }
    };

    loadUsers();
  }, []);

  if (isLoading) return <Spinner />;

  return (
    <div className="min-h-screen bg-gray-900 p-6">
      <h1 className="text-3xl text-white font-bold mb-6">إدارة المستخدمين</h1>
      {error && (
        <div className="bg-red-600 text-white px-4 py-2 rounded mb-4">
          {error}
        </div>
      )}
      <UsersList users={users} setUsers={setUsers} setError={setError} />
    </div>
  );
};

export default ManageUsersPage;
