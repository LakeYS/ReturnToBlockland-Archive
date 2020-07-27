//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 120 $
//#      $Date: 2010-10-02 20:15:02 +0100 (Sat, 02 Oct 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/modules/client/guiControl.cs $
//#
//#      $Id: guiControl.cs 120 2010-10-02 19:15:02Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Modules / Client / GUI Control
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::GUIControl = 1;

//*********************************************************
//* Client GUI Control
//*********************************************************
//- clientCmdRTB_OpenGui (Opens an rtb gui)
function clientCmdRTB_OpenGui(%gui)
{
   if(strPos(%gui,"RTB_") $= 0)
      Canvas.pushDialog(%gui);
}

//- clientCmdRTB_CloseGui (Closes an rtb gui)
function clientCmdRTB_CloseGui(%gui)
{
   if(strPos(%gui,"RTB_") $= 0)
      Canvas.popDialog(%gui);
}