import React from "react";
import { useEffect, useState } from "react";
import { Book } from "../../models/Book";
import { getReservations } from "../../fetches";
import { ReservationRow } from "./ReservationRow";
import { SearchBar } from "../../components/SearchBar";

export default function Reservations() {
    const [reservations, setReservations] = useState<Array<Book>>([]);

    const loadReservations = async (searchText: string = "") => {
        const reservations = await getReservations(searchText);
        setReservations(reservations);
    };

    useEffect(() => {
        loadReservations();
    }, []);

    const searchReservations = (searchText: string) => {
        loadReservations(searchText);
    };

    const renderHeader = () => (
        <thead>
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Date</th>
                <th>Publisher</th>
                <th>Borrow</th>
            </tr>
        </thead>
    );

    return (
        <div>
            <SearchBar onSearch={searchReservations} />
            <table className="table">
                {renderHeader()}
                <tbody>
                    {reservations?.map((book: Book) => (
                        <ReservationRow
                            book={book}
                            onBookBorrowed={loadReservations}
                        />
                    ))}
                </tbody>
            </table>
        </div>
    );
}
