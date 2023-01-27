import React, { useEffect, useState } from "react";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { getUser, logout } from "../fetches";
import history from "../history";
import { User } from "../models/User";

export default function NavigationBar() {
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

    const onLogout = async () => {
        await logout();
        setUser(undefined);
        history.replace("/home");
    };

    useEffect(() => {
        const listenSessionChange = () => {
            getAndSetUser();
        };
        window.addEventListener("session", listenSessionChange);
        return () => window.removeEventListener("session", listenSessionChange);
    }, []);

    return (
        <Navbar bg="light" variant="light">
            <Container>
                <Navbar.Brand href="/home">Home</Navbar.Brand>
                <Nav className="me-auto">
                    <Nav.Link href="books">Books</Nav.Link>
                    <Nav.Link href="reservations">Reservations</Nav.Link>
                    {user?.isAdmin ? (
                        <Nav.Link href="borrowings">Borrowings</Nav.Link>
                    ) : null}
                </Nav>
                {user?.username ? (
                    <>
                        <Nav.Link href="account">
                            Hello {user.username}!
                        </Nav.Link>
                        <Nav.Link
                            onClick={onLogout}
                            style={{ marginLeft: "10px" }}
                        >
                            Logout
                        </Nav.Link>
                    </>
                ) : (
                    <Nav.Link href="login">Login</Nav.Link>
                )}
            </Container>
        </Navbar>
    );
}
