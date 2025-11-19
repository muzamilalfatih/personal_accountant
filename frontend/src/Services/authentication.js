import api from "./api";

// export async function resetPassword(token, newPassword) {
//     await api.po
// }

export async function forgetPassword(email) {
  console.log(email);
  await api.post("authentication/forget-password", JSON.stringify(email), {
    headers: {
      "Content-Type": "application/json",
    },
  });
}

export async function resetPassword(token, newPassword) {
  await api.post(
    "authentication/reset-password",
    { token, newPassword },
    {
      headers: {
        "Content-Type ": "application/json",
      },
    }
  );
}
