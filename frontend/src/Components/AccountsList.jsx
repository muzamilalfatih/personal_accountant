import React, { useEffect, useState } from "react";
import { deleteAcount, getAllAccounts } from "../Services/accountApi";
import Spinner from "./Spinner";
import { useAuth } from "../CustomHooks/useAuth";
import AddUpdateAccount from "./AddUpdateAccount";
import Account from "./Account";
import TransactionList from "./TransactionList";
import ProtectedRoute from "../Pages/ProtectedRoute";
import { createSearchParams, useLocation, useNavigate } from "react-router-dom";

export default function AccountsList() {
  const [accounts, setAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isModelOpen, setIsModelOpen] = useState(false);
  const [error, setError] = useState();
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [showTransactions, setShowTrnasactions] = useState(false);
  const { user } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const loadData = async () => {
      try {
        const accounts = await getAllAccounts(user.id);
        setAccounts(accounts);
      } catch (err) {
        console.log(err?.message || "Failed to load accounts");
        // setError(err?.message || "Failed to load accounts");
      } finally {
        setIsLoading(false);
      }
    };

    loadData();
  }, []);

  const handleDelete = async (id) => {
    try {
      setIsLoading(true);
      setError("");
      setAccounts(accounts.filter((accounts) => accounts.id !== id));
      await deleteAcount(id);
    } catch (error) {
      setError(error?.message || "Failed to delete account");
    } finally {
      setIsLoading(false);
    }
  };
  const onSave = (savedAccount, isEditing) => {
    if (isEditing) {
      setAccounts(
        accounts.map((acc) => (acc.id === savedAccount.id ? savedAccount : acc))
      );
    } else {
      setAccounts([...accounts, savedAccount]);
    }
    setIsModelOpen(false);
    setSelectedAccount(null);
  };

  const handleShowTransactions = (account) => {
    navigate({
      pathname: "/transactions",
      search: `?${createSearchParams({
        accountId: account.id,
        accountName: account.name,
      })}`,
    });
  };
  return (
    <>
      {showTransactions && (
        <ProtectedRoute allowRoles={["Admin", "User"]} from={location}>
          <TransactionList
            account={selectedAccount}
            onClose={() => setShowTrnasactions(false)}
          />
        </ProtectedRoute>
      )}
      <div className="bg-gray-900 min-h-screen min-w-[360px] text-gray-100 flex flex-col items-center py-10">
        <div className="w-full max-w-2xl bg-gray-800 rounded-2xl shadow-lg overflow-hidden border border-gray-700">
          <div className="flex items-center justify-between p-4 border-b border-gray-700 bg-gray-850">
            <div className="flex items-center space-x-3">
              <img
                src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
                alt="Logo"
                className="h-8 w-8"
              />
              <h2 className="text-xl font-bold text-white">قائمة الحسابات</h2>
            </div>
          </div>
          {error && (
            <div className="text-red-500 text-center py-10">{error}</div>
          )}
          {isLoading ? (
            <div className="flex justify-center items-center py-10">
              <Spinner />
            </div>
          ) : (
            <>
              <button
                className="w-full bg-green-600 hover:bg-green-500 text-white font-semibold py-3 transition-all duration-200 cursor-pointer"
                onClick={(e) => {
                  e.stopPropagation();
                  setIsModelOpen(true);
                }}
              >
                + إضافة حساب جديد
              </button>

              <ul className="divide-y divide-gray-700">
                {accounts.map((account) => (
                  <Account
                    key={account.id}
                    account={account}
                    handleDelete={handleDelete}
                    onEdit={(acc) => {
                      setSelectedAccount(acc);
                      setIsModelOpen(true);
                    }}
                    handleShowTransactions={handleShowTransactions}
                  />
                ))}
              </ul>

              {isModelOpen && (
                <AddUpdateAccount
                  onSave={onSave}
                  onCancel={() => {
                    setSelectedAccount(null);
                    setIsModelOpen(false);
                  }}
                  account={selectedAccount}
                />
              )}
            </>
          )}
        </div>
      </div>
    </>
  );
}
