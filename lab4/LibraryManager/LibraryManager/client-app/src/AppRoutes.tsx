import React, { useEffect, useState } from "react";
import "./App.css";
import Books from "./pages/books/Books";
import { Navigate, Route, Routes } from "react-router-dom";
import Reservations from "./pages/reservations/Reservations";
import Borrowings from "./pages/borrowings/Borrowings";
import MyReservations from "./pages/myReservations/MyReservations";
import { Login } from "./pages/account/Login";
import { Home } from "./pages/home/Home";
import { Register } from "./pages/account/Register";
import { Account } from "./pages/account/Account";
import { getUser } from "./fetches";
import { User } from "./models/User";

export const AppRoutes = () => {
    const [user, setUser] = useState<User | undefined>(undefined);

    const getAndSetUser = async () => {
        const user = await getUser();
        if (user) {
            setUser(user);
        }
    };

    useEffect(() => {
        getAndSetUser();
    }, []);

    return (
        <Routes>
            <Route path={"/home"} element={<Home />} />
            <Route path={"/login"} element={<Login />} />
            <Route path={"/register"} element={<Register />} />
            {!!user?.username && !user?.isAdmin ? (
                <Route path={"/account"} element={<Account />} />
            ) : null}
            <Route path={"/books"} element={<Books />} />
            <Route path={"/"} element={<Navigate to="/home" />} />
            <Route
                path={"/reservations"}
                element={user?.isAdmin ? <Reservations /> : <MyReservations />}
            />
            {user?.isAdmin ? (
                <Route path={"/borrowings"} element={<Borrowings />} />
            ) : null}
        </Routes>
    );
};
