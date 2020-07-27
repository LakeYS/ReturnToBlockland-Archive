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
//#   Interface / Start Mission Gui Changes
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Interface::StartMission = 1;

// Place color manager button on start mission gui
function interfaceFunction()
{
   for(%i=0;%i<startMissionGui.getObject(0).getCount();%i++)
   {
      %ctrl = startMissionGui.getObject(0).getObject(%i);
      if(%ctrl.text $= "Add-Ons")
      {
         %btn = new GuiBitmapButtonCtrl()
         {
            profile = BlockButtonProfile;
            horizSizing = "right";
            vertSizing = "top";
            position = vectorAdd(%ctrl.position,"120 0");
            extent = "113 19";
            command = "canvas.pushDialog(RTB_ColorManager);";
            text = "Color Manager";
            bitmap = "base/client/ui/button1";
            mColor = "255 255 255 255";
         };
         startMissionGui.getObject(0).add(%btn);
         break;
      }
   }
}
interfaceFunction();