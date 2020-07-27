//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Bug Tracker (RTBBT/CBugTracker)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_BugTracker = 1;

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_BugTracker))
	exec("./RTB_BugTracker.gui");

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::CBugTracker::HostSite= "returntoblockland.com";
$RTB::CBugTracker::FilePath = "/blockland/rtbBugReport.php";

//*********************************************************
//* Transmission Protocol
//*********************************************************
function RTBBT_InitSC()
{
   if(!isObject(RTBBT_SC))
   {
      new TCPObject(RTBBT_SC)
      {
         site = $RTB::CBugTracker::HostSite;
         port = 80;
         cmd = "";
         filePath = $RTB::CBugTracker::FilePath;
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      
      RTBBT_SC.addResponseHandle("REPORT","RTBBT_onReportReply");
      RTBBT_SC.addFailHandle("REPORT","RTBBT_onReportFail");
   }
}

function RTBBT_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBBT_SC))
      RTBBT_InitSC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   RTBBT_SC.sendRequest(%cmd,%argString,%layer);
}

//*********************************************************
//* Operational Functions
//*********************************************************
function RTBBT_pushBugReporter(%feature,%summary,%priority,%description)
{
   canvas.pushDialog(RTB_BugTracker);
   RTBBT_Feature.setValue(%feature);
   RTBBT_Summary.setValue(%summary);
   
   RTBBT_Priority.clear();
   RTBBT_Priority.add("Minor",0);
   RTBBT_Priority.add("Major",1);
   RTBBT_Priority.add("Critical",2);
   
   RTBBT_Priority.setSelected(%priority);
   RTBBT_Description.setValue(%description);
}

function RTBBT_sendBugReport()
{
   %feature = RTBBT_Feature.getValue();
   if(%feature $= "")
   {
      MessageBoxOK("Ooops","You haven't selected a Feature in your Bug Report.");
      return;
   }
   %summary = RTBBT_Summary.getValue();
   if(%summary $= "")
   {
      MessageBoxOK("Ooops","You haven't summarised your Bug Report.");
      return;
   }
   %description = RTBBT_Description.getValue();
   if(%description $= "")
   {
      MessageBoxOK("Ooops","You haven't written a description for your Bug Report.");
      return;
   }
   %priority = RTBBT_Priority.getSelected();
   
   MessagePopup("Connecting...","Now sending your Bug Report to the RTB Database.\n\nThanks for your co-operation.");
   
	%mod = FindFirstFile("Add-Ons/*_*/description.txt");
	while(strLen(%mod) > 0)
	{
	   %modName = getSubStr(%mod,8,strLen(%mod)-24);
	   %modVarName = getSafeVariableName(%modName);
	   
	   if($AddOn__[%modVarName] $= 1 || (isFile("Add-Ons/"@%modName@"/client.cs") && !isFile("Add-Ons/"@%modName@"/server.cs")))
	   {
         if(isFile("Add-Ons/"@%modName@".zip"))
            %field = %modName@".zip";
         else
            %field = %modName;

	      %modArray = (%modArray $= "") ? "" : %modArray@";";
         %modArray = %modArray@%field;
	   }
		%mod = FindNextFile("Add-Ons/*_*/description.txt");
	}

   if(isObject(ServerConnection))
   {
      %address = ServerConnection.getAddress();
      if(%address $= "local")
      {
         if($Server::LAN)
            if($Server::ServerType $= "SinglePlayer")
               %status = "Hosting Singleplayer Server";
            else
               %status = "Hosting LAN Server";
         else
            %status = "Hosting Internet Server";
      }
      else
         %status = "Playing on Internet Server";
   }
   else
      %status = "Not on a Server";
   
   RTBBT_SendRequest("REPORT",1,%feature,%summary,%priority,%description,getFileCRC("Add-Ons/System_ReturnToBlockland.zip"),%modArray,getTimeString($Sim::Time),$RTB::Version,%status);
}

function RTBBT_onReportReply(%this,%line)
{
   MessagePopup("","",1);
   if(getField(%line,0) $= "WIN")
   {
      canvas.popDialog(RTB_BugTracker);
      MessageBoxOK("Success!","Your Bug Report has been submitted successfully. Thank you for your assistance.\n\nReports you have submitted: "@getField(%line,1));
   }
   else
   {
      MessageBoxOK("Oh Poop.","Your Bug Report failed to be submitted due to the following reason:\n\n"@getField(%line,1));
   }
}

function RTBBT_onReportFail()
{
   MessagePopup("","",1);
   MessageBoxOK("Crap...","You could not be connected to the RTB Server. Please make sure you're connected to the internet correctly.");
}