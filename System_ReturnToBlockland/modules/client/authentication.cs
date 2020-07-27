//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 266 $
//#      $Date: 2010-08-04 07:29:41 +0100 (Wed, 04 Aug 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/modules/client/serverControl.cs $
//#
//#      $Id: serverControl.cs 266 2010-08-04 06:29:41Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Modules / Client / Authentication
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::Authentication = 1;

//*********************************************************
//* Module Class
//*********************************************************
new ScriptObject(RTB_Client_Authentication)
{
   // tcp factory
   tcp = RTB_Networking.createFactory("api.returntoblockland.com","80","/apiRouter.php?d=APICA");
};
RTBGroup.add(RTB_Client_Authentication);

//*********************************************************
//* API Methods
//*********************************************************
//- RTB_Client_Authentication::auth (authenticates the client with the rtb api)
function RTB_Client_Authentication::auth(%this)
{
   if(!RTBCO_getPref("CA::Auth"))
      return;
   
   if(isEventPending(%this.authSchedule))
      cancel(%this.authSchedule);
      
   %this.authSchedule = %this.schedule(180000,"auth");
   
   %name = $pref::Player::NetName;
   %location = %this.establishLocation();
   %playtime = %this.getPlaytime();
   
   %data = %this.tcp.getPostData("AUTH",%name,%location,%playtime,$RTB::Version,$Version);
   
   %this.tcp.post(%data,%this,"onAuthReply");
}

//- RTB_Client_Authentication::onAuthReply (handles auth reply)
function RTB_Client_Authentication::onAuthReply(%this,%tcp,%factory,%line)
{
   if(%line $= "PREFS")
      %this.sendPrefs();
}

//- RTB_Client_Authentication::sendPrefs (sends prefs to server)
function RTB_Client_Authentication::sendPrefs(%this)
{
   %data = %this.tcp.getPostData("UPDATEPREFS",RTBCO_getPref("CA::ShowOnline"),RTBCO_getPref("CA::ShowServer"),RTBCO_getPref("SA::ShowPlayers"));
   
   %this.tcp.post(%data);
}

//- RTB_Client_Authentication::sendAvatar (sends avatar to server)
function RTB_Client_Authentication::sendAvatar(%this)
{
   %data = %this.tcp.getPostData("UPDATEAVATAR",%this.getAvatarString());
   
   %this.tcp.post(%data);
}

//*********************************************************
//* Helper Methods
//*********************************************************
//- RTB_Client_Authentication::getPlaytime (Gets a nice formatted string of how long the user has had the game open)
function RTB_Client_Authentication::getPlaytime(%this)
{
   %timestring = getTimeString($Sim::Time);
   
   return %timestring;
}

//- RTB_Client_Authentication::getAvatarString (Returns a string representing the user's avatar)
function RTB_Client_Authentication::getAvatarString(%this)
{
    %partList = "Accent\tAccentColor\tChest\tDecalName\tFaceName\tHat\tHatColor\tHeadColor\tHip\tHipColor\tLArm\tLArmColor\tLHand\tLHandColor\tLLeg\tLLegColor\tPack\tPackColor\tRArm\tRArmColor\tRHand\tRHandColor\tRLeg\tRLegColor\tSecondPack\tSecondPackColor\tTorsoColor";
    %parts = getFieldCount(%partList);

    %string = "";
    for(%i=0;%i<%parts;%i++)
    {
        %part = "::" @ getField(%partList,%i);
        %string = %string @ $Pref::Avatar[%part] @ ",";
    }
    return %string;
}

//- RTB_Client_Authentication::establishLocation (Decides where the player is currently)
function RTB_Client_Authentication::establishLocation(%this)
{
   if(!RTBCO_getPref("CA::ShowServer"))
      return "UNK";
      
   if(isObject(ServerConnection))
   {
      %address = ServerConnection.getAddress();
      if(%address $= "local")
      {
         if($Server::LAN)
            if($Server::ServerType $= "SinglePlayer")
               %address = "SNG";
            else
               %address = "LAN";
         else
            %address = "LOC "@$Pref::Server::Port;
      }
      else
      {
         %address = strReplace(%address,":"," ");  
         %ip = getWord(%address,0);
         %port = getWord(%address,1);
         
         if(strPos(%ip,"192.") $= 0 || strPos(%ip,"10.") $= 0)
            %address = "NWK "@%ip@":"@%port;
         else
            %address = "WEB "@%ip@":"@%port;
      }
      %location = %address;
   }
   else
      %location = "NOS";
      
   return %location;
}

//*********************************************************
//* Module Package
//*********************************************************
package RTB_Modules_Client_Authentication
{
   function MM_AuthBar::blinkSuccess(%this)
   {
      Parent::blinkSuccess(%this);
      
      RTB_Client_Authentication.auth();
      RTB_Client_Authentication.sendAvatar();
   }
   
   function AvatarGui::onSleep(%this)
   {
      Parent::onSleep(%this);
      RTB_Client_Authentication.sendAvatar();
   }
   
   function disconnectedCleanup()
   {
      Parent::disconnectedCleanup();
      
      RTB_Client_Authentication.schedule(1000,"auth");
   }
   
   function GameConnection::onConnectionAccepted(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k)
   {
      Parent::onConnectionAccepted(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k);
      
      RTB_Client_Authentication.auth();
   }
};