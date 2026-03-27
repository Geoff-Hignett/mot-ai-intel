import { useQuery, useMutation } from "@tanstack/react-query";
import { getProfile, updateProfile } from "../api/user";
import { useState, useEffect } from "react";
import { getToken } from "../lib/auth";

export default function Profile() {
    const token = getToken();

    const { data, isLoading } = useQuery({
        queryKey: ["profile"],
        queryFn: getProfile,
        enabled: !!token,
    });

    const mutation = useMutation({
        mutationFn: updateProfile,
    });

    const [mileage, setMileage] = useState("");
    const [drivingType, setDrivingType] = useState("");
    const [knowledge, setKnowledge] = useState("");

    useEffect(() => {
        if (data) {
            setMileage(data.yearlyMileage?.toString() ?? "");
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
        <div className="card">
            <h3>Your Driving Profile</h3>

            <p style={{ fontSize: 14, color: "#666" }}>
                This information helps tailor AI recommendations to your driving habits and experience level.
            </p>

            {/*  Yearly Mileage */}
            <div style={{ marginBottom: 15 }}>
                <label>
                    <strong>Yearly Mileage</strong>
                </label>
                <p style={{ fontSize: 12, color: "#666" }}>
                    Approximate miles you drive per year. Higher mileage may increase wear and maintenance needs.
                </p>
                <input type="number" value={mileage} onChange={(e) => setMileage(e.target.value)} placeholder="e.g. 12000" />
            </div>

            {/*  Driving Type */}
            <div style={{ marginBottom: 15 }}>
                <label>
                    <strong>Driving Type</strong>
                </label>
                <p style={{ fontSize: 12, color: "#666" }}>
                    Helps assess importance of the vehicle. Daily commuters need more reliability than occasional social drivers.
                </p>
                <select value={drivingType} onChange={(e) => setDrivingType(e.target.value)}>
                    <option value="">Select...</option>
                    <option value="light">Light (Rarely use the car)</option>
                    <option value="medium">Medium (Mixture of business and pleasure)</option>
                    <option value="heavy">Heavy (Rely on vehicle on a daily basis)</option>
                </select>
            </div>

            {/*  Mechanical Knowledge */}
            <div style={{ marginBottom: 15 }}>
                <label>
                    <strong>Mechanical Knowledge</strong>
                </label>
                <p style={{ fontSize: 12, color: "#666" }}>Used to adjust recommendations - simpler guidance vs more technical detail.</p>
                <select value={knowledge} onChange={(e) => setKnowledge(e.target.value)}>
                    <option value="">Select...</option>
                    <option value="low">Low (prefer simple explanations)</option>
                    <option value="medium">Medium (basic understanding)</option>
                    <option value="high">High (comfortable with technical detail)</option>
                </select>
            </div>

            <button onClick={handleSave}>Save Profile</button>

            {mutation.isSuccess && <p style={{ color: "green" }}>Saved!</p>}
        </div>
    );
}
