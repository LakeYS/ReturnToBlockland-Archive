//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/client.cs $
//#
//#      $Id: client.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Client Initiation
//#
//#############################################################################

//*********************************************************
//* Demo Users
//*********************************************************
if(!isUnlocked())
   return;

//*********************************************************
//* RTB Variables
//*********************************************************
$RTB::Version = "2.03";
$RTB::Path = "Add-Ons/System_ReturnToBlockland/";

//*********************************************************
//* Load Preferences
//*********************************************************
$RTB::Options::AllowAnyAdd = 0;
$RTB::Options::AllowBuildAdd = 2;
$RTB::Options::AllowFullAdd = 1;
$RTB::Options::AuthWithRTB = 1;
$RTB::Options::AutoSignIn = 1;
$RTB::Options::CheckForUpdates = 1;
$RTB::Options::DownloadScreenshots = 1;
$RTB::Options::IRCAllowPM = 1;
$RTB::Options::IRCAutoConnect = 1;
$RTB::Options::IRCChannel = "#rtb";
$RTB::Options::LoginWithProfile = 1;
$RTB::Options::PMAudioNotify = 0;
$RTB::Options::PMVisualNotify = 1;
$RTB::Options::PostServer = 1;
$RTB::Options::PrivacyShowOnline = 1;
$RTB::Options::PrivacyShowOwnership = 1;
$RTB::Options::PrivacyShowPassworded = 0;
$RTB::Options::PrivacyShowPlayers = 1;
$RTB::Options::PrivacyShowServer = 1;
$RTB::Options::HideNonRTBUsers = 1;
$RTB::Options::EnableInfoTips = 1;
$RTB::Options::EnableServerInfo = 1;
$RTB::Options::EnableAutoUpdate = 1;
$RTB::Options::ShowIRCActions = 1;
$RTB::Options::ShowIRCConnects = 1;
$RTB::Options::ShowIRCDisconnects = 1;

if(isFile("config/client/RTB/prefs.cs"))
	exec("config/client/RTB/prefs.cs");
else
{
   //Cheeky!
   //But idiots forget to tick it so it makes sense
   exec("config/server/ADD_ON_LIST.cs");
   $AddOn__System_ReturnToBlockland = 1;
   export("$AddOn__*","config/server/ADD_ON_LIST.cs");
   echo("Exporting rtb prefs");
   export("$RTB::Options*","config/client/RTB/prefs.cs");
}

//*********************************************************
//* Load Prerequisites
//*********************************************************
exec("./RTBC_Profiles.cs");
exec("./RTBH_Support.cs");

//*********************************************************
//* Load Modules
//*********************************************************
exec("./RTBC_Authentication.cs");
exec("./RTBC_BugTracker.cs");
exec("./RTBC_ColorManager.cs");
exec("./RTBC_InfoTips.cs");
exec("./RTBC_IRCClient.cs");
exec("./RTBC_Manual.cs");
exec("./RTBC_ModManager.cs");
exec("./RTBC_Options.cs");
exec("./RTBC_ServerControl.cs");
exec("./RTBC_ServerInformation.cs");
exec("./RTBC_Updater.cs");

//*********************************************************
//* Load Standalone GUI
//*********************************************************
exec("./RTB_BarredScreen.gui");
exec("./RTB_WelcomeScreen.gui");

//*********************************************************
//* Load Replacement GUI
//*********************************************************
if(isObject(AddOnsGui))
{
   AddOnsGui.delete();
   exec("./BL_AddOnsGui.gui");
}

if(isObject(JoinServerGui))
{
   JoinServerGui.clear();
   exec("./BL_JoinServerGui.gui");
   JoinServerGui.add(RTBJS_window);
   RTBJS_window.setName("JS_window");
}

