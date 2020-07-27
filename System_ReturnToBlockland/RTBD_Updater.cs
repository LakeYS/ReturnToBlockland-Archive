//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Auto-Updater for RTB
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBD_Updater = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::DUpdater::HostSite = "returntoblockland.com";
$RTB::DUpdater::FilePath = "/blockland/rtbUpdateRouter.php";

//*********************************************************
//* Version Checking
//*********************************************************
function RTBDU_InitSC()
{
   if(!isObject(RTBDU_SC))
   {
      new TCPObject(RTBDU_SC)
      {
         site = $RTB::DUpdater::HostSite;
         port = 80;
         cmd = "";
         filePath = $RTB::DUpdater::FilePath;
         
         defaultFailHandle = "RTBDU_onCommFail";
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      
      //Check for Updates
      RTBDU_SC.addResponseHandle("GETVERSION","RTBDU_onVersion");
   }
}

function RTBDU_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBDU_SC))
      RTBDU_InitSC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   RTBDU_SC.sendRequest(%cmd,%argString,%layer);
}

function RTBDU_onCommFail()
{
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Could not connect to RTB Update Routing Service.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
}

function checkRTBUpdates()
{
   RTBDU_SendRequest("GETVERSION",1,$RTB::Version);
}

function RTBDU_onVersion(%this,%line)
{
   if(getField(%line,0) $= 1)
   {
      $RTB::DUpdater::Cache::Version = getField(%line,1);
      
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("RTB v"@getField(%line,1)@" is Available");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("File Size: "@byteRound(getField(%line,3)));
      RTBDU_drawDOSRow("Release Date: "@getField(%line,2));
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("** Type doRTBUpdate(); to Install this Version **");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
   }
   else
   {
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("No RTB Updates are available at this time.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
   }
}

//*********************************************************
//* Update Downloading
//*********************************************************
function RTBDU_InitFC()
{
   if(isObject(RTBDU_FC))
      return;
      
   new TCPObject(RTBDU_FC);
}

function doRTBUpdate()
{
   %vers = $RTB::DUpdater::Cache::Version;
   if(%vers $= "")
   {
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("No Version Selected!");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("Please do checkRTBUpdates(); first!");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
      return;
   }
   
   RTBDU_InitFC();
   
   RTBDU_FC.setBinary(0);
   RTBDU_FC.lastLine = "";
   RTBDU_FC.targetVersion = %vers;
   
   RTBDU_FC.connect($RTB::DUpdater::HostSite@":80");
}

function RTBDU_FC::onConnected(%this)
{
   %content = "c=GETDOWNLOAD&n="@urlEnc($Pref::Player::NetName)@"&arg1="@%this.targetVersion;
   %this.send("POST http://"@$RTB::DUpdater::HostSite@$RTB::DUpdater::FilePath@" HTTP/1.1\r\nHost: "@$RTB::DUpdater::HostSite@"\r\nUser-Agent: Torque/1.0\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: "@strLen(%content)@"\r\n\r\n"@%content@"\r\n");
}

function RTBDU_FC::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("The Filepath to RTB v"@%this.targetVersion@" is Invalid.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
      return;
   }
   
   if(strPos(%line,"Content-Length:") $= 0)
      %this.contentSize = getWord(%line,1);
      
   if(%line $= "")
      %this.setBinarySize(%this.contentSize);
      
   %this.lastLine = %line;
}

function RTBDU_FC::onBinChunk(%this,%chunk)
{
   if(%chunk >= %this.contentSize)
   {
      %this.saveBufferToFile("Add-Ons/System_ReturnToBlockland.zip");
      %this.disconnect();
      
      echo("");
      RTBDU_drawSpacer();
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("RTB v"@%this.targetVersion@" has been downloaded and installed successfully.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("You may now restart your server.");
      RTBDU_drawDOSRow("");
      RTBDU_drawDOSRow("");
      RTBDU_drawSpacer();
      echo("");
   }
}

//*********************************************************
//* Support Functions
//*********************************************************
function RTBDU_drawDOSRow(%string)
{
   %boxStart = ((80-70)-2)/2;
   %white = RTBDU_getWhitespace(%boxStart);
   
   %edgeSpace = (68-strLen(%string))/2;
   if(strPos(%edgeSpace,".5") >= 0)
      %minus = 1;
   %space = RTBDU_getWhitespace(%edgeSpace);
   %space2 = RTBDU_getWhitespace(%edgeSpace-%minus);
   
   echo(%white@"*"@%space@%string@%space2@"*");
}

function RTBDU_drawSpacer()
{
   echo("    **********************************************************************");
}

function RTBDU_getWhitespace(%length)
{
   for(%i=0;%i<%length;%i++)
   {
      %white = %white@" ";
   }
   return %white;
}