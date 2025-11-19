import api from "./api";

export async function getAllAccounts(userId) {
  try {
    const res = await api.get(`accounts`, {
      params: {
        userId,
      },
    });

    return res?.data;
  } catch (error) {
    console.log(error);
    throw error?.response?.data;
  }
}

export async function addNewAccount(account) {
  try {
    const response = await api.post("accounts", account, {
      headers: {
        "Content-Type": "application/json ",
      },
    });
    return response.data;
  } catch (error) {
    console.log(error);
    throw error?.response?.data;
  }
}

export async function deleteAcount(id) {
  try {
    await api.delete(`accounts/${id}`);
  } catch (error) {
    console.log(error);
    throw error.response.data;
  }
}

export async function updateAcount(account) {
  console.log(account);
  try {
    const response = await api.put(`accounts/${account.id}`, account, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.log(error);
    throw error?.response?.data;
  }
}
