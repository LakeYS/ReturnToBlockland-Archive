//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 283 $
//#      $Date: 2012-08-12 12:33:12 +0100 (Sun, 12 Aug 2012) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/interface/mods/mainmenu.cs $
//#
//#      $Id: mainmenu.cs 283 2012-08-12 11:33:12Z Ephialtes $
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
   MainMenuButtonsGui.add(%version);
}
interfaceFunction();