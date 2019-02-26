<?php
session_start();

//if already logged in
if(isset($_SESSION["loggedin"]) && $_SESSION["loggedin"] === true){
    header("location: welcome.php");
    exit;
}

require_once "db_connect.php";

$username = $password = "";
$username_err = $password_err = "";

if ($_SERVER["REQUEST_METHOD"] == "POST") {

    if (empty(trim($_POST["username"]))) {
        $username_err = "Zadaj username.";
    } else {
        $username = trim($_POST["username"]);
    }

    if (empty(trim($_POST["password"]))) {
        $password_err = "Zadaj heslo";
    } else {
        $password = trim($_POST["password"]);
    }

    if(empty($username_err) && empty($password_err)) {
        $sql = "SELECT id, username, password FROM logintable WHERE username = ?";

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

                            if ($username != "root") {  //budem ich ukladat do databazy , ale by stacil jeden ukazkovo
                                header("location: welcome.php");
                            } else {
                                header("location: welcomeAdmin.php");
                            }
                        } else {
                            $password_err = "Zle heslo.";
                        }
                    }
                } else {
                    $username_err = "Zle meno";
                }
            } else {
                echo "...skus znova";
            }
        }

        mysqli_stmt_close($stmt);
    }

    mysqli_close($link);
}
?>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Prihlasenie</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.css"">
    <link rel="stylesheet" href="uctystyle.css">
</head>
<body>
    <div class="col-lg-4 col-lg-offset-4" id="blocik">
        <h2>Login</h2>
        <img src="other/keyface.png" />
        <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
            <div class="form-group <?php echo (!empty($username_err)) ? 'has-error' : ''; ?>">
                <label name= "labeluzivatel">Uzivatelske meno</label>
                    <input type="text" name="username" class="form-control" value="<?php echo $username; ?>">
                <span class="help-block"><?php echo $username_err; ?></span>
            </div>
            <div class="form-group <?php echo (!empty($password_err)) ? 'has-error' : ''; ?>">
                <label name="labelheslo">Heslo</label>
                <input type="password" name="password" class="form-control">
                <span class="help-block"><?php echo $password_err; ?></span>
            </div>
            <div class="form-group">
                <input type="submit" class="btn btn-dark" value="Login">
            </div>
            <p>Nemáš účet? <a href="register.php">Vytvor si ho tu</a>.</p>
        </form>
    </div>
</body>
</html>





































