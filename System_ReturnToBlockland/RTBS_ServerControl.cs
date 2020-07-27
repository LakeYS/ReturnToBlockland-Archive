//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Control (RTBSC/SServerControl)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBS_ServerControl = 1;

//*********************************************************
//* Requirements
//*********************************************************
if(!$RTB::RTBR_ServerControl_Hook)
   exec("./RTBR_ServerControl_Hook.cs");

//*********************************************************
//* Server Options
//*********************************************************
function serverCmdRTB_setServerOptions(%client,%msString,%csString,%gsString,%vlString,%eqString,%output)
{
   if(!%client.isSuperAdmin)
      return;

   %serverName = getField(%msString,0);
   %welcomeMessage = getField(%msString,1);
   %maxPlayers = getField(%msString,2);
   %serverPass = getField(%msString,3);
   %adminPass = getField(%msString,4);
   %superAdminPass = getField(%msString,5);
   
   %etardFilter = getField(%csString,0);
   %etardList = getField(%csString,1);
   %floodProtect = getField(%csString,2);
   
   %maxBps = getField(%gsString,0);
   %fallDamage = getField(%gsString,1);
   %toofar = getField(%gsString,2);
   %wrenchadmin = getField(%gsString,3);
   %pdomain = getField(%gsString,4);
   
   %maxPlV = getField(%vlString,0);
   %maxPhV = getField(%vlString,1);
   %maxPlVPP = getField(%vlString,2);
   %maxPhVPP = getField(%vlString,3);
   
   %mSch = getField(%eqString,0);
   %mMisc = getField(%eqString,1);
   %mProj = getField(%eqString,2);
   %mItem = getField(%eqString,3);
   %mEmit = getField(%eqString,4);
   
   if(strLen(%serverName) <= 0)
      %error = "You need to enter a Server Name.";
   else if(%maxBps < 0)
      %error = "The max bricks per second must be more than -1.";
   else if(%toofar < 0)
      %error = "The Too Far limit must be more than 0.";
   else if(%pdomain < -1)
      %error = "The Ownership Timeout must be greater than -1. (-1 disables it)";
      
   if(%error !$= "")
   {
      commandtoclient(%client,'MessageBoxOK',"Whoops",%error);
      return;
   }
   
   messageAll('MsgAdminForce','\c3%1 \c0updated the server settings.',%client.name);
   
   if(%output)
   {
      if($Pref::Server::Name !$= %serverName)
         messageAll('','\c3+\c0 The Server Name is now \c5%1',%serverName);
      if($Pref::Server::WelcomeMessage !$= %welcomeMessage)
      {
         if(%welcomeMessage $= "")
            messageAll('','\c3+\c0 The Welcome Message has been removed.');
         else
            messageAll('','\c3+\c0 The Welcome Message is now \c5%1',%welcomeMessage);
      }
      if($Pref::Server::MaxPlayers !$= %maxPlayers)
         messageAll('','\c3+\c0 The Max Players is now \c6%1',%maxPlayers);
      if($Pref::Server::Password $= "" && %serverPass !$= "")
         messageAll('','\c3+\c0 The Server is now Passworded');
      if($Pref::Server::Password !$= "" && %serverPass $= "")
         messageAll('','\c3+\c0 The Server is no longer Passworded');
      if($Pref::Server::Password !$= "" && %serverPass !$= "" && %serverPass !$= $Pref::Server::Password)
         messageAll('','\c3+\c0 The Server Password has been changed');
      if($Pref::Server::AdminPassword !$= %adminPass)
      {
         for(%i=0;%i<ClientGroup.getCount();%i++)
         {
            %cl = ClientGroup.getObject(%i);
            if(%cl.isSuperAdmin)
               messageClient(%cl,'','\c3+\c0 The Admin Password has been changed');
         }
      }
      if($Pref::Server::SuperAdminPassword !$= %superAdminPass)
      {
         for(%i=0;%i<ClientGroup.getCount();%i++)
         {
            %cl = ClientGroup.getObject(%i);
            if(%cl.isSuperAdmin)
               messageClient(%cl,'','\c3+\c0 The Super Admin Password has been changed');
         }
      }
      if($Pref::Server::EtardFilter !$= %etardFilter)
         messageAll('','\c3+\c0 The E-Tard Filter is now \c5%1',(%etardFilter == 1)?"On":"Off");
      if($Pref::Server::FloodProtectionEnabled !$= %floodProtect)
         messageAll('','\c3+\c0 Flood Protection is now \c5%1',(%floodProtect == 1)?"On":"Off");
      if($Pref::Server::MaxBricksPerSecond !$= %maxBps)
         messageAll('','\c3+\c0 The Max Bricks per Second is now \c5%1',%maxBps);
      if($Pref::Server::FallingDamage !$= %fallDamage)
         messageAll('','\c3+\c0 Falling Damage is now \c5%1',(%fallDamage == 1)?"On":"Off");
      if($Pref::Server::TooFarDistance !$= %toofar)
         messageAll('','\c3+\c0 The Too Far Distance is now \c5%1',%toofar);
      if($Pref::Server::WrenchEventsAdminOnly !$= %wrenchadmin)
         messageAll('','\c3+\c0 Admin Only Wrench Events are now \c5%1',(%wrenchadmin == 1)?"On":"Off");
      if($Pref::Server::BrickPublicDomainTimeout !$= %pdomain)
         messageAll('','\c3+\c0 The Brick Public Domain Timeout is now \c5%1',(%pdomain == -1)?"Disabled":%pdomain);
      if($Pref::Server::MaxPlayerVehicles_Total !$= %maxPlV)
         messageAll('','\c3+\c0 Total Player Vehicles is now \c5%1',%maxPlV);
      if($Pref::Server::MaxPhysVehicles_Total !$= %maxPhV)
         messageAll('','\c3+\c0 Total Physics Vehicles is now \c5%1',%maxPhV);
      if($Pref::Server::Quota::Player !$= %maxPlVPP)
         messageAll('','\c3+\c0 Total Player Vehicles per Player is now \c5%1',%maxPlVPP);
      if($Pref::Server::Quota::Vehicle !$= %maxPhVPP)
         messageAll('','\c3+\c0 Total Physics Vehicles per Player is now \c5%1',%maxPhVPP);
      if($Pref::Server::Quota::Schedules !$= %mSch)
         messageAll('','\c3+\c0 The Schedule Event Quota is now \c5%1',%mSch);
      if($Pref::Server::Quota::Misc !$= %mMisc)
         messageAll('','\c3+\c0 The Miscellaenous Event Quota is now \c5%1',%mMisc);
      if($Pref::Server::Quota::Projectile !$= %mProj)
         messageAll('','\c3+\c0 The Projectile Event Quota is now \c5%1',%mProj);
      if($Pref::Server::Quota::Item !$= %mItem)
         messageAll('','\c3+\c0 The Item Event Quota is now \c5%1',%mItem);
      if($Pref::Server::Quota::Environment !$= %mEmit)
         messageAll('','\c3+\c0 The Emitters/Lights Event Quota is now \c5%1',%mEmit);
   }
   
   $Pref::Server::Name = %serverName;
   $Pref::Server::WelcomeMessage = %welcomeMessage;
   $Pref::Server::MaxPlayers = %maxPlayers;
   $Pref::Server::Password = %serverPass;
   $Pref::Server::AdminPassword = %adminPass;
   $Pref::Server::SuperAdminPassword = %superAdminPass;
   $Pref::Server::EtardFilter = %etardFilter;
   $Pref::Server::EtardList = %etardList;
   $Pref::Server::FloodProtectionEnabled = %floodProtect;
   $Pref::Server::MaxBricksPerSecond = %maxBps;
   $Pref::Server::FallingDamage = %fallDamage;
   $Pref::Server::TooFarDistance = %toofar;
   $Pref::Server::WrenchEventsAdminOnly = %wrenchadmin;
   $Pref::Server::BrickPublicDomainTimeout = %pdomain;
   $Pref::Server::MaxPlayerVehicles_Total = %maxPlV;
   $Pref::Server::MaxPhysVehicles_Total = %maxPhV;
   $Pref::Server::Quota::Player = %maxPlVPP;
   $Pref::Server::Quota::Vehicle = %maxPhVPP;
   $Pref::Server::Quota::Schedules = %mSch;
   $Pref::Server::Quota::Misc = %mMisc;
   $Pref::Server::Quota::Projectile = %mProj;
   $Pref::Server::Quota::Item = %mItem;
   $Pref::Server::Quota::Environment = %mEmit;
   
   commandtoclient(%client,'RTB_closeGui',"RTB_ServerControl");
   if(!$Server::LAN)
      WebCom_PostServer();
}

