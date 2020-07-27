//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 522 $
//#      $Date: 2011-10-30 11:13:52 +0000 (Sun, 30 Oct 2011) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/dedicated.cs $
//#
//#      $Id: dedicated.cs 522 2011-10-30 11:13:52Z Ephialtes $
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
//* Activate Packages
//*********************************************************
activatePackage(RTB_Support_Networking);