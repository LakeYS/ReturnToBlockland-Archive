//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 515 $
//#      $Date: 2011-10-16 17:17:04 +0100 (Sun, 16 Oct 2011) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/server.cs $
//#
//#      $Id: server.cs 515 2011-10-16 16:17:04Z Ephialtes $
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
$RTB::Version = "4.01";
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
   function GameConnection::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i)
   {
      if(%g !$= "")
      {
         %this.hasRTB = 1;
         %this.rtbVersion = firstWord(%g);
      }
      Parent::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i);
   }
};
activatePackage(RTB_Server);