//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 187 $
//#      $Date: 2010-01-21 23:51:47 +0000 (Thu, 21 Jan 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/interface/mods/mainmenu.cs $
//#
//#      $Id: mainmenu.cs 187 2010-01-21 23:51:47Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Interface / Join Server Gui Changes
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Interface::JoinServer = 1;

// Replace join server gui with RTB modification
function interfaceFunction()
{
   joinServerGui.clear();
   exec("Add-Ons/System_ReturnToBlockland/interface/joinServer.gui");
   joinServerGui.add(RTBJS_window);
   RTBJS_window.setName("JS_window");
}
interfaceFunction();