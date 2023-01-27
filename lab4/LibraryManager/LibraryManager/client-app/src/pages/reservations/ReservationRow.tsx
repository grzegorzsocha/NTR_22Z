import React from "react";
import { Book } from "../../models/Book";
import { borrowBook } from "../../fetches";

interface ReservationRowProps {
    book: Book;
    onBookBorrowed?: () => void;
}

export const ReservationRow = ({
    book,
    onBookBorrowed = () => {},
}: ReservationRowProps) => {
    const onBorrowBook = async () => {
        await borrowBook(book.id, book.rowVersion);
        onBookBorrowed && onBookBorrowed();
    };

    return (
        <tr key={book.id}>
            <td>{book.title}</td>
            <td>{book.author}</td>
            <td>{book.date}</td>
            <td>{book.publisher}</td>
            <td>
                <button className="btn btn-primary" onClick={onBorrowBook}>
                    Borrow book
                </button>
            </td>
        </tr>
    );
};
