import { useParams } from "react-router-dom";
import ResetPassword from "../Components/ResetPassword";
import useTitle from "../CustomHooks/useTitle";

const ResetPasswordPage = () => {
  const { id, token } = useParams();
  useTitle("إعادة تعيين كلمة المرور");
  return <ResetPassword id={id} token={token} />;
};
export default ResetPasswordPage;
