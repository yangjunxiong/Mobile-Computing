<?php
$id = $_GET["id"];
$roundID = $_GET["roundID"];
$win = $_GET["win"];
include("db_connection.php");
$sql = "SELECT * FROM Round WHERE id=$roundID";
$result = $connection->query($sql);
while ($row = $result->fetch_assoc()) {
    if ($row["winner"] == $row["attackerID"] || $row["winner"] == $row["defenderID"]) {
        echo "YES";
        return;
    }
    if ($win == "true")
        $winner = $id;
    else 
        $winner = $id==$row["attackerID"] ? $row["defenderID"] : $row["attackerID"];
    date_default_timezone_set("Asia/Hong_Kong");
    $time = date("Y-m-d H:i:s");
    $buffer = "GAMEOVER#$winner";
    $sql = "UPDATE Round SET end_time='$time', winner=$winner, attackerBuffer='$buffer', defenderBuffer='$buffer' WHERE id=$roundID";
    $connection->query($sql);
}
include("db_close.php");
echo "YES";
?>