<?php

//This should be the unique key assigned to this site
$SITE_KEY = "F57AE67C-56E1-4C7B-9A4F-3211BCA6E76A";

if(isset($_GET['Category-Name']))
  $CategoryName = $_GET['Category-Name'];
else
  $CategoryName = "";

//Set the user agent
ini_set('user_agent','NaturalLinkExchange.com PHP Client');

$url = "http://www.NaturalLinkExchange.com/ServerServices/LinksHtml/?sk=".$SITE_KEY."&cn=".$CategoryName;

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