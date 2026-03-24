import { useState } from "react";
import { login, register } from "../api/auth";
import { setToken } from "../lib/auth";

export default function AuthForm() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleRegister = async () => {
        const res = await register(email, password);
        setToken(res.token);
        alert("Registered + logged in");
    };

    const handleLogin = async () => {
        const res = await login(email, password);
        setToken(res.token);
        alert("Logged in");
    };

    return (
        <div style={{ marginBottom: 20 }}>
            <h3>Auth</h3>

            <input placeholder="email" value={email} onChange={(e) => setEmail(e.target.value)} />

            <input placeholder="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />

            <button onClick={handleRegister}>Register</button>
            <button onClick={handleLogin}>Login</button>
        </div>
    );
}
