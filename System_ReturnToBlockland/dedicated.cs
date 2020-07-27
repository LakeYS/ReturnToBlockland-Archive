//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/dedicated.cs $
//#
//#      $Id: dedicated.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Dedicated Handling
//#
//#############################################################################

//*********************************************************
//* Dedicated Help
//*********************************************************
function rtbHelp()
{
   RTBDU_drawSpacer();
   RTBDU_drawDOSRow("");
   RTBDU_drawDOSRow("Setting up RTB:");
   RTBDU_drawDOSRow("Enter promptRTBSetup(); for more help.");
   RTBDU_drawDOSRow("");
   RTBDU_drawDOSRow("");
   RTBDU_drawDOSRow("Updating RTB:");
   RTBDU_drawDOSRow("Check for updates by typing checkRTBUpdates();");
   RTBDU_drawDOSRow("");
   RTBDU_drawSpacer();
}

//*********************************************************
//* Setting up the Server
//*********************************************************
function promptRTBSetup()
{
   if($Pref::Server::RTB::Setup)
   {
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Your server appears to be working with RTB!");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Type the following command into the dedicated server");
      RTBDU_drawDOSRow("to change setup options:");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("setupRTBDedicated(\"YourUsername\",1);");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("YourUsername is the username you use for your BL ID ("@getNumKeyID()@")");
      RTBDU_drawDOSRow("that runs this server and the 1 is so people");
      RTBDU_drawDOSRow("can see the server players and add-ons it has.");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
   }
   else
   {
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Your server has not been setup to work with RTB yet!");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Type the following command into the dedicated server");
      RTBDU_drawDOSRow("to complete setup:");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("setupRTBDedicated(\"YourUsername\",1);");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("YourUsername is the username you use for your BL ID ("@getNumKeyID()@")");
      RTBDU_drawDOSRow("that runs this server and the 1 is so people");
      RTBDU_drawDOSRow("can see the server players and add-ons it has.");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
   }
}

function setupRTBDedicated(%name,%post)
{
   if(%name $= "" || %post $= "")
   {
      echo("Usage: setupRTBDedicated(\"your username\", allow users to see server info (1 or 0));");
      return;
   }
   
   if(%post !$= "0")
      %post = 1;
   
   $Pref::Server::RTB::Setup = 1;
   $Pref::Server::RTB::Username = %name;
   $Pref::Server::RTB::Post = %post;
   
   $RTB::Options::PostServer = %post;
   $Pref::Player::NetName = %name;
   
   export("$Pref::Server*","config/server/prefs.cs");
   export("$Pref::*","config/client/prefs.cs");
   
   echo("rtb is now setup on your server");
   RTBSA_Post();
}

if($Pref::Server::RTB::Setup)
{
   $RTB::Options::PostServer = $Pref::Server::RTB::Post;
   $Pref::Player::netName = $Pref::Server::RTB::Username;
}
else
{
   schedule(5000,0,"promptRTBSetup");
}

//#############################################################################