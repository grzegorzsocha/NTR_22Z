## NTR22Z Socha Grzegorz

Repozytorium zawiera rozwiązania laboratoryjne z przedmiotu Narzędzia typu RAD w semestrze 22Z.

Komenda do uruchomienia bazy danych w kontenerze Dockera

`docker run --name postgres_db -p 5432:5432 -e POSTGRES_DB=library -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -d postgres`
