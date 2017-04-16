<?php
$host = "localhost";
$user = "codingbear";
$pass = "";
$db = "c9";
$port = 3306;
$connection = new mysqli($host, $user, $pass, $db, $port);
if ($connection->connect_error) {
    die("Cannot connect to database: " . $connection->connect_error);
}
?>