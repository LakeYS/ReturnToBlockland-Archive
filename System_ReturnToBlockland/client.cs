//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 493 $
//#      $Date: 2013-04-21 12:48:33 +0100 (Sun, 21 Apr 2013) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/client.cs $
//#
//#      $Id: client.cs 493 2013-04-21 11:48:33Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Client Initiation
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Client = 1;

//*********************************************************
//* Debug
//* -------------------------------------------------------
//* Level 0 -> Off
//* Level 1 -> Low
//* Level 2 -> High
//* Level 3 -> API Profiling/Debugging (DO NOT USE)
//*********************************************************
$RTB::Debug = 0;

//*********************************************************
//* Variables
//*********************************************************
$RTB::Version = "4.05";
$RTB::Path = "Add-Ons/System_ReturnToBlockland/";

//*********************************************************
//* Demo Users
//* -------------------------------------------------------
//* Believe it or not this is here for a reason.
//*********************************************************
if(!isUnlocked())
{
   $RTB::Client = 0;
   echo("\c2ERROR: RTB failed to load because you are in demo mode.");
   return;
}

//*********************************************************
//* RTB Group for keeping stuff in
//*********************************************************
if(!isObject(RTBGroup))
   new SimGroup(RTBGroup);

//*********************************************************
//* Load Prefs
//*********************************************************
exec("./modules/client/options.cs");
RTBCO_setDefaultPrefs();
if(isFile("config/client/rtb/prefs.cs"))
   exec("config/client/rtb/prefs.cs");
   
//*********************************************************
//* Always enable RTB
//*********************************************************
if(isFile("config/server/ADD_ON_LIST.cs"))
   exec("config/server/ADD_ON_LIST.cs");
else
   exec("base/server/defaultAddonList.cs");
$AddOn__System_ReturnToBlockland = 1;
export("$AddOn__*","config/server/ADD_ON_LIST.cs");

//*********************************************************
//* Load GUI Profiles
//*********************************************************
exec("./interface/profiles/generic.cs");
exec("./interface/profiles/connectClient.cs");
exec("./interface/profiles/infoTips.cs");
exec("./interface/profiles/modManager.cs");

//*********************************************************
//* Load Support
//*********************************************************
exec("./support/fileCache.cs");
exec("./support/functions.cs");
exec("./support/gui.cs");
exec("./support/networking.cs");
exec("./support/overlay.cs");
exec("./support/xmlParser.cs");

//*********************************************************
//* Runtime Functions
//*********************************************************
RTB_FileCache.refresh();

//*********************************************************
//* Load Interface
//*********************************************************
exec("./interface/overlay.gui");
exec("./interface/mods/addOns.cs");
exec("./interface/mods/joinServer.cs");
exec("./interface/mods/loading.cs");
exec("./interface/mods/mainMenu.cs");
exec("./interface/mods/customGame.cs");
exec("./interface/colorManager.gui");
exec("./interface/connectClient.gui");
exec("./interface/manual.gui");
exec("./interface/modManager.gui");
exec("./interface/modUpdates.gui");
exec("./interface/options.gui");
exec("./interface/serverControl.gui");
exec("./interface/serverInformation.gui");
exec("./interface/updater.gui");

//*********************************************************
//* Load Modules
//*********************************************************
exec("./modules/client/authentication.cs");
exec("./modules/client/colorManager.cs");
exec("./modules/client/connectClient.cs");
exec("./modules/client/guiControl.cs");
exec("./modules/client/guiTransfer.cs");
exec("./modules/client/infoTips.cs");
exec("./modules/client/manual.cs");
exec("./modules/client/modManager.cs");
exec("./modules/client/serverControl.cs");
exec("./modules/client/serverInformation.cs");
exec("./modules/client/updater.cs");

//*********************************************************
//* Activate Packages
//*********************************************************
activatePackage(RTB_Modules_Client_Authentication);
activatePackage(RTB_Modules_Client_ColorManager);
activatePackage(RTB_Modules_Client_ConnectClient);
activatePackage(RTB_Modules_Client_InfoTips);
activatePackage(RTB_Modules_Client_GuiTransfer);
activatePackage(RTB_Modules_Client_ServerControl);
activatePackage(RTB_Modules_Client_ServerInformation);

//*********************************************************
//* Version Establishment
//*********************************************************
//- clientCmdSendRTBVersion (Receives the server's RTB version and whether it has RTB)
function clientCmdSendRTBVersion(%version)
{
   $RTB::Client::Cache::ServerHasRTB = 1;
   $RTB::Client::Cache::ServerRTBVersion = firstWord(%version);
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTB_Client
{
   function disconnectedCleanup()
   {
      deleteVariables("$RTB::Client::Cache::*");
      Parent::disconnectedCleanup();
   }
   
	function onExit()
	{
	   if(RTBCC_Socket.connected)
	      RTBCC_Socket.hardDisconnect();
	      
		Parent::onExit();
		echo("Exporting rtb prefs");
		export("$RTB::Options*","config/client/rtb/prefs.cs");
	}
	
   function GameConnection::setConnectArgs(%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p)
   {
      Parent::setConnectArgs(%a,%b,%c,%d,%e,%f,$RTB::Version,%h,%i,%j,%k,%l,%m,%n,%o,%p);
   }
};
activatePackage(RTB_Client);