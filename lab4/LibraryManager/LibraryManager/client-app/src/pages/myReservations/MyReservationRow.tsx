import React from "react";
import { Book } from "../../models/Book";
import { cancelReservation } from "../../fetches";

interface MyReservationRowProps {
    book: Book;
    onReservationCancelled?: () => void;
}

export const MyReservationRow = ({
    book,
    onReservationCancelled = () => {},
}: MyReservationRowProps) => {
    const onCancelReservations = async () => {
        await cancelReservation(book.id, book.rowVersion);
        onReservationCancelled && onReservationCancelled();
    };

    return (
        <tr key={book.id}>
            <td>{book.title}</td>
            <td>{book.author}</td>
            <td>{book.date}</td>
            <td>{book.publisher}</td>
            <td>
                <button
                    className="btn btn-warning"
                    onClick={onCancelReservations}
                >
                    Cancel reservation
                </button>
            </td>
        </tr>
    );
};
