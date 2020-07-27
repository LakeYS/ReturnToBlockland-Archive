//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBC_Authentication.cs $
//#
//#      $Id: RTBC_Authentication.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Client Authentication (RTBCA/CAuthentication)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_Authentication = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::CAuthentication::AuthServer = "returntoblockland.com";
$RTB::CAuthentication::AuthPath = "/blockland/rtbClientAuth.php";

//*********************************************************
//* Operational Functions
//*********************************************************
function RTBCA_InitASC()
{
   if(!isObject(RTBCA_ASC))
   {
      new TCPObject(RTBCA_ASC)
      {
         site = $RTB::CAuthentication::AuthServer;
         port = 80;
         cmd = "";
         filePath = $RTB::CAuthentication::AuthPath;
         
         defaultFailHandle = "RTBCA_handleTimeout";
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      RTBCA_ASC.addResponseHandle("AUTH","RTBCA_onAuthResponse");
      RTBCA_ASC.addResponseHandle("UPDATEPREFS","RTBCA_onUpdatePrefs");
      RTBCA_ASC.addResponseHandle("PUSHUPDATE","RTBCA_onPushUpdate");
   }
}

function RTBCA_handleTimeout()
{
}

function RTBCA_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBCA_ASC))
      RTBCA_InitASC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   
   RTBCA_ASC.sendRequest(%cmd,%argString,%layer);
}

function RTBCA_onPushUpdate()
{
   RTBCU_Update();
}

function RTBCA_onAuthResponse(%this,%line)
{
   if(%line $= "SUCCESS")
      $RTB::Options::IAmBarred = 0;
   else if(getField(%line,0) $= "WELCOME")
   {
      %hasLinked = getField(%line,1);
      canvas.pushDialog(RTB_WelcomeScreen);
      
      if(%hasLinked)
      {
         RTBWS_ThanksBtn.position = RTBWS_NextBtn.position;
         RTBWS_Frame1Text.setText("Congratulations on your first successful login to RTB from in-game!\n\n We're glad to see you've already linked your RTB Profile with your Blockland Account!");
      }
      else
      {
         RTBWS_Frame1Text.setText("Congratulations on your first successful login to RTB from in-game!\n\n We noticed you haven't linked your RTB Profile with your Blockland Account yet. Click Next to find out more!");
      }
   }
   else if(%line $= "SENDPREFS")
      RTBCA_SendPrefs();
   else if(getField(%line,0) $= "BARRED")
   {
      cancel($RTB::CAuthentication::AuthSS);
      if(!$RTB::Options::IAmBarred)
      {
         $RTB::Options::IAmBarred = 1;
         canvas.pushDialog(RTB_BarredScreen);
         if(getField(%line,1) $= "")
            RTBB_Text.setText("No reason has been specified.");
         else
            RTBB_Text.setText(getField(%line,1));
      }
   }
}

function RTBCA_Post()
{
   if(!$RTB::Options::AuthWithRTB)
	   return;

   %name = $pref::Player::NetName;
   
   if(isEventPending($RTB::CAuthentication::AuthSS))
      cancel($RTB::CAuthentication::AuthSS);
      
   $RTB::CAuthentication::AuthSS = schedule(180000,0,"RTBCA_Post");
   if(RTBIC_SC.connected)
      RTBCA_SendRequest("AUTH",1,RTBCA_EstablishLocation(),RTBCA_GetPlaytime(),$RTB::Version,$RTB::CIRCClient::Cache::NickName);
   else
      RTBCA_SendRequest("AUTH",1,RTBCA_EstablishLocation(),RTBCA_GetPlaytime(),$RTB::Version,"");
}

function RTBCA_onUpdatePrefs(%this,%line)
{
   if(getField(%line,0) $= "FAIL")
      error("ERROR (UPDATEPREFS): "@getField(%line,1));
}

function RTBCA_SendPrefs()
{
   %name = $pref::Player::NetName;
   RTBCA_SendRequest("UPDATEPREFS",2,$RTB::Options::PrivacyShowOnline,$RTB::Options::PrivacyShowServer,$RTB::Options::PrivacyShowPlayers,$RTB::Options::PrivacyShowPassworded,$RTB::Options::PrivacyShowOwnership);
}

//*********************************************************
//* Support Functions
//*********************************************************
function RTBCA_GetPlaytime()
{
   %timestring = getTimeString($Sim::Time);
   return %timestring;
}

function RTBCA_EstablishLocation()
{
   if(!$RTB::Options::PrivacyShowServer)
      return 0;
      
   %location = "Unknown";
   if(isObject(ServerConnection))
   {
      %address = ServerConnection.getAddress();
      if(%address $= "local")
      {
         if($Server::LAN)
            if($Server::ServerType $= "SinglePlayer")
               %address = "Singleplayer Server";
            else
               %address = "LAN Server";
         else
            %address = "local "@$Pref::Server::Port;
      }
      else
      {
         %address = getSubStr(%address,3,strLen(%address));
         %address = strReplace(%address,":"," ");  
      }
      %location = %address;
   }
   else
      %location = "Not on a Server";
      
   return %location;
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTBC_Authentication
{
   function MM_AuthBar::blinkSuccess(%this)
   {
      Parent::blinkSuccess(%this);
      RTBCA_Post();
   }
   
   function disconnectedCleanup()
   {
      Parent::disconnectedCleanup();
      schedule(1000,0,"RTBCA_Post");
   }
   
   function GameConnection::onConnectionAccepted(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k)
   {
      Parent::onConnectionAccepted(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k);
      RTBCA_Post();
   }
};
activatePackage(RTBC_Authentication);