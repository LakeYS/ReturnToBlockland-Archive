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
//#   Interface / Add-Ons Gui Changes
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Interface::AddOns = 1;

// Replace add-ons gui with RTB modification
function interfaceFunction()
{
   AddOnsGui.delete();
   
   exec("Add-Ons/System_ReturnToBlockland/interface/addOns.gui");
}
interfaceFunction();