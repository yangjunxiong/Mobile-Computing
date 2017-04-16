<?php
$id = $_GET["id"];
$roundID = $_GET["roundID"];
$side = $_GET["side"];
if ($side == "attacker") {
    $myColumnName = "attackerID";
    $targetColumnName = "defenderID";
}
else {
    $myColumnName = "defenderID";
    $targetColumnName = "attackerID";
}

include("db_connection.php");
// Round host player
if ($roundID != "" && $roundID != NULL && $roundID > 0) {
    $sql = "SELECT * FROM Round WHERE id=$roundID";
    $result = $connection->query($sql);
    if ($row = $result->fetch_assoc()) {
        if ($row[$targetColumnName] != "" && $row[$targetColumnName] != NULL) {
            $obj = new stdClass;
            $obj->id = $row["id"];
            $obj->attackerID = $row["attackerID"];
            $obj->defenderID = $row["defenderID"];
            echo json_encode($obj);
        }
        else
            echo "NO";
    }
}
// Round guest player || Start new round
else {
    $sql = "SELECT * FROM Round WHERE $myColumnName IS NULL ORDER BY start_time ASC";
    $result = $connection->query($sql);
    // Found existing opponent
    if ($row = $result->fetch_assoc()) {
        $roundID = $row["id"];
        $sql = "UPDATE Round SET $myColumnName=$id WHERE id=$roundID";
        $connection->query($sql);
        $sql = "SELECT * FROM Round WHERE id=$roundID";
        $result = $connection->query($sql);
        if ($row = $result->fetch_assoc()) {
            $obj = new stdClass;
            $obj->id = $row["id"];
            $obj->attackerID = $row["attackerID"];
            $obj->defenderID = $row["defenderID"];
            echo json_encode($obj);
        }
    }
    // No opponent found, host new round
    else {
        date_default_timezone_set("Asia/Hong_Kong");
        $time = date("Y-m-d H:i:s");
        $sql = "INSERT INTO Round ($myColumnName, start_time) VALUES ($id, '$time')";
        $connection->query($sql);
        $sql = "SELECT * FROM Round WHERE $myColumnName=$id AND $targetColumnName IS NULL ORDER BY start_time DESC";
        $result = $connection->query($sql);
        if ($row = $result->fetch_assoc()) {
            $obj = new stdClass;
            $obj->id = $row["id"];
            $obj->attackerID = $row["attackerID"];
            $obj->defenderID = $row["defenderID"];
            echo json_encode($obj);
        }
    }
}
include("db_close.php");
?>