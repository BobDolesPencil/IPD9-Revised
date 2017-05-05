
<?php

// CONECT TO DATABASE
session_start();

$db_host = "localhost";
$db_user = "shoutbox";
$db_pass = "fZ1r7pk5QVmEGAqF";
$db_name = "shoutbox";


$link = mysqli_connect($db_host, $db_user, $db_pass, $db_name);

if (!$link) {
    echo "Error: Unable to connect to MySQL." . PHP_EOL;
    echo "Debugging errno: " . mysqli_connect_errno() . PHP_EOL;
    echo "Debugging error: " . mysqli_connect_error() . PHP_EOL;
    exit;
}

// CONNECT TO DATABASE ----- END
// BEGIN FORM FUNCTION
function get_form($n = "", $m = "") {
    // use of heredoc
    $form = <<< ENDOFIT
            <h1>Shout Box</h1>
<form method="post">
    Name: <input type="text" name="name" value="$n" ><br>
    Message: <br><textarea name="message">$m</textarea><br><br>
    <input type="submit" value="Shout!"><br>
    
    
</form>
ENDOFIT;
    return $form;
}
// END FORM FUNCTION
if(isset($_POST['name']))
{
    // extract variables
    $name = $_POST['name'];
    $message = $_POST['message'];
    
    // verify data submitted
    $errorList = array();

    
    if(strlen($name) <3 || strlen($name) >20 || !preg_match("#^[a-zA-Z0-9]+$#", $name)){
        array_push($errorList, "Name must between 3 and 20 charecters and contain <i>ONLY</i>"
                ." letter and numbers.");
    }
    if(strlen($message)<1){
        array_push($errorList, "Message can not be empty");
    }
    //
    if(!$errorList)
    {//Stae 2: successful submission
        
        // SQL INJECTION prevention ---- required on all queries
        $query = sprintf("INSERT INTO shouts VALUES (NULL,'%s','%s')",
        mysqli_real_escape_string($link,$name),
        mysqli_real_escape_string($link,$message)
         );
        $result = mysqli_query($link, $query);
        //echo "------------------" .$query;
        if (!$result) {
    echo "Error: Can not execute Query." . PHP_EOL;
    echo "Debugging errno: " . mysqli_errno($link) . PHP_EOL;
    echo "Debugging error: " . mysqli_errno($link) . PHP_EOL;
    exit;
    }
    echo get_form($name);
    echo '<p>Success! Message posted!</p>';
    
        
    }else{
        //State 3: failed submission
        echo get_form($name);
        
        echo '<p class="error">Error in your submission:</p>';
        echo "\n<ul>\n";
        foreach ($errorList as $error){
            echo "<li>$error</li>";
        }
        echo "</ul>\n";
    }
    //echo get_form($name);
}else {
    // first show
    echo get_form();
}


//--------------------------- display shouts ----------------
    
    $query2 = "SELECT * FROM shouts"
        . " ORDER BY id DESC"
        . " LIMIT 10";
$result2 = mysqli_query($link, $query2);
if (!$result2) {
    echo "Error executing SQL query: " . PHP_EOL;
    echo "Debugging errno: " . mysqli_errno($link) . PHP_EOL;
    echo "Debugging error: " . mysqli_error($link) . PHP_EOL;
    exit;
}
echo '<ul>';
while ($shout = mysqli_fetch_assoc($result2)) {
  
    echo '<li><b>' . $shout['name'] . ' </b>wrote: <i>' . $shout['message'] . '</i></li>';
}
//-------------------------------- display shouts END -------------
?>

