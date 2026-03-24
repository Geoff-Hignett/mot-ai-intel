import axios from "axios";
import { getToken } from "../lib/auth";

export const api = axios.create({
    baseURL: "https://localhost:7001/api",
});

api.interceptors.request.use((config) => {
    const token = getToken();

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});
