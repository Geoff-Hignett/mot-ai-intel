type Props = {
    email?: string | null;
    onLogout: () => void;
};

export default function Header({ email, onLogout }: Props) {
    return (
        <div
            style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                padding: "10px 20px",
                borderBottom: "1px solid #ccc",
                marginBottom: 20,
            }}>
            <h2>MOT AI</h2>

            {email ? (
                <div>
                    <span style={{ marginRight: 10 }}>👤 {email}</span>
                    <button onClick={onLogout}>Logout</button>
                </div>
            ) : (
                <span>Guest mode</span>
            )}
        </div>
    );
}
