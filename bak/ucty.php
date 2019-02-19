<?php
/**
 * Created by PhpStorm.
 * User: Daniel
 * Date: 1/8/2019
 * Time: 3:21 PM
 */
require_once  "db_connect.php";

$username = $password = $confirm_password = $new_password = "";
$username_err = $password_err = $confirm_password_err = $new_password_err = "";


$action=$_POST['key'];

switch ($action) {
    case "login":
        makeLogin();
        break;
    case "signin":
        makeSignin();
        break;
    case "changepass":
        makeNewPass();
        break;
    case "check":
        makeaCheck();
        break;
    case "checklog":
        makeaChecklog();
        break;
    case "checklogout":
        makeLogout();
        break;
}

function makeLogout() {
    session_start();

    $_SESSION = array();
    session_destroy();

    exit("locationlogin");
}

function makeaChecklog() {
    session_start();

    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true){
        exit("locationlogin");
    }
}

function makeaCheck() {
    session_start();

    if(isset($_SESSION["loggedin"]) && $_SESSION["loggedin"] === true){
        if(isset($_SESSION["username"]) && $_SESSION["username"] === "root") {
            exit("locationwelcomeadmin");
        }
        exit("locationwelcome");
    }
}

function makeNewPass() {
    if (!isset($_POST["new_password"])) {
        $new_password_err = "Zadaj heslo";
        exit("Zadaj heslo");
    } else if (empty(trim($_POST["new_password"]))){
        $new_password_err = "Zadaj heslo";
        exit("Zadaj heslo");
    } elseif (strlen(trim($_POST["new_password"])) < 6) {
        $new_password_err = "Heslo musi mat aspon 6 znakov";
        exit("Heslo musi mat aspon 6 znakov");
    } else {
        $new_password = trim($_POST["new_password"]);
    }

    if (!isset($_POST["confirm_password"])) {
        $confirm_password_err = "Please confirm the password";
        exit("Potvrd heslo");
    } else if(empty(trim($_POST["confirm_password"]))){
        $confirm_password_err = "Please confirm the password";
        exit("Potvrd heslo");
    } else {
        $confirm_password = trim($_POST["confirm_password"]);
        if(empty($new_password_err) && ($new_password != $confirm_password)){
            $confirm_password_err = "Password did not match";
            exit("Hesla sa nerovnaju");
        }
    }

    if(empty($new_password_err) && empty($confirm_password_err)) {
        $sql = "UPDATE logintable SET password = ? WHERE id = ?";

        $link = OpenConn();
        if ($stmt = mysqli_prepare($link, $sql)) {
            mysqli_stmt_bind_param($stmt, "si", $param_password, $param_id);

            $param_password = password_hash($new_password, PASSWORD_DEFAULT);
            $param_id = $_SESSION["id"];

            if (mysqli_stmt_execute($stmt)) {
                mysqli_stmt_close($stmt);
                CloseConn($link);
                exit("locationlogin");
            }
        }

        mysqli_stmt_close($stmt);
    }
    mysqli_close($link);
}

