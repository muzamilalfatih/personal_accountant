// api.js
import axios from "axios";
import { baseUrl } from "./utility";

const api = axios.create({
  baseURL: baseUrl,
});

export const setAuthToken = (token) => {
  api.defaults.headers.common["Authorization"] = token
    ? `Bearer ${token}`
    : undefined;
};

export default api;
