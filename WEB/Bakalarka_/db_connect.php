<?php
function OpenConn()
{
    $dbHost = "127.0.0.1";
    $dbUser = "root";
    $dbPass = "";
    $dbName = "bakalarkadb";

    $conn = new mysqli($dbHost, $dbUser, $dbPass, $dbName) or die ("Conn failed" . $conn->error);
    return $conn;
}

function CloseConn($conn)
{
    //$conn->close  TO DO not equivalent
    mysqli_close($conn);
}



