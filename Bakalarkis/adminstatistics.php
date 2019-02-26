<?php

session_start();

if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
    header("location: login.php");
    exit;
}

if ($_SESSION["username"] != "root") {
    header("location: welcome.php");
    exit;
}

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Statistika</title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="style.css">
    <script src="js/changeTheme.js" type="text/javascript"></script>
    <script src="js/adminstatistics.js" type="text/javascript"></script>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/dataTables.bootstrap.min.js" type="text/javascript"></script>
</head>
<body>

    <nav class="navbar fixed-top navbar-expand-md navbar-light bg-light">
        <div class="container">
            <a class="navbar-brand" href="#">Sklad</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarCollapse">

                <ul class="navbar-nav mr-auto">

                    <li class="nav-item">
                        <a class="nav-link" href="adminwelcome.php">Home</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" href="adminsearche.php">Searche</a>
                    </li>

                    <li class="nav-item active">
                        <a class="nav-link" href="#">Statistika
                            <span class="sr-only">(current)</span>
                        </a>
                    </li>

                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Upravy
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="dropdown-item" href="adminadd.php">Pridat Zaznamy</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#">Zmazat Zaznamy</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#">Zmeny</a>
                        </div>
                    </li>
                </ul>

                <ul class="nav navbar-nav ml-auto">
                    <li class="nav-item">
                        <a class="nav-link" onclick="changeTheme()" id="temachange" name="temachange">Zmen temu</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="logout.php">Logout</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="container">
        <h2>Statistika</h2>
        <div id="content">
            <table class="table table-hover table-bordered">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Prve meno</th>
                        <th>Druhe meno</th>
                        <th>Pocet Navstev</th>
                        <th>CelkovaVlastnost1</th>
                        <th>CelkovaVlastnost2</th>
                        <th>CelkovaVlastnost3</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>

</body>


</html>