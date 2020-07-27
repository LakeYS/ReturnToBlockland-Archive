//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 493 $
//#      $Date: 2013-04-21 12:48:33 +0100 (Sun, 21 Apr 2013) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/server.cs $
//#
//#      $Id: server.cs 493 2013-04-21 11:48:33Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Initiation
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Server = 1;

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
   $RTB::Server = 0;
   echo("\c2ERROR: RTB failed to load because you are in demo mode.");
   return;
}

//*********************************************************
//* RTB Group for keeping stuff in
//*********************************************************
if(!isObject(RTBGroup))
   new SimGroup(RTBGroup);
   
//*********************************************************
//* Dedicated Init
//*********************************************************
if($Server::Dedicated)
   exec("./dedicated.cs");

//*********************************************************
//* Load Modules
//*********************************************************
exec("./modules/server/authentication.cs");
exec("./modules/server/guiTransfer.cs");
exec("./modules/server/serverControl.cs");

//*********************************************************
//* Activate Packages
//*********************************************************
activatePackage(RTB_Modules_Server_Authentication);
activatePackage(RTB_Modules_Server_GuiTransfer);
activatePackage(RTB_Modules_Server_ServerControl);

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTB_Server
{
   function GameConnection::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p)
   {
      if(%g !$= "")
      {
         %this.hasRTB = 1;
         %this.rtbVersion = firstWord(%g);
      }
      Parent::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p);
   }
};
activatePackage(RTB_Server);