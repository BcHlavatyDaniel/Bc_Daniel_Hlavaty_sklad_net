<?php
/**
 * Created by PhpStorm.
 * User: Daniel
 * Date: 1/4/2019
 * Time: 9:25 PM
 */

require_once "db_connect.php";

$action=$_POST["action"];

$firstname = $secondname ="";
$datearrival = $dateleave = "";
$dateleave_err = $datearrival_err = 0;
$firstname_err = $secondname_err = $stat = $stat_err = 0;
$cskladom = $type = 0;
//CLOSECONN

if ($action=="actionadd") {

    if(empty(trim($_POST["fname"]))) {
        $firstname_err = 1;
    } else {
        $firstname = trim($_POST["fname"]);
    }

    if (empty(trim($_POST["sname"]))) {
        $secondname_err = 1;
    } else {
        $secondname = trim($_POST["sname"]);
    }

    if (empty(trim($_POST["stat"]))) {
        $stat_err = 1;
    } else if (!is_numeric(trim($_POST["stat"]))) {
        $stat_err = 1;
    } else  {
        $stat = trim($_POST["stat"]);
    }

    if (empty(trim($_POST["adate"]))) {
        $datearrival_err = 1;
    } else {
        $datearrival = trim($_POST["adate"]);
    }

    if (empty(trim($_POST["ldate"]))) {
        $dateleave_err = 1;
    } else {
        $dateleave = trim($_POST["ldate"]);
    }

    if (($_POST['bskladom']) == "1") {
        $cskladom = 1;
    }

    if (($_POST['boxftype']) =="1") {
        $type = 1;
    }

    if (($_POST['boxstype']) =="1") {
        $type = 2;
    }

    if (($_POST['boxttype']) == "1") {
        $type = 3;
    }


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

        $query = "SELECT * FROM main";
        $link = OpenConn();
        $show = mysqli_query($link, $query);
        echo "
            <div class=\"table-responsive\">
                <table class=\"table table-grey table-hover\">
                    <thread>
                        <tr>
                        <th scope=\"col\">#</th>
                        <th scope=\"col\">Prve meno</th>
                        <th scope=\"col\">Druhe meno</th>
                        <th scope=\"col\">Vlastnosti</th>
                        <th scope=\"col\">Cas prichodu</th>
                        <th scope=\"col\">Cas odchodu</th>
                        <th scope=\"col\">Na sklade</th>
                        <th scope=\"col\">Typ</th>
                        </tr>
                    </thread>
                    <tbody>";

        $count = 0;
        while ($row = mysqli_fetch_array($show)) {
            $count++;
            echo "<tr>
                <td>" . $count . "</td>
                <td>" . $row['FirstName'] . "</td>
                <td>" . $row['SecondName'] . "</td>
                <td>" . $row['Stat'] . "</td>
                <td>" . $row['StampAdded'] . "</td>
                <td>" . $row['StampToLeave'] . "</td>
                <td>" . $row['CheckedOut'] . "</td>
                <td>" . $row['Type'] . "</td>";
        }
        echo "</tbody>
            </table>
        </div>";
        CloseConn($link);

}else if ($action=="getall") {

    $query = "SELECT * FROM main";
    $link = OpenConn();
    $show = mysqli_query($link, $query);
    echo "
    <div class=\"table-responsive\">
    <table class=\"table table-grey table-hover\">
        <thread>
            <tr>
                <th scope=\"col\">#</th>
                <th scope=\"col\">Prve meno</th>
                <th scope=\"col\">Druhe meno</th>
                <th scope=\"col\">Vlastnosti</th>
                <th scope=\"col\">Cas prichodu</th>
                <th scope=\"col\">Cas odchodu</th>
                <th scope=\"col\">Na sklade</th>
                <th scope=\"col\">Typ</th>
            </tr>
        </thread>
        <tbody>
    ";
    $count = 0;
    while ($row = mysqli_fetch_array($show)) {
        $count++;
        echo "<tr>
            <td>" . $count . "</td>
        <td>" . $row['FirstName'] . "</td>
        <td>" . $row['SecondName'] . "</td>
        <td>" . $row['Stat'] . "</td>
        <td>" . $row['StampAdded'] . "</td>
        <td>" . $row['StampToLeave'] . "</td>
        <td>" . $row['CheckedOut'] . "</td>
        <td>" . $row['Type'] . "</td>
        ";
    }
    echo "</tbody>
        </table>
        </div>";
    CloseConn($link);

}