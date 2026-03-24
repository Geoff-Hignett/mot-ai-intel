import { api } from "./client";

export const register = async (email: string, password: string) => {
    const res = await api.post("/auth/register", { email, password });
    return res.data;
};

export const login = async (email: string, password: string) => {
    const res = await api.post("/auth/login", { email, password });
    return res.data;
};