function serverCmdRTB_getServerOptions(%client)
{
   if(!%client.isSuperAdmin)
      return;
      
   %msString = $Pref::Server::Name TAB $Pref::Server::WelcomeMessage TAB $Pref::Server::MaxPlayers TAB $Pref::Server::Password TAB $Pref::Server::AdminPassword TAB $Pref::Server::SuperAdminPassword;
   %csString = $Pref::Server::EtardFilter TAB $Pref::Server::EtardList TAB $Pref::Server::FloodProtectionEnabled;
   %gsString = $Pref::Server::MaxBricksPerSecond TAB $Pref::Server::FallingDamage TAB $Pref::Server::TooFarDistance TAB $Pref::Server::WrenchEventsAdminOnly TAB $Pref::Server::BrickPublicDomainTimeout;
   %vlString = $Pref::Server::MaxPlayerVehicles_Total TAB $Pref::Server::MaxPhysVehicles_Total TAB $Pref::Server::Quota::Player TAB $Pref::Server::Quota::Vehicle;
   %eqString = $Pref::Server::Quota::Schedules TAB $Pref::Server::Quota::Misc TAB $Pref::Server::Quota::Projectile TAB $Pref::Server::Quota::Item TAB $Pref::Server::Quota::Environment;
   commandtoclient(%client,'RTB_getServerOptions',%msString,%csString,%gsString,%vlString,%eqString);
}

