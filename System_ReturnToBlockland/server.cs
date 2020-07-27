//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 286 $
//#      $Date: 2012-08-21 20:38:18 +0100 (Tue, 21 Aug 2012) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/server.cs $
//#
//#      $Id: server.cs 286 2012-08-21 19:38:18Z Ephialtes $
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
$RTB::Version = "4.04";
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