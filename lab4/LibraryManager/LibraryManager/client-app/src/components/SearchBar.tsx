import React, { useState } from "react";

interface SearchBarProps {
    onSearch: (searchText: string) => void;
}

export const SearchBar = ({ onSearch }: SearchBarProps) => {
    const [searchText, setSearchText] = useState<string>("");

    return (
        <div className="search-bar">
            <input onChange={(e) => setSearchText(e.target.value)} />
            <button
                className="btn btn-primary"
                onClick={() => onSearch(searchText)}
            >
                Search
            </button>
        </div>
    );
};
