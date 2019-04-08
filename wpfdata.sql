-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hostiteľ: 127.0.0.1
-- Čas generovania: Po 08.Apr 2019, 17:43
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
  `description` text NOT NULL,
  `size` text NOT NULL,
  `price` double NOT NULL,
  `photo` varchar(500) NOT NULL,
  `created_at` date NOT NULL,
  `returned_at` date NOT NULL,
  `sold_at` date NOT NULL,
  `paid_at` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `item`
--

INSERT INTO `item` (`id`, `name`, `user_year`, `user_numbers`, `description`, `size`, `price`, `photo`, `created_at`, `returned_at`, `sold_at`, `paid_at`) VALUES
(23, 'Lyze', 19, 102, 'cele su posahane', 'XL', 99.99, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-04-07', '2019-04-08', '0000-00-00', '0000-00-00'),
(24, 'Topankis', 19, 102, 'bagandze', '45', 59, 'C:\\Users\\Daniel\\Desktop\\unavailable.png', '2019-04-07', '2019-04-07', '2019-04-10', '2019-04-08'),
(25, 'Klavesnica', 19, 102, 'cervena', 'XXL', 99.99, 'C:\\Users\\Daniel\\Desktop\\death.jpg', '2019-04-07', '2019-04-08', '2019-04-08', '2019-04-08'),
(26, 'Knizka', 19, 102, 'Potrhana', '150 stranova', 13.99, 'C:\\Users\\Daniel\\Desktop\\available.jpg', '2019-04-07', '2019-04-08', '0000-00-00', '0000-00-00'),
(34, 'ble', 19, 102, 'bleeee', '1', 1, '', '2019-04-07', '2019-04-08', '2019-04-08', '2019-04-08'),
(35, 'Logis', 19, 102, 'Testis', 'Maximalis', 1, '', '2019-04-07', '0000-00-00', '2019-04-08', '0000-00-00'),
(36, 'Kacicka', 19, 105, 'farebna', 'mini', 0.99, 'C:\\Users\\Daniel\\Desktop\\5123n4V63EL._SX425_.jpg', '2019-04-08', '0000-00-00', '2019-04-08', '2019-04-08'),
(37, 'ToSomJa', 19, 107, 'zahaleny tienom', '175', 9999999, 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage83.png', '2019-04-08', '0000-00-00', '2019-04-08', '0000-00-00'),
(38, 'ToSomTiezJa', 19, 107, 'stale zahaleny tienom', '175', 9999, 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage84.png', '2019-04-08', '0000-00-00', '0000-00-00', '0000-00-00'),
(39, 'HmHm', 19, 107, 'ad', 'mhmh', 123, 'C://Users/Daniel/source/repos/materialApp/materialApp/imageres/webImage89.png', '2019-04-08', '0000-00-00', '2019-04-08', '0000-00-00');

--
-- Spúšťače `item`
--
DELIMITER $$
CREATE TRIGGER `on_insert` AFTER INSERT ON `item` FOR EACH ROW BEGIN
INSERT INTO log(i_id, price, item_created_at, item_returned_at, item_paid_at, item_sold_at, created_at, type) VALUES (new.id, new.price , new.created_at, new.returned_at, new.paid_at, new.sold_at, NOW(), 'INSERT');
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `on_update` AFTER UPDATE ON `item` FOR EACH ROW BEGIN
IF new.paid_at <> old.paid_at
THEN
INSERT INTO log (i_id, item_paid_at, type, created_at) VALUES (new.id, new.paid_at, 'UPDATE', NOW());
END IF;
IF new.sold_at <> old.sold_at
THEN
INSERT INTO log (i_id, item_sold_at, type, created_at) VALUES (new.id, new.sold_at, 'UPDATE', NOW());
END IF;
IF new.returned_at <> old.returned_at
THEN
INSERT INTO log (i_id, item_returned_at, type, created_at) VALUES (new.id, new.returned_at, 'UPDATE', NOW());
END IF;
IF new.price <> old.price
THEN
INSERT INTO log (i_id, price, type, created_at) VALUES (new.id, new.price, 'UPDATE', NOW());
END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Štruktúra tabuľky pre tabuľku `log`
--

CREATE TABLE `log` (
  `id` int(11) NOT NULL,
  `i_id` int(11) NOT NULL,
  `price` double NOT NULL,
  `item_created_at` date NOT NULL,
  `item_paid_at` date NOT NULL,
  `item_sold_at` date NOT NULL,
  `item_returned_at` date NOT NULL,
  `created_at` date NOT NULL,
  `type` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sťahujem dáta pre tabuľku `log`
--

INSERT INTO `log` (`id`, `i_id`, `price`, `item_created_at`, `item_paid_at`, `item_sold_at`, `item_returned_at`, `created_at`, `type`) VALUES
(7, 34, 1, '2019-04-07', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-07', 'INSERT'),
(8, 35, 1, '2019-04-07', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-07', 'INSERT'),
(12, 36, 0.99, '2019-04-08', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'INSERT'),
(13, 36, 0.99, '2019-04-08', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(15, 24, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(16, 23, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(17, 23, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(18, 23, 0, '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', '2019-04-08', 'UPDATE'),
(19, 34, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(20, 35, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(21, 25, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(22, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(23, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(24, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', '2019-04-08', 'UPDATE'),
(25, 26, 13.99, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(26, 26, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(27, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(28, 23, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(29, 23, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(30, 23, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(31, 23, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(32, 26, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(33, 26, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(34, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(35, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(36, 26, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(37, 26, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(38, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(39, 26, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(40, 25, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(41, 25, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(42, 25, 0, '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', '2019-04-08', 'UPDATE'),
(43, 25, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(44, 25, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(45, 34, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(46, 34, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(47, 34, 0, '0000-00-00', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(48, 34, 0, '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', '2019-04-08', 'UPDATE'),
(49, 34, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(50, 34, 0, '0000-00-00', '2019-04-08', '0000-00-00', '0000-00-00', '2019-04-08', 'UPDATE'),
(51, 37, 9999999, '2019-04-08', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'INSERT'),
(52, 38, 9999, '2019-04-08', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'INSERT'),
(53, 37, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE'),
(54, 39, 123, '2019-04-08', '0000-00-00', '0000-00-00', '0000-00-00', '2019-04-08', 'INSERT'),
(55, 39, 0, '0000-00-00', '0000-00-00', '2019-04-08', '0000-00-00', '2019-04-08', 'UPDATE');

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
(19, 102, 'Danilael', 'Hlavaty', 'Dubova 3 Sala 927 01', 2131323, '2019-03-31'),
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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT pre tabuľku `log`
--
ALTER TABLE `log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=56;

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
