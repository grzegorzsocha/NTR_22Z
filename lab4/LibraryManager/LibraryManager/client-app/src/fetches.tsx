import axios from "axios";
import history from "./history";

axios.defaults.withCredentials = true;

axios.interceptors.response.use(
    function (response) {
        return response;
    },
    function (error) {
        console.log(error.response.data);
        if (error.response.status === 401) {
            history.replace("/login");
        } else if (error?.response?.data) {
            window.alert(error?.response?.data);
        }
        return Promise.reject(error);
    }
);

export const login = async (
    userName: string,
    password: string,
    rememberMe: boolean
) => {
    return axios
        .post("https://localhost:7126/account/login", {
            userName,
            password,
            rememberMe,
        })
        .then((response) => true);
};

export const logout = async () => {
    const response = await axios.get("https://localhost:7126/account/logout");
    return response.data;
};

export const register = async (userName: string, password: string) => {
    const response = await axios.post("https://localhost:7126/account", {
        userName,
        password,
    });

    response.status === 200
        ? alert("Account created")
        : alert("Failed to create account");
    return true;
};

export const deleteAccount = async (password: string) => {
    return axios
        .put("https://localhost:7126/account", {
            password,
        })
        .then((response) => true);
};

export const getAccountInfo = async () => {
    const response = await axios.get("https://localhost:7126/account/info");
    return response.data;
};

export const getUser = async () => {
    return axios
        .get("https://localhost:7126/account/user")
        .then((response) => response.data);
};

export const getBooks = async (searchText: string = "") => {
    return axios
        .get(
            `https://localhost:7126/books${
                searchText ? `?searchString=${searchText}` : ""
            }`
        )
        .then((response) => response.data);
};

export const getReservations = async (searchText: string = "") => {
    return axios
        .get(
            `https://localhost:7126/reservations${
                searchText ? `?searchString=${searchText}` : ""
            }`
        )
        .then((response) => response.data);
};

export const getMyReservations = async (searchText: string = "") => {
    return axios
        .get(
            `https://localhost:7126/myreservations${
                searchText ? `?searchString=${searchText}` : ""
            }`
        )
        .then((response) => response.data);
};

export const getBorrowings = async (searchText: string = "") => {
    return axios
        .get(
            `https://localhost:7126/borrowings${
                searchText ? `?searchString=${searchText}` : ""
            }`
        )
        .then((response) => response.data);
};

export const makeReservation = async (bookId: number, rowVersion: number) => {
    const response = await axios.post(
        `https://localhost:7126/books/${bookId}/reservations?rowVersion=${rowVersion}`
    );

    response.status === 200
        ? alert("Reservation made")
        : alert("Reservation failed");
    return response.data;
};

export const cancelReservation = async (bookId: number, rowVersion: number) => {
    const response = await axios.delete(
        `https://localhost:7126/books/${bookId}/reservations?rowVersion=${rowVersion}`
    );

    response.status === 200
        ? alert("Reservation cancelled")
        : alert("Cancellation failed");
    return response.data;
};

export const borrowBook = async (bookId: number, rowVersion: number) => {
    const response = await axios.post(
        `https://localhost:7126/books/${bookId}/borrowings?rowVersion=${rowVersion}`
    );

    response.status === 200
        ? alert("Book borrowed")
        : alert("Borrowing failed");
    return response.data;
};

export const returnBook = async (bookId: number, rowVersion: number) => {
    const response = await axios.delete(
        `https://localhost:7126/books/${bookId}/borrowings?rowVersion=${rowVersion}`
    );

    response.status === 200
        ? alert("Book returned")
        : alert("Returning failed");
    return response.data;
};
