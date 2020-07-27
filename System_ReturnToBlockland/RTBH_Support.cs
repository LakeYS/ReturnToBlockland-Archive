//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBH_Support.cs $
//#
//#      $Id: RTBH_Support.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Hybrid Support Script
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBH_Support = 1;

//*********************************************************
//* Global Variables
//*********************************************************
$RTB::CModManager::DefaultBLMods = -1;
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Sword";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Spear";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Rocket_Launcher";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Push_Broom";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Horse_Ray";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Guns_Akimbo";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Gun";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Weapon_Bow";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Tank";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Magic_Carpet";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Jeep";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Horse";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Flying_Wheeled_Jeep";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Vehicle_Ball";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Sound_Synth4";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Sound_Phone";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Sound_Beeps";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Projectile_Radio_Wave";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Projectile_Pong";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Projectile_Pinball";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Projectile_GravityRocket";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Print_Letters_Default";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Print_2x2r_Default";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Print_2x2f_Default";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Print_1x2f_Default";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Player_Quake";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Player_No_Jet";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Player_Leap_Jet";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Player_Jump_Jet";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Player_Fuel_Jet";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Particle_Player";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Particle_Grass";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Particle_FX_Cans";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Particle_Basic";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Tutorial";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Slopes";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Slate";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_KitchenDark";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Kitchen";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Construct";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_BedroomDark";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Map_Bedroom";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Light_Basic";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Light_Animated";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Item_Skis";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Emote_Love";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Emote_Hate";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Emote_Confusion";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Emote_Alarm";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Brick_Large_Cubes";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "Script_ClearSpam";
$RTB::CModManager::DefaultBLMod[$RTB::CModManager::DefaultBLMods++] = "System_ReturnToBlockland";

//*********************************************************
//* Global Transmission Variables
//*********************************************************
$RTB::TransmissionTimeout = 60; //> 30 Second Timeout (Sometimes the server just "hangs")
$RTB::TransmissionRetries = 3; //> 3 Retries, then fail message

//*********************************************************
//* TCPObject Transmission Queueing
//*********************************************************
function TCPObject::pushQueue(%obj,%cmd,%string,%layer,%specialstring)
{
   for(%i=0;%i<%obj.queueSize;%i++)
   {
      %qString = %obj.queueTransmitString[%i];
      %qCmd = getField(%qString,0);
      %qLayer = getField(%qString,2);
      if(%qCmd $= %cmd)
      {
         %obj.queueTransmitString[%i] = %cmd TAB %string TAB %layer TAB %specialstring;
         return %obj.queueSize;
      }
      
      if(%qLayer $= %layer)
      {
         %obj.queueTransmitString[%i] = %cmd TAB %string TAB %layer TAB %specialstring;
         return %obj.queueSize;
      }
   }
   
   %obj.queueTransmitString[%obj.queueSize] = %cmd TAB %string TAB %layer TAB %specialstring;
   %obj.queueSize++;
   
   return %obj.queueSize;
}

function TCPObject::hasQueue(%obj)
{
   if(%obj.queueSize >= 1)
      return 1;
   else
      return 0;
}

function TCPObject::popQueue(%obj)
{
   if(%obj.queueSize < 1)
      return -1;
      
   %retDat = %obj.queueTransmitString0;
   for(%i=0;%i<%obj.queueSize;%i++)
   {
      %obj.queueTransmitString[%i] = %obj.queueTransmitString[%i+1];
   }
   %obj.queueSize--;
   
   return %retDat;
}

function TCPObject::addResponseHandle(%obj,%cmd,%call)
{
   if(!isFunction(%call))
      return -1;
   if(%cmd $= "")
      return -1;
   %obj.responseHandle[strUpr(%cmd)] = %call;
}

function TCPObject::addFailHandle(%obj,%cmd,%call)
{
   if(!isFunction(%call))
      return -1;
   if(%cmd $= "")
      return -1;
   %obj.failHandle[strUpr(%cmd)] = %call;
}

