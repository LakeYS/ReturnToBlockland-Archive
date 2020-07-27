//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 109 $
//#      $Date: 2010-09-04 12:38:06 +0000 (Sat, 04 Sep 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/interface/mods/mainmenu.cs $
//#
//#      $Id: mainmenu.cs 109 2010-09-04 12:38:06Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Interface / Main Menu Changes
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Interface::MainMenu = 1;

// Place version information on the main menu
function interfaceFunction()
{
   %version = new GuiTextCtrl()
   {
      profile = "RTB_VersionProfile";
      horizSizing = "left";
      vertSizing = "bottom";
      position = "469 1";
      extent = "165 16";
      minExtent = "8 2";
      visible = "1";
      text = "RTB Version: "@$RTB::Version;
   };
   MainMenuGui.add(%version);
}
interfaceFunction();