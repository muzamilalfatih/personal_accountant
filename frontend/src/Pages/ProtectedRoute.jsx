import { Navigate } from "react-router-dom";
import { useAuth } from "../CustomHooks/useAuth";

const ProtectedRoute = ({ children, allowRoles = [], from = "/" }) => {
  const { token, user } = useAuth();
  if (!token || !user) return <Navigate to="/" replace />;
  if (allowRoles.length > 0 && !allowRoles.includes(user.role))
    return <Navigate to="/forbidden" state={{ from }} />;

  return children;
};
export default ProtectedRoute;
