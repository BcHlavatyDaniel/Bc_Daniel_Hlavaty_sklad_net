-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hostiteľ: 127.0.0.1
-- Čas generovania: Po 25.Mar 2019, 03:01
-- Verzia serveru: 10.1.28-MariaDB
-- Verzia PHP: 7.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Databáza: `bakalarkadb`
--

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `logintable`
--

CREATE TABLE `logintable` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `logintable`
--

INSERT INTO `logintable` (`id`, `username`, `password`, `created_at`) VALUES
(1, 'Kiwish', '$2y$10$DnBAp7xC3LzUNa1686TD/.ctPeolNcNq8wllAv6plqLbGEZq05TPq', '2018-12-29 16:26:03'),
(2, 'Eliska', '$2y$10$v.O273R2i0JJB/ocDt2hTu4kivMz6kCX.Aek.2UaXdjsLBO2dUKWC', '2018-12-29 16:34:48'),
(3, 'Ulalala', '$2y$10$R3IeG7KgotQkiCGszJAl9.X2zMtPD3g1rylQSkAJmvZhIVyAzRf72', '2018-12-29 16:37:04'),
(4, 'root', '$2y$10$pf.CGBUQPe.jQ1qXHvail.GmsrnJcoqV0BsfK.W//L.uFo4UafMiy', '2019-01-04 17:07:08'),
(5, 'NewName', '$2y$10$qKNqhHKq8TTzpbECMQxexeXRmKumiTGfcIWesPVhtVPIlnSzq2WS6', '2019-01-08 16:38:29'),
(6, 'Novyhrac', '$2y$10$a09TBPfey/3P6Kf5bWBY2eiU6TYWnG6R0iWqCCWfdZoqJTHNfDFMC', '2019-01-08 17:54:33'),
(7, 'Najnovskie', '$2y$10$LtMC09NLaE35jJlo0JwvyOirpEGYCkzSaMN4T9tnWE.agUDWzBIk.', '2019-01-09 01:24:12'),
(8, 'SkusamJs', '$2y$10$R0XaZF.4pwdBW3iE1.VNzeFZtB6BaAt/Wm.Ng7FAirIKruX3dfJdu', '2019-01-09 03:39:24');

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `main`
--

