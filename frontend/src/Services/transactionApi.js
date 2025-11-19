import api from "./api";

export async function getAllTransactions(accountId) {
  try {
    const res = await api.get(`transactions`, {
      params: {
        accountId,
      },
    });

    return res?.data;
  } catch (error) {
    console.log(error);
    throw error;
  }
}

export async function addNewTransaction(transaction) {
  const res = await api.post("transactions", transaction, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  return res.data;
}
export async function deleteTransaction(id) {
  await api.delete(`transactions/${id}`);
}
export async function updateTransaction(transaction) {
  console.log(transaction);
  const res = await api.put(`transactions/${transaction.id}`, transaction, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  return res.data;
}
