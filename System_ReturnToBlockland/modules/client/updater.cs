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
//#   Modules / Client / Updater
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::Updater = 1;
	
//*********************************************************
//* Module Class
//*********************************************************
new ScriptObject(RTB_Client_Updater)
{
   // tcp factory
   tcp = RTB_Networking.createFactory("api.returntoblockland.com","80","/apiRouter.php?d=APIUM");
};
RTBGroup.add(RTB_Client_Updater);
	
//*********************************************************
//* API Methods
//*********************************************************
//- RTB_Client_Updater::getUpdates (retrieves rtb updates from the server)
function RTB_Client_Updater::getUpdates(%this)
{
   %data = %this.tcp.getPostData("GETUPDATES",0,$RTB::Version,$Version);
   
   %this.tcp.post(%data,%this,"onUpdateReply");
}
	
//- RTB_Client_Updater::onUpdateReply (Reply from version controller)
function RTB_Client_Updater::onUpdateReply(%this,%tcp,%factory,%line)
{
   if(getField(%line,1))
   {
      %version = getField(%line,2);
      %date = getField(%line,3);
      %filesize = getField(%line,4);
      
      canvas.pushDialog(RTB_Updater);
      RTBCU_Version.setText("v"@%version);
      RTBCU_Date.setText(%date);
      RTBCU_Size.setText(byteRound(%filesize));
      RTBCU_Speed.setText("N/A");
      RTBCU_Done.setText("0kb");
      
      RTBCU_Progress.setValue(0);
      RTBCU_ProgressText.setValue("Ready to Download");
      
      RTBCU_UpdateButton.setActive(1);
      RTBCU_UpdateButton.command = "RTB_Client_Updater.downloadUpdate(\""@%version@"\");";
      RTBCU_ChangeLogButton.command = "RTB_Client_Updater.getChangeLog(\""@%version@"\");";
   }
}

//- RTB_Client_Updater::getChangeLog (retrieves change log for specific version)
function RTB_Client_Updater::getChangeLog(%this,%version)
{
   %data = %this.tcp.getPostData("GETCHANGELOG",%version);
   
   %this.tcp.post(%data,%this,"onChangeLog","","","onChangeLogEnd");
   
   RTBCU_ChangeLog_Title.setText("Change Log for RTB v"@%version@":");
   RTBCU_ChangeLog_Text.setText("");
   
   MessagePopup("Please Wait","Locating Change Log for RTB v"@%version@"...");
}

//- RTB_Client_Updater::onChangeLog (handles change log response)
function RTB_Client_Updater::onChangeLog(%this,%tcp,%factory,%line)
{
   if(getField(%line,1) $= 0)
   {
      MessageBoxOK("Oh Dear","The Change Log for this update could not be found. Sorry.");
      MessagePopup("","",1);
   }
   else
      RTBCU_ChangeLog_Text.setText(RTBCU_ChangeLog_Text.getText()@getField(%line,1)@"<br>");
}

//- RTB_Client_Updater::onChangeLogEnd (Callback for end of changelog transmission)
function RTB_Client_Updater::onChangeLogEnd(%this,%tcp,%factory)
{
   if(RTBCU_ChangeLog_Text.getText() !$= "")
   {
      canvas.pushDialog(RTBCU_ChangeLog);
   }
   MessagePopup("","",1);
}
RTB_Client_Updater.getUpdates();

//*********************************************************
//* Update Downloader
//*********************************************************
//- RTB_Client_Updater::downloadUpdate (downloads a new version)
function RTB_Client_Updater::downloadUpdate(%this,%version)
{
   if(!isWriteableFilename("Add-Ons/System_ReturnToBlockland.zip"))
   {
      MessageBoxOK("Oh Dear!","Your System_ReturnToBlockland.zip is read-only and cannot be overwritten.\n\nPlease download the latest RTB manually from our website, or set System_ReturnToBlockland.zip to not be read-only.");
      return;
   }
   
   if(isObject(RTB_Client_Updater_TCP))
      RTB_Client_Updater_TCP.setName("");
   
   %tcp = new TCPObject(RTB_Client_Updater_TCP)
   {
      version = %version;
   };
   RTBGroup.add(%tcp);

   %tcp.connect("api.returntoblockland.com:80");
   
   RTBCU_UpdateButton.setActive(0);
   RTBCU_ProgressText.setText("Locating Update...");
}

