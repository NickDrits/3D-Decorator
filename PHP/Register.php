<?php
// Error reporting
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect('localhost', 'root', '', 'hy360_2023');

// Check connection
if (mysqli_connect_errno()) {
    echo "1"; 
    exit();
}

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

// Check username 
$namecheckquery = "SELECT Username FROM client WHERE Username='" . $username . "';";
$namecheck = mysqli_query($con, $namecheckquery) or die("2");

if (mysqli_num_rows($namecheck) > 0) {
    echo "3"; // Username exists
    exit();
}

// Check email 
$emailcheckquery = "SELECT Email FROM client WHERE Email='" . $email . "';";
$emailcheck = mysqli_query($con, $emailcheckquery) or die("4");

if (mysqli_num_rows($emailcheck) > 0) {
    echo "5"; // Email exists
    exit();
}

// Hash the password 
$hashedPassword = password_hash($password, PASSWORD_BCRYPT);

// Insert new client 
$insertquery = "INSERT INTO client (Fname, Sname, Username, Email, Password, Phone, Day, Month, Year, Country, Town, Street, Number, Postal) 
VALUES ('" . $fname . "', '" . $sname . "', '" . $username . "', '" . $email . "', '" . $hashedPassword . "', '" . $phone . "', '" . $day . "', '" . $month . "', '" . $year . "', '" . $country . "', '" . $town . "', '" . $street . "', '" . $number . "', '" . $postal . "');";

if (!mysqli_query($con, $insertquery)) {
    echo "6"; // Insert query failed
    exit();
}

echo "0"; // Success
?>
