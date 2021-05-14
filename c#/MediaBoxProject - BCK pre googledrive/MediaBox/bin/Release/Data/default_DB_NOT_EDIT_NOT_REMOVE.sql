CREATE TABLE IF NOT EXISTS `Configuration` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`WidthPanel`	INTEGER NOT NULL DEFAULT 0,
	`HeightPanel`	INTEGER NOT NULL DEFAULT 0,
	`CountElements`	INTEGER NOT NULL DEFAULT 0,
	`WidthMediaBox`	INTEGER NOT NULL DEFAULT 0,
	`HeightMediaBox`	INTEGER NOT NULL DEFAULT 0,
	`TopMediaBox`	INTEGER NOT NULL DEFAULT 0,
	`LeftMediaBox`	INTEGER NOT NULL DEFAULT 0,
	`BackGrondColorMain`	VARCHAR (11),
	`ForeColor`	VARCHAR (11),
	`BorderColorMedia`	VARCHAR (11),
	`DefaultPictureSize`	INTEGER NOT NULL DEFAULT 0,
	`WidthBorder`	INTEGER NOT NULL DEFAULT 0,
	`nPicturesOverPanel`	INTEGER NOT NULL DEFAULT 0,
	`PictureInterval`	DOUBLE NOT NULL DEFAULT 0,
	`VideoInterval`	DOUBLE NOT NULL DEFAULT 0,
	`WidthBorderMedia`	INTEGER NOT NULL DEFAULT 0,
	`AutoRunAnimationMedia`	INTEGER NOT NULL DEFAULT 0,
	`SelectedPlayListName`	VARCHAR (255),
	`VolumeAudio`	INTEGER NOT NULL DEFAULT 0,
	`MediaMovementSpeed`	INTEGER NOT NULL DEFAULT 0,
	`Demo`	INTEGER NOT NULL DEFAULT 0,
	`IDLanguage`	INTEGER DEFAULT 0
);

CREATE TABLE IF NOT EXISTS `Directory` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Type`	VARCHAR (255),
	`Path`	VARCHAR (4000)
);

CREATE TABLE IF NOT EXISTS `Language` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`CultureName`	VARCHAR (255)
);

CREATE TABLE IF NOT EXISTS `Translater` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Name`	VARCHAR (4000),
	`IDLanguage`	INTEGER DEFAULT 0,
	`Text`	VARCHAR (4000),
	FOREIGN KEY(IDLanguage) REFERENCES Language(ID)
);

CREATE TABLE IF NOT EXISTS `Catalog` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`PathFile`	VARCHAR (4000),
	`Title`	VARCHAR (4000),
	`DateCreated`	DATETIME DEFAULT '0000-00-00 00:00:00',
	`WidthMedia`	INTEGER DEFAULT 0,
	`HeightMedia`	INTEGER DEFAULT 0,
	`DurationInSeconds`	DOUBLE DEFAULT 0,
	`Type`	VARCHAR (255),
	`WidthThumbnail`	INTEGER DEFAULT 0,
	`HeightThumbnail`	INTEGER DEFAULT 0,
	`TopThumbnail`	INTEGER DEFAULT 0,
	`LeftThumbnail`	INTEGER DEFAULT 0,
	`Thumbnail`	BLOB
);

