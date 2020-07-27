//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 405 $
//#      $Date: 2011-03-05 22:09:06 +0000 (Sat, 05 Mar 2011) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/client.cs $
//#
//#      $Id: client.cs 405 2011-03-05 22:09:06Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Legacy File
//#
//#############################################################################

//Load Real Module
if(!$RTB::Hooks::GuiTransfer)
   exec("Add-Ons/System_ReturnToBlockland/hooks/guiTransfer.cs");

//Set old indication var
$RTB::RTBR_GUITransfer_Hook = 1;

echo("\c2WARNING: RTBR_GUITransfer_Hook.cs has moved. Please alter to use hooks/guiTransfer.cs");