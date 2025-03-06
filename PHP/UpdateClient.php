<?php

$con = mysqli_connect('localhost', 'root', '', 'hy360_2023');

// Check connection
if (mysqli_connect_errno()) {
    echo "1"; // Connection failed
    exit();
}

$cid = $_POST["Cid"];
$fname = $_POST["Fname"];
$sname = $_POST["Sname"];
$username = $_POST["Username"];
$email = $_POST["Email"];
$password = $_POST["Password"];
$phone = $_POST["Phone"];
$day = $_POST["Day"];
$month = $_POST["Month"];
$year = $_POST["Year"];
$country = $_POST["Country"];
$town = $_POST["Town"];
$street = $_POST["Street"];
$number = $_POST["Number"];
$postal = $_POST["Postal"];

// Check client ID
$clientCheckQuery = "SELECT * FROM client WHERE Cid='" . $cid . "';";
$clientCheck = mysqli_query($con, $clientCheckQuery) or die("2"); // Client ID not found

if (mysqli_num_rows($clientCheck) == 0) {
    echo "2"; // Client ID not found
    exit();
}

// Check username
$usernameCheckQuery = "SELECT * FROM client WHERE Username='" . $username . "' AND Cid != '" . $cid . "';";
$usernameCheck = mysqli_query($con, $usernameCheckQuery) or die("4"); // Username check query failed

if (mysqli_num_rows($usernameCheck) > 0) {
    echo "3"; // Username already exists
    exit();
}

// Check email 
$emailCheckQuery = "SELECT * FROM client WHERE Email='" . $email . "' AND Cid != '" . $cid . "';";
$emailCheck = mysqli_query($con, $emailCheckQuery) or die("4"); // Email check query failed

if (mysqli_num_rows($emailCheck) > 0) {
    echo "4"; // Email already exists
    exit();
}

// Hash password 
$hashedPassword = password_hash($password, PASSWORD_BCRYPT);

// Update client information
$updateQuery = "UPDATE client SET Fname='" . $fname . "', Sname='" . $sname . "', Username='" . $username . "', Email='" . $email . "', Password='" . $hashedPassword . "', Phone='" . $phone . "', Day='" . $day . "', Month='" . $month . "', Year='" . $year . "', Country='" . $country . "', Town='" . $town . "', Street='" . $street . "', Number='" . $number . "', Postal='" . $postal . "' WHERE Cid='" . $cid . "';";
mysqli_query($con, $updateQuery) or die("5"); // Update client query failed

echo "0"; // Update successful

?>
