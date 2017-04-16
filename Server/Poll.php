<?php
$id = $_GET["id"];
$roundID = $_GET["roundID"];
include("db_connection.php");
$sql = "SELECT * FROM Round WHERE id=$roundID";
$result = $connection->query($sql);
while ($row = $result->fetch_assoc()) {
    if ($row["winner"] != NULL) {
        $winner = $row["winner"];
        echo "GAMEOVER#$winner";
    }
    else {
        if ($id == $row["attackerID"])
            $bufferName = "defenderBuffer";
        else
            $bufferName = "attackerBuffer";
        $buffer = $row[$bufferName];
        echo $buffer;
        $sql = "UPDATE Round SET $bufferName='' WHERE id=$roundID";
        $connection->query($sql);
    }
}
include("db_close.php");
?>