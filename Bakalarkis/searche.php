<?php

session_start();

if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
    header("location: login.php");
    exit;
}

//TO DO CHANGETHEME SESSION

require_once "databaseactions.php";

$firstname = $secondname = "";

$result = "";

$firstname_err = $secondname_err = 0;

$boxftype = $boxttype = $boxskladom = $boxstype = 0;

$datearrival = $dateleave = "";

$datearrival_err = $dateleave_err = 0;

$_SESSION["apost"] = false;

if ($_SERVER["REQUEST_METHOD"] == "POST") {                 //ERROR CHECKS TO DO, i know very imperfect atm

    $_SESSION["apost"] = true;

    if (isset($_POST['searchByDate'])) {

        if ((empty(trim($_POST["datearrival"]))) || ($_POST["datearrival"] === "2019-01-01")) {
            $datearrival_err = 1;
        } else {
            $datearrival = $_POST["datearrival"];
        }

        if ((empty(trim($_POST["dateleave"]))) || ($_POST["dateleave"] === "2019-01-01")) {
            $dateleave_err = 1;
        } else {
            $dateleave = $_POST["dateleave"];
        }

        $result = GetSearchByDate($datearrival, $dateleave, $datearrival_err, $dateleave_err);

    } else if (isset($_POST['searchByNames'])) {

        if (empty(trim($_POST["firstname"]))) {
            $firstname_err = 1;
        } else {
            $firstname = trim($_POST["firstname"]);
        }

        if (empty(trim($_POST["secondname"]))) {
            $secondname_err = 1;
        } else {
            $secondname = trim($_POST["secondname"]);
        }

        $result = GetSearchByName($firstname, $secondname, $firstname_err, $secondname_err);

    } else if (isset($_POST['searchByType'])) {

        if (isset($_POST['boxftype'])) {
            $boxftype = 1;
        }

        if (isset($_POST['boxstype'])) {
            $boxstype = 1;
        }

        if (isset($_POST['boxttype'])) {
            $boxttype = 1;
        }

        $result = GetSearchByType($boxftype, $boxstype, $boxttype);

    } else if (isset($_POST['searchSkladom'])) {

        if (isset($_POST['boxskladom'])) {
            $boxskladom = 1;
        }

        $result = GetSearchBySkladom($boxskladom);
    }

} else {
    $result = GetAllRows();
}

if ($_SESSION["apost"] != true) {
    $result = GetAllRows();
}


?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Vyhladavanie</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css">
    <link rel="stylesheet" href="style.css">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="js/changeTheme.js" type="text/javascript"></script>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/dataTables.bootstrap.min.js" type="text/javascript"></script>
</head>

<body>
    <nav class="navbar fixed-top navbar-expand-md navbar-light bg-light" id="changenavbar">
        <div class="container">
            <a class="navbar-brand" href="#">Sklad</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarCollapse">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="welcome.php">Home</a>
                    </li>

                    <li class="nav-item active">
                        <a class="nav-link" href="#">Searche
                            <span class="sr-only">(current)</span>
                        </a>
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
        <h2>Vyhľadávanie</h2>
        <div class="row">
            <div class="col-lg-8">
                <div class="content">
                    <div class="table-responsive">
                        <table class="table table-dark table-hover " id="changetable">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Prve meno</th>
                                    <th>Druhe meno</th>
                                    <th>Vlastnosti</th>
                                    <th>Cas prichodu</th>
                                    <th>Cas odchodu</th>
                                    <th>Na sklade</th>
                                    <th>Typ</th>
                                </tr>
                            </thead>
                            <tbody>
                            <?php
                            $count = 0;
                            while ($row = mysqli_fetch_array($result)):         //TO DO co sa stane ked sa objavy moc prvkov na screen
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
            </div>
            <div class="col-lg-4">
                <form class="form-group"  action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
                    <div class="form-group">
                        <label class="sr-only">Prve Meno</label>
                        <input type="search" name="firstname" class="form-control" placeholder="Prve meno" value="<?php echo $firstname;?>">
                        <label class="sr-only">Druhe Meno</label>
                        <input type="search" name="secondname" class="form-control" placeholder="Druhe meno" value="<?php echo $secondname;?>">
                        <input type="submit" class="btn btn-dark" name="searchByNames" id = "searchByNames" value="Hladat">
                    </div>
                    <div class="form-group">
                        <label class="block"><input type="checkbox" name="boxftype" id="inlinetype1">1</label>
                        <label class="block"><input type="checkbox" name="boxstype" id="inlineType2">2</label>
                        <label class="block"><input type="checkbox" name="boxttype" id="inlineType3">3</label>
                        <input type="submit" class="btn btn-dark" name="searchByType" id= "searchByType" value="Hladat">
                    </div>
                    <div class="form-group">
                        <label><input type="checkbox" name="boxskladom" id="inlineType4">Skladom?</label>
                        <input type="submit" class="btn btn-dark" name="searchSkladom" id = "searchSkadom" value="Hladat">
                    </div>
                    <div class="form-group">
                        <input class="form-control" type="date" name="datearrival" value="2019-01-01">
                        <input class="form-control" type="date" name="dateleave" value="2019-01-01">
                        <input type="submit" class="btn btn-dark" name="searchByDate"  id = "searchByDate" value="Hladat">
                    </div>
                </form>
            </div>
        </div>
    </div>

</body>
