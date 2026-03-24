import { api } from "./client";

export const getVehicle = async (reg: string) => {
    const res = await api.get(`/vehicle/${reg}`);
    return res.data;
};
