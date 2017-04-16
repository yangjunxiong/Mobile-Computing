<?php
$id = $_GET["id"];
$roundID = $_GET["roundID"];
include("db_connection.php");
$sql = "DELETE FROM Round WHERE id=$roundID";
$connection->query($sql);
include("db_close.php");
?>