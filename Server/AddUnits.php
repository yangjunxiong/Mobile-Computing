<?php
$id = $_GET["id"];
$inputItemName = $_GET["name"];
$inputItemNum = $_GET["number"];
$tempNum = 0;

include("db_connection.php");
$sql = "SELECT * FROM Player WHERE id=$id";
$result = $connection->query($sql);
if ($row = $result->fetch_assoc()) {
    $tempNum = $row[$inputItemName];
    $tempNum += $inputItemNum;
    $sql = "UPDATE Player SET $inputItemName=$tempNum WHERE id=$id";
    $connection->query($sql);
}
include("db_close.php");
?>