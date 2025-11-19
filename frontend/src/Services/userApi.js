import axios from "axios";
import { baseUrl } from "./utility";
import api from "./api";
export const addUserAsync = async (user) => {
  try {
    const response = await axios.post(`${baseUrl}/users`, user, {
      header: {
        "content-type": "application/json",
      },
    });

    return response.data;
  } catch (err) {
    throw err?.response?.data;
  }
};

export const logIn = async (loginModel) => {
  try {
    const response = await api.post(`authentication/login`, loginModel, {
      header: {
        "content-type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.log(error.response);
    throw error?.response?.data;
  }
};

export const updateUser = async (updatedUser) => {
  const res = await api.put(`users/${updatedUser.id}`, updatedUser, {
    headers: {
      "Content-Type": "application/json",
    },
  });

  return res.data;
};

export const getAllUsers = async (currentUserId) => {
  console.log(currentUserId);
  const res = await api.get("users", {
    params: {
      currentUserId,
    },
  });
  return res.data;
};

export const updateUserRole = async (userId, newRole) => {
  await api.patch(`users/${userId}`, JSON.stringify(newRole), {
    headers: {
      "Content-Type": "application/json",
    },
  });
};

export const deleteUser = async (id) => {
  await api.delete(`users/${id}`);
};

export const changePassword = async (id, oldPassword, newPassword) => {
  await api.post(
    `users/${id}/reset-password`,
    { currentPassword: oldPassword, newPassword },
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
};
export const confirmEmailAsync = async (token) => {
  console.log(token);
  await api.get("users/confirm-email", {
    params: { token },
  });
};
