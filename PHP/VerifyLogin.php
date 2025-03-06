<?php 
$con = mysqli_connect('localhost', 'root', '', 'hy360_2023');

// Check connection
if (mysqli_connect_errno()) {
    echo "Connection failed";
    exit();
}

$username = $_POST["Username"];
$password = $_POST["Password"];

// Query to find the user by username or email
$sql = "SELECT Password FROM client WHERE Username = '$username' OR Email = '$username' LIMIT 1";
$result = mysqli_query($con, $sql);

if (mysqli_num_rows($result) > 0) {
    $row = mysqli_fetch_assoc($result);
    $hashedPassword = $row['Password'];

    // Verify the provided password with the hashed password in the database
    if (password_verify($password, $hashedPassword)) {
        echo "0"; // Login successful
    } else {
        echo "Invalid credentials"; // Password doesn't match
    }
} else {
    echo "Invalid credentials"; // No user found
}
?>
