<?php
require_once  "db_connect.php";

$username = $password = $confirm_password = "";
$username_err = $password_err = $confirm_password_err = "";

if ($_SERVER["REQUEST_METHOD"] == "POST") {

    if (empty(trim($_POST["username"]))){
        $username_err = "Zadaj username";
    } else {
        $sql = "SELECT id FROM logintable WHERE username = ?";

        if ($stmt = mysqli_prepare($link, $sql))
        {
            mysqli_stmt_bind_param($stmt, "s", $param_username);
            $param_username = trim($_POST["username"]);

            //execute
            if (mysqli_stmt_execute($stmt)) {
                mysqli_stmt_store_result($stmt);

                if (mysqli_stmt_num_rows($stmt) == 1) {
                    $username_err = "Tvoj username uz existuje";
                } else {
                    $username = trim($_POST["username"]);
                }
            } else {
                echo "Tu tu tu skus znova.";
            }
        }
        mysqli_stmt_close($stmt);                   //EERROR
    }

    if (empty(trim($_POST["password"]))){           //PASSWD CHECK
        $password_err = "Zadaj password";
    } elseif (strlen(trim($_POST["password"])) < 6) {
        $password_err = "password musi mat aspon 6 znakov";
    } else {
        $password = trim($_POST["password"]);
    }

    if (empty(trim($_POST["confirm_password"]))) {      //CONFIRM PASSWD CHECK
        $confirm_password_err = "Potvrd heslo";
    } else {
        $confirm_password = trim($_POST["confirm_password"]);
        if (empty($password_err) && ($password != $confirm_password)){
            $confirm_password_err = "Hesla sa nerovnaju";
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
                header("location: index.php"); //redirect ktory este chyba
            } else {
                echo "Something went wrong. Please try again later.";
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
    <link rel="stylesheet" href=https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.css"">
    <link rel="stylesheet" href="uctystyle.css">
</head>
<body>

<div class="col-lg-4 col-lg-offset-4" id="blocik">
    <h2>Registracia</h2>
    <img src="other/logface.png"/>
    <form action="<?php echo  htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
        <div class="form-group  <?php echo (!empty($username_err)) ? 'has-error' : ''; ?>">
            <label name="labeluzivatel">Uzivatelske meno</label>
            <input type="text" name="username" class="form-control"  value="<?php echo $username; ?>">
            <span class="help-block"><?php echo $username_err; ?></span>
        </div>
        <div class="form-group  <?php echo (!empty($password_err)) ? 'has-error' : ''; ?>">
            <label name="labelheslo">Heslo</label>
            <input type="password" name="password" class="form-control" value="<?php echo $password; ?>">
            <span class="help-block"><?php echo $password_err; ?></span>
        </div>
        <div class="form-group" <?php echo (!empty($confirm_password_err)) ? 'has-error' : ''; ?>>
            <label name="labelconfheslo">Zopakuj Heslo</label>
            <input type="password" name="confirm_password" class="form-control" value="<?php echo $confirm_password; ?>">
            <span class="help-block"><?php echo $confirm_password_err; ?></span>
        </div>
        <div class="form-group">
            <input type="submit" class="btn btn-dark" value="Submit">
        </div>
        <p>Máš už účet ?<a href="index.php">Prihlás sa tu</a>.</p>
    </form>
</div>

</body>
</html>