<?php

//This should be the unique key assigned to this site
$SITE_KEY = "{siteKey}";

if(isset($_GET['Category-Name']))
  $CategoryName = $_GET['Category-Name'];
else
  $CategoryName = "";

//Set the user agent
ini_set('user_agent','NaturalLinkExchange.com PHP Client');

//Get the script name to grab the parameters and the URL format
$ScriptName = $_SERVER["SCRIPT_NAME"];

$url = "http://www.NaturalLinkExchange.com/ServerServices/LinksHtml/?sk=".$SITE_KEY."&cn=".$CategoryName."&sn=".$ScriptName;

//echo $url;

$fp = fopen ($url, "r");
if($fp)
{
	while ((!feof ($fp)))
	{
		$line = fgets($fp, 4096);
		echo $line;
	}
	fclose($fp);
}

?>