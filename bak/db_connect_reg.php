<?php
/**
 * Created by PhpStorm.
 * User: Daniel
 * Date: 12/28/2018
 * Time: 2:15 PM
 */

define('DB_SERVER', '127.0.0.1');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define("DB_NAME", 'bakalarkadb');

$link = mysqli_connect(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

if ($link === false){
    die ("ERROR: Could not connect. " . mysqli_connect_error());
}
