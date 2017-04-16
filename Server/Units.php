<?php
$playerID = $_GET["id"];
$side = $_GET["side"];
$sql = "SELECT * FROM Player WHERE id=$playerID";
include("db_connection.php");
$result = $connection->query($sql);
include("db_close.php");
if ($result->num_rows>0) {
    while ($row = $result->fetch_assoc()) {
        $listObj = new stdClass;
        $listObj->propList = array();
        if ($side == "attacker") {
            $obj = new stdClass;
            $obj->name = "Scroll";
            $obj->number = $row["scroll"];
            $obj->cooldown = 5.0;
            $listObj->propList[0] = $obj;
            $obj = new stdClass;
            $obj->name = "Book";
            $obj->number = $row["book"];
            $obj->cooldown = 5.0;
            $listObj->propList[1] = $obj;
            $obj = new stdClass;
            $obj->name = "Bomb";
            $obj->number = $row["bomb"];
            $obj->cooldown = 10.0;
            $listObj->propList[2] = $obj;
            $obj = new stdClass;
            $obj->name = "Skull";
            $obj->number = $row["skull"];
            $obj->cooldown = 10.0;
            $listObj->propList[3] = $obj;
        }
        else {
            $obj = new stdClass;
            $obj->name = "Pencil";
            $obj->number = $row["pencil"];
            $obj->cooldown = 5.0;
            $listObj->propList[0] = $obj;
            $obj = new stdClass;
            $obj->name = "Desk";
            $obj->number = $row["desk"];
            $obj->cooldown = 7.0;
            $listObj->propList[1] = $obj;
            $obj = new stdClass;
            $obj->name = "Cabinet";
            $obj->number = $row["cabinet"];
            $obj->cooldown = 10.0;
            $listObj->propList[2] = $obj;
            $obj = new stdClass;
            $obj->name = "Perfume";
            $obj->number = $row["perfume"];
            $obj->cooldown = 10.0;
            $listObj->propList[3] = $obj;
        }
        $json = json_encode($listObj);
        echo $json;
    }
}
else {
    echo "FAIL";
}
?>