//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 274 $
//#      $Date: 2012-07-15 10:55:52 +0100 (Sun, 15 Jul 2012) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/dedicated.cs $
//#
//#      $Id: dedicated.cs 274 2012-07-15 09:55:52Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Dedicated Initiation
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Dedicated = 1;

//*********************************************************
//* Load Prefs
//*********************************************************
exec("./modules/client/options.cs");
RTBCO_setDefaultPrefs();
if(isFile("config/client/rtb/prefs.cs"))
   exec("config/client/rtb/prefs.cs");
   
//*********************************************************
//* Load Support
//*********************************************************
exec("./support/fileCache.cs");
exec("./support/functions.cs");
exec("./support/networking.cs");
exec("./support/xmlParser.cs");

//*********************************************************
//* Dedicated Server Modules
//*********************************************************
// Coming soon? :)

//*********************************************************
//* Activate Packages
//*********************************************************
activatePackage(RTB_Support_Networking);