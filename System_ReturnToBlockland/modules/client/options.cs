//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 259 $
//#      $Date: 2011-11-05 00:41:50 +0000 (Sat, 05 Nov 2011) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/modules/client/options.cs $
//#
//#      $Id: options.cs 259 2011-11-05 00:41:50Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Modules / Client / Options
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::Options = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::MCO::Options = 0;

//*********************************************************
//* Module Meat
//*********************************************************
//- RTBCO_registerOption (registers a pref that rtb needs to save/load/manage)
function RTBCO_registerOption(%varName,%value)
{
   $RTB::MCO::OptionName[$RTB::MCO::Options] = %varName;
   $RTB::MCO::OptionDefault[$RTB::MCO::Options] = %value;
   $RTB::MCO::Options++;
}

//- RTBCO_getPref (returns value of a pref)
function RTBCO_getPref(%varName)
{
   eval("%return = $RTB::Options::"@%varName@";");
   
   return %return;
}

//- RTBCO_setPref (sets value of a pref)
function RTBCO_setPref(%varName,%value)
{
   eval("$RTB::Options::"@%varName@" = \""@%value@"\";");
}

//- RTBCO_setDefaultPrefs (sets the pref values to default prefs)
function RTBCO_setDefaultPrefs()
{
   for(%i=0;%i<$RTB::MCO::Options;%i++)
      eval("$RTB::Options::"@$RTB::MCO::OptionName[%i]@" = \""@$RTB::MCO::OptionDefault[%i]@"\";");
}

//- RTBCO_Save (saves all the settings)
function RTBCO_Save()
{
   export("$RTB::Options::*","config/client/rtb/prefs.cs");
}

//*********************************************************
//* GUI Functionality
//*********************************************************
//- RTB_Options::onWake (options wake)
function RTB_Options::onWake(%this)
{
   %binding = strReplace(getField(GlobalActionMap.getBinding("RTB_toggleOverlay"),1)," "," + ");
   
   RTBCO_Overlay_Keybind.setText("<font:Verdana:12><color:888888><just:right>" @ %binding);
   
   for(%i=0;%i<RTBCO_Swatch.getCount();%i++)
   {
      %ctrl = RTBCO_Swatch.getObject(%i);
      if(%ctrl.getClassName() !$= "GuiCheckboxCtrl")
         continue;
         
      %value = RTBCO_getPref(%ctrl.optionName);

      if(%ctrl.getValue() !$= %value)
         %ctrl.performClick();
   }
}

//- RTB_Options::onSleep (options sleep)
function RTB_Options::onSleep(%this)
{
   for(%i=0;%i<RTBCO_Swatch.getCount();%i++)
   {
      %ctrl = RTBCO_Swatch.getObject(%i);
      if(%ctrl.getClassName() !$= "GuiCheckboxCtrl")
         continue;

      RTBCO_setPref(%ctrl.optionName,%ctrl.getValue());
   }
   RTBCO_Save();
   
   if(isObject(RTBCO_OV_Remap))
   {
      RTBCO_OV_Remap.delete();
      RTBCO_OV_RemapSwatch.setVisible(false);
   }
   RTB_Client_Authentication.sendPrefs();
}

//- RTB_Options::toggleDependant (toggles dependant blocker)
function RTB_Options::toggleDependant(%this,%ctrl)
{
   if(%ctrl.getValue() $= 1)
      %ctrl.optionDependant.setVisible(false);
   else
      %ctrl.optionDependant.setVisible(true);
}

//- RTB_Options::changeKeybind (opens keybind change window)
function RTB_Options::changeKeybind(%this)
{
   RTBCO_OV_RemapSwatch.setVisible(true);
   
   %remapper = new GuiInputCtrl(RTBCO_OV_Remap);
   RTBCO_OV_RemapSwatch.add(%remapper);
   %remapper.makeFirstResponder(1);
   
   RTBCO_OV_RemapText.setText("<font:Verdana:12><color:888888>Please press a key ...");
}

//- RTBCO_OV_Remap::onInputEvent (captures input and assigns)
function RTBCO_OV_Remap::onInputEvent(%this,%device,%key)
{
   if(%device $= "mouse0")
      return;
      
   if(getWordCount(%key) $= 1 && strLen(%key) $= 1)
   {
      %disallowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789[]\\/{};:'\"<>,./?!@#$%^&*-_=+`~";
      for(%i=0;%i<strLen(%disallowed);%i++)
      {
         %char = getSubStr(%disallowed,%i,1);
         if(%char $= %key)
         {
            RTBCO_OV_RemapText.setText("<font:Verdana:12><color:888888>You can't use that key.");
            return;
         }
      }
   }
      
   RTBCO_OV_RemapSwatch.setVisible(false);
   RTBCO_Overlay_Keybind.setText("<font:Verdana:12><color:888888><just:right>" @ strReplace(%key," "," + "));
   
   %binding = GlobalActionMap.getBinding("RTB_toggleOverlay");
   GlobalActionMap.unbind(getField(%binding,0),getField(%binding,1));
   GlobalActionMap.bind(%device,%key,"RTB_toggleOverlay");
   
   RTBCO_setPref("OV::OverlayKeybind",%device TAB %key);
   RTBCO_Save();
   
   %this.delete();
}

//*********************************************************
//* Register Options
//*********************************************************
RTBCO_registerOption("OV::OverlayKeybind","keyboard\tshift tab");
RTBCO_registerOption("OV::EscapeClose",0);

RTBCO_registerOption("CA::Auth",1);
RTBCO_registerOption("CA::ShowOnline",1);
RTBCO_registerOption("CA::ShowServer",1);

RTBCO_registerOption("SA::Auth",1);
RTBCO_registerOption("SA::ShowPlayers",1);

RTBCO_registerOption("MM::Animate",1);
RTBCO_registerOption("MM::CheckUpdates",1);

RTBCO_registerOption("IT::Enable",1);
RTBCO_registerOption("IT::ShowAddons",1);

RTBCO_registerOption("GT::Enable",1);

RTBCO_registerOption("SC::NotifySettings",0);

RTBCO_registerOption("CC::AutoSignIn",1);
RTBCO_registerOption("CC::EnableSounds",1);
RTBCO_registerOption("CC::StickyNotifications",1);
RTBCO_registerOption("CC::ChatLogging",0);
RTBCO_registerOption("CC::SeparateOffline",0);
RTBCO_registerOption("CC::ShowTimestamps",0);
RTBCO_registerOption("CC::SavePositions",1);
RTBCO_registerOption("CC::InviteReq",0);
RTBCO_registerOption("CC::ShowServer",1);
RTBCO_registerOption("CC::AllowPM",1);
RTBCO_registerOption("CC::AllowInvites",1);
RTBCO_registerOption("CC::SignIn::Beep",1);
RTBCO_registerOption("CC::SignIn::Note",0);
RTBCO_registerOption("CC::Message::Beep",1);
RTBCO_registerOption("CC::Message::Note",1);
RTBCO_registerOption("CC::Join::Beep",1);
RTBCO_registerOption("CC::Join::Note",0);
RTBCO_registerOption("CC::PirateMode",0);