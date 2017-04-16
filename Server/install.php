<?php
include("db_connection.php");
$sql = "DROP TABLE Player, Round";
$connection->query($sql);
$sql = "CREATE TABLE Player (
        id INT(4) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
        name VARCHAR(30) NOT NULL,
        password VARCHAR(30) NOT NULL,
        reg_time DATETIME NOT NULL,
        pencil INT(3) UNSIGNED NOT NULL,
        desk INT(3) UNSIGNED NOT NULL,
        cabinet INT(3) UNSIGNED NOT NULL,
        perfume INT(3) UNSIGNED NOT NULL,
        scroll INT(3) UNSIGNED NOT NULL,
        book INT(3) UNSIGNED NOT NULL,
        bomb INT(3) UNSIGNED NOT NULL,
        skull INT(3) UNSIGNED NOT NULL
        )";
$connection->query($sql);
echo "Table player created<br/>";
$sql = "CREATE TABLE Round (
        id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
        attackerID INT(4) UNSIGNED,
        defenderID INT(4) UNSIGNED,
        start_time DATETIME,
        end_time DATETIME,
        winner INT(4) UNSIGNED,
        attackerBuffer VARCHAR(200),
        defenderBuffer VARCHAR(200)
        )";
$connection->query($sql);
echo "Table Round created<br/>";

$sql = "INSERT INTO Player (name, password, reg_time, pencil, desk, cabinet, perfume, scroll, book, bomb, skull)
        VALUES ('Test1', 'Test1Password', '2016-04-01 00:00:01', 50, 10, 10, 10, 50, 25, 10, 10)";
$connection->query($sql);
echo "Player 1 added<br/>";
$sql = "INSERT INTO Player (name, password, reg_time, pencil, desk, cabinet, perfume, scroll, book, bomb, skull)
        VALUES ('Test2', 'Test2Password', '2016-04-01 15:15:01', 50, 10, 10, 10, 50, 25, 10, 10)";
$connection->query($sql);
echo "Player 2 added<br/>";
include("db_close.php");
?>