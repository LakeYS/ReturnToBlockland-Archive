//#############################################################################
//#
//#   Return to Blockland - Version 3.0
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 39 $
//#      $Date: 2009-02-23 10:45:55 +0000 (Mon, 23 Feb 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/trunk/old/dedicated.cs $
//#
//#      $Id: dedicated.cs 39 2009-02-23 10:45:55Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Dedicated Handling
//#
//#############################################################################

//*********************************************************
//* Load Prerequisites
//*********************************************************
exec("./RTBH_Support.cs");

//*********************************************************
//* Load Modules
//*********************************************************
exec("./RTBD_Updater.cs");

//*********************************************************
//* Dedicated Help
//*********************************************************
//- rtbHelp (displays a help menu)
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
//- promptRTBSetup (displays a setup guide)
function promptRTBSetup()
{
   if($Pref::Server::RTBSetup::User !$= "")
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
      RTBDU_drawDOSRow("setupRTBDedicated(\"YourUsername\");");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("YourUsername is the username you use for your BL ID ("@getNumKeyID()@")");
      RTBDU_drawDOSRow("that runs this server.");
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
      RTBDU_drawDOSRow("setupRTBDedicated(\"YourUsername\");");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("YourUsername is the username you use for your BL ID ("@getNumKeyID()@")");
      RTBDU_drawDOSRow("that runs this server.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow(" - OR - ");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Simply join your dedicated server to do this automatically.");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
   }
}

//- setupRTBDedicated (a command to setup the server for RTB usage)
function setupRTBDedicated(%name)
{
   if(%name $= "")
   {
      echo("Usage: setupRTBDedicated(\"your username\");");
      return;
   }
   
   $Pref::Server::RTBSetup::User = %name;
   $Pref::Player::NetName = %name;
   
   export("$Pref::Server*","config/server/prefs.cs");
   export("$Pref::*","config/client/prefs.cs");
   
   RTBSA_Post();
}

//*********************************************************
//* Runtime
//*********************************************************
if($Pref::Server::RTBSetup::User !$= "")
{
   $Pref::Player::NetName = $Pref::Server::RTBSetup::User;
}
else
{
   schedule(5000,0,"promptRTBSetup");
}

//*********************************************************
//* Support Functions
//*********************************************************
//- RTBDU_drawDOSRow (draws a centered string of text in a box)
function RTBDU_drawDOSRow(%string)
{
   %boxStart = ((80-70)-2)/2;
   %white = RTBDU_getWhitespace(%boxStart);
   
   %edgeSpace = (68-strLen(%string))/2;
   if(strPos(%edgeSpace,".5") >= 0)
      %minus = 1;
   %space = RTBDU_getWhitespace(%edgeSpace);
   %space2 = RTBDU_getWhitespace(%edgeSpace-%minus);
   
   echo(%white@"*"@%space@%string@%space2@"*");
}

//- RTBDU_drawSpacer (draws a spacer)
function RTBDU_drawSpacer()
{
   echo("    **********************************************************************");
}

//- RTBDU_getWhitespace (generates a string of whitespace for padding)
function RTBDU_getWhitespace(%length)
{
   for(%i=0;%i<%length;%i++)
   {
      %white = %white@" ";
   }
   return %white;
}