import React, { useEffect, useState } from "react";
import { Book } from "../../models/Book";
import { getUser, makeReservation } from "../../fetches";
import { getDate } from "../../utils/dateUtils";
import { User } from "../../models/User";

interface BookRowProps {
    book: Book;
    onBookReserved: () => void;
}

export const BookRow = ({ book, onBookReserved = () => {} }: BookRowProps) => {
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

    const onReserve = async () => {
        await makeReservation(book.id, book.rowVersion);
        onBookReserved && onBookReserved();
    };

    return (
        <>
            {user?.username && (
                <tr key={book.id}>
                    <td>{book.title}</td>
                    <td>{book.author}</td>
                    <td>{book.date}</td>
                    <td>{book.publisher}</td>
                    <td>
                        {user?.isAdmin ? (
                            <>{getDate(book.reserved)}</>
                        ) : book.reserved ? (
                            <>{getDate(book.reserved)}</>
                        ) : !book.leased ? (
                            <button
                                className="btn btn-primary"
                                onClick={onReserve}
                            >
                                Reserve
                            </button>
                        ) : (
                            <></>
                        )}
                    </td>
                    <td>{getDate(book.leased)}</td>
                </tr>
            )}
        </>
    );
};