INSERT INTO `Language` (`CultureName`) VALUES ('it-IT');
INSERT INTO `Language` (`CultureName`) VALUES ('en-US');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('MediaBoxMenuBtn',1,'Apri/Chiudi Menu');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('MediaBoxMenuBtn',2,'Open/Close Menu');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseManageDirectoryBtn',1,'Gestione dei cataloghi multimediali');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseManageDirectoryBtn',2,'Management of multimedia catalogs');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('UpdateCatalogBtn',1,'Aggiorna cataloghi');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('UpdateCatalogBtn',2,'Update catalogs');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenClosePersonalizeColorBtn',1,'Personalizza i colori');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenClosePersonalizeColorBtn',2,'Personalize Colors');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseGeneralSettingsBtn',1,'Impostazioni generali');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseGeneralSettingsBtn',2,'General settings');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('StartStopAnimateBtn',1,'Avvia/Ferma Animazione');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('StartStopAnimateBtn',2,'Start/Stop Animate');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseMusicBtn',1,'Gestione della musica');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseMusicBtn',2,'Management music');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseInfoBtn',1,'Info');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('OpenCloseInfoBtn',2,'Info');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeBackGrondColorMain',1,'Colore di sfondo principale');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeBackGrondColorMain',2,'BackGrond Color Main');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeForeColor',1,'Colore anteriore');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeForeColor',2,'Fore Color');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeBorderColorMedia',1,'Colore del bordo Media');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeBorderColorMedia',2,'Border Color Media');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeDefault',1,'Seleziona quello che vuoi modificare');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ColorTypeDefault',2,'Select what you want to modify');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('RedLbl',1,'Rosso');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('RedLbl',2,'Red');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('GreenLbl',1,'Verde');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('GreenLbl',2,'Green');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('BlueLbl',1,'Blu');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('BlueLbl',2,'Blue');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PictureIntervalLbl',1,'Attesa Immaigini (secondi)');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PictureIntervalLbl',2,'Picture Wait (sedonds)');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('VideoIntervalLbl',1,'Attesa Video (secondi)');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('VideoIntervalLbl',2,'Video Wait (sedonds)');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('WidthBorderMediaLbl',1,'Dim. del bordo Media');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('WidthBorderMediaLbl',2,'Width Border Media');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('MediaMovementSpeedLbl',1,'Velocita` di movimento');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('MediaMovementSpeedLbl',2,'Media movement speed');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AutoRunAnimationMediaLbl',1,'Animazione auto');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AutoRunAnimationMediaLbl',2,'Auto animation');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AutoRunAnimationMediaBtn',1,'Attivare per eseguire in automatico l`animazione al prossimo avvio');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AutoRunAnimationMediaBtn',2,'Activate to automatically run the animation at the next start');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('UpdateCatalogMessage',1,'Aggiornamento catalogo, sei Sicuro ?');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('UpdateCatalogMessage',2,'Update catalog, are you sure ?');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DurationLblDemo',1,'La durata della presentazione e` di circa 5 minuti');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DurationLblDemo',2,'The duration of the presentation is about 5 minutes');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('LanguageitITBtnDemo',1,'Selezionare la lingua desiderata');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('LanguageitITBtnDemo',2,'Select the desired language');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryOpenBtnDemo',1,'Selezionare la cartella desiderata');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryOpenBtnDemo',2,'Select the desired folder');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryTxtDemo',1,'O digitare qui il percorso della cartella desiderata');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryTxtDemo',2,'Or type the path of the desired folder here');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryListDemo',1,'Selezionare la voce che si vuole eliminare');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('DirectoryListDemo',2,'Select the item you want to delete');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ChoiceColorCmbDemo',1,'Selezionare la sezione dove si vuole cambiare colore');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('ChoiceColorCmbDemo',2,'Select the section where you want to change color');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PictureIntervalTxtDemo',1,'Digitare i secondi di attesa tra la visualizzazione di un elemento multimediale e quello successivo');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PictureIntervalTxtDemo',2,'Type the seconds to wait between the display of a media element and the next one');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('VideoIntervalTxtDemo',1,'Digitare i secondi di attesa tra la visualizzazione di un elemento multimediale e quello successivo');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('VideoIntervalTxtDemo',2,'Type the seconds to wait between the display of a media element and the next one');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('WidthBorderMediaTxtDemo',1,'Digitare la dimensione del bordo dell`elemento multimediale');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('WidthBorderMediaTxtDemo',2,'Type the border size of the media item');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('CloseMediaBoxBtnDemo',1,'Per chiudere l`applicazione o uscire dalla Demo');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('CloseMediaBoxBtnDemo',2,'To close the application or exit from Demo');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PlayListMusicCmbDemo',1,'Puoi selezionare una playlist esistente in "Windows Media Player"');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('PlayListMusicCmbDemo',2,'You can select an existing playlist in "Windows Media Player"');

INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('FinishDemo',1,'Chiudere l`applicazione per uscire dalla Demo');
INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('FinishDemo',2,'Close the application to exit from Demo');

--INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AAA',1,'111');
--INSERT INTO `Translater` (`Name`,`IDLanguage`,`Text`) VALUES ('AAA',2,'222');

INSERT INTO `Configuration` (`WidthPanel`,`HeightPanel`, `CountElements`,`BackGrondColorMain`,`ForeColor`,`BorderColorMedia`,`DefaultPictureSize`,`WidthBorder`,`nPicturesOverPanel`,`PictureInterval`,`VideoInterval`,`WidthBorderMedia`,`AutoRunAnimationMedia`,`VolumeAudio`,`Demo`,`IDLanguage`,`MediaMovementSpeed`) VALUES (100,100,0,'250,250,250','30,30,30','255,255,255',100,4,2,3,6,5,0,10,0,1,33);