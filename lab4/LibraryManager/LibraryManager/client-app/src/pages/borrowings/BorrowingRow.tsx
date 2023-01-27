import React from "react";
import { Book } from "../../models/Book";
import { returnBook } from "../../fetches";

interface BorrowingRowProps {
    book: Book;
    onBookReturned?: () => void;
    onReservationCancelled?: () => void;
}

export const BorrowingRow = ({
    book,
    onBookReturned = () => {},
}: BorrowingRowProps) => {
    const onReturnBorrowings = async () => {
        await returnBook(book.id, book.rowVersion);
        onBookReturned && onBookReturned();
    };

    return (
        <tr key={book.id}>
            <td>{book.title}</td>
            <td>{book.author}</td>
            <td>{book.date}</td>
            <td>{book.publisher}</td>
            <td>
                <button
                    className="btn btn-primary"
                    onClick={onReturnBorrowings}
                >
                    Return book
                </button>
            </td>
        </tr>
    );
};