package RTBH_Support
{
   function TCPObject::onLine(%this,%line)
   {
      if(%this.isRTBObject)
      {
         if(%line $= "END")
         {
            %lastCmd = %this.lastCmd;
            %cmdName = %this.responseHandle[%lastCmd]@"End";
            if(isFunction(%cmdName))
            {
               %call = %cmdName@"("@%this@");";
               eval(%call);
            }
            %this.disconnect();
            %this.onDisconnect();
            return;
         }
         
         %line = strReplace(%line,"\"","\\\"");
         %line = strReplace(%line,"\n","");
         %line = strReplace(%line,"\r","");
         %line = strReplace(%line,"\r\n","");
         
         if($RTB::Debug)
            echo("\c2>> Transmit Line ("@%line@")");
         
         if(strPos(%line,"Set-Cookie:") $= 0)
         {
            %cookie = getSubStr(%line,strPos(%line,": ")+2,strLen(%line));
            $RTB::Cache::Cookie = getSubStr(%cookie,0,strPos(%cookie,";"));
         }
         
         cancel(%this.timeoutSchedule);
         if(%line $= "" && %this.lastLine !$= "")
            %this.receiving = 1;
         %this.lastLine = %line;
            
         if(!%this.receiving)
            return;
         
         if(%line $= "")
            return;
         
         if(%this.lineCallback !$= "")
         {
            %call = %this.lineCallback@"("@%this@",\""@%line@"\");";
            eval(%call);
         }
         
         if(getField(%line,0) $= "SQLERROR")
         {
            %this.transmitting = 0;
            if(!$Server::Dedicated)
            {
               RTBBT_pushBugReporter("RTB","SQL Error",2,getFields(%line,1,getFieldCount(%line)));
               MessageBoxOK("ERROR","There has been a critical error. We would appreciate it if you could send this Bug Report so it can be fixed ASAP.");
            }
            else
               echo("ERROR: SQL ERROR OCCURRED!");
            return;
         }
         
         if(getField(%line,0) $= "ERROR")
         {
            %this.transmitting = 0;
            echo("\c2TCP ERROR ("@getField(%line,1)@"): "@getField(%line,2));
            if(!$Server::Dedicated)
               MessageBoxOK(getfield(%line,1),getfield(%line,2));
            return;
         }
         
         %cmd = getField(%line,0);
         if(%this.responseHandle[%cmd] !$= "")
         {
            %this.lastCmd = %cmd;
            %call = %this.responseHandle[%cmd]@"("@%this@",\""@getFields(%line,1,getFieldCount(%line))@"\");";
            eval(%call);
         }
      }
      else
      {
         if(isFunction(%this,"onLine"))
            Parent::onLine(%this,%line);
      }
   }

   function TCPObject::onConnected(%this)
   {
      if(%this.isRTBObject)
      {
         if($RTB::Debug)
            echo("\c2>> Transmit Connected");
         %this.connected = 1;
         %this.beginTransmit();
      }
      else
      {
         if(isFunction(%this,"onConnected"))
            Parent::onConnected(%this);
      }
   }

   function TCPObject::onConnectFailed(%this)
   {
      if(%this.isRTBObject)
      {
         if(%this.numTries < $RTB::TransmissionRetries)
         {
            if($RTB::Debug)
            {
               echo("\c2>> Transmit Failed [R"@%this.numTries@"]");
               echo("\c4>> Retrying Transmit...");
            }
            %this.timeoutSchedule = %this.schedule($RTB::TransmissionTimeout*1000,"onTimeout");
            %this.beginTransmit();
            %this.numTries++;
         }
         else
         {
            if($RTB::Debug)
               echo("\c2>> Transmit Failed [END]");
            %this.transmitting = 0;
            %this.receiving = 0;
            %cmd = %this.t_command;
            cancel(%this.timeoutSchedule);
            if(%this.failHandle[%cmd] !$= "")
            {
               %call = %this.failHandle[%cmd]@"("@%this@",\"Fail\");";
               eval(%call);
            }
            else if(%this.defaultFailHandle !$= "")
            {
               %call = %this.defaultFailHandle@"("@%this@",\""@%cmd@"\",\"Fail\");";
               eval(%call);
            }
         }
      }
      else
      {
         if(isFunction(%this,"onConnectFailed"))
            Parent::onConnectFailed(%this);
      }
   }

   function TCPObject::onDNSFailed(%this)
   {
      if(%this.isRTBObject)
      {
         if($RTB::Debug)
            echo("\c2>> Transmit DNS Failed [END]");
         %this.transmitting = 0;
         %this.receiving = 0;
         %cmd = %this.t_command;
         cancel(%this.timeoutSchedule);
         if(%this.failHandle[%cmd] !$= "")
         {
            %call = %this.failHandle[%cmd]@"("@%this@",\"DNS\");";
            eval(%call);
         }
         else if(%this.defaultFailHandle !$= "")
         {
            %call = %this.defaultFailHandle@"("@%this@",\""@%cmd@"\",\"DNS\");";
            eval(%call);
         }
      }
      else
      {
         if(isFunction(%this,"onDNSFailed"))
            Parent::onDNSFailed(%this);
      }
   }

   function TCPObject::onTimeout(%this)
   {
      if(%this.isRTBObject)
      {
         if(%this.numTries < $RTB::TransmissionRetries)
         {
            if($RTB::Debug)
            {
               echo("\c2>> Transmit Timed Out [R"@%this.numTries@"]");
               echo("\c4>> Retrying Transmit...");
            }
            %this.timeoutSchedule = %this.schedule($RTB::TransmissionTimeout*1000,"onTimeout");
            %this.beginTransmit();
            %this.numTries++;
         }
         else
         {
            if($RTB::Debug)
               echo("\c2>> Transmit Timed Out [END]");
            %this.transmitting = 0;
            %this.receiving = 0;
            %cmd = %this.t_command;
            if(%this.failHandle[%cmd] !$= "")
            {
               %call = %this.failHandle[%cmd]@"("@%this@",\"Timeout\");";
               eval(%call);
            }
            else if(%this.defaultFailHandle !$= "")
            {
               %call = %this.defaultFailHandle@"("@%this@",\""@%cmd@"\",\"Timeout\");";
               eval(%call);
            }
            else
            {
               if($Server::Dedicated)
                  echo("ERROR: RTB Connection timed out!");
               else
                  MessageBoxOK("Whoops","For some reason the connection Timed Out. You may want to check your internet or seek support if this is ongoing.");
            }
         }
      }
   }

   function TCPObject::onDisconnect(%this,%skip)
   {
      if(%this.isRTBObject)
      {
         if($RTB::Debug)
            echo("\c2>> Transmit Disconnect");
         %this.connected = 0;
         %this.transmitting = 0;
         %this.receiving = 0;
         %this.lastLine = "";
         cancel(%this.timeoutSchedule);
         
         if(%this.hasQueue() && !%skip)
         {
            %request = %this.popQueue();
            %this.sendRequest(getField(%request,0),getField(%request,1),getField(%request,2),getField(%request,3));
         }
      }
      else
      {
         if(isFunction(%this,"onDisconnect"))
            Parent::onDisconnect(%this);
      }
   }
};
activatePackage(RTBH_Support);

