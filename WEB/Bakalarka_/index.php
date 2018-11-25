<?php
include_once  'db_connect.php';

if(!isset($_SESSION))
{
    session_start();
}

$_SESSION['firstPressed'] = false;
$_SESSION['secondPressed'] = false;
?>

<head>
    <meta charset="UTF-8">
    <title>MainPage</title>
    <link rel="stylesheet" href="style.css">
    <h2>Zaznamy</h2>
 </head>

<?php
$conn = OpenConn();
if ($_SESSION['firstPressed'] == true)
    $result = mysqli_query($conn, "SELECT * FROM main ORDER BY 'FirstName' ASC");
elseif ($_SESSION['secondPressed'] == true)
    $result = mysqli_query($conn, "SELECT * FROM main ORDER BY 'StampToLeave' ASC");
else
    $result = mysqli_query($conn, "SELECT * FROM main");
CloseConn($conn);
?>

<div class="split left">
    <form method="post">
    <button name="test1" value="sortByName" id="test1" class="button1">SortByTArrived</button>
    </form>
    <form method="post">
    <button name="test2" value="sortByTime" id ="test2" class="button2">SortByStat</button>
    </form>
    <button name="refresh" value="refresh" onClick="window.location.reload()">Refresh</button>
</div>

<div class="split right">
    <table>
        <tr>
            <td>CheckedOut</td>
            <td>FirstName</td>
            <td>SecondName</td>
            <td>Stat</td>
            <td>StampAdded</td>
            <td>StampToLeave</td>
            <td>Type</td>
        </tr>

        <?php
        while($row = mysqli_fetch_array($result)):
        ?>
        <tr>
            <td><?=$row['CheckedOut']?></td>
            <td><?=$row['FirstName']?></td>
            <td><?=$row['SecondName']?></td>
            <td><?=$row['Stat']?></td>
            <td><?=$row['StampAdded']?></td>
            <td><?=$row['StampToLeave']?></td>
            <td><?=$row['Type']?></td>
        </tr>
        <?php
        endwhile;
        ?>

</table>
</div>



<!--php
if ($_GET) {
    if (isset($_GET['sortByName'])) {
        $_SESSION['firstPressed'] = true;
        ?>
        <script>
            window.location.reload();
        </script>
        <php
    }

    if (isset($_GET['sortByTime'])) {
        $_SESSION['secondPressed'] = true;
        ?>
        <script>
            window.location.reload();
        </script>
        <php
    }
}

if(array_key_exists('test1', $_POST))
{
    $_SESSION['firstPressed'] = true;
    ?>
    <script>
        window.location.reload();
    </script>
    <php
}

if(array_key_exists('test1', $_POST))
{
    $_SESSION['firstPressed'] = true;
    ?>
    <script>
        window.location.reload();
    </script>
    <php
}