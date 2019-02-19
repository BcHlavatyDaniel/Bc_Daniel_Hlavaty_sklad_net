<?php
require_once "db_connect.php";

$action = $_POST['key'];

switch ($action) {
    case "check":
        makeAdminCheck();
        break;
    case "getall":
        makeGetAllAdmin();
        break;
    case "getallstat":
        makeGetAllStat();
        break;
    case "getalldelete":
        makeGetAllDelete();
        break;
    case "delete":
        makeDelete();
        break;
    case "getallchange":
        makeGetAllChange();
        break;
    case "editvalues":
        makeEditValues();
        break;
    case "find":
        makeFindAdmin();
        break;
}

function makeGetAllDelete() {
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

function makeDelete() {
    $num = $_POST['number'];
    $link = OpenConn();
    $link->query("DELETE FROM main WHERE id = '$num'");
    CloseConn($link);
    exit($num);
}

function makeGetAllChange() {

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

function makeEditValues() {
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

function makeFindAdmin(){
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

function makeGetAllStat() {
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

function makeAdminCheck() {
    session_start();

    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
        exit("locationlogin");
    }

    if($_SESSION['username'] != "root") {
        exit("locationwelcome");
    }

    exit(" ");
}

function makeGetAllAdmin() {
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

/*
if ($action == "check") {

    session_start();

    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
        exit("locationlogin");
    }

    if($_SESSION['username'] != "root") {
        exit("locationwelcome");
    }

    exit(" ");
} else if ($action == "getall") {

    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM main LIMIT $start, $limit";
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
        exit($response);
    } else {
        exit('done');
    }
} else if ($action == "getallstat") {
    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM statistics LIMIT $start, $limit";
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
        exit($response);
    } else {
        exit('done');
    }
} else if ($action == "getalldelete") {
    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM main LIMIT $start, $limit";
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
        exit($response);
    } else {
        exit('done');
    }

} else if ($action == "delete") {
    $num = $_POST['number'];
    $link->query("DELETE FROM main WHERE id = '$num'");
    exit($num);
} else if ($action == "getallchange") {

    $start=$_POST['start'];
    $limit=$_POST['limit'];

    $query = "SELECT * FROM main LIMIT $start, $limit";
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
        exit($response);
    } else {
        exit('done');
    }
} else if($action == "editvalues") {
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
        $query .= " CheckedOut='$bksladom',";
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

    $res = mysqli_query($link, $query);

    exit("done");

} else if ($action == "find") {
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
*/