//- RTB_Client_Updater_TCP::onConnected (Connection success callback)
function RTB_Client_Updater_TCP::onConnected(%this)
{
   %content = "c=DLUPDATE&n="@$Pref::Player::NetName@"&arg1="@%this.version@"&"@$RTB::Connection::Session;
   %contentLen = strLen(%content);
   
   %this.send("POST /apiRouter.php?d=APIUM HTTP/1.1\r\nHost: api.returntoblockland.com\r\nUser-Agent: Torque/1.0\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: "@%contentLen@"\r\n\r\n"@%content@"\r\n");
}

//- RTB_Client_Updater_TCP::onLine (Callback for line response)
function RTB_Client_Updater_TCP::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      MessageBoxOK("Error!","An error occured with the updater service and the update could not be located.");
      canvas.popDialog(RTB_Updater);
      return;
   }
   
   if(strPos(%line,"DL-Result:") $= 0)
   {
      %this.dlResult = getWord(%line,1);
      if(getWord(%line,1) $= 0)
      {
         MessageBoxOK("Error!","An error occured with the updater service and the update could not be located.");
         Canvas.popDialog(RTB_Updater);
         return;
      }
   }
   
   if(strPos(%line,"Content-Length:") $= 0)
      %this.contentSize = getWord(%line,1);
      
   if(%line $= "")
   {
      if(%this.dlResult !$= 1)
      {
         MessageBoxOK("Error!","An error occured with the updater service and the update could not be located.");
         Canvas.popDialog(RTB_Updater);
         return;
      }
      %this.setBinarySize(%this.contentSize);
   }
}

//- RTB_Client_Updater_TCP::onBinChunk (On binary chunk received)
function RTB_Client_Updater_TCP::onBinChunk(%this,%chunk)
{
   if(%this.timeStarted $= "")
      %this.timeStarted = getSimTime();
      
   if(%chunk >= %this.contentSize)
   {
      if(isWriteableFilename("Add-Ons/System_ReturnToBlockland.zip"))
      {
         %this.saveBufferToFile("Add-Ons/System_ReturnToBlockland.zip.new");
         %this.disconnect();
         
         RTBCU_Progress.setValue(1);
         RTBCU_ProgressText.setText("Download Complete");
         RTBCU_Speed.setText("N/A");
         RTBCU_Done.setText(byteRound(%this.contentSize));
         
         if(fileDelete("Add-Ons/System_ReturnToBlockland.zip"))
         {
            fileCopy("Add-Ons/System_ReturnToBlockland.zip.new","Add-Ons/System_ReturnToBlockland.zip");
            fileDelete("Add-Ons/System_ReturnToBlockland.zip.new");
            
            MessageBoxOK("Huzzah!","You have successfully downloaded RTB v"@%this.version@".\n\nBlockland must now close to complete the install.","quit();");
         }
         else
         {
            MessageBoxOK("Whoops!","Unable to delete System_ReturnToBlockland.zip to replace it with the new version.\n\nPlease go to your Add-Ons folder and replace System_ReturnToBlockland.zip with System_ReturnToBlockland.zip.new to complete the update.");
         }
      }
      else
      {
         MessageBoxOK("Oh Dear!","Unable to save RTB v"@%this.version@". Your System_ReturnToBlockland.zip is read-only and cannot be overwritten.\n\nPlease download the latest RTB manually from the website.");
      }
   }
   else
   {
      RTBCU_Progress.setValue(%chunk/%this.contentSize);
      RTBCU_ProgressText.setText(mFloor((%chunk/%this.contentSize)*100)@"%");
      RTBCU_Speed.setText(mFloatLength(%chunk/(getSimTime()-%this.timeStarted),2)@"kb/s");
      RTBCU_Done.setText(byteRound(%chunk));
   }
}

//- RTB_Client_Updater_TCP::onDisconnect (disconnected callback)
function RTB_Client_Updater_TCP::onDisconnect(%this)
{
   %this.delete();
}