//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 532 $
//#      $Date: 2011-12-06 19:01:32 +0000 (Tue, 06 Dec 2011) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/support/networking.cs $
//#
//#      $Id: networking.cs 532 2011-12-06 19:01:32Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Support / Networking
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Support::Networking = 1;

//*********************************************************
//* Module Class
//*********************************************************
new ScriptGroup(RTB_Networking)
{
   cookie = "";
};
RTBGroup.add(RTB_Networking);

//- RTB_Networking::createFactory (creates a new tcp factory)
function RTB_Networking::createFactory(%this,%host,%port,%resource)
{
   %factory = new ScriptGroup()
   {
      class = "RTB_TCPFactory";
      
      host = %host;
      port = %port;
      resource = %resource;
      
      headers = 0;
   };
   %factory.setHeader("Host",%host);
   %factory.setHeader("User-Agent","RTB/4.0");
   %factory.setHeader("Connection","close");
   
   %this.add(%factory);
   
   return %factory;
}

//*********************************************************
//* Factory Methods
//*********************************************************
//- RTB_TCPFactory::setHeader (sets a header)
function RTB_TCPFactory::setHeader(%this,%header,%value)
{
   if(%this.header[%header] !$= "")
   {
      %this.header[%header] = %value;
      return %this;
   }
   %this.header[%header] = %value;
   %this.header[%this.headers] = %header;
   %this.headers++;
   
   return %this;
}

//- RTB_TCPFactory::getHeader (returns header value)
function RTB_TCPFactory::getHeader(%this,%header)
{
   return %this.header[%header];
}

//- RTB_TCPFactory::getPostData (puts data into string)
function RTB_TCPFactory::getPostData(%this,%cmd,%d1,%d2,%d3,%d4,%d5,%d6,%d7,%d8,%d9,%d10,%d11)
{
   if(%d11 !$= "")
   {
      error("Only 10 arguments accepted in RTB_TCPFactory::getPostData");
      return "";
   }
   
   if(%cmd $= "")
      return "";
   
   %data = "n=" @ urlEnc($pref::Player::NetName) @ "&b=" @ getNumKeyID() @ "&c=" @ urlEnc(%cmd) @ "&";
   for(%i=1;%i<11;%i++)
   {
      %data = %data @ "arg" @ %i @ "=" @ urlEnc(%d[%i]) @ "&";
   }
   return getSubStr(%data,0,strLen(%data)-1);
}

//- RTB_TCPFactory::get (makes a GET request)
function RTB_TCPFactory::get(%this,%data,%module,%successCallback,%failCallback,%startCallback,%endCallback)
{
   %request = "GET " @ %this.resource @ "&" @ %data @ " HTTP/1.1\r\n";

   if(RTB_Networking.cookie !$= "")
      %request = %request @ "Cookie: " @ RTB_Networking.cookie @ "\r\n";
      
   for(%i=0;%i<%this.headers;%i++)
   {
      %header = %this.header[%i];
      %request = %request @ %header @ ": " @ %this.header[%header] @ "\r\n";
   }
   %request = %request @ "\r\n";
   
   %this.request(%request,%module,%successCallback,%failCallback,%startCallback,%endCallback);
}

//- RTB_TCPFactory::get (makes a POST request)
function RTB_TCPFactory::post(%this,%data,%module,%successCallback,%failCallback,%startCallback,%endCallback)
{
   %request = "POST " @ %this.resource @ " HTTP/1.1\r\n";
   
   %request = %request @ "Content-Length: " @ strlen(%data) @ "\r\n";
   %request = %request @ "Content-Type: application/x-www-form-urlencoded\r\n";
   
   if(RTB_Networking.cookie !$= "")
      %request = %request @ "Cookie: " @ RTB_Networking.cookie @ "\r\n";
      
   for(%i=0;%i<%this.headers;%i++)
   {
      %header = %this.header[%i];
      %request = %request @ %header @ ": " @ %this.header[%header] @ "\r\n";
   }
   %request = %request @ "\r\n" @ %data;
   
   %this.request(%request,%module,%successCallback,%failCallback,%startCallback,%endCallback);
}

//- RTB_TCPFactory::request (sets up a request using the data built in one of the request methods)
function RTB_TCPFactory::request(%this,%request,%module,%lineCallback,%failCallback,%startCallback,%endCallback)
{
   %tcp = new TCPObject()
   {
      connected = false;
      receiving = false;
      
      dead = false;
      
      request = %request;
      
      module = %module;
      lineCallback = %lineCallback;
      failCallback = %failCallback;
      startCallback = %startCallback;
      endCallback = %endCallback;
      
      factory = %this;
      
      rtb = true;
   };
   %this.add(%tcp);
   
   %tcp.connect(%this.host @ ":" @ %this.port);
}

//- RTB_TCPFactory::getTCP (gets the active tcp object if it exists)
function RTB_TCPFactory::getTCP(%this,%index)
{
   if(!%index)
      %index = 0;
      
   for(%i=0;%i<%this.getCount();%i++)
   {
      %obj = %this.getObject(%i);
      if(%obj.getClassName() $= "TCPObject" && !%obj.dead)
      {
         %index--;
         if(%index < 0)
            return %obj;
      }
   }
   return false;
}

