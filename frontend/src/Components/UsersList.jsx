import UserItem from "./UserItem";

const UsersList = ({ users, setUsers, setError }) => {
  console.log(users);
  if (!users.length)
    return <div className="text-gray-300">لا يوجد مستخدمين.</div>;
  return (
    <ul className="divide-y divide-gray-700">
      {users.map((user) => (
        <UserItem
          key={user.id}
          user={user}
          setUsers={setUsers}
          setError={setError}
        />
      ))}
    </ul>
  );
};
export default UsersList;