//*********************************************************
//* Auto Admin
//*********************************************************
function serverCmdRTB_getAutoAdminList(%client)
{
	if(%client.isSuperAdmin)
	{	
		%adminList = $Pref::Server::AutoAdminList;
		%superAdminList = $Pref::Server::AutoSuperAdminList;
		commandtoclient(%client,'RTB_getAutoAdminList',%adminList,%superAdminList);
	}
}

function servercmdRTB_addAutoStatus(%client,%bl_id,%status)
{
	if(%client.isSuperAdmin)
	{
	   if(%bl_id $= "" || !isInt(%bl_id) || %bl_id < 0)
	   {
	      commandtoclient(%client,'MessageBoxOK',"Whoops","You have entered an invalid BL_ID.");
	      return;
	   }
	   if(%status !$= "Admin" && %status !$= "Super Admin")
	   {
         commandtoclient(%client,'MessageBoxOK',"Whoops","You have entered an invalid Status.");
	      return;
	   }
		$Pref::Server::AutoAdminList = removeItemFromList($Pref::Server::AutoAdminList,%bl_id);
		$Pref::Server::AutoSuperAdminList = removeItemFromList($Pref::Server::AutoSuperAdminList,%bl_id);
		if(%status $= "Admin")
		{
			$Pref::Server::AutoAdminList = addItemToList($Pref::Server::AutoAdminList,%bl_id);
		}
		else if(%status $= "Super Admin")
		{
			$Pref::Server::AutoSuperAdminList = addItemToList($Pref::Server::AutoSuperAdminList,%bl_id);
		}
		serverCmdRTB_getAutoAdminList(%client);
		
		for(%i=0;%i<ClientGroup.getCount();%i++)
		{
			%cl = ClientGroup.getObject(%i);
			if(%cl.bl_id $= %bl_id)
			{
			   if(%status $= "Super Admin")
            {
               if(%cl.isSuperAdmin)
                  return;
                  
               %cl.isAdmin = 1;
               %cl.isSuperAdmin = 1;
               commandtoclient(%cl,'setAdminLevel',2);
               messageAll('MsgClientJoin','',%cl.name,%cl,%cl.bl_id,%cl.score,0,%cl.isAdmin,%cl.isSuperAdmin);
               messageAll('MsgAdminForce','\c2%1 has become Super Admin (Auto)',%cl.name);
               
               RTBSC_SendPrefList(%client);
            }
            else if(%status $= "Admin")
            {
               if(%cl.isAdmin)
                  return;
                  
               %cl.isAdmin = 1;
               %cl.isSuperAdmin = 0;
               commandtoclient(%cl,'setAdminLevel',1);
               messageAll('MsgClientJoin','',%cl.name,%cl,%cl.bl_id,%cl.score,0,%cl.isAdmin,%cl.isSuperAdmin);
               messageAll('MsgAdminForce','\c2%1 has become Admin (Auto)',%cl.name);
            }
			}
		}
	}
}

function servercmdRTB_removeAutoStatus(%client,%bl_id)
{
	if(%client.isSuperAdmin)
	{
		$Pref::Server::AutoAdminList = removeItemFromList($Pref::Server::AutoAdminList,%bl_id);
		$Pref::Server::AutoSuperAdminList = removeItemFromList($Pref::Server::AutoSuperAdminList,%bl_id);

		serverCmdRTB_getAutoAdminList(%client);
	}
}

