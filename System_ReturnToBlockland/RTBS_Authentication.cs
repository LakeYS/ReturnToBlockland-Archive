//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Authentication (RTBSA/SAuthentication)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBS_Authentication = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::SAuthentication::AuthServer = "returntoblockland.com";
$RTB::SAuthentication::AuthPath = "/blockland/rtbServerAuth.php";
$RTB::SAuthentication::Cache::SentMods = 0;

//*********************************************************
//* Operational Functions
//*********************************************************
function RTBSA_InitASC()
{
   if(!isObject(RTBSA_ASC))
   {
      new TCPObject(RTBSA_ASC)
      {
         site = $RTB::SAuthentication::AuthServer;
         port = 80;
         cmd = "";
         filePath = $RTB::SAuthentication::AuthPath;
         
         defaultFailHandle = "RTBSA_handleTimeout";
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      
      RTBSA_ASC.addResponseHandle("POST","RTBSA_onPostResponse");
      RTBSA_ASC.addResponseHandle("POSTMODS","RTBSA_onPostModsResponse");
   }
}

function RTBSA_handleTimeout()
{
}

function RTBSA_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBSA_ASC))
      RTBSA_InitASC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   
   RTBSA_ASC.sendRequest(%cmd,%argString,%layer);
}

function RTBSA_onPostModsResponse(%this,%line)
{
   if(%line $= 1)
      $RTB::SAuthentication::Cache::SentMods = 1;
}

function RTBSA_PostMods()
{
	%mod = FindFirstFile("Add-Ons/*_*/description.txt");
	while(strLen(%mod) > 0)
	{
	   %modName = getSubStr(%mod,8,strLen(%mod)-24);
	   %modVarName = getSafeVariableName(%modName);
	   
      %isDefault = 0;
      for(%i=0;%i<$RTB::CModManager::DefaultBLMods+1;%i++)
      {
         if($RTB::CModManager::DefaultBLMod[%i] $= %modName)
         {
            %isDefault = 1;
            break;
         }
      }
	   if($AddOn__[%modVarName] $= 1 && isFile("Add-Ons/"@%modName@".zip") && !%isDefault)
	   {
         %descriptionData = getFileContents("Add-Ons/"@%modName@"/description.txt");
         if(isFile("Add-Ons/"@%modName@"/rtbInfo.txt"))
         {
            %rtbData = getFileContents("Add-Ons/"@%modName@"/rtbInfo.txt");
            %id = RTBMM_getFieldFromContents(%rtbData,"ID");
            %field = "rtb~"@%id@"~"@%modName@".zip";
         }
         else
         {
            %title = RTBMM_getFieldFromContents(%descriptionData,"Title");
            if(%title !$= "0")
               %field = "bl~"@%title@"~"@%modName@".zip";
         }
	   
	      if(%field !$= "")
	      {
            %modArray = (%modArray $= "") ? "" : %modArray@";";
            %modArray = %modArray@%field;
	      }
	   }
		%mod = FindNextFile("Add-Ons/*_*/description.txt");
	}
	
	%modArray = strReplace(%modArray,";","\t");
	for(%i=0;%i<getFieldCount(%modArray);%i++)
	{
      %section = strReplace(getField(%modArray,%i),"~","\t");
      %zip = getSubStr(getField(%section,2),0,strPos(getField(%section,2),".zip"));
      if(strLen(findFirstFile("Add-Ons/"@%zip@"/*.dts")) > 0 || strLen(findFirstFile("Add-Ons/"@%zip@"/*.wav")) > 0 || strLen(findFirstFile("Add-Ons/"@%zip@"/*.ogg")) > 0 || strLen(findFirstFile("Add-Ons/"@%zip@"/*.png")) > 0 || strLen(findFirstFile("Add-Ons/"@%zip@"/*.jpg")) > 0)
         %modLine = getField(%modArray,%i)@"~1";
      else
         %modLine = getField(%modArray,%i)@"~0";
         
      %finalModArray = (%finalModArray $= "") ? "" : %finalModArray@";";
      %finalModArray = %finalModArray@%modLine;
	}
   RTBSA_SendRequest("POSTMODS",1,$Pref::Server::Port,%finalModArray);
}

function RTBSA_onPostResponse(%this,%line)
{
   if(%line $= "SUCCESS" && !$RTB::SAuthentication::Cache::SentMods)
      RTBSA_PostMods();
}

function RTBSA_Post()
{
   if(!$RTB::Options::PostServer || RTBSA_getServerType() !$= "Multiplayer")
	   return;

   %name = $pref::Player::NetName;
   
   if(isEventPending($RTB::SAuthentication::AuthSS))
      cancel($RTB::SAuthentication::AuthSS);
      
   $RTB::SAuthentication::AuthSS = schedule(180000,0,"RTBSA_Post");
   RTBSA_SendRequest("POST",1,$Pref::Server::Port,ClientGroup.getCount(),RTBSA_getPlayerList());
   echo("Posting to rtb server");
}

//*********************************************************
//* Support Functions
//*********************************************************
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