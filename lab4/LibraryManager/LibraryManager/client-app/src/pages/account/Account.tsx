import React, { useEffect, useState } from "react";
import { deleteAccount, getAccountInfo } from "../../fetches";
import history from "../../history";
import { AccountInfo } from "../../models/AccountInfo";

export const Account = () => {
    const [password, setPassword] = useState<string>("");
    const [accountInfo, setAccountInfo] = useState<AccountInfo | undefined>(
        undefined
    );

    const loadAccountInfo = async () => {
        const accountInfo = await getAccountInfo();
        setAccountInfo(accountInfo);
    };

    useEffect(() => {
        loadAccountInfo();
    }, []);

    const onDeleteCick = async () => {
        const success = await deleteAccount(password);
        if (!success) {
            return;
        }
        window.dispatchEvent(new Event("session"));
        history.replace("/home");
    };

    return (
        <>
            {accountInfo !== undefined && (
                <div className="login-form">
                    {accountInfo.numberOfBorrowings > 0 ? (
                        <form>
                            <div className="alert alert-warning" role="alert">
                                <p>
                                    <strong>
                                        You have{" "}
                                        {accountInfo.numberOfBorrowings} books
                                        borrowed.
                                    </strong>
                                </p>
                            </div>
                        </form>
                    ) : (
                        <form>
                            <div className="alert alert-warning" role="alert">
                                <p>
                                    <strong>
                                        Deleting this data will permanently
                                        remove your account, and this cannot be
                                        recovered. Enter your password to
                                        proceed.
                                    </strong>
                                </p>
                            </div>

                            <div className="form-outline mb-4">
                                <input
                                    onChange={(e) =>
                                        setPassword(e.target.value)
                                    }
                                    type="password"
                                    id="form2Example2"
                                    className="form-control"
                                    value={password}
                                />
                                <label
                                    className="form-label"
                                    htmlFor="form2Example2"
                                >
                                    Password
                                </label>
                            </div>

                            <button
                                onClick={onDeleteCick}
                                type="button"
                                disabled={!password}
                                className="btn btn-danger btn-block mb-4"
                            >
                                Confirm
                            </button>
                        </form>
                    )}
                </div>
            )}
        </>
    );
};