CREATE TABLE `main` (
  `FirstName` varchar(50) NOT NULL,
  `SecondName` varchar(50) NOT NULL,
  `Stat` double DEFAULT NULL,
  `CheckedOut` tinyint(1) DEFAULT NULL,
  `Type` tinyint(4) DEFAULT NULL,
  `StampAdded` date DEFAULT NULL,
  `StampToLeave` date DEFAULT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `main`
--

INSERT INTO `main` (`FirstName`, `SecondName`, `Stat`, `CheckedOut`, `Type`, `StampAdded`, `StampToLeave`, `id`) VALUES
('Zlatokop', 'Hlavaty', 3, 0, 2, '2019-05-05', '2019-06-10', 17),
('Alchemista', 'Hlavaty', 3, 0, 3, '2019-05-05', '2019-06-10', 18),
('Elena', 'Hlavaty', 3, 0, 2, '2019-05-05', '2019-06-10', 22),
('Eliska', 'Hlavaty', 3, 0, 3, '2019-05-05', '2019-06-10', 23),
('Marcela', 'Hlavaty', 3, 0, 1, '2019-05-05', '2019-06-10', 24),
('Posledna', 'Hlavaty', 3, 0, 3, '2019-05-05', '2019-06-10', 26),
('Vzumm', 'Zmena', 451, 0, 2, '2019-02-04', '2019-03-05', 28),
('DalsiPokus', 'Stastitiky', 1248, 1, 2, '2019-02-04', '2019-03-05', 29),
('DalsiPokus', 'Stastitiky', 1248, 1, 3, '2019-02-04', '2019-03-05', 30),
('Pridany', 'Strankou', 45, 1, 2, '2019-02-04', '2019-03-04', 32),
('Novucicke', 'Mienko', 25, 1, 2, '2019-05-05', '2019-04-09', 33),
('Novucicke', 'Mienko', 25, 1, 2, '2019-05-05', '2019-04-09', 34),
('Najnvosie', 'Meninononi', 456, 1, 1, '2019-08-08', '2019-09-09', 35),
('asdasd', 'asdaer', 87, 0, 1, '2019-08-08', '2019-10-10', 36),
('Bliatt', 'bliatatat', 4568, 1, 2, '2019-07-07', '2019-09-09', 37),
('Prvemeno', 'SecondName', 25, 0, 2, '2019-03-12', '2019-03-29', 46),
('asd', 'yu', 44, 0, 2, '2019-03-14', '2019-04-05', 52);

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `osoba_zak`
--

CREATE TABLE `osoba_zak` (
  `id` int(11) NOT NULL,
  `prve_meno` varchar(255) NOT NULL,
  `druhe_meno` varchar(255) NOT NULL,
  `telefonne_cislo` int(11) NOT NULL,
  `adresa` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `osoba_zak`
--

INSERT INTO `osoba_zak` (`id`, `prve_meno`, `druhe_meno`, `telefonne_cislo`, `adresa`) VALUES
(1, 'Daniel', 'Hladaty', 918654714, 'Dolna 10 Sala'),
(2, 'Tomas', 'Oni', 910654741, 'Zapadakov 1 Velke Lopusany'),
(3, 'Pridany', 'Modalon', 0, 'Bubulakov 14'),
(4, 'Zlatokop', 'Zmizat', 918605682, 'Dolná 10, 15'),
(5, 'NoveMeno', 'Statistiky', 986665455, 'Zilina Skolacka 52'),
(6, 'DalsiPokus', 'Hlavaty', 0, 'Dalsia dresa 145'),
(7, 'DalsiPokus', 'Hlavaty', 0, 'Dalsia dresa 145'),
(8, 'Zlatokop', 'Zmizat', 918605682, 'Dolná 10, 15'),
(9, 'NoveMeno', 'Hlavaty', 918605682, 'Dolná 10, 15');

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `polozka`
--

CREATE TABLE `polozka` (
  `id` int(11) NOT NULL,
  `nazov` varchar(500) NOT NULL,
  `velkost` double NOT NULL,
  `foto` varchar(500) NOT NULL,
  `stav` tinyint(1) NOT NULL,
  `prichod` date NOT NULL,
  `odchod` date NOT NULL,
  `cena` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `polozka`
--

INSERT INTO `polozka` (`id`, `nazov`, `velkost`, `foto`, `stav`, `prichod`, `odchod`, `cena`) VALUES
(1, 'lyze', 157, '/images/Pridat.png', 0, '2019-03-24', '2019-03-24', 99.99),
(2, 'topanky', 157, '/images/Zmazat.png', 0, '2019-03-24', '2019-03-24', 99.99);

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `statistics`
--

CREATE TABLE `statistics` (
  `id` int(11) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `SecondName` varchar(50) NOT NULL,
  `VisitCount` int(11) NOT NULL,
  `StatTypeFirst` double NOT NULL,
  `StatTypeSecond` double NOT NULL,
  `StatTypeThird` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `statistics`
--

INSERT INTO `statistics` (`id`, `FirstName`, `SecondName`, `VisitCount`, `StatTypeFirst`, `StatTypeSecond`, `StatTypeThird`) VALUES
(1, 'DalsiPokus', 'Stastitiky', 0, 0, 1699, 1248),
(2, 'Absolutne', 'Najnovsi', 0, 1, 0, 0),
(3, 'Pridany', 'Strankou', 0, 0, 45, 0),
(4, 'Novucicke', 'Mienko', 0, 0, 50, 0),
(5, 'Najnvosie', 'Meninononi', 0, 456, 0, 0),
(6, 'asdasd', 'asdaer', 0, 87, 0, 0),
(7, 'Bliatt', 'bliatatat', 0, 0, 4568, 0);

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `tovar`
--

CREATE TABLE `tovar` (
  `id` int(11) NOT NULL,
  `nazov` varchar(500) NOT NULL,
  `velkost` double NOT NULL,
  `cena` double NOT NULL,
  `foto` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `tovar`
--

INSERT INTO `tovar` (`id`, `nazov`, `velkost`, `cena`, `foto`) VALUES
(1, 'lyze', 15.2, 156.5, '/images/databaza.png'),
(2, 'topanky', 36, 99.99, '/images/Zmeny.png'),
(3, 'auto', 1000, 15600.5, '/images/Pridat.png');

--
-- Kľúče pre exportované tabuľky
--

--
-- Indexy pre tabuľku `logintable`
--
ALTER TABLE `logintable`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `usernm` (`username`);

--
-- Indexy pre tabuľku `main`
--
ALTER TABLE `main`
  ADD PRIMARY KEY (`id`);

--
-- Indexy pre tabuľku `osoba_zak`
--
ALTER TABLE `osoba_zak`
  ADD PRIMARY KEY (`id`);

--
-- Indexy pre tabuľku `polozka`
--
ALTER TABLE `polozka`
  ADD PRIMARY KEY (`id`);

--
-- Indexy pre tabuľku `statistics`
--
ALTER TABLE `statistics`
  ADD PRIMARY KEY (`id`);

--
-- Indexy pre tabuľku `tovar`
--
ALTER TABLE `tovar`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT pre exportované tabuľky
--

--
-- AUTO_INCREMENT pre tabuľku `logintable`
--
ALTER TABLE `logintable`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT pre tabuľku `main`
--
ALTER TABLE `main`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT pre tabuľku `osoba_zak`
--
ALTER TABLE `osoba_zak`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT pre tabuľku `polozka`
--
ALTER TABLE `polozka`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pre tabuľku `statistics`
--
ALTER TABLE `statistics`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pre tabuľku `tovar`
--
ALTER TABLE `tovar`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