//- RTB_TCPFactory::killTCP (kills the active tcp)
function RTB_TCPFactory::killTCP(%this)
{
   if(%tcp = %this.getTCP())
      %tcp.dead = true;
}

//*********************************************************
//* RTB_Support_Networking Package
//*
//* Kind of relies on the following assumptions given that
//* calling Parent::whatever kinda sucks in this case.
//*
//* 1) This package can't overwrite default BL TCP Objects.
//* 2) RTB gets executed before any other add-on using
//*    TCP objects.
//*********************************************************
package RTB_Support_Networking
{
   //- TCPObject::onConnected (onConnected callback)
   function TCPObject::onConnected(%this)
   {
      if(!%this.rtb)
         return Parent::onConnected(%this);
      
      if(%this.dead)
         return;
         
      if($RTB::Debug)
         echo("\c4>> TCP Connected");

      %this.connected = true;      
      
      %this.makeRequest();
   }
   
   //- TCPObject::onLine (onLine callback)
   function TCPObject::onLine(%this,%line)
   {
      if(!%this.rtb)
         return Parent::onLine(%this,%line);
         
      if(%this.dead)
         return;
         
      if($RTB::Debug)
         echo("\c2>> TCP Line Received ("@%line@")");
         
      if(%this.httpResponseCode $= "")
      {
         %this.httpResponseCode = getWord(%line,1);
         
         if(%this.httpResponseCode !$= "200")
         {
            if(%this.failCallback !$= "")
               eval(%this.module.getName()@"::"@%this.failCallback@"("@%this.module.getID()@","@%this@","@%this.factory@",\"HTTP\",\""@%this.httpResponseCode@"\");");
            %this.dead = true;
            return;
         }
      }
      
      if(%line $= "END")
      {
         if(%this.endCallback !$= "")
            eval(%this.module.getName()@"::"@%this.endCallback@"("@%this.module.getID()@","@%this@","@%this.factory@");");
            
         %this.receiving = false;
         %this.dead = true;
         return;
      }
      
      if(getField(%line,0) $= "NOTIFY")
      {
         echo("\c2"@getFields(%line,1,getFieldCount(%line)));
         return;
      }
      
      %escapedLine = strReplace(%line,"\\","\\\\");
      %escapedLine = strReplace(%escapedLine,"\"","\\\"");
         
      if(%this.receiving)
         if(%this.lineCallback !$= "")
            eval(%this.module.getName()@"::"@%this.lineCallback@"("@%this.module.getID()@","@%this@","@%this.factory@",\""@%escapedLine@"\");");

      if(!%this.receiving)
      {
         if(%line $= "")
         {
            %this.receiving = true;
            
            if(%this.startCallback !$= "")
               eval(%this.module.getName()@"::"@%this.startCallback@"("@%this.module.getID()@","@%this@","@%this.factory@");");
         }
         else
         {
            if(firstWord(%line) $= "Set-Cookie:")
               RTB_Networking.cookie = restWords(%line);
         }
      }
   }
   
   //- TCPObject::onConnectFailed (onConnectFailed callback)
   function TCPObject::onConnectFailed(%this)
   {
      if(!%this.rtb)
         return Parent::onConnectFailed(%this);
         
      if(%this.dead)
         return;
         
      if($RTB::Debug)
         echo("\c2>> TCP Connect Failed");
      
      if(%this.failCallback !$= "")
         eval(%this.module.getName()@"::"@%this.failCallback@"("@%this.module@","@%this@","@%this.factory@",\"Fail\");");
   }
   
   //- TCPObject::onDNSFailed (onDNSFailed callback)
   function TCPObject::onDNSFailed(%this)
   {
      if(!%this.rtb)
         return Parent::onDNSFailed(%this);
         
      if(%this.dead)
         return;
      
      if($RTB::Debug)
         echo("\c2>> TCP DNS Failed");
      
      if(%this.failCallback !$= "")
         eval(%this.module.getName()@"::"@%this.failCallback@"("@%this.module@","@%this@","@%this.factory@",\"DNS\");");
   }
   
   //- TCPObject::onDisconnect (onDisconnect callback)
   function TCPObject::onDisconnect(%this)
   {
      if(!%this.rtb)
         return Parent::onDisconnect(%this);
         
      %this.delete();
      
      if(%this.dead)
         return;
   }
   
   //- TCPObject::makeRequest (makes a pre-defined request)
   function TCPObject::makeRequest(%this)
   {
      if(!%this.connected || %this.dead)
         return;
         
      if($RTB::Debug)
         echo("\c5>> TCP Data Sent");
         
      if($RTB::Debug > 1)
         echo("\c5"@strReplace(%this.request,"\r\n","\r\n\c5"));
         
      %this.send(%this.request);
   }
   
   function Script_GUI::getClassName(%this)
   {
      return %this.className;
   }
   
   function Script_GUI::getValue(%this)
   {
      return %this.text;
   }
};