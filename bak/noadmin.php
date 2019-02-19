<?php
require_once "db_connect.php";

$action = $_POST['key'];

switch ($action) {
    case "check":
        makeCheck();
        break;
    case "getall":
        makeGetAll();
        break;
    case "find":
        makeFind();
        break;
}

function makeCheck() {
    session_start();

    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
        exit("locationlogin");
    } else {
        exit($_SESSION["username"]);
    }
}

function makeGetAll() {

    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM main LIMIT $start, $limit";
    $link = OpenConn();
    $show = mysqli_query($link, $query);
    $count = 0;
    if($show->num_rows > 0) {
        $response = "";
        while ($row = mysqli_fetch_array($show)) {
            $count++;
            $response .= '
        <tr>
            <th>' . $row["id"] . '</th>          
            <td>' . $row["FirstName"] . '</td>
            <td>' . $row["SecondName"] . '</td>
            <td>' . $row["Stat"] . '</td>
            <td>' . $row["StampAdded"] . '</td>
            <td>' . $row["StampToLeave"] . '</td>
            <td>' . $row["CheckedOut"] . '</td>
            <td>' . $row["Type"] . '</td>
       </tr>
        ';
        }
        CloseConn($link);
        exit($response);
    } else {
        CloseConn($link);
        exit('done');
    }
}

function makeFind() {
    $number = $_POST['num'];
    $fname_err = 0;
    $sname_err = 0;
    $firstname = $secondname = "";
    $err = "";
    $conn = OpenConn();

    if ($number == 1) {

        if (!isset($_POST['firstname'])) {
            $fname_err = 1;
        } else if (empty(trim($_POST["firstname"]))) {
            $fname_err = 1;
        } else {
            $firstname = $_POST['firstname'];
        }

        if (!isset($_POST['secondname'])) {
            $sname_err = 1;
        } else if (empty(trim($_POST["secondname"]))) {
            $sname_err = 1;
        } else {
            $secondname = $_POST['secondname'];
        }

        if($fname_err == 0 && $sname_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE FirstName = '$firstname' AND SecondName = '$secondname'");
            //TO DO ked som sa jej este nedotkol taak co?
            $err="done";
        } else if ($fname_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE FirstName = '$firstname'");
            $err="done";
        } else if ($sname_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE SecondName = '$secondname'");
        } else {
            $err="missing";
        }

    } else if ($number == 2) {
        $boxftype = $boxstype = $boxttype = 0;

        if($_POST['typef'] == "1") {
            $boxftype = 1;
        }
        if ($_POST['types'] == "1") {
            $boxstype = 1;
        }
        if ($_POST['typet'] == "1") {
            $boxttype = 1;
        }

        if (($boxftype == 1 && $boxstype == 1 && $boxttype == 1) ||($boxftype == 0 && $boxstype == 0 && $boxttype == 0)) {
            $result = mysqli_query($conn, "SELECT * FROM main");
            $err="done";
        } else if ($boxftype == 1 && $boxttype == 1){
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype' OR Type='3'");
            $err="done";
        } else if ($boxftype == 1 && $boxstype == 1) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype' OR Type='2'");
            $err="done";
        } else if ($boxstype == 1 && $boxttype == 1) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='2' OR Type='3'");
            $err="done";
        } else if ($boxftype == 1) {;
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype'");
            $err="done";
        } else if ($boxstype == 1) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='2'");
            $err="done";
        } else if ($boxttype == 1) {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='3'");
            $err="done";
        }

    } else if ($number == 3) {

        if ($_POST['boxskladom'] == "1") {
            $result = mysqli_query($conn, "SELECT * FROM main  WHERE CheckedOut = '1'");
            $err="done";
        } else {
            $result = mysqli_query($conn, "SELECT * FROM main WHERE CheckedOut = '0'");
            $err="done";
        }


    } else if ($number == 4) {

        $datearrival_err = $dateleave_err = 0;
        $datearrival = $dateleave = 0;

        if((empty(trim($_POST["adate"]))) || ($_POST["adate"] === "2019-01-01")){
            $datearrival_err = 1;
        } else {
            $datearrival=$_POST["adate"];
        }

        if((empty(trim($_POST["ldate"]))) || ($_POST["ldate"] === "2019-01-01")) {
            $dateleave_err = 1;
        } else {
            $dateleave=$_POST["ldate"];
        }

        if ($dateleave_err == 0 && $datearrival_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main A WHERE A.StampAdded >='$datearrival' AND A.StampToLeave<='$dateleave'");
            $err="done";
        } else if ($dateleave_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main A WHERE A.StampToLeave <= '$dateleave'");
            $err="done";
        } else if ($datearrival_err == 0) {
            $result = mysqli_query($conn, "SELECT * FROM main A WHERE StampAdded >= '$datearrival'");
            $err="done";
        } else {
            $err="missing";
        }
    }

    if ($err == "done") {
        $response = "";
        while ($row = mysqli_fetch_array($result)) {
            $response .= '
        <tr>
            <th>' . $row["id"] . '</th>          
            <td>' . $row["FirstName"] . '</td>
            <td>' . $row["SecondName"] . '</td>
            <td>' . $row["Stat"] . '</td>
            <td>' . $row["StampAdded"] . '</td>
            <td>' . $row["StampToLeave"] . '</td>
            <td>' . $row["CheckedOut"] . '</td>
            <td>' . $row["Type"] . '</td>
       </tr>
        ';
        }
        CloseConn($conn);
        exit($response);
    }
    CloseConn($conn);
    exit($err);
}