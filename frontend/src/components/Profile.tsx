import { useQuery, useMutation } from "@tanstack/react-query";
import { getProfile, updateProfile } from "../api/user";
import { useState, useEffect } from "react";

export default function Profile() {
    const { data, isLoading } = useQuery({
        queryKey: ["profile"],
        queryFn: getProfile,
    });

    const mutation = useMutation({
        mutationFn: updateProfile,
    });

    const [mileage, setMileage] = useState("");
    const [drivingType, setDrivingType] = useState("");
    const [knowledge, setKnowledge] = useState("");

    useEffect(() => {
        if (data) {
            setMileage(data.yearlyMileage ?? "");
            setDrivingType(data.drivingType ?? "");
            setKnowledge(data.mechanicalKnowledge ?? "");
        }
    }, [data]);

    const handleSave = () => {
        mutation.mutate({
            yearlyMileage: mileage ? Number(mileage) : undefined,
            drivingType,
            mechanicalKnowledge: knowledge,
        });
    };

    if (isLoading) return <p>Loading profile...</p>;

    return (
        <div style={{ marginBottom: 20 }}>
            <h3>Profile</h3>

            <input placeholder="Yearly mileage" value={mileage} onChange={(e) => setMileage(e.target.value)} />

            <input placeholder="Driving type (city/highway)" value={drivingType} onChange={(e) => setDrivingType(e.target.value)} />

            <input placeholder="Mechanical knowledge (low/medium/high)" value={knowledge} onChange={(e) => setKnowledge(e.target.value)} />

            <button onClick={handleSave}>Save</button>

            {mutation.isSuccess && <p>Saved!</p>}
        </div>
    );
}
