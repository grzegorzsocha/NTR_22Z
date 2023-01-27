import React from "react";
import { useEffect, useState } from "react";
import { Book } from "../../models/Book";
import { getBooks } from "../../fetches";
import { BookRow } from "./BookRow";
import { SearchBar } from "../../components/SearchBar";

export default function Books() {
    const [books, setBooks] = useState<Array<Book>>([]);

    const loadBooks = async (searchText: string = "") => {
        const books = await getBooks(searchText);
        setBooks(books);
    };

    useEffect(() => {
        loadBooks();
    }, []);

    const onBookReserved = () => {
        loadBooks();
    };

    const searchBooks = (searchText: string) => {
        loadBooks(searchText);
    };

    const renderHeader = () => (
        <thead>
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Date</th>
                <th>Publisher</th>
                <th>Reserved</th>
                <th>Leased</th>
            </tr>
        </thead>
    );

    return (
        <div>
            <SearchBar onSearch={searchBooks} />
            <table className="table">
                {renderHeader()}
                <tbody>
                    {books?.map((book: Book) => (
                        <BookRow book={book} onBookReserved={onBookReserved} />
                    ))}
                </tbody>
            </table>
        </div>
    );
}
