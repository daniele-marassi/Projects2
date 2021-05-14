CREATE TABLE `ESTRAZIONI_ATTESTATI` (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Gruppo`	TEXT,
	`id_allievo`	INTEGER,
	`codfisc`	TEXT,
	`cognome`	TEXT,
	`nome`	TEXT,
	`nickname`	TEXT,
	`id_classe`	INTEGER,
	`NomeClasse`	TEXT,
	`data_esame`	TEXT,
	`filename`	TEXT,
	`pdf`	BLOB
);