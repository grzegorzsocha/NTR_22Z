import React, { useState } from "react";
import { register } from "../../fetches";
import history from "../../history";

export const Register = () => {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");

    const onRegisterClick = async () => {
        const success = await register(username, password);
        if (!success) return;
        history.replace("/login");
    };

    return (
        <div className="login-form">
            <form>
                <div className="form-outline mb-4">
                    <input
                        onChange={(e) => setUsername(e.target.value)}
                        id="form2Example1"
                        className="form-control"
                        value={username}
                    />
                    <label className="form-label" htmlFor="form2Example1">
                        Username
                    </label>
                </div>

                <div className="form-outline mb-4">
                    <input
                        onChange={(e) => setPassword(e.target.value)}
                        type="password"
                        id="form2Example2"
                        className="form-control"
                        value={password}
                    />
                    <label className="form-label" htmlFor="form2Example2">
                        Password
                    </label>
                </div>

                <div className="form-outline mb-4">
                    <input
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        type="password"
                        id="form2Example3"
                        className="form-control"
                        value={confirmPassword}
                    />
                    <label className="form-label" htmlFor="form2Example3">
                        Confirm password
                    </label>
                </div>

                <div className="row mb-4">
                    <div className="col d-flex justify-content-center">
                        {password !== confirmPassword && (
                            <p className="text-danger">Passwords must match!</p>
                        )}
                    </div>
                </div>

                <button
                    onClick={onRegisterClick}
                    disabled={
                        password !== confirmPassword || !password || !username
                    }
                    type="button"
                    className="btn btn-primary btn-block mb-4"
                >
                    Register
                </button>
            </form>
        </div>
    );
};
