import { User } from './User';

export interface Book {
    id: number;
    author: string;
    title: string;
    date: number;
    publisher: string;
    user: User;
    reserved: string;
    leased: string;
    rowVersion: number;
}