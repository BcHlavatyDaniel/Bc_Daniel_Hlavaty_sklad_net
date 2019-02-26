<?php

require_once  "db_connect_spec.php";
//TO DO unite del/edit methods
$action = $_POST['key'];

switch ($action) {
    case "getallstat":
        getAllStatisticsRows();
        break;
    case "getallchange":
        getAllTableRows();
        break;
    case "editvalues":
        editValues();
        break;
    case "delete":
        deleteRow();
        break;
    case "getalldelete":
        getAllDelTableRows();
        break;
}

function deleteRow() {
    $num = $_POST['number'];
    $link = OpenConn();
    $link->query("DELETE FROM main WHERE id = '$num'");
    CloseConn($link);
    exit($num);
}

function getAllDelTableRows() {

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
            <td>' . $row["id"] . '</td>          
            <td>' . $row["FirstName"] . '</td>
            <td>' . $row["SecondName"] . '</td>
            <td>' . $row["Stat"] . '</td>
            <td>' . $row["StampAdded"] . '</td>
            <td>' . $row["StampToLeave"] . '</td>
            <td>' . $row["CheckedOut"] . '</td>
            <td>' . $row["Type"] . '</td>
            <td><input type="button" onclick="deleterow('.$row["id"].')" class="btn btn-dark" id="mazatbutton('.$row["id"].')" value="Zmazat"></td>
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

function editValues() {
    $response = "";
    $number = $_POST['number'];

    $query = "UPDATE main SET";
    if (isset($_POST['fname'])) {
        $fname = $_POST['fname'];
        $query .= " FirstName= '$fname',";
    }
    if (isset($_POST['sname'])) {
        $sname = $_POST['sname'];
        $query .= " SecondName='$sname',";
    }
    if (isset($_POST['type'])) {
        $type = $_POST['type'];
        $query .= " Type='$type',";
    }
    if (isset($_POST['stat'])) {
        $stat = $_POST['stat'];
        $query .= " Stat='$stat',";
    }
    if (isset($_POST['bskladom'])) {
        $bskladom = $_POST['bskladom'];
        $query .= " CheckedOut='$bskladom',";
    }
    if (isset($_POST['adate'])) {
        $adate = $_POST['adate'];
        $query .= " StampAdded='$adate',";
    }
    if (isset($_POST['ldate'])) {
        $ldate = $_POST['ldate'];
        $query .= " StampToLeave='$ldate'";
    }

    if (substr("$query", -1) == ",") {
        $query = substr_replace($query, "", -1);
    }

    $query .=  " WHERE id='$number'";

    $link = OpenConn();
    $res = mysqli_query($link, $query);
    CloseConn($link);
    exit("done");
}

function getAllTableRows() {

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
            <td>' . $row["id"] . '</td>          
            <td>' . $row["FirstName"] . '</td>
            <td>' . $row["SecondName"] . '</td>
            <td>' . $row["Stat"] . '</td>
            <td>' . $row["StampAdded"] . '</td>
            <td>' . $row["StampToLeave"] . '</td>
            <td>' . $row["CheckedOut"] . '</td>
            <td>' . $row["Type"] . '</td>
            <td><input type="button" onclick="editrow('.$row["id"].')" class="btn btn-dark" id="editbutton('.$row["id"].')" value="Zmenit"></td>
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


function GetAllStatisticsRows() {

    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM statistics LIMIT $start, $limit";
    $link = OpenConn();
    $show = mysqli_query($link, $query);

    if($show->num_rows > 0) {
        $response = "";
        while ($row = mysqli_fetch_array($show)) {
            $response .= '
        <tr>
            <td>' . $row["id"] . '</td>          
            <td>' . $row["FirstName"] . '</td>
            <td>' . $row["SecondName"] . '</td>
            <td>' . $row["VisitCount"] . '</td>
            <td>' . $row["StatTypeFirst"] . '</td>
            <td>' . $row["StatTypeSecond"] . '</td>
            <td>' . $row["StatTypeThird"] . '</td>
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

