<?php
$name = $_GET["name"];
$password = $_GET["password"];
include("db_connection.php");
date_default_timezone_set("Asia/Hong_Kong");
$reg_time = date("Y-m-d H:i:s");
$sql = "INSERT INTO Player (name, password, reg_time, pencil, desk, cabinet, perfume, scroll, book, bomb, skull)
        VALUES ('$name', '$password', '$reg_time', 50, 25, 25, 10, 50, 25, 25, 10)";
$connection->query($sql);
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