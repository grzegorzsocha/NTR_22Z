# Zadanie 4 Biblioteka w React (w TypeScript)

- Wersja prezentowana prowadzącemu MUSI składać się z JEDNEGO serwera (czyli front-end i back-end serwowane przez JEDNĄ aplikację)
- Student decyduje czy ten serwer jest aplikacją w C# czy TypeScript
- Aplikacja przechowuje dane w bazie danych
- Każdy student tworzy własną bazę
- Zapis do bazy ma uwzględniać weryfikację nadpisywania - nie można nadpisać informacji, której użytkownik nie zobaczył (użytkownik ponosi odpowiedzialność tylko za to co widzi)
- Proszę napisać prosty system obsługi biblioteki
- Do formatowania proszę wykorzystać bibliotekę bootstrap
- Istnieje jeden specjalny (istniejący) użytkownik 'librarian'
- Użytkownik musi się zalogować do swojego konta, jak nie ma konta to może takie stworzyć w procesie rejestracji konta (bez potwierdzania mailowego)
- Zalogowany użytkownik który nie ma wypożyczonych książek może skasować swoje konto (librarian nie może skasować konta)
- Lista książek w bibliotece jest stała (książki można tylko wypożyczać i zwracać) i jest odczytywana z danych programu
- Użytkownik może wyszukiwać książki.
- Jeżeli książka nie jest wypożyczona lub zarezerwowana użytkownik może ją zarezerwować.
- Rezerwacja jest ważna do końca dnia następnego
- Użytkownik może wyświetlić swoje rezerwacje i niektóre (wskazane) kasować.
- Użytkownik librarian może wyświetlić listę wszystkich rezerwacji i wyszukaną rezerwację zmienić na wypożyczenie
- Użytkownik librarian może wyświetlić listę wszystkich wypożyczeń i wyszukane wypożyczenie zmienić informację, że książka jest dostępna
