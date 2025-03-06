<?php

$conn = mysqli_connect('localhost','root','','hy360_2023');

// Get the sort value and cid from the GET request
$sort_value = isset($_GET['sort_value']) ? $_GET['sort_value'] : 'id'; // Default sort by 'id'
$cid = isset($_GET['cid']) ? intval($_GET['cid']) : 0; // Default cid to 0 if not provided

// Validate the sort value to prevent SQL injection
$allowed_columns = ['price', 'transport', 'state', 'num_of_prod', 'day', 'month', 'year', 'cid']; // Add allowed column names here

// Determine the ORDER BY clause based on sort_value
switch ($sort_value) {
    case 'ascending_price':
        $order_by = 'price + transport ASC';
        break;
    case 'descending_price':
        $order_by = 'price + transport DESC';
        break;
    case 'ascending_date':
        $order_by = 'year ASC, month ASC, day ASC';
        break;
    case 'descending_date':
        $order_by = 'year DESC, month DESC, day DESC';
        break;
    default:
        die("Invalid sort value");
}

// Prepare the SQL statement with cid filter
$sql = "SELECT * FROM orders WHERE cid = ? ORDER BY $order_by";

// Prepare and bind
$stmt = $conn->prepare($sql);
if ($stmt === false) {
    die("Failed to prepare the SQL statement: " . $conn->error);
}

// Bind parameters
$stmt->bind_param("i", $cid);

// Execute the statement
$stmt->execute();

// Get the result
$result = $stmt->get_result();

// Check if there are results
if ($result->num_rows > 0) {
    $orders = [];
    while ($row = $result->fetch_assoc()) {
        $order = implode(',', [$row['Oid'], $row['Price'], $row['Transport'], $row['State'], $row['Num_of_prod'], $row['Day'], $row['Month'], $row['Year'], $row['Cid']]);
        $orders[] = $order;
    }
    // Join all orders with *
    echo implode('*', $orders);
} else {
    echo '';
}

// Close connection
$stmt->close();
$conn->close();

?>