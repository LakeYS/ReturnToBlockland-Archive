//#############################################################################
//#
//#   Return to Blockland - Version 3.0
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 39 $
//#      $Date: 2009-02-23 10:45:55 +0000 (Mon, 23 Feb 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/trunk/old/RTBS_Authentication.cs $
//#
//#      $Id: RTBS_Authentication.cs 39 2009-02-23 10:45:55Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Authentication (RTBSA/SAuthentication)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBS_Authentication = 1;

//*********************************************************
//* Load Switchboard
//*********************************************************
%RTBSA_SB = RTB_createSwitchboard("SA","APISA");
%RTBSA_SB.registerLine(1,1);

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::SAuthentication::Cache::SentMods = 0;

//*********************************************************
//* Request Gateway
//*********************************************************
//- RTBSA_SendRequest (Compiles arguments into POST string for transfer)
function RTBSA_SendRequest(%cmd,%line,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   for(%i=1;%i<11;%i++)
   {
      %string = %string@strReplace(urlEnc(%arg[%i]),"\t","")@"\t";
   }
   RTB_SB_SA.placeCall(%line,%cmd,%string);
}

//*********************************************************
//* Meat
//*********************************************************
//- RTBSA_Post (Updates RTB API with player's details)
%RTBSA_SB.registerResponseHandler("POST","RTBSA_onPostResponse");
function RTBSA_Post()
{
   if(!$RTB::Options::SA::PostServer || RTBSA_getServerType() !$= "Multiplayer")
	   return;
	   
   if($Pref::Server::RTBSetup::User $= "" && $Server::Dedicated)
      return;
   
   if(isEventPending($RTB::SAuthentication::Auth))
      cancel($RTB::SAuthentication::Auth);
      
   $RTB::SAuthentication::Auth = schedule(180000,0,"RTBSA_Post");
   RTBSA_SendRequest("POST",1,$Pref::Server::Port,ClientGroup.getCount(),RTBSA_getPlayerList());
   echo("Posting to rtb server");
}

//- RTBSA_onPostResponse (Reply from POST request)
function RTBSA_onPostResponse(%tcp,%line)
{
   if(%line $= 1)
      RTBSA_PostMods();
}

//- RTBSA_PostMods (Sends a list of add-ons the server has to the RTB API)
%RTBSA_SB.registerResponseHandler("POSTMODS","RTBSA_onPostModsResponse");
function RTBSA_PostMods()
{
   if($RTB::SAuthentication::Cache::SentMods)
      return;
      
   RTBSA_SendRequest("POSTMODS",1,$Pref::Server::Port,RTBSA_getModList());
}

//- RTBSA_onPostModsResponse (Reply from attempt to post mods list)
function RTBSA_onPostModsResponse(%this,%line)
{
   if(%line $= 1)
      $RTB::SAuthentication::Cache::SentMods = 1;
}

//*********************************************************
//* Support Functions
//*********************************************************
//- RTBSA_getServerType (Establishes what type of server is being hosted)
function RTBSA_getServerType()
{
   if(isObject(MissionGroup))
   {
      if($Server::LAN)
         return "LAN";
      else
         return "Multiplayer";
   }
   else
      return 0;
}

//- RTBSA_getPlayerList (Gets an http-friendly list of players)
function RTBSA_getPlayerList()
{
   for(%i=0;%i<ClientGroup.getCount();%i++)
   {
      %client = ClientGroup.getObject(%i);
      %client_name = %client.netName;
      %client_ip = %client.getAddress();
      
      if(%client.isSuperAdmin)
         %client_state = 2;
      else if(%client.isAdmin)
         %client_state = 1;
      else
         %client_state = 0;
         
      %client_score = %client.score;
      %client_blid = %client.bl_id;
         
      if(%client_ip $= "local")
         %client_state = 3;
      else
      {
         %client_ip = getSubStr(%client_ip,3,strLen(%client_ip));
         %client_ip = getSubStr(%client_ip,0,strPos(%client_ip,":"));
      }
         
      %playerList = (%playerList $= "") ? %client_name@";"@%client_ip@";"@%client_score@";"@%client_state : %playerList@"~"@%client_name@";"@%client_ip@";"@%client_score@";"@%client_state;
   }
   return %playerList;
}

//- RTBSA_getModList (Gets a list of mods running on the server)
function RTBSA_getModList()
{
   %filepath = findFirstFile("Add-Ons/*_*/server.cs");
   while(strlen(%filepath) > 0)
   {
      %zip = getSubStr(%filepath,8,strLen(%filepath)-strLen("/server.cs")-strLen("Add-Ons/"));
      
      if($AddOn__[%zip] $= 1)
      {
         %fileObject = new FileObject();
         if(isFile("Add-Ons/"@%zip@"/rtbInfo.txt"))
         {
            if(%fileObject.openForRead("Add-Ons/"@%zip@"/rtbInfo.txt"))
            {
               while(!%fileObject.isEOF())
               {
                  %line = %fileObject.readLine();
                  if(strPos(%line,"id:") !$= -1)
                  {
                     %id = trim(strReplace(%line,"id:",""));
                  }
                  else if(strPos(%line,"version:") !$= -1)
                  {
                     %version = trim(strReplace(%line,"version:",""));
                  }
               }
               %modArray = %modArray@"rtb~"@%id@"~"@%version@"~"@%zip@".zip;";
               %fileObject.close();
            }
         }
         else
         {
            if(%fileObject.openForRead("Add-Ons/"@%zip@"/description.txt"))
            {
               while(!%fileObject.isEOF())
               {
                  %line = %fileObject.readLine();
                  if(strPos(%line,"Title:") !$= -1)
                  {
                     %title = trim(strReplace(%line,"Title:",""));
                     break;
                  }
               }
               %modArray = %modArray@"bl~"@%title@"~"@%zip@".zip;";
               %fileObject.close();
            }
         }
      }
      %filepath = findNextFile("Add-Ons/*_*/server.cs");
   }
   if(strLen(%modArray) > 0)
      %modArray = getSubStr(%modArray,0,strLen(%modArray)-1);
   return %modArray;
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTBS_Authentication
{
   function postServerTCPObj::connect(%this,%addr)
   {
      Parent::connect(%this,%addr);
      RTBSA_Post();
   }
};
activatePackage(RTBS_Authentication);