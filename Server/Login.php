<?php
$name = $_GET["name"];
$password = $_GET["password"];
include("db_connection.php");
$sql = "SELECT DISTINCT * FROM Player WHERE name='$name' AND password='$password'";
$result = $connection->query($sql);
if ($row = $result->fetch_assoc()) {
    echo $row["id"]."#".$row["name"];
}
else {
    echo "NO";
}
include("db_close.php");
?>