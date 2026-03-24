import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getVehicle } from "./api/vehicle";

export default function App() {
    const [reg, setReg] = useState("");
    const [search, setSearch] = useState("");

    const { data, isLoading, error } = useQuery({
        queryKey: ["vehicle", search],
        queryFn: () => getVehicle(search),
        enabled: !!search,
    });

    return (
        <div style={{ padding: 20 }}>
            <h1>MOT AI</h1>

            <input value={reg} onChange={(e) => setReg(e.target.value)} placeholder="Enter reg" />

            <button onClick={() => setSearch(reg)}>Search</button>

            {isLoading && <p>Loading...</p>}

            {error && <p>Error</p>}

            {data && (
                <div>
                    <h2>
                        {data.vehicle.make} {data.vehicle.model}
                    </h2>

                    <p>Risk: {data.ai.risk}</p>
                    <p>{data.ai.summary}</p>

                    <ul>
                        {data.ai.recommendations.map((r: string, i: number) => (
                            <li key={i}>{r}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
}