function filterKey(%string)
{
   %string = strReplace(%string,"-","\t");
   %string = strReplace(%string," ","\t");
   
   %stageCheck = 0;
   for(%i=0;%i<getFieldCount(%string);%i++)
   {
      if(stringMatch(getField(%string,%i),"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
      {
         if(%stageCheck $= 0)
         {
            if(strlen(getField(%string,%i)) $= 5)
            {
               %stageCheck++;
            }
         }
         else
         {
            if(strlen(getField(%string,%i)) $= 4)
            {
               %stageCheck++;
               
               if(%stageCheck > 3)
                  return 1;
            }
            else
            {
               %stageCheck = 0;
            }
         }
      }
      else
         %stageCheck = 0;
   }
   return 0;
}

function TCPObject::sendRequest(%this,%cmd,%string,%layer,%specialstring)
{
   if(%this.transmitting || %this.connected)
   {
      if(%this.requestLayer $= %layer)
      {
         %this.disconnect();
         %this.onDisconnect(1);
         %this.schedule(50,"sendRequest",%cmd,%string,%layer,%specialstring);
      }
      else
         %this.pushQueue(%cmd,%string,%layer);
         
      return;
   }
   
   %cmd = strUpr(%cmd);
   %this.requestLayer = %layer;
   %this.transmitting = 1;
   %this.t_command = %cmd;
   %this.t_command[%layer] = %cmd;
   %this.t_string[%layer] = %string;
   %this.t_specialstring[%layer] = %specialstring;
   if(%string $= "")
      %string = "c="@strUpr(%cmd)@"&n="@urlEnc($pref::Player::NetName);
   else
      %string = "c="@strUpr(%cmd)@"&n="@urlEnc($pref::Player::NetName)@"&"@%string;
   
   if($RTB::Cache::Cookie !$= "")
      %string = %string@"&"@$RTB::Cache::Cookie;
   
   %string = %string@%specialstring;
      
   %contentLength = strLen(%string);
   %this.cmd = "POST "@%this.filePath@" HTTP/1.1\r\nHost: "@%this.site@"\r\nUser-Agent: Torque/1.0\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: "@%contentLength@"\r\n\r\n"@%string@"\r\n";
   
   if(isEventPending(%this.timeoutSchedule))
      cancel(%this.timeoutSchedule);
      
   %this.numTries = 0;
   %this.timeoutSchedule = %this.schedule($RTB::TransmissionTimeout*1000,"onTimeout");
   
   if($RTB::Debug)
      echo("\c4>> Transmit Requested ("@%cmd@" - "@%string@")");
      
   if(!%this.connected)
      %this.connect(%this.site@":"@%this.port);
   else
      %this.beginTransmit();
}

function TCPObject::beginTransmit(%this)
{
   if(!%this.connected)
      return;
      
   %this.send(%this.cmd);
}

//*********************************************************
//* Support Functions
//*********************************************************
function filterString(%string,%allowed)
{
   for(%i=0;%i<strLen(%string);%i++)
   {
      %char = getSubStr(%string,%i,1);
      if(strPos(%allowed,%char) >= 0)
         %return = %return@%char;
   }
   return %return;
}

function stringMatch(%string,%allowed)
{
   for(%i=0;%i<strLen(%string);%i++)
   {
      %char = getSubStr(%string,%i,1);
      if(strPos(%allowed,%char) < 0)
         return 0;
   }
   return 1;
}

function SimGroup::clear(%this)
{
   while(%this.getCount() > 0)
   {
      %this.getObject(0).delete();
   }
}

function isReadonly(%file)
{
   if(isWriteableFilename(%file))
      return 0;
   else
      return 1;
}

function RTB_addControlMap(%inputDevice,%keys,%name,%command)
{
   if(!$RTB::addedCatSep)
   {
	   $remapDivision[$remapCount] = "Return to Blockland";
	   $RTB::addedCatSep = 1;
   }
	$remapName[$remapCount] = %name;
	$remapCmd[$remapCount] = %command;
	$remapCount++;
}

function RTBMM_getFieldFromContents(%contents,%field)
{
   for(%i=0;%i<getFieldCount(%contents);%i++)
   {
      %item = getField(%contents,%i);
      if(strPos(%item,":") >= 0)
      {
         if(getSubStr(%item,0,strPos(%item,":")) $= %field)
            return getSubStr(%item,strPos(%item,":")+2,strLen(%item));
      }
      else if(%field $= "" && %item !$= "")
         return %item;
      else
         return 0;
   }
}

function byteRound(%bytes)
{
	if(%bytes $= "")
		return "0b";

	if(%bytes > 1048576)
		%result = roundMegs(%bytes/1048576)@"MB";
	else if(%bytes > 1024)
		%result = mFloatLength(%bytes/1024,2)@"kB";
	else
		%result = %bytes@"b";
	return %result;
}

function isInt(%string)
{
	%numbers = "-0123456789";
	for(%i=0;%i<strLen(%string);%i++)
	{
		%char = getSubStr(%string,%i,1);
		if(strPos(%numbers,%char) $= -1)
			return 0;
	}
	return 1;
}

function alphaCompare(%string1,%string2)
{
   %alphabet = "abcdefghijklmnopqrstuvwxyz";
   
   %longString = %string1;
   %shortString = %string2;
   %longNum = 1;
   %shortNum = 2;
   if(strLen(%string2) > strLen(%string1))
   {
      %longString = %string2;
      %shortString = %string1;
      %longNum = 2;
      %shortNum = 1;
   }
   
   %winString = 1;
   for(%i=0;%i<strLen(%longString);%i++)
   {
      if(%i<strLen(%shortString))
      {
         %longChar = strLwr(getSubStr(%longString,%i,1));
         %shortChar = strLwr(getSubStr(%shortString,%i,1));
         
         if(strPos(%alphabet,%longChar) < strPos(%alphabet,%shortChar))
         {
            %winString = %longNum;
            break;
         }
         else if(strPos(%alphabet,%longChar) > strPos(%alphabet,%shortChar))
         {
            %winString = %shortNum;
            break;
         }
      }
   }
   return %winString;
}

function getFileContents(%file)
{
   %IO = new FileObject();
   if(%IO.openForRead(%file))
   {
      while(!%IO.isEOF())
      {
         %return = (%return $= "") ? %IO.readLine() : %return TAB %IO.readLine();
      }
      %IO.delete();
      return %return;
   }
   else
      return 0;
}