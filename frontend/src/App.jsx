import { Route, Routes } from "react-router-dom";
import "./App.css";
import ProtectedRoute from "./Pages/ProtectedRoute";
import MainLayout from "./Layouts/MainLayout";

import {
  RegisterPage,
  NotFoundPage,
  LoginPage,
  AccountsPage,
  ForbiddenPage,
  TransactionPage,
  ProfilePage,
  ManageUsersPage,
  ResetPasswordPage,
  ForgotPasswordPage,
  EmailConfirmationPage,
} from "./Pages";

function App() {
  return (
    <>
      <Routes>
        <Route element={<MainLayout />}>
          <Route
            path="/accounts"
            element={
              <ProtectedRoute allowRoles={["User", "Admin"]}>
                <AccountsPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/transactions"
            element={
              <ProtectedRoute>
                <TransactionPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/profile"
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/manage-users"
            element={
              <ProtectedRoute allowRoles={["Admin"]}>
                <ManageUsersPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/users/:id/change-password"
            element={
              <ProtectedRoute>
                <ResetPasswordPage />
              </ProtectedRoute>
            }
          />
        </Route>
        <Route path="/reset-password/:token" element={<ResetPasswordPage />} />
        <Route path="/" element={<LoginPage />} />
        <Route path="forget-password" element={<ForgotPasswordPage />} />
        <Route
          path="confirm-email/:token"
          element={<EmailConfirmationPage />}
        />
        <Route path="/register" element={<RegisterPage />} />

        <Route path="/forbidden" element={<ForbiddenPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  );
}

export default App;