function makeSignin() {

    if (!isset($_POST["username"])) {
        $username_err = "Zadaj username";
        exit("Zadaj username");
    } else if (empty(trim($_POST["username"]))){
        $username_err = "Zadaj username";
        exit("Zadaj username");
    } else {
        $sql = "SELECT id FROM logintable WHERE username = ?";

        $link = OpenConn();
        if ($stmt = mysqli_prepare($link, $sql))
        {
            mysqli_stmt_bind_param($stmt, "s", $param_username);
            $param_username = trim($_POST["username"]);

            //execute
            if (mysqli_stmt_execute($stmt)) {
                mysqli_stmt_store_result($stmt);

                if (mysqli_stmt_num_rows($stmt) == 1) {
                    $username_err = "Tvoj username uz existuje";
                    CloseConn($link);
                    exit("Tvoj username uz existuje");
                } else {
                    $username = trim($_POST["username"]);
                }
            }
        }
        mysqli_stmt_close($stmt);
    }

    if (!isset($_POST["password"])) {
        $password_err = "Zadaj password";
        CloseConn($link);
        exit("Zadaj password");
    } else if (empty(trim($_POST["password"]))){
        $password_err = "Zadaj password";
        CloseConn($link);
        exit("Zadaj password");
    } elseif (strlen(trim($_POST["password"])) < 6) {
        $password_err = "password musi mat aspon 6 znakov";
        CloseConn($link);
        exit("password musi mat aspon 6 znakov");
    } else {
        $password = trim($_POST["password"]);
    }

    if (!isset($_POST['confirm_password'])) {
        $confirm_password_err = "Potvrd heslo";
        CloseConn($link);
        exit("Potvrd heslo");
    } else if (empty(trim($_POST["confirm_password"]))) {
        $confirm_password_err = "Potvrd heslo";
        CloseConn($link);
        exit("Potvrd heslo");
    } else {
        $confirm_password = trim($_POST["confirm_password"]);
        if (empty($password_err) && ($password != $confirm_password)){
            $confirm_password_err = "Hesla sa nerovnaju";
            CloseConn($link);
            exit("Hesla sa nerovnaju");
        }
    }


    if (empty($username_err) && empty($password_err) && empty($confirm_password_err)) {
        $sql = "INSERT INTO logintable (username, password) VALUES (?, ?)";

        if ($stmt = mysqli_prepare($link, $sql))
        {
            mysqli_stmt_bind_param($stmt, "ss",$param_username, $param_password);

            $param_username = $username;
            $param_password = password_hash($password, PASSWORD_DEFAULT); // musi  byt zahashovane ofc

            if (mysqli_stmt_execute($stmt)) {
                mysqli_stmt_close($stmt);
                CloseConn($link);
                exit("locationlogin");
            }
        }

        mysqli_stmt_close($stmt);
    }

    mysqli_close($link);
}

function makeLogin() {

    if(!isset($_POST["username"])) {
        exit("Zadaj username.");
    } else if (empty(trim($_POST["username"]))) {
        exit("Zadaj username");
    } else {
        $username = trim($_POST["username"]);
    }

    if(!isset($_POST["password"])) {
        exit("Zadaj heslo");
    } else if (empty(trim($_POST["password"]))) {
        exit("Zadaj heslo");
    } else {
        $password = trim($_POST["password"]);
    }

    if(empty($username_err) && empty($password_err)) {
        $sql = "SELECT id, username, password FROM logintable WHERE username = ?";

        $link = OpenConn();
        if ($stmt = mysqli_prepare($link, $sql)) {
            mysqli_stmt_bind_param($stmt, "s", $param_username);
            $param_username = $username;

            if (mysqli_stmt_execute($stmt)) {
                mysqli_stmt_store_result($stmt);

                if (mysqli_stmt_num_rows($stmt) == 1) {
                    mysqli_stmt_bind_result($stmt, $id,$username, $hashed_password);

                    if (mysqli_stmt_fetch($stmt)) {
                        if (password_verify($password, $hashed_password)) {
                            session_start(); //TO DO

                            $_SESSION["loggedin"] = true;
                            $_SESSION["id"] = $id;
                            $_SESSION["username"] = $username;

                            if ($username != "root") {  //mozem ich ukladat do databazy , ale by stacil jeden ukazkovo
                                mysqli_stmt_close($stmt);
                                CloseConn($link);
                                exit("locationwelcome");
                            } else {
                                mysqli_stmt_close($stmt);
                                CloseConn($link);
                                exit("locationwelcomeadmin");
                            }
                        } else {
                            mysqli_stmt_close($stmt);
                            CloseConn($link);
                            exit("Zle heslo.");
                        }
                    }
                } else {
                    mysqli_stmt_close($stmt);
                    CloseConn($link);
                    exit("Zle meno");
                }
            }
        }
        mysqli_stmt_close($stmt);
    }
}