import React, { useState } from "react";
import { login } from "../../fetches";
import history from "../../history";

export const Login = () => {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [rememberMe, setRememberMe] = useState<boolean>(true);

    const onLoginClick = async () => {
        const success = await login(username, password, rememberMe);
        if (!success) {
            return;
        }
        window.dispatchEvent(new Event("session"));
        history.replace("/home");
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

                <div className="row mb-4">
                    <div className="col d-flex justify-content-center">
                        <div className="form-check">
                            <input
                                onChange={(e) =>
                                    setRememberMe(e.target.checked)
                                }
                                className="form-check-input"
                                type="checkbox"
                                value=""
                                id="form2Example31"
                                checked={rememberMe}
                            />
                            <label
                                className="form-check-label"
                                htmlFor="form2Example31"
                            >
                                Remember me
                            </label>
                        </div>
                    </div>
                </div>

                <button
                    onClick={onLoginClick}
                    type="button"
                    className="btn btn-primary btn-block mb-4"
                >
                    Sign in
                </button>

                <div className="text-center">
                    <p>
                        Not a member? <a href="/register">Register</a>
                    </p>
                </div>
            </form>
        </div>
    );
};
