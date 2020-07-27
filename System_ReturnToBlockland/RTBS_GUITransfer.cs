//#############################################################################
//#
//#   Return to Blockland - Version 3.0
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 82 $
//#      $Date: 2009-07-25 15:51:53 +0100 (Sat, 25 Jul 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/trunk/RTBS_GUITransfer.cs $
//#
//#      $Id: RTBS_GUITransfer.cs 82 2009-07-25 14:51:53Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   GUI Transfer (RTBST/SGUITransfer)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBS_GUITransfer = 1;

//*********************************************************
//* Required Modules
//*********************************************************
if(!$RTB::RTBR_GUITransfer_Hook)
   exec("./RTBR_GUITransfer_Hook.cs");

//*********************************************************
//* Transfer Management/Handling
//*********************************************************
//- GameConnection::transmitGUI (Transmits a gui to the client)
function GameConnection::transmitGUI(%client,%gui,%element)
{
   if(!isObject(%client))
      return;
 
   if(%gui $= "")
      %gui = 0;
   if(%element $= "")
      %element = -1;
 
   if(%gui >= RTBRT_GUIManifest.getCount())
   {
      %client.onGUIDone();
      return;
   } 
   
   if(%element $= -1)
   {
      commandtoclient(%client,'RTB_receiveGUI',RTBRT_GUIManifest.getObject(%gui).name);
   }
   else
   {
      %control = RTBRT_GUIManifest.getObject(%gui).elementClass[%element];   
      %name = RTBRT_GUIManifest.getObject(%gui).elementName[%element];   
      %props = RTBRT_GUIManifest.getObject(%gui).elementProps[%element];
      %depth = RTBRT_GUIManifest.getObject(%gui).elementDepth[%element];
      if(strLen(%props) > 255)
      {
         %reAllocs = 0;
         %strAlloc = mFloor(getFieldCount(%props)/255);
         for(%i=0;%i<getFieldCount(%props);%i++)
         {
            if(strLen(getField(%props,%i)) > %strAlloc)
            {
               %reAlloc[%reAllocs] = %i;
               %reAllocs++;
               %newProps = %newProps@"\t";
            }
            else
               %newProps = %newProps@getField(%props,%i)@"\t";
         }
         %newProps = getSubStr(%newProps,0,strLen(%newProps)-1);
         commandtoclient(%client,'RTB_receiveElement',%control,%name,%props,%depth);
         
         for(%i=0;%i<%reAllocs;%i++)
         {
            %prop = %reAlloc[%i];
            %propVal = getField(%props,%prop);
            if(strLen(%propVal) > 255)
            {
               %inc = 0;
               %buffer = strLen(%propVal);
               while(%buffer > 0)
               {
                  if(%buffer > 255)
                     %sendBytes = 255;
                  else
                     %sendBytes = %buffer;
                  %propSend = getSubStr(%propVal,0,%sendBytes);
                  commandtoclient(%client,'RTB_receiveProperty',%prop,%propSend,%inc);
                  %inc = 1;
                  %buffer -= %sendBytes;
                  %propVal = getSubStr(%propVal,%sendBytes,strLen(%propVal));
               }
            }
            else
               commandtoclient(%client,'RTB_receiveProperty',%prop,%propVal);
         }
      }
      else
      {
         commandtoclient(%client,'RTB_receiveElement',%control,%name,%props,%depth);
      }
   }
   %element++;
   
   if(%element >= RTBRT_GUIManifest.getObject(%gui).elements)
   {
      %gui++;
      %element = -1;
   }
   %client.schedule(50,"transmitGUI",%gui,%element);
}

//- GameConnection::onGUIDone (Callback when gui is completely downloaded)
function GameConnection::onGUIDone(%client)
{
   %client.currentPhase = 0;
   %client.hasDownloadedGUI = 1;
   commandToClient(%client,'RTB_receiveComplete');
   commandToClient(%client,'MissionStartPhase1',$missionSequence,$Server::MissionFile);
}

//*********************************************************
//* Module Package
//*********************************************************
package RTBS_GUITransfer
{
   function GameConnection::loadMission(%this)
   {
      if(%this.isAIControlled())
      {
         %this.onClientEnterGame();
      }
      else
      {
         if(RTBRT_GUIManifest.getCount() >= 1 && %this.hasRTB && %this.rtbVersion >= 3 && !%this.hasDownloadedGUI)
         {
            %this.currentPhase = -1;
            commandToClient(%this,'MissionStartPhase0',RTBRT_getControlCRC(),RTBRT_GUIManifest.getCount(),RTBRT_GUIManifest.elements);
            echo("*** Sending mission load to client: " @ $Server::MissionFile);
         }
         else
            Parent::loadMission(%this);
      }
   }
   
   function serverCmdMissionStartPhase0Ack(%client,%skip)
   {
      if(%client.currentPhase !$= "-1")
         return;
         
      if(%skip)
      {
         %client.currentPhase = "";
         commandToClient(%client,'MissionStartPhase1',$missionSequence,$Server::MissionFile);
         return;
      }
         
      %client.currentPhase = -0.5;
      %client.transmitGUI();
   }
};
activatePackage(RTBS_GUITransfer);