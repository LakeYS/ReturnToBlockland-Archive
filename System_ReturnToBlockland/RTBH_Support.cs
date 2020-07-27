//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Hybrid Support Script
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBH_Support = 1;

//*********************************************************
//* Global Transmission Variables
//*********************************************************
$RTB::TransmissionTimeout = 30; //> 30 Second Timeout (Sometimes the server just "hangs")
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
            RTBBT_pushBugReporter("RTB","SQL Error",2,getFields(%line,1,getFieldCount(%line)));
            MessageBoxOK("ERROR","There has been a critical error. We would appreciate it if you could send this Bug Report so it can be fixed ASAP.");
            return;
         }
         
         if(getField(%line,0) $= "ERROR")
         {
            %this.transmitting = 0;
            echo("\c2TCP ERROR ("@getField(%line,1)@"): "@getField(%line,2));
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
         Parent::onLine(%this,%line);
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
         Parent::onConnected(%this);
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
         Parent::onConnectFailed(%this);
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
         Parent::onDNSFailed(%this);
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
	%numbers = "0123456789";
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