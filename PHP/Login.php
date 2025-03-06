<?php 
$con = mysqli_connect('localhost', 'root', '', 'hy360_2023');

// Check connection
if (mysqli_connect_errno()) {
    echo "Connection failed";
    exit();
}

$sql = "SELECT Cid, Fname, Sname, Username, Email, Phone, Day, Month, Year, Country, Town, Street, Number, Postal FROM client";
$result = mysqli_query($con, $sql);

if ($result) {
    while ($row = mysqli_fetch_assoc($result)) {
        echo $row["Fname"] . "," . $row["Sname"] . "," . $row["Username"] . "," . $row["Email"] . "," . $row["Phone"] . "," .
            $row["Day"] . "," . $row["Month"] . "," . $row["Year"] . "," . $row["Cid"] . "," . $row["Country"] . "," . $row["Town"] .
            "," . $row["Street"] . "," . $row["Number"] . "," . $row["Postal"] . "*";
    }
} else {
    echo "Error fetching data";
}
?>
