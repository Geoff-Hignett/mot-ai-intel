import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getVehicle } from "./api/vehicle";
import { getToken, clearToken, getUserFromToken } from "./lib/auth";
import AuthForm from "./components/AuthForm";
import Profile from "./components/Profile";
import Header from "./components/Header";

export default function App() {
    const [reg, setReg] = useState("");
    const [search, setSearch] = useState("");
    const [token, setTokenState] = useState(getToken());

    const user = getUserFromToken();

    const { data, isLoading, error } = useQuery({
        queryKey: ["vehicle", search],
        queryFn: () => getVehicle(search),
        enabled: !!search,
    });

    const handleLogout = () => {
        clearToken();
        setTokenState(null);
    };

    return (
        <div>
            {/* 🔝 HEADER */}
            <Header email={user?.email} onLogout={handleLogout} />

            <div style={{ padding: 20 }}>
                {/* 🔐 AUTH / USER AREA */}
                {!token ? (
                    <div style={{ marginBottom: 20 }}>
                        <h3>Welcome</h3>
                        <p>You can search as a guest or create an account for better personalised results.</p>

                        <AuthForm onAuth={(t) => setTokenState(t)} />
                    </div>
                ) : (
                    <div style={{ marginBottom: 20 }}>
                        <Profile />
                    </div>
                )}

                {/* 🔍 SEARCH */}
                <div style={{ marginBottom: 20 }}>
                    <input value={reg} onChange={(e) => setReg(e.target.value)} placeholder="Enter registration" style={{ marginRight: 10 }} />

                    <button onClick={() => setSearch(reg)}>Search</button>
                </div>

                {/* 📡 STATES */}
                {isLoading && <p>Loading...</p>}
                {error && <p>Error loading vehicle</p>}

                {/* 🚗 RESULT */}
                {data && (
                    <div style={{ marginTop: 20 }}>
                        <h2>
                            {data.vehicle.make} {data.vehicle.model}
                        </h2>

                        <p>
                            <strong>Risk:</strong> {data.ai.risk}
                        </p>

                        <p>{data.ai.summary}</p>

                        <ul>
                            {data.ai.recommendations.map((r: string, i: number) => (
                                <li key={i}>{r}</li>
                            ))}
                        </ul>
                    </div>
                )}
            </div>
        </div>
    );
}
