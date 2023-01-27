export const getDate = (dateString: string | undefined | null) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
};