<?php

session_start();

if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
    header("location: login.php");
    exit;
}
//TO DO SAVE THEME SESSION

require_once "databaseactions.php"
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Vitajte</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css">
    <link rel="stylesheet" href="style.css">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="js/changeTheme.js" type="text/javascript"></script>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/dataTables.bootstrap.min.js" type="text/javascript"></script>
</head>

<body>
<?php
$result = GetAllRows();
?>

    <nav class="navbar fixed-top navbar-expand-md navbar-light bg-light" id="changenavbar">
        <div class="container">
            <a class="navbar-brand" href="#">Sklad</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarCollapse">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item active">
                        <a class="nav-link" href="#">Home
                            <span class="sr-only">(current)</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="searche.php">Searche</a>
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

    <div class="jumbotron">
        <h1 class="display-2">Momentalny stav skladu</h1>
        <p>Prihlaseny ako <?php echo $_SESSION["username"]?> </p>
    </div>

    <div id="content">
        <div class="col-md12">
            <table class="table table-dark table-hover table-bordered" id="changetable">
                <thead>
                <tr>
                    <td>#</td>
                    <td>Prve meno</td>
                    <td>Druhe meno</td>
                    <td>Vlastnosti</td>
                    <td>Cas prichodu</td>
                    <td>Cas odchodu</td>
                    <td>Na sklade</td>
                    <td>Typ</td>
                </tr>
                </thead>
                <tbody>
                <?php
                $count = 0;
                while ($row = mysqli_fetch_array($result)):
                    $count++;
                    ?>
                    <tr>
                        <th scope="row"><?=$count?></th>
                        <td><?=$row['FirstName']?></td>
                        <td><?=$row['SecondName']?></td>
                        <td><?=$row['Stat']?></td>
                        <td><?=$row['StampAdded']?></td>
                        <td><?=$row['StampToLeave']?></td>
                        <td><?=$row['CheckedOut']?></td>
                        <td><?=$row['Type']?></td>
                    </tr>
                <?php
                endwhile;
                ?>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>