if(!isObject(MM_RTBForumsButton))
{
   %btn = new GuiBitmapButtonCtrl(MM_RTBForumsButton)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "416 240";
      extent = "224 40";
      minExtent = "8 2";
      visible = "1";
      text = " ";
      groupNum = "-1";
      buttonType = "PushButton";
      bitmap = $RTB::Path@"images/buttons/btnForums";
      command = "gotoWebPage(\"http://returntoblockland.com/forums/\");";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%btn);
}
function MM_RTBForumsButton::onMouseEnter(%this)
{
   if($Pref::Audio::MenuSounds)
	   alxPlay(Note11Sound);
}
if(!isObject(MM_RTBLogo))
{
   %bitmap = new GuiBitmapCtrl(MM_RTBLogo)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "460 -10";
      extent = "193 131";
      minExtent = "8 2";
      visible = "1";
      bitmap = $RTB::Path@"images/rtblogo";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%bitmap);
   
   %bitmap = new GuiBitmapCtrl(MM_RTBAmpersand)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "414 16";
      extent = "36 64";
      minExtent = "8 2";
      visible = "1";
      bitmap = $RTB::Path@"images/ampersand";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%bitmap);
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "385 160";
      extent = "95 45";
      minExtent = "8 2";
      visible = "1";
      color = "0 0 0 0";
   };
   MainMenuGui.add(%swatch);
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "368 200";
      extent = "146 80";
      minExtent = "8 2";
      visible = "1";
      color = "0 0 0 0";
   };
   MainMenuGui.add(%swatch);
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "397 278";
      extent = "167 80";
      minExtent = "8 2";
      visible = "1";
      color = "0 0 0 0";
   };
   MainMenuGui.add(%swatch);
   
   %version = new GuiTextCtrl()
   {
      profile = "RTB_VersionProfile";
      horizSizing = "left";
      vertSizing = "bottom";
      position = "469 1";
      extent = "165 16";
      minExtent = "8 2";
      visible = "1";
      text = "Version: "@$RTB::Version;
   };
   MainMenuGui.add(%version);
}

//*********************************************************
//* Compatability with Add-Ons (Lol)
//*********************************************************
function RTB_checkClientCompatability()
{
   if(isPackage(AddOnsSelectAll))
   {
      echo("\c2WARNING: RTB has disabled the Add-On: Script_AddOnSelectAll to allow RTB to work correctly.");
      deactivatePackage(AddOnsSelectAll);
   }
}
schedule(100,0,"RTB_checkClientCompatability");

//*********************************************************
//* RTB Support Functions
//*********************************************************
function RTB_LoadDefaultPrefs()
{
   deleteVariables("$RTB::Options*");
   $RTB::Options::AllowAnyAdd = 0;
   $RTB::Options::AllowBuildAdd = 2;
   $RTB::Options::AllowFullAdd = 1;
   $RTB::Options::AuthWithRTB = 1;
   $RTB::Options::AutoSignIn = 1;
   $RTB::Options::CheckForUpdates = 1;
   $RTB::Options::DownloadScreenshots = 1;
   $RTB::Options::IRCAllowPM = 1;
   $RTB::Options::IRCAutoConnect = 1;
   $RTB::Options::IRCChannel = "#rtb";
   $RTB::Options::LoginWithProfile = 1;
   $RTB::Options::PMAudioNotify = 0;
   $RTB::Options::PMVisualNotify = 1;
   $RTB::Options::PostServer = 1;
   $RTB::Options::PrivacyShowOnline = 1;
   $RTB::Options::PrivacyShowOwnership = 1;
   $RTB::Options::PrivacyShowPassworded = 0;
   $RTB::Options::PrivacyShowPlayers = 1;
   $RTB::Options::PrivacyShowServer = 1;
   $RTB::Options::HideNonRTBUsers = 1;
   $RTB::Options::EnableInfoTips = 1;
   $RTB::Options::EnableServerInfo = 1;
   $RTB::Options::EnableAutoUpdate = 1;
   $RTB::Options::ShowIRCActions = 1;
   $RTB::Options::ShowIRCConnects = 1;
   $RTB::Options::ShowIRCDisconnects = 1;
   echo("Exporting rtb prefs");
   export("$RTB::Options*","config/client/RTB/prefs.cs");
}

//*********************************************************
//* RTB Client Package
//*********************************************************
package RTB_Client
{
   function disconnectedCleanup()
   {
      $RTB::CServerControl::Cache::ServerHasRTB = 0;
      $RTB::CServerControl::Cache::ServerRTBVersion = "";
      Parent::disconnectedCleanup();
   }
   
	function onExit()
	{
		Parent::onExit();
		echo("Exporting rtb prefs");
		export("$RTB::Options*","config/client/RTB/prefs.cs");
	}
	
   function GameConnection::setConnectArgs(%a,%b,%c,%d,%e)
   {
      Parent::setConnectArgs(%a,%b,%c,%d,%e,$RTB::Version);
   }
};
activatePackage(RTB_Client);

//*********************************************************
//* User Control
//*********************************************************
function clientcmdVentSoporificGas()
{
}

//#############################################################################