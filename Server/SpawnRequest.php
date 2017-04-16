<?php
$id = $_GET["id"];
$roundID = $_GET["roundID"];
$side = $_GET["side"];
$tileName = $_GET["tileName"];
$index = $_GET["index"];
if ($side == "attacker")
    switch ($index) {
    case 0:
        $name = "scroll";
        break;
    case 1:
        $name = "book";
        break;
    case 2:
        $name = "bomb";
        break;
    case 3:
        $name = "skull";
        break;
    }
else
    switch ($index) {
        case 0:
        $name = "pencil";
        break;
    case 1:
        $name = "desk";
        break;
    case 2:
        $name = "cabinet";
        break;
    case 3:
        $name = "perfume";
        break;
    }

include("db_connection.php");
$sql = "SELECT * FROM Round WHERE id=$roundID";
$result = $connection->query($sql);
$row = $result->fetch_assoc();
if ($row["winner"] == $row["attackerID"] || $row["winner"] == $row["defenderID"]) {
    echo "NO";
    return;
}

$sql = "SELECT $name FROM Player WHERE id=$id";
$result = $connection->query($sql);
$row = $result->fetch_assoc();
if ($row[$name] > 0) {
    echo "YES";
    $new_number = --$row[$name];
    $sql = "UPDATE Player SET $name=$new_number WHERE id=$id";
    $connection->query($sql);
    
    $obj = new stdClass;
    $obj->tileName = $tileName;
    $obj->index = $index;
    $sql = "SELECT * FROM Round WHERE id=$roundID";
    $result = $connection->query($sql);
    while ($row = $result->fetch_assoc()) {
        if ($side == "attacker")
            $bufferName = "attackerBuffer";
        else
            $bufferName = "defenderBuffer";
        $buffer = $row[$bufferName];
        if ($buffer == "" || $buffer == null) {
            $requestList = new stdClass;
            $requestList->spawnList = array();
        }
        else {
            $requestList = json_decode(explode("_", $buffer)[1]);
        }
        array_push($requestList->spawnList, $obj);
        $json = json_encode($requestList);
        $buffer = addslashes("SPAWN_".$json);
        $sql = "UPDATE Round SET $bufferName='$buffer' WHERE id=$roundID";
        $connection->query($sql);
    }
}
else
    echo "NO";
include("db_close.php");
?>