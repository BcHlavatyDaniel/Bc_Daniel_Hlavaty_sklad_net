-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hostiteľ: 127.0.0.1
-- Čas generovania: Út 23.Apr 2019, 15:55
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
-- Databáza: `wpfdata`
--

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `item`
--

CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `name` varchar(500) NOT NULL,
  `user_year` int(11) NOT NULL,
  `user_numbers` int(11) NOT NULL,
  `size` text NOT NULL,
  `price` double NOT NULL,
  `description` text,
  `photo` varchar(500) NOT NULL,
  `stav` int(11) NOT NULL,
  `archived` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `item`
--

INSERT INTO `item` (`id`, `name`, `user_year`, `user_numbers`, `size`, `price`, `description`, `photo`, `stav`, `archived`) VALUES
(61, 'Klavesnica', 19, 101, '150cm', 199.99, 'Oskreta', 'webImage2.png', 5, 0),
(62, 'Kniha', 19, 101, '1345stran', 19.99, 'potrhana', 'webImage3.png', 1, 0),
(63, 'Kniha', 19, 101, 'mala', 5561, 'asdasd', 'unavailable.png', 0, 0),
(64, 'Item', 19, 102, 'maly', 5465, 'dasda', 'webImage4.png', 0, 0),
(65, 'Itemis', 19, 102, 'mensi', 545, 'asdas', '5123n4V63EL._SX425_.jpg', 4, 0),
(66, 'Dalsi', 19, 102, 'velky', 9.999, 'l;kjnjkb jhvjcfj', 'webImage5.png', 0, 0),
(67, 'Novsi', 19, 102, '46', 456, 'dfsggj asd aa dasd', 'webImage6.png', 0, 0),
(68, 'Klavesnica', 19, 101, '56', 66121, 'asfrfgfd', 'webImage7.png', 0, 0),
(69, 'Klavesnica', 19, 101, 'asd', 46412, 'asdz', 'webImage8.png', 0, 0);

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `log`
--

CREATE TABLE `log` (
  `id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `user_id` varchar(500) NOT NULL,
  `type` int(255) NOT NULL,
  `change_text` text NOT NULL,
  `time` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `log`
--

INSERT INTO `log` (`id`, `item_id`, `user_id`, `type`, `change_text`, `time`) VALUES
(292, 62, '19-101', 1, 'Tovar predany kartou', '2019-04-22 01:46:10'),
(293, 61, '19-101', 1, 'Tovar predany hotovostou', '2019-04-22 01:46:14'),
(294, 61, '19-101', 1, 'Tovar zaplateny hotovostou', '2019-04-22 01:46:14'),
(295, 62, '19-101', 3, 'Zmena obrazku.', '2019-04-22 01:48:38'),
(296, 65, '19-102', 1, 'Tovar predany kartou', '2019-04-22 01:51:30'),
(297, 65, '19-102', 1, 'Tovar zaplateny kartou', '2019-04-22 01:51:30');

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `user`
--

CREATE TABLE `user` (
  `year` int(11) NOT NULL,
  `_numbers` int(11) NOT NULL,
  `first_name` varchar(500) NOT NULL,
  `second_name` varchar(500) NOT NULL,
  `address` varchar(500) NOT NULL,
  `telephone` int(11) NOT NULL,
  `created_at` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `user`
--

INSERT INTO `user` (`year`, `_numbers`, `first_name`, `second_name`, `address`, `telephone`, `created_at`) VALUES
(19, 100, 'Daniel', 'Hlavaty', 'Dubova 3 Sala 92701', 910654714, '2019-04-22'),
(19, 101, 'Tomas', 'Bohunicky', 'Tomasikova 16 Zilina', 910852413, '2019-04-22'),
(19, 102, 'Jano', 'Cerveny', 'Ulica 1 Mesta PSC', 915486125, '2019-04-22');

--
-- Kľúče pre exportované tabuľky
--

--
-- Indexy pre tabuľku `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_year` (`user_year`,`user_numbers`);

--
-- Indexy pre tabuľku `log`
--
ALTER TABLE `log`
  ADD PRIMARY KEY (`id`);

--
-- Indexy pre tabuľku `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`year`,`_numbers`);

--
-- AUTO_INCREMENT pre exportované tabuľky
--

--
-- AUTO_INCREMENT pre tabuľku `item`
--
ALTER TABLE `item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=70;

--
-- AUTO_INCREMENT pre tabuľku `log`
--
ALTER TABLE `log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=298;

--
-- Obmedzenie pre exportované tabuľky
--

--
-- Obmedzenie pre tabuľku `item`
--
ALTER TABLE `item`
  ADD CONSTRAINT `item_ibfk_1` FOREIGN KEY (`user_year`,`user_numbers`) REFERENCES `user` (`year`, `_numbers`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
