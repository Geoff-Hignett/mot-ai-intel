import { api } from "./client";

export const getProfile = async () => {
    const res = await api.get("/user/profile");
    return res.data;
};

export const updateProfile = async (data: { yearlyMileage?: number; drivingType?: string; mechanicalKnowledge?: string }) => {
    const res = await api.put("/user/profile", data);
    return res.data;
};