function servercmdRTB_clearAutoAdminList(%client)
{
	if(%client.isSuperAdmin)
	{
		$Pref::Server::AutoAdminList = "";
		$Pref::Server::AutoSuperAdminList = "";

		serverCmdRTB_getAutoAdminList(%client);
	}
}

function servercmdRTB_DeAdminPlayer(%client,%victim)
{
   if(!%client.isSuperAdmin)
      return;
   
   if(findLocalClient() $= %victim)
   {
      messageClient(%client,'','\c2You cannot de-admin the host.');
      return;
   }
   
   if(%victim.isSuperAdmin || %victim.isAdmin)
   {
      %victim.isAdmin = 0;
      %victim.isSuperAdmin = 0;
      commandtoclient(%victim,'setAdminLevel',0);
      messageAll('MsgClientJoin','',%victim.name,%victim,%victim.bl_id,%victim.score,0,%victim.isAdmin,%victim.isSuperAdmin);
      messageAll('MsgAdminForce','\c2%1 has been De-Admined (Manual)',%victim.name);
   }
}

function servercmdRTB_AdminPlayer(%client,%victim)
{
   if(!%client.isSuperAdmin)
      return;
      
   if(findLocalClient() $= %victim && %victim.isSuperAdmin)
   {
      messageClient(%client,'','\c2You cannot de-admin the host.');
      return;
   }
      
   if(!%victim.isAdmin || (%victim.isAdmin && %victim.isSuperAdmin))
   {
      %victim.isAdmin = 1;
      %victim.isSuperAdmin = 0;
      commandtoclient(%victim,'setAdminLevel',1);
      messageAll('MsgClientJoin','',%victim.name,%victim,%victim.bl_id,%victim.score,0,%victim.isAdmin,%victim.isSuperAdmin);
      messageAll('MsgAdminForce','\c2%1 has become Admin (Manual)',%victim.name);
   }
}

function servercmdRTB_SuperAdminPlayer(%client,%victim)
{
   if(!%client.isSuperAdmin)
      return;
      
   if(!%victim.isSuperAdmin)
   {
      %victim.isAdmin = 1;
      %victim.isSuperAdmin = 1;
      commandtoclient(%victim,'setAdminLevel',2);
      messageAll('MsgClientJoin','',%victim.name,%victim,%victim.bl_id,%victim.score,0,%victim.isAdmin,%victim.isSuperAdmin);
      messageAll('MsgAdminForce','\c2%1 has become Super Admin (Manual)',%victim.name);
      
      RTBSC_SendPrefList(%victim);
   }
}

//*********************************************************
//* Pref Manager
//*********************************************************
function RTBSC_SendPrefList(%client)
{
   if(%client.isSuperAdmin && !%client.hasPrefList)
   {
      %client.hasPrefList = 1;
      if($RTB::ServerPrefs <= 0)
         return;
         
      for(%i=0;%i<$RTB::ServerPrefs;%i++)
      {
         commandtoclient(%client,'RTB_addPref',%i,0,$RTB::ServerPref[%i,0]);
         for(%j=1;%j<$RTB::ServerPrefCount[%i];%j++)
         {
            eval("%prefValue = $"@getField($RTB::ServerPref[%i,%j],1)@";");
            %prefArray = %prefArray@%i@","@%j@","@%prefValue@"\t";
            commandtoclient(%client,'RTB_addPref',%i,%j,getField($RTB::ServerPref[%i,%j],0) TAB getField($RTB::ServerPref[%i,%j],2) TAB getField($RTB::ServerPref[%i,%j],4));
         }
      }
      %prefArray = getSubStr(%prefArray,0,strLen(%prefArray)-1);
      commandtoclient(%client,'RTB_updatePrefs',%prefArray);
   }
}

function RTBSC_SendPrefValues(%client)
{
   if(%client.isSuperAdmin)
   {
      for(%i=0;%i<$RTB::ServerPrefs;%i++)
      {
         for(%j=1;%j<$RTB::ServerPrefCount[%i];%j++)
         {
            eval("%prefValue = $"@getField($RTB::ServerPref[%i,%j],1)@";");
            %prefArray = %prefArray@%i@","@%j@","@%prefValue@"\t";
         }
      }
      %prefArray = getSubStr(%prefArray,0,strLen(%prefArray)-1);
      commandtoclient(%client,'RTB_updatePrefs',%prefArray);
   }
}

