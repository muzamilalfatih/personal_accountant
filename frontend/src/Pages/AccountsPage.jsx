import AccountList from "../Components/AccountsList";
import useTitle from "../CustomHooks/useTitle";

const AccountsPage = () => {
  useTitle("الحسابات");
  return <AccountList />;
};
export default AccountsPage;
