<?php

$con = mysqli_connect('localhost', 'root', '', 'hy360_2023');

// Check connection
if (!$con) {
    die("Connection failed: " . mysqli_connect_error());
}

// SQL query to select all rows from the Furniture table
$sql = "SELECT * FROM Furniture";
$result = mysqli_query($con, $sql);

// Check if query was successful
if ($result) {
    // Fetch and output each row
    while ($row = mysqli_fetch_assoc($result)) {
        echo $row["Fid"] . "," . 
             $row["Name"] . "," . 
             $row["Type"] . "," . 
             $row["Sett"] . "," . 
             $row["Material"] . "," . 
             $row["Assemblage"] . "," . 
             $row["Color"] . "," . 
             $row["Description"] . "," . 
             $row["Width"] . "," . 
             $row["Depth"] . "," . 
             $row["Height"] . "," . 
             $row["Weight"] . "," . 
             $row["Room"] . "," . 
             $row["Price"] . "," . 
             $row["Did"] . "*";
    }
} else {
    echo "Error: " . mysqli_error($con);
}

// Close the connection
mysqli_close($con);

?>