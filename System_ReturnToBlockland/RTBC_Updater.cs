//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBC_Updater.cs $
//#
//#      $Id: RTBC_Updater.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Auto-Updater for RTB
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_Updater = 1;

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_Updater))
	exec("./RTB_Updater.gui");

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::CUpdater::HostSite = "returntoblockland.com";
$RTB::CUpdater::FilePath = "/blockland/rtbUpdateRouter.php";

//*********************************************************
//* Version Checking
//*********************************************************
function RTBCU_InitSC()
{
   if(!isObject(RTBCU_SC))
   {
      new TCPObject(RTBCU_SC)
      {
         site = $RTB::CUpdater::HostSite;
         port = 80;
         cmd = "";
         filePath = $RTB::CUpdater::FilePath;
         
         defaultFailHandle = "RTBCU_onCommFail";
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      
      //Check for Updates
      RTBCU_SC.addResponseHandle("GETVERSION","RTBCU_onVersion");
      
      //Get Change Log
      RTBCU_SC.addResponseHandle("GETCHANGELOG","RTBCU_onChangeLog");
      RTBCU_SC.addResponseHandle("ENDGETCHANGELOG","RTBCU_onEndChangeLog");
   }
}

function RTBCU_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBCU_SC))
      RTBCU_InitSC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   RTBCU_SC.sendRequest(%cmd,%argString,%layer);
}

function RTBCU_onCommFail()
{
   MessagePopup("","",1);
}

function RTBCU_Update()
{
   if($RTB::Options::EnableAutoUpdate && !$RTB::CUpdater::Cache::HasBeenPrompted)
   {
      $RTB::CUpdater::Cache::HasBeenPrompted = 1;
      RTBCU_SendRequest("GETVERSION",1,$RTB::Version,$Version);
   }
}

function RTBCU_GetChangeLog(%version)
{
   RTBCU_SendRequest("GETCHANGELOG",1,%version);
   RTBCU_ChangeLog_Title.setText("Change Log for RTB v"@%version@":");
   RTBCU_ChangeLog_Text.setText("");
   
   MessagePopup("Please Wait","Locating Change Log for RTB v"@%version@"...");
}

function RTBCU_onChangeLog(%this,%line)
{
   if(%line $= 0)
   {
      MessageBoxOK("Oh Dear","The Change Log for this update could not be found. Sorry.");
      MessagePopup("","",1);
      %this.disconnect();
   }
   else
      RTBCU_ChangeLog_Text.setText(RTBCU_ChangeLog_Text.getText()@%line@"<br>");
}

function RTBCU_onEndChangeLog()
{
   if(RTBCU_ChangeLog_Text.getText() !$= "")
   {
      canvas.pushDialog(RTBCU_ChangeLog);
   }
   MessagePopup("","",1);
}

function RTBCU_onVersion(%this,%line)
{
   if(getField(%line,0) $= 1)
   {
      canvas.pushDialog(RTB_Updater);
      RTBCU_Version.setText("v"@getField(%line,1));
      RTBCU_Date.setText(getField(%line,2));
      RTBCU_Size.setText(byteRound(getField(%line,3)));
      RTBCU_Speed.setText("N/A");
      RTBCU_Done.setText("0kb");
      
      RTBCU_Progress.setValue(0);
      RTBCU_ProgressText.setValue("Ready to Download");
      
      RTBCU_UpdateButton.setActive(1);
      RTBCU_UpdateButton.command = "RTBCU_DownloadUpdate(\""@getField(%line,1)@"\");";
      RTBCU_ChangeLogButton.command = "RTBCU_GetChangeLog(\""@getField(%line,1)@"\");";
   }
}
RTBCU_Update();

//*********************************************************
//* Update Downloading
//*********************************************************
function RTBCU_InitFC()
{
   if(isObject(RTBCU_FC))
      return;
      
   new TCPObject(RTBCU_FC);
}

function RTBCU_DownloadUpdate(%vers)
{
   if(isReadonly("Add-Ons/System_ReturnToBlockland.zip"))
   {
      messageBoxOK("ERROR", "It looks like Blockland cannot overwrite the System_ReturnToBlockland.zip folder so the auto-updater cannot work. Please either resolve this or download the latest RTB version manually from our website.");
      return;
   }
   
   RTBCU_InitFC();
   
   RTBCU_FC.setBinary(0);
   RTBCU_FC.lastLine = "";
   RTBCU_FC.targetVersion = %vers;
   
   RTBCU_FC.connect($RTB::CUpdater::HostSite@":80");
   
   RTBCU_UpdateButton.setActive(0);
   RTBCU_ProgressText.setText("Locating Update...");
}

function RTBCU_FC::onConnected(%this)
{
   %content = "c=GETDOWNLOAD&n="@urlEnc($pref::Player::NetName)@"&arg1="@%this.targetVersion;
   %this.send("POST http://"@$RTB::CUpdater::HostSite@$RTB::CUpdater::FilePath@" HTTP/1.1\r\nHost: "@$RTB::CUpdater::HostSite@"\r\nUser-Agent: Torque/1.0\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: "@strLen(%content)@"\r\n\r\n"@%content@"\r\n");
}

function RTBCU_FC::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      MessageBoxOK("Error!","There appears to be an error with the update and the ZIP cannot be located.");
      canvas.popDialog(RTB_Updater);
      return;
   }
   
   if(strPos(%line,"Content-Length:") $= 0)
      %this.contentSize = getWord(%line,1);
      
   if(%line $= "")
      %this.setBinarySize(%this.contentSize);
      
   %this.lastLine = %line;
}

function RTBCU_FC::onBinChunk(%this,%chunk)
{
   if(%this.timeStarted $= "")
      %this.timeStarted = getSimTime();
      
   if(%chunk >= %this.contentSize)
   {
      %this.saveBufferToFile("Add-Ons/System_ReturnToBlockland.zip");
      %this.disconnect();
         
      RTBCU_Progress.setValue(1);
      RTBCU_ProgressText.setText("Download Complete");
      RTBCU_Speed.setText("N/A");
      RTBCU_Done.setText(byteRound(%this.contentSize));
      
      MessageBoxOK("Huzzah!","You have successfully downloaded RTB v"@%this.targetVersion@".\n\nBlockland must now close to complete the install.","quit();");
   }
   else
   {
      RTBCU_Progress.setValue(%chunk/%this.contentSize);
      RTBCU_ProgressText.setText(mFloor((%chunk/%this.contentSize)*100)@"%");
      RTBCU_Speed.setText(mFloatLength(%chunk/(getSimTime()-%this.timeStarted),2)@"kb/s");
      RTBCU_Done.setText(byteRound(%chunk));
   }
}