-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hostiteľ: 127.0.0.1
-- Čas generovania: Št 18.Apr 2019, 02:54
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
(23, 'Lyze', 19, 102, 'XK', 99.99, 'uz popis', 'C:\\Users\\Daniel\\Desktop\\available.jpg', 0, 0),
(24, 'Topankis', 19, 102, '45', 59, 'Posanahne Topanky 24', 'C:\\Users\\Daniel\\Desktop\\unavailable.png', 0, 1),
(25, 'Klavesnica', 19, 102, 'XXL', 999.99, 'cerveniak', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage100.png', 1, 0),
(26, 'Knizka', 19, 102, '150 stranova', 13.99, 'Potrhana', 'C:\\Users\\Daniel\\Desktop\\available.jpg', 4, 1),
(34, 'blife', 19, 102, '15', 1, 'bleeee mbhj', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage105.png', 1, 0),
(35, 'Logis', 19, 102, 'Maximalis', 1, 'asdTestisisisis', '', 5, 0),
(36, 'Kacicka', 19, 105, 'mini', 0.99, '', 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', 1, 0),
(37, 'ToSomJa', 19, 107, '175', 9999999, 'zahaleny tienom', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage83.png', 2, 1),
(38, 'ToSomTiezJa', 19, 107, '175', 9989, 'som odhaleny lost focusom', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage84.png', 3, 0),
(39, 'HmHm', 19, 107, 'mhmh', 123, 'ad', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage89.png', 4, 1),
(40, 'Testis', 19, 107, 'updatis', 21, '45ble', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage93.png', 0, 0),
(41, 'Dalsifix', 19, 102, 'Mensi', 10, 'funguj jo', '', 4, 0),
(42, 'vdvffdsvfdss', 19, 107, 's', 514, 'dfvdvs', '', 5, 0),
(43, 'Dlais', 19, 102, 'velksot', 123, '123kjnk12', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage94.png', 3, 0),
(44, 'Michal', 19, 102, '125', 12453, 'poasdpj', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage95.png', 4, 0),
(45, 'Item', 19, 101, 'velky', 2115, 'hjhbjkasdasd', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage96.png', 0, 1),
(46, 'Dalsi', 19, 101, 'mali', 12, 'FDDCZXcx', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage97.png', 1, 0),
(47, 'Meno', 19, 100, '45310', 64.125, 'asd', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage98.png', 5, 0),
(48, 'Tricko', 19, 105, 's', 19, 'potrhane', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage101.png', 3, 0),
(49, 'Nohavice', 19, 105, '19.2', 19, 'asdads', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage102.png', 4, 0),
(50, 'Triko', 19, 102, 'XXX', 995649, 'jasjhgyan', 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage104.png', 0, 1),
(51, 'r3e432e22', 19, 102, '25', 25, 'yhthyry', '', 1, 0),
(52, '4r4r', 19, 102, 'yj', 52, 'iukuy', '', 5, 0);

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
(220, 25, '19-102', 0, 'Zmena ceny tovaru z XXL na 999.99', '2019-04-17 14:08:22'),
(221, 26, '19-102', 0, 'Zmena ceny tovaru z 150 stranova na 13.99', '2019-04-17 14:14:44'),
(222, 26, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:18:58'),
(223, 34, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:21:13'),
(224, 25, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:23:21'),
(225, 34, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:24:56'),
(226, 34, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:26:43'),
(227, 34, '19-102', 2, 'Tovar odarchivovany', '2019-04-17 14:27:00'),
(228, 34, '19-102', 2, 'Tovar archivovany', '2019-04-17 14:27:36'),
(229, 34, '19-102', 2, 'Tovar odarchivovany', '2019-04-17 14:27:53'),
(230, 34, '19-102', 1, 'Zmena stavu z 4 na 1', '2019-04-17 14:27:54'),
(231, 34, '19-102', 1, 'Zmena stavu z 1 na 2', '2019-04-17 12:00:00'),
(232, 34, '19-102', 1, 'Zmena stavu z 2 na 3', '2019-04-17 14:28:22'),
(233, 34, '19-102', 3, 'Zmena nazvu tovaru z ble na blife', '2019-04-17 14:28:23'),
(234, 34, '19-102', 3, 'Zmena velkosti tovaru z 1 na 15', '2019-04-17 14:28:23'),
(235, 34, '19-102', 1, 'Zmena stavu z 3 na 1', '2019-04-17 14:28:52'),
(236, 34, '19-102', 1, 'Tovar zaplateny kartou', '2019-04-17 14:54:05'),
(237, 41, '19-102', 1, 'Tovar zaplateny kartou', '2019-04-17 14:54:08'),
(238, 35, '19-102', 1, 'Zmena stavu z 5 na 0', '2019-04-17 14:54:12'),
(239, 35, '19-102', 1, 'Tovar predany hotovostou', '2019-04-17 14:54:21'),
(240, 35, '19-102', 1, 'Tovar zaplateny hotovostou', '2019-04-17 14:54:22'),
(241, 52, '19-102', 1, 'Tovar zaplateny hotovostou', '2019-04-17 14:54:43'),
(242, 24, '19-102', 1, 'Zmena stavu z zaplateny hotovostou na nepredany', '2019-04-17 15:01:32'),
(243, 25, '19-102', 1, 'Zmena stavu z zaplateny kartou na predany hotovostou', '2019-04-17 15:01:38'),
(244, 26, '19-102', 1, 'Zmena stavu z vrateny na zaplateny hotovostou', '2019-04-17 15:01:45'),
(245, 34, '19-102', 1, 'Zmena stavu z zaplateny hotovostou na predany hotovostou', '2019-04-17 15:01:52'),
(246, 24, '19-102', 2, 'Tovar archivovany', '2019-04-17 17:12:06'),
(247, 26, '19-102', 2, 'Tovar archivovany', '2019-04-17 17:12:12'),
(248, 25, '19-102', 2, 'Tovar archivovany', '2019-04-17 17:12:23'),
(249, 25, '19-102', 2, 'Tovar odarchivovany', '2019-04-17 21:48:12'),
(250, 25, '19-102', 1, 'Zmena stavu z predany hotovostou na nepredany', '2019-04-18 00:53:41'),
(251, 25, '19-102', 1, 'Tovar predany hotovostou', '2019-04-18 01:19:30'),
(252, 25, '19-102', 1, 'Tovar zaplateny hotovostou', '2019-04-18 01:19:31'),
(253, 25, '19-102', 1, 'Zmena stavu z zaplateny kartou na predany hotovostou', '2019-04-18 01:19:44'),
(254, 35, '19-102', 2, 'Tovar archivovany', '2019-04-18 02:12:05'),
(255, 35, '19-102', 2, 'Tovar odarchivovany', '2019-04-18 02:12:13'),
(256, 23, '19-102', 1, 'Zmena stavu z zaplateny hotovostou na nepredany', '2019-04-18 02:46:33'),
(257, 23, '19-102', 1, 'Tovar predany kartou', '2019-04-18 02:46:46'),
(258, 23, '19-102', 1, 'Tovar zaplateny kartou', '2019-04-18 02:46:47'),
(259, 23, '19-102', 1, 'Zmena stavu z zaplateny hotovostou na nepredany', '2019-04-18 02:46:52');

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
(19, 100, 'Bonifac', 'rajcina', 'Rajec vo svete bla bla asdoa 4', 54556121, '2019-04-10'),
(19, 101, 'Hugh', 'Mung5us', 'Hentak tam sem podtym 10 v pravo', 542121531, '2019-04-10'),
(19, 102, 'Danilamasdkel', 'Hlavaty', 'Dubova 3 Sala 927 01', 213541323, '2019-03-31'),
(19, 105, 'Martin', 'Zeleny', 'Bratislava Ulica 14 927', 996665888, '2019-03-31'),
(19, 107, 'Palo', 'Cerveny', 'Bratislavska 15 Bratislava 45785', 905124587, '2019-03-31');

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT pre tabuľku `log`
--
ALTER TABLE `log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=260;

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
