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
    <title>Vitajte</title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="style.css">
    <script src="js/modal.js" type="text/javascript"></script>
    <script src="js/changeTheme.js" type="text/javascript"></script>
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

                    <li class="nav-item">
                        <a class="nav-link" href="adminstatistics.php">Statistika</a>
                    </li>

                    <li class="nav-item active dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Upravy
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="dropdown-item" href="#">Pridat Zaznamy</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="admindelete.php">Zmazat Zaznamy</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="adminedit.php">Zmeny</a>
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
        <h2>Pridať záznam <button type="button" class="btn btn-dark modal-control-btn" data-toggle="modal" data-target="#addRow">Pridať</button></h2>
        <div id="content">
        </div>
        <div class="modal fade" tabindex="-1" role="dialog" id="addRow" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Pridať záznam</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <form>
                            <label>Meno a priezvisko</label>
                            <div class="form-group">
                                <input type="search" name="firstname" class="form-control" id="firstname" placeholder="Prve Meno">
                                <input type="search" name="secondname" class="form-control" id="inlineFormInput" placeholder="Druhe Meno">
                            </div>
                            <div class="form-group">
                                <label class="block" for="inlineType1">
                                    <input id = "boxftype" type="checkbox" name="boxftype" id="inlineType1">Type1
                                </label>
                                <label class="block" form="inlineType2">
                                    <input id = "boxstype" type="checkbox" name="boxstype" id="inlineType2">Type2
                                </label>
                                <label class="block" form="inlineType3">
                                    <input id="boxttype" type="checkbox" name="boxttype" id="inlineType3">Type3
                                </label>
                                <label class="block" form="inlineType4">
                                    <input id="boxskladom" type="checkbox" name="boxskladom" id="inlineType4">Skladom?
                                </label>
                            </div>
                            <div class="form-group">
                                <label>Stat</label>
                                <input type="number" name="statnum" class="form-control" placeholder="0">
                            </div>
                            <div class="form-group">
                                <label>Dátum príchodu</label>
                                <input class="form-control" type="date" name="datearrival" value="2019-01-01">
                                <label>Dátum odchodu</label>
                                <input class="form-control" type="date" name="dateleave" value="2019-01-01">
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <div class="form-group">
                            <input type="button" class="btn btn-dark"  onclick="toggleModal()" id="addbutton" value="Pridat" name="addbutton">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</body>
</html>

