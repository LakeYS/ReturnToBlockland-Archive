//#############################################################################
//#
//#   Return to Blockland - Version 2.0
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
$RTB::Version = "2b8";
$RTB::Path = "Add-Ons/System_ReturnToBlockland/";

//*********************************************************
//* Load Preferences
//*********************************************************
if(isFile("config/client/RTB/prefs.cs"))
	exec("config/client/RTB/prefs.cs");
else
{
   //Cheeky!
   exec("config/server/ADD_ON_LIST.cs");
   $AddOn__System_ReturnToBlockland = 1;
   export("$AddOn__","config/server/ADD_ON_LIST.cs");
   
   $RTB::Options::AllowAnyAdd = 0;
   $RTB::Options::AllowBuildAdd = 2;
   $RTB::Options::AllowFullAdd = 1;
   $RTB::Options::AuthWithRTB = 1;
   $RTB::Options::AutoSignIn = 1;
   $RTB::Options::CheckForUpdates = 1;
   $RTB::Options::DownloadScreenshots = 1;
   $RTB::Options::DownloadUpdates = 1;
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
	alxPlay(Note10Sound);
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
//* RTB Support Functions
//*********************************************************
function RTB_LoadDefaultPrefs()
{
   $RTB::Options::AllowAnyAdd = 0;
   $RTB::Options::AllowBuildAdd = 2;
   $RTB::Options::AllowFullAdd = 1;
   $RTB::Options::AuthWithRTB = 1;
   $RTB::Options::AutoSignIn = 1;
   $RTB::Options::CheckForUpdates = 1;
   $RTB::Options::DownloadScreenshots = 1;
   $RTB::Options::DownloadUpdates = 1;
   $RTB::Options::IRCAllowPM = 1;
   $RTB::Options::IRCAutoConnect = 1;
   $RTB::Options::IRCChannel = "#rtb";
   $RTB::Options::LoginWithProfile = 1;
   $RTB::Options::PMAudioNotify = 0;
   $RTB::Options::PMVisualNotify = 1;
   $RTB::Options::PostServer = 1;
   $RTB::Options::PrivacyShowOnline = 1;
   $RTB::Options::PrivacyShowOwnership = 0;
   $RTB::Options::PrivacyShowPassworded = 1;
   $RTB::Options::PrivacyShowPlayers = 1;
   $RTB::Options::PrivacyShowServer = 1;
   $RTB::Options::HideNonRTBUsers = 1;
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