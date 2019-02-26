<?php

require_once  "db_connect_spec.php";
require_once "db_connect.php";

function GetAllRows() {
    $conn = OpenConn();
    $result = mysqli_query($conn, "SELECT * FROM main");
    CloseConn($conn);
    return $result;
}

function GetSearchByDate($datearrival, $dateleave, $datearrival_err, $dateleave_err) {

    if ($dateleave_err == 0 && $datearrival_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main A WHERE A.StampAdded >='$datearrival' AND A.StampToLeave<='$dateleave'");
        CloseConn($conn);
    } else if ($dateleave_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main A WHERE A.StampToLeave <= '$dateleave'");
        CloseConn($conn);
    } else if ($datearrival_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main A WHERE StampAdded >= '$datearrival'");
        CloseConn($conn);
    } else {
        $result = GetAllRows();
    }
    return $result;
}

function GetSearchByName($firstname, $secondname, $firstname_err, $secondname_err) {
    if ($firstname_err == 0 && $secondname_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE FirstName = '$firstname' AND SecondName= '$secondname'");
        CloseConn($conn);

    } else if ($firstname_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE FirstName = '$firstname'");
        CloseConn($conn);

    } else if ($secondname_err == 0) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE SecondName='$secondname'"); //TO DO IF res empty
        CloseConn($conn);
    } else {
        $result = GetAllRows();
    }

    return $result;
}

function GetSearchByType($boxftype, $boxstype, $boxttype) {
    if (($boxftype == 1 && $boxstype == 1 && $boxttype == 1) || ($boxttype == 0 && $boxstype == 0 && $boxftype == 0)) {
        $result = GetAllRows();

    } else if ($boxftype == 1 && $boxttype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype' OR Type='3'");
        CloseConn($conn);
    } else if ($boxftype == 1 && $boxstype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype' OR Type='2'");
        CloseConn($conn);
    } else if ($boxstype == 1 && $boxttype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='2' OR Type='3'");
        CloseConn($conn);
    } else if ($boxftype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='$boxftype'");
        CloseConn($conn);
    } else if ($boxstype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='2'");
        CloseConn($conn);
    } else if ($boxttype == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE Type='3'");
        CloseConn($conn);
    }

    return $result;
}


function GetSearchBySkladom($boxskladom) {
    if ($boxskladom == 1) {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE CheckedOut='1'");
        CloseConn($conn);
    } else {
        $conn = OpenConn();
        $result = mysqli_query($conn, "SELECT * FROM main WHERE CheckedOut='0'");
        CloseConn($conn);
    }

    return $result;
}

function AddRow($firstname, $secondname, $firstname_err, $secondname_err, $datearrival, $dateleave, $dateleave_err, $datearrival_err, $stat, $stat_err, $cskladom, $type) {

    if ($firstname_err == 0 && $secondname_err == 0 && $stat_err == 0 && $dateleave_err == 0 && $datearrival_err == 0) {
        $sql = "INSERT into main (FirstName, SecondName, Stat, Type, StampAdded, StampToLeave, CheckedOut) VALUES (?,?,?,?,?,?,?)";
        $link = OpenConn();
        if ($stmt = mysqli_prepare($link, $sql)) {
            mysqli_stmt_bind_param($stmt, "sssssss", $param_firstname, $param_secondname, $param_stat, $param_type, $param_datearrived, $param_dateleave, $param_checked);
            $param_firstname = $firstname;
            $param_secondname = $secondname;
            $param_stat = $stat;
            $param_datearrived = $datearrival;
            $param_dateleave = $dateleave;
            $param_type = $type;
            $param_checked = $cskladom;

            if(mysqli_stmt_execute($stmt)) {
                mysqli_stmt_store_result($stmt);
            }
        }

        $sql = "SELECT * FROM statistics WHERE FirstName = '$firstname' AND SecondName = '$secondname'";
        $show = mysqli_query($link, $sql);
        if($show->num_rows > 0) {
            $row = mysqli_fetch_array($show);
            $visits = $row["VisitCount"];
            if($type==1) {
                $stat += $row["StatTypeFirst"];
                $sql = "UPDATE statistics SET StatTypeFirst='$stat', VisitCount='$visits' WHERE FirstName ='$firstname' AND SecondName ='$secondname'";
            }
            if($type==2) {
                $stat += $row["StatTypeSecond"];
                $sql = "UPDATE statistics SET StatTypeSecond='$stat', VisitCount='$visits' WHERE FirstName ='$firstname' AND SecondName ='$secondname'";
            }
            if($type==3) {
                $stat += $row["StatTypeThird"];
                $sql = "UPDATE statistics SET StatTypeThird='$stat', VisitCount='$visits' WHERE FirstName ='$firstname' AND SecondName ='$secondname'";
            }
            mysqli_query($link, $sql);

        } else {
            $sql = "INSERT into statistics (FirstName, SecondName, VisitCount, StatTypeFirst, StatTypeSecond, StatTypeThird) VALUES (?,?,?,?,?,?)";

            if($stmt = mysqli_prepare($link, $sql)) {

                mysqli_stmt_bind_param($stmt, "ssssss", $param_firstname, $param_secondname, $param_visitcount, $param_stattypefirst, $param_stattypesecond, $param_statypethird);
                $param_firstname = $firstname;
                $param_secondname = $secondname;
                $param_visitcount = 0;

                if ($type==1) {
                    $param_stattypefirst = $stat;
                    $param_stattypesecond = 0;
                    $param_statypethird = 0;
                }
                if ($type==2) {
                    $param_stattypefirst = 0;
                    $param_stattypesecond = $stat;
                    $param_statypethird = 0;
                }
                if ($type==3) {
                    $param_stattypefirst = 0;
                    $param_stattypesecond = 0;
                    $param_statypethird = $stat;
                }

                if(mysqli_stmt_execute($stmt)) {
                    mysqli_stmt_store_result($stmt);
                }
            }
        }

    } //ELSE ERROR
    $result = GetAllRows();
    return $result;

}