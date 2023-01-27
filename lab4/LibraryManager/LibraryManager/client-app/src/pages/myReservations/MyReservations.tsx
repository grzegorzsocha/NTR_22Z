import React from "react";
import { useEffect, useState } from "react";
import { Book } from "../../models/Book";
import { getReservations } from "../../fetches";
import { MyReservationRow } from "./MyReservationRow";
import { SearchBar } from "../../components/SearchBar";

export default function MyReservations() {
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
                <th>Cancel</th>
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
                        <MyReservationRow
                            book={book}
                            onReservationCancelled={loadReservations}
                        />
                    ))}
                </tbody>
            </table>
        </div>
    );
}
