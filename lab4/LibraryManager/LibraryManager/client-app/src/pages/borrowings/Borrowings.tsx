import React from "react";
import { useEffect, useState } from "react";
import { Book } from "../../models/Book";
import { getBorrowings } from "../../fetches";
import { BorrowingRow } from "./BorrowingRow";
import { SearchBar } from "../../components/SearchBar";

export default function Borrowings() {
    const [borrowings, setBorrowings] = useState<Array<Book>>([]);

    const loadBorrowings = async (searchText: string = "") => {
        const borrowings = await getBorrowings(searchText);
        setBorrowings(borrowings);
    };

    useEffect(() => {
        loadBorrowings();
    }, []);

    const searchBorrowings = (searchText: string) => {
        loadBorrowings(searchText);
    };

    const renderHeader = () => (
        <thead>
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Date</th>
                <th>Publisher</th>
                <th>Return</th>
            </tr>
        </thead>
    );

    return (
        <div>
            <SearchBar onSearch={searchBorrowings} />
            <table className="table">
                {renderHeader()}
                <tbody>
                    {borrowings?.map((book: Book) => (
                        <BorrowingRow
                            book={book}
                            onBookReturned={loadBorrowings}
                            onReservationCancelled={loadBorrowings}
                        />
                    ))}
                </tbody>
            </table>
        </div>
    );
}