function serverCmdRTB_updatePrefs(%client,%prefArray)
{
   if(!%client.isSuperAdmin)
      return;
      
   for(%i=0;%i<getFieldCount(%prefArray);%i++)
   {
      %pref = strReplace(getField(%prefArray,%i),",","\t");
      %idA = getField(%pref,0);
      %idB = getField(%pref,1);
      %value = getField(%pref,2);
      
      %entry = $RTB::ServerPref[%idA,%idB];
      %pref = getField(%entry,1);
      %type = getWord(getField(%entry,2),0);
      eval("%currVal = $"@%pref@";");
      
      if(%type $= "bool")
      {
         if(%value !$= 1 && %value !$= 0)
            continue;
      }
      else if(%type $= "string")
      {
         %max = getWord(getField(%entry,2),1);
         if(strLen(%value) > %max)
            %value = getSubStr(%value,0,%max);
         %value = strReplace(%value,"\"","\\\"");
      }
      else if(%type $= "int")
      {
         if(!isInt(%value))
            continue;
         %min = getWord(getField(%entry,2),1);
         %max = getWord(getField(%entry,2),2);
         
         if(%value < %min)
            %value = %min;
         if(%value > %max)
            %value = %max;
      }
      else if(%type $= "list")
      {
         %list = restWords(getField(%entry,2));
         for(%i=0;%i<getWordCount(%list);%i++)
         {
            %word = getWord(%list,%i);
            if(%word $= %value && %i%2 $= 0)
            {
               %foundInList = 1;
               break;
            }
         }
         
         if(!%foundInList)
            continue;
      }
      
      if(%currVal !$= %pref)
      {
         eval("$"@%pref@" = \""@%value@"\";");
         %numChanged++;
         
         %newPrefArray = %newPrefArray@%idA@","@%idB@","@%value@"\t";
      }
   }
   
   commandtoclient(%client,'RTB_closeGui',"RTB_ServerControl");
   if(%numChanged <= 0)
      return;
      
   %newPrefArray = getSubStr(%newPrefArray,0,strLen(%newPrefArray)-1);
   
   for(%i=0;%i<ClientGroup.getCount();%i++)
   {
      %cl = ClientGroup.getObject(%i);
      if(%cl.isSuperAdmin)
         commandtoclient(%client,'RTB_updatePrefs',%newPrefArray);
   }
   
   messageAll('MsgAdminForce','\c3%1 \c0updated the server preferences.',%client.name);
   
   %file = new FileObject();
   %file.openForWrite("config/server/RTB/modPrefs.cs");
   for(%i=0;%i<$RTB::ServerPrefs;%i++)
   {
      for(%j=1;%j<$RTB::ServerPrefCount[%i];%j++)
      {
         eval("%prefValue = $"@getField($RTB::ServerPref[%i,%j],1)@";");
         %file.writeLine("$"@getField($RTB::ServerPref[%i,%j],1)@" = \""@%prefValue@"\";");
      }
   }
   %file.delete();
}

//*********************************************************
//* Support Functions
//*********************************************************
function addItemToList(%string,%item)
{
	if(hasItemOnList(%string,%item))
		return %string;

	if(%string $= "")
		return %item;
	else
		return %string SPC %item;
}

function hasItemOnList(%string,%item)
{
	for(%i=0;%i<getWordCount(%string);%i++)
	{
		if(getWord(%string,%i) $= %item)
			return 1;
	}
	return 0;
}

function removeItemFromList(%string,%item)
{
	if(!hasItemOnList(%string,%item))
		return %string;

	for(%i=0;%i<getWordCount(%string);%i++)
	{
		if(getWord(%string,%i) $= %item)
		{
			if(%i $= 0)
				return getWords(%string,1,getWordCount(%string));
			else if(%i $= getWordCount(%string)-1)
				return getWords(%string,0,%i-1);
			else
				return getWords(%string,0,%i-1) SPC getWords(%string,%i+1,getWordCount(%string));
		}
	}
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTBS_ServerControl
{
   function GameConnection::onConnectRequest(%this,%a,%b,%c,%d,%e,%f)
   {
      if(%f !$= "")
      {
         %this.hasRTB = 1;
         %this.rtbVersion = %f;
      }
      Parent::onConnectRequest(%this,%a,%b,%c,%d,%e);
   }
   
   function serverCmdSAD(%client,%pass)
   {
      RTBSC_SendPrefList(%client);
      Parent::serverCmdSAD(%client,%pass);
   }
   
   function GameConnection::autoAdminCheck(%this)
   {
      Parent::autoAdminCheck(%this);
      
      if(%this.hasRTB)
      {
         commandtoclient(%this,'sendRTBVersion',$RTB::Version);
         RTBSC_SendPrefList(%this);
      }
   }
};
activatePackage(RTBS_ServerControl);