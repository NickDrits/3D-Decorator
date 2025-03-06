<?php

$con = mysqli_connect('localhost','root','','hy360_2023');

$sql = "SELECT * FROM address ";
$result = mysqli_query($con, $sql);

if($result){
    while($row = mysqli_fetch_assoc($result)){
        echo $row["Country"] ."," . $row["Town"] ."," .$row["Street"] . "," . $row["Number"] . "," . 
        $row["Postal"] . "," . $row["Cid"] ."*";
    }
}

?>