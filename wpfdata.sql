-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hostiteľ: 127.0.0.1
-- Čas generovania: Št 04.Apr 2019, 10:54
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
  `user_year` int(11) NOT NULL,
  `user_numbers` int(11) NOT NULL,
  `description` text NOT NULL,
  `size` double NOT NULL,
  `price` double NOT NULL,
  `photo` varchar(500) NOT NULL,
  `created_at` date NOT NULL,
  `returned_at` date NOT NULL,
  `sold_at` date NOT NULL,
  `paid_at` date NOT NULL,
  `is_Stored` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `item`
--

INSERT INTO `item` (`id`, `user_year`, `user_numbers`, `description`, `size`, `price`, `photo`, `created_at`, `returned_at`, `sold_at`, `paid_at`, `is_Stored`) VALUES
(3, 19, 102, 'popis som popis', 25.2, 150.5, 'C:\\Users\\Daniel\\Desktop\\death.jpg', '2019-03-31', '0000-00-00', '2019-04-18', '0000-00-00', 1),
(4, 19, 102, 'lyze', 30, 170, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '2019-03-31', '0000-00-00', '0000-00-00', '0000-00-00', 1),
(5, 19, 102, 'Auto', 10000, 150000.5, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '2019-03-31', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(6, 19, 105, 'popis som popis', 25.2, 150.5, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '2019-03-31', '0000-00-00', '2019-04-10', '2019-04-22', 1),
(7, 19, 105, 'stol', 2, 10, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-03-31', '2019-04-02', '2019-04-10', '2019-04-09', 1),
(8, 19, 107, 'popis som popis', 25.2, 150.5, 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', '2019-03-31', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(9, 19, 107, 'pizza', 25, 1.5, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '2019-03-31', '2019-04-09', '0000-00-00', '0000-00-00', 0),
(10, 19, 105, 'Popis', 154, 78, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', 1),
(11, 19, 105, 'Venecky', 25, 150, 'C:\\\\Users\\\\Daniel\\\\Desktop\\\\unavailable.png', '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(12, 19, 105, 'item', 150, 250, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-04-01', '0000-00-00', '0000-00-00', '0000-00-00', 1),
(13, 19, 105, 'dalsiItem', 852, 101010, 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', '2019-04-01', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(14, 19, 105, 'nohy', 121, 0.1, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-04-01', '0000-00-00', '2019-04-03', '0000-00-00', 1),
(15, 19, 105, 'Palovpredmet', 190, 100, 'C:\\Users\\Daniel\\Desktop\\unavailable.png', '2019-04-01', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(16, 19, 102, 'FkinKatana', 152.2, 99.99, 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', '2019-04-02', '0000-00-00', '2019-04-10', '2019-04-10', 0),
(17, 19, 105, 'Michaliem', 45, 12, 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', '2019-04-02', '0000-00-00', '0000-00-00', '0000-00-00', 1),
(18, 19, 105, 'KubovTovar', 1, 9.99, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-04-02', '0000-00-00', '0000-00-00', '0000-00-00', 0),
(19, 19, 105, 'Text', 99, 1, '', '2019-04-04', '0000-00-00', '0000-00-00', '0000-00-00', 0);

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
(19, 102, 'Daniel', 'Hlavaty', 'Dubova 3 Sala 927 01', 918654714, '2019-03-31'),
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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

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
