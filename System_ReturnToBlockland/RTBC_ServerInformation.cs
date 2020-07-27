//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBC_ServerInformation.cs $
//#
//#      $Id: RTBC_ServerInformation.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Information (RTBSI/CServerInformation)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_ServerInformation = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::CServerInformation::HostSite = "returntoblockland.com";
$RTB::CServerInformation::FilePath = "/blockland/rtbMasterServer.php";

//*********************************************************
//* Initialisation of Required Elements
//*********************************************************
if(!isObject(RTB_ServerInformation))
	exec("./RTB_ServerInformation.gui");

//*********************************************************
//* Transmission Protocol
//*********************************************************
function RTBSI_InitSC()
{
   if(!isObject(RTBSI_SC))
   {
      new TCPObject(RTBSI_SC)
      {
         site = $RTB::CServerInformation::HostSite;
         port = 80;
         cmd = "";
         filePath = $RTB::CServerInformation::FilePath;
         
         defaultFailHandle = "RTBSI_handleTimeout";
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         isRTBObject = 1;
      };
      
      RTBSI_SC.addResponseHandle("MASTERRTB","RTBSI_onMasterReply");
      RTBSI_SC.addResponseHandle("GETSERVERINFO","RTBSI_onServerInfo");
      RTBSI_SC.addResponseHandle("GETSERVERPLAYERS","RTBSI_onServerPlayers");
      RTBSI_SC.addResponseHandle("GETSERVERMODS","RTBSI_onServerMods");
      RTBSI_SC.addResponseHandle("ENDGETSERVERINFO","RTBSI_onServerInfoEnd");
   }
}

function RTBSI_handleTimeout(%this,%command)
{
   if(%command $= "GETSERVERINFO")
   {
      if(RTBSI_Loading.visible)
      {
         RTBMM_stopLoadingRing(RTBSI_Loading.getObject(0).getObject(0));
         RTBSI_Loading.getObject(0).getObject(0).setBitmap($RTB::Path@"images/loadRingFail");
         RTBSI_Loading.getObject(0).getObject(1).setText("<just:center><color:555555>Connection Failed");
      }
   }
}

function RTBSI_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBSI_SC))
      RTBSI_InitSC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   RTBSI_SC.sendRequest(%cmd,%argString,%layer);
}

function RTBSI_getRTBServers()
{
   RTBSI_SendRequest("MASTERRTB",0);
}

function RTBSI_onMasterReply(%this,%line)
{
   %ip = strReplace(getField(%line,0),".","_");
   %port = getField(%line,1);
   %searchable = %ip@"X"@%port;
   
   %soIndex = $ServerSOFromIP[%searchable];
   %so = $ServerSO[%soIndex];
   if(isObject(%so))
   {
      %so.hasRTB = 1;
      %so.display();
   }
}

function joinServerGui::specialJoin(%this)
{
   %ip = $RTB::CServerInformation::Cache::CurrentIP;
   %port = $RTB::CServerInformation::Cache::CurrentPort;
   %scriptObjectName = strReplace(%ip,".","_")@"X"@%port;
   %so = $ServerSOFromIP[%scriptObjectName];
   
   JS_ServerList.setSelectedById(%so);
   joinServerGui.join();
}

function joinServerGui::info(%this)
{    
   if(JS_ServerList.getValue() $= "")
   {
      %ip = $RTB::CServerInformation::Cache::CurrentIP;
      %port = $RTB::CServerInformation::Cache::CurrentPort;
   }
   else
   {
      %info = getField(JS_ServerList.getValue(),9);
      %ip = getSubStr(%info,0,strPos(%info,":"));
      %port = getSubStr(%info,strPos(%info,":")+1,strLen(%info));
      
      $RTB::CServerInformation::Cache::CurrentIP = %ip;
      $RTB::CServerInformation::Cache::CurrentPort = %port;
   }
   
   RTBSI_getServerInfo(%ip,%port);
}

function RTBSI_getServerInfo(%ip,%port)
{
   canvas.pushDialog(RTB_ServerInformation);
   RTBSI_Loading.setVisible(1);
   while(RTBSI_Loading.getObject(0).getCount() > 0)
   {
      RTBSI_Loading.getObject(0).getObject(0).delete();
   }
   
   %ring = new GuiBitmapCtrl()
   {
      horizSizing = "center";
      vertSizing = "bottom";
      position = "205 154";
      extent = "31 31";
      bitmap = "./images/loadRing";
   };
   RTBMM_createLoadingRing(%ring);
   %text = new GuiMLTextCtrl()
   {
      position = "116 193";
      extent = "209 12";
      profile = RTB_Verdana12Pt;
      text = "<just:center><color:555555>Retrieving Server Information...";
   };
   RTBSI_Loading.getObject(0).add(%ring,%text);
   
   RTBSI_PlayerCover.setVisible(0);
   RTBSI_PlayerList.clear();
   $RTB::CServerInformation::Cache::NumMods = 0;
   $RTB::CServerInformation::Cache::NumPlayers = 0;
   $RTB::CServerInformation::Cache::ShowPlayers = 0;
   $RTB::CServerInformation::Cache::IncomingData = 0;
   RTBSI_ModScroll.getObject(0).clear();
   RTBSI_SendRequest("GETSERVERINFO",1,%ip,%port);
}

function RTB_ServerInformation::onSleep(%this)
{
   while(RTBSI_Loading.getObject(0).getCount() > 0)
   {
      RTBSI_Loading.getObject(0).getObject(0).delete();
   }
   RTBSI_Loading.setVisible(0);
   
   if(isObject(RTBSI_SC))
      RTBSI_SC.disconnect();
}

function RTBSI_onServerInfo(%this,%line)
{
   %reply = getField(%line,0);
   if(%reply)
   {
      %owner = getField(%line,1);
      RTBSI_Owner.setText(%owner);
      $RTB::CServerInformation::Cache::IncomingData = 1;
      
      %showPlayers = getField(%line,2);
      $RTB::CServerInformation::Cache::ShowPlayers = %showPlayers;
   }
   else
   {
      if(RTBSI_Loading.visible)
      {
         RTBMM_stopLoadingRing(RTBSI_Loading.getObject(0).getObject(0));
         RTBSI_Loading.getObject(0).getObject(0).setBitmap($RTB::Path@"images/loadRingFail");
         RTBSI_Loading.getObject(0).getObject(1).setText("<just:center><color:555555>No Information Available");
      }    
      return;  
   }
}

function RTBSI_onServerPlayers(%this,%players)
{
   %playerArray = strReplace(%players,"~","\t");
   for(%i=0;%i<getFieldCount(%playerArray);%i++)
   {
      %playerLine = strReplace(getField(%playerArray,%i),";","\t");
      $RTB::CServerInformation::Cache::Player[$RTB::CServerInformation::Cache::NumPlayers] = %playerLine;
      $RTB::CServerInformation::Cache::NumPlayers++;
   }
}

function RTBSI_onServerMods(%this,%mods)
{
   %modArray = strReplace(%mods,";","\t");
   for(%i=0;%i<getFieldCount(%modArray);%i++)
   {
      %modLine = strReplace(getField(%modArray,%i),"~","\t");
      %modType = getField(%modLine,0);
      %modInfo = getField(%modLine,1);
      
      if(%modType $= "rtb")
      {
         %modId = getField(%modLine,2);
         %zip = getField(%modLine,3);
         %content = getField(%modLine,4);
         $RTB::CServerInformation::Cache::Mod[$RTB::CServerInformation::Cache::NumMods] = %modType@"\t"@%modInfo@"\t"@%modId@"\t"@%zip@"\t"@%content;
         if(isFile("Add-Ons/"@%zip))
            $RTB::CServerInformation::Cache::HasMod[$RTB::CServerInformation::Cache::NumMods] = 1;
         else if(findFirstFile("Add-Ons/"@getSubStr(%zip,0,strLen(%zip)-4)@"/*") !$= "")
            $RTB::CServerInformation::Cache::HasMod[$RTB::CServerInformation::Cache::NumMods] = 1;
         else
            $RTB::CServerInformation::Cache::HasMod[$RTB::CServerInformation::Cache::NumMods] = 0;
         $RTB::CServerInformation::Cache::NumMods++;
      }
      else if(%modType $= "bl")
      {
         %zip = getField(%modLine,2);
         $RTB::CServerInformation::Cache::Mod[$RTB::CServerInformation::Cache::NumMods] = %modType@"\t"@%modInfo@"\t"@%modId@"\t"@%zip@"\t"@%content;
         if(isFile("Add-Ons/"@%zip))
            $RTB::CServerInformation::Cache::HasMod[$RTB::CServerInformation::Cache::NumMods] = 1;
         else
            $RTB::CServerInformation::Cache::HasMod[$RTB::CServerInformation::Cache::NumMods] = 0;
         $RTB::CServerInformation::Cache::NumMods++;
      }
   }
}

function RTBSI_onServerInfoEnd(%this)
{
   if($RTB::CServerInformation::Cache::IncomingData)
   {
      if(RTBSI_Loading.visible)
      {
         while(RTBSI_Loading.getObject(0).getCount() > 0)
         {
            RTBSI_Loading.getObject(0).getObject(0).delete();
         }
         RTBSI_Loading.setVisible(0);
      }
      
      if(!$RTB::CServerInformation::Cache::ShowPlayers)
      {
         RTBSI_PlayerCover.setVisible(1);
      }
      
      %ip = $RTB::CServerInformation::Cache::CurrentIP;
      %port = $RTB::CServerInformation::Cache::CurrentPort;
      %scriptObjectName = strReplace(%ip,".","_")@"X"@%port;
      %so = $ServerSOFromIP[%scriptObjectName];
      %so = $ServerSO[%so];
      
      RTBSI_Bricks.setText(%so.brickCount);
      RTBSI_Dedicated.setText(%so.ded);
      RTBSI_Password.setText(%so.pass);
      RTBSI_Ping.setText(%so.ping);
      RTBSI_Players.setText(%so.currPlayers@"/"@%so.maxPlayers);
      RTBSI_Map.setText(%so.map);
      
      for(%i=3;%i>-1;%i--)
      {
         for(%j=0;%j<$RTB::CServerInformation::Cache::NumPlayers;%j++)
         {
            %playerLine = $RTB::CServerInformation::Cache::Player[%j];
            %playerName = getField(%playerLine,0);
            %playerBLID = getField(%playerLine,1);
            %playerScore = getField(%playerLine,2);
            %playerStatus = getField(%playerLine,3);
            
            if(%playerStatus $= %i)
            {
               if(%playerStatus $= 0)
                  %playerStatus = "";
               else if(%playerStatus $= 1)
                  %playerStatus = "A";               
               else if(%playerStatus $= 2)
                  %playerStatus = "S";    
               else if(%playerStatus $= 3)
                  %playerStatus = "H";  
               RTBSI_PlayerList.addRow(0,%playerStatus TAB %playerName TAB %playerBLID TAB %playerScore);
            }
         }
      }

      if($RTB::CServerInformation::Cache::NumMods >= 1)
      {
         //Sorted by RTB mods you dont have that need files
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod !$= 1 && getField(%modLine,0) $= "rtb" && getField(%modLine,4) $= 1)
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),0,getField(%modLine,4));
            }
         }
         
         //then by RTB mods you dont have that dont need files
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod !$= 1 && getField(%modLine,0) $= "rtb" && getField(%modLine,4) !$= 1)
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),0,getField(%modLine,4));
            }
         }
         
         //then BL mods you dont have that need files
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod !$= 1 && getField(%modLine,0) $= "bl" && getField(%modLine,4) $= 1)
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),0,getField(%modLine,4));
            }
         }
         
         //then BL mods you dont have that dont need files
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod !$= 1 && getField(%modLine,0) $= "bl" && getField(%modLine,4) !$= 1)
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),0,getField(%modLine,4));
            }
         }
         
         //then all the RTB ones you do have
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod $= 1 && getField(%modLine,0) $= "rtb")
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),1,getField(%modLine,4));
            }
         }
         
         //and finally all the ones bl you have
         for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
         {
            %modLine = $RTB::CServerInformation::Cache::Mod[%i];
            %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
            
            if(%hasMod $= 1 && getField(%modLine,0) $= "bl")
            {
               RTBSI_createAddonListing(getField(%modLine,0),getField(%modLine,1),getField(%modLine,2),1,getField(%modLine,4));
            }
         }
      }
      else
      {
         //No custom mods
      }
      
      %rtbMods = 0;
      %blMods = 0;
      for(%i=0;%i<$RTB::CServerInformation::Cache::NumMods;%i++)
      {
         %modLine = $RTB::CServerInformation::Cache::Mod[%i];
         %hasMod = $RTB::CServerInformation::Cache::HasMod[%i];
         
         if(getField(%modLine,0) $= "rtb")
         {
            %rtbMods++;
         }
         else if(getField(%modLine,0) $= "bl")
         {
            %blMods++;
         }
      }
      RTBSI_Mods.setText("RTB ("@%rtbMods@"), BL ("@%blMods@")");
   }
}

function RTBSI_createAddonListing(%type,%title,%id,%hasMod,%needsFiles)
{
   %container = RTBSI_ModScroll.getObject(0);
   %yPos = (%container.getCount()*32)+%container.getCount()+1;
   
   if(%type $= "bl")
      %logo = "smallbllogo";
   else
      %logo = "smallrtblogo";
   
   %listing = new GuiSwatchCtrl()
   {
      position = "1" SPC %yPos;
      extent = "433 32";
      color = "255 255 255 255";
      
      new GuiSwatchCtrl()
      {
         position = "0 0";
         extent = "32 32";
         color = "220 220 220 255";
         
         new GuiBitmapCtrl()
         {
            position = "7 7";
            extent = "16 16";
            bitmap = "./images/"@%logo;
         };
      };
      
      new GuiSwatchCtrl()
      {
         position = "33 0";
         extent = "357 32";
         color = "220 220 220 255";
         
         new GuiTextCtrl()
         {
            profile = "RTB_Verdana12Pt";
            position = "5 1";
            extent = "300 18";
            text = %title;
         };
         
         new GuiBitmapCtrl()
         {
            position = "313 8";
            extent = "16 16";
            bitmap = "./images/plugin";
            
            new GuiBitmapButtonCtrl()
            {
               position = "0 0";
               extent = "16 16";
               bitmap = "./images/buttons/btnInvisible";
               text = " ";
            };
         };
         
         new GuiBitmapCtrl()
         {
            position = "334 8";
            extent = "16 16";
            bitmap = "./images/disk";
            
            new GuiBitmapButtonCtrl()
            {
               position = "0 0";
               extent = "16 16";
               bitmap = "./images/buttons/btnInvisible";
               text = " ";
            };
         };
      };
      
      new GuiSwatchCtrl()
      {
         position = "391 0";
         extent = "32 32";
         color = "220 220 220 255";
         
         new GuiBitmapCtrl()
         {
            position = "7 7";
            extent = "16 16";
            bitmap = "./images/exclamation";
         };
      };
   };
   %container.add(%listing);
   %container.resize(1,1,getWord(%container.extent,0),%yPos+33);
   %listing.getObject(1).getObject(1).getObject(0).command = "RTBSI_queueDownload("@%id@","@%listing@",1);";
   %listing.getObject(1).getObject(2).getObject(0).command = "RTBSI_queueDownload("@%id@","@%listing@");";
   %listing.getObject(1).getObject(1).setVisible(0);
   if(%type $= "bl" || %hasMod)
      %listing.getObject(1).getObject(2).setVisible(0);
      
   if(%needsFiles)
   {
      %swatch = new GuiSwatchCtrl()
      {
         position = "0 16";
         extent = "192 18";
         color = "0 0 0 0";
         
         new GuiBitmapCtrl()
         {
            position = "0 -2";
            extent = "16 16";
            bitmap = "./images/bullet_plugin";
         };
         
         new GuiTextCtrl()
         {
            profile = "RTB_Verdana12Pt";
            position = "20 -1";
            extent = "174 18";
            text = "Contains shapes/textures/sounds";
         };
         
         new GuiSwatchCtrl()
         {
            position = "0 0";
            extent = "192 18";
            color = "220 220 220 100";
         };
      };
      %listing.getObject(1).add(%swatch);
   }
   
   if(%hasMod || %type $= "bl")
   {      
      if(%hasMod)
         %listing.getObject(2).getObject(0).setBitmap($RTB::Path@"images/accept");
   }
}

function RTBSI_queueDownload(%fileid,%container)
{
   if(!isObject(RTBSI_DownloadQueue))
      RTBSI_InitDQ();
      
   if(isObject(%container.getObject(1).getObject(3)))
      %container.getObject(1).getObject(3).delete();
      
   %swatch = new GuiSwatchCtrl()
   {
      position = "1 15";
      extent = "306 16";
      color = "0 0 0 0";
   };
   %container.getObject(1).add(%swatch);
   
   for(%i=0;%i<20;%i++)
   {
      %bullet = new GuiBitmapCtrl()
      {
         position = (%i*10) SPC "0";
         extent = "16 16";
         bitmap = "./images/bullet_gray";
      };
      %swatch.add(%bullet);
   }
   %container.getObject(1).getObject(2).setVisible(0);
   RTBSI_DownloadQueue.pushFile(%fileid,%container);
}

//*********************************************************
//* Add-On Download Procedures
//*********************************************************
function RTBSI_InitDQ()
{
   if(!isObject(RTBSI_DownloadManifest))
      new SimGroup(RTBSI_DownloadManifest);
      
   %dQueue = new ScriptObject(RTBSI_DownloadQueue)
   {
      class = RTBSI_DownloadQueueSO;
      totalDownloads = 0;
   };
   RTBSI_DownloadManifest.add(%dQueue);
   
   return 1;
}
function RTBSI_DownloadQueueSO::hasFile(%this,%file)
{
   if(!isObject(RTBSI_DownloadQueue))
      RTBSI_InitDQ();
      
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      %fileID = %this.download[%i].file_id;
      if(%fileID $= %file)
         return 1;
   }
   return 0;
}
function RTBSI_DownloadQueueSO::pushFile(%this,%file,%container)
{
   if(!isObject(RTBSI_DownloadQueue))
      RTBSI_InitDQ();
      
   if(%file $= "" || %file < 0 || !isint(%file))
      return 0;
      
   if(%this.hasFile(%file))
      return 0;
      
   %fileSO = new ScriptObject()
   {
      class = RTBSI_DownloadFileSO;
      file_id = %file;
      gui_container = %container;
   };
   %this.getGroup().add(%fileSO);
   %this.download[%this.totalDownloads] = %fileSO;
   %this.totalDownloads++;
   
   %this.poke();
   
   return 1;
}
function RTBSI_DownloadQueueSO::popFile(%this,%file)
{
   if(!isObject(RTBSI_DownloadQueue))
      RTBSI_InitDQ();
      
   if(!%this.hasFile(%file))
      return 0;
      
   %k = 0;
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      %fileID = %this.download[%i].file_id;
      if(%fileID $= %file)
         continue;
      %this.download[%k] = %this.download[%i];
      %k++;
   }
   %this.download[%i-1].delete();
   %this.download[%i-1] = "";
   %this.totalDownloads--;
   
   return 1;
}
function RTBSI_DownloadQueueSO::popIndex(%this,%index)
{
   if(!isObject(RTBSI_DownloadQueue))
      RTBSI_InitDQ();
      
   if(%index >= %this.totalDownloads)
      return 0;
      
   %k = 0;
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      if(%i $= %index)
      {
         %this.download[%i].delete();
         continue;
      }
      %this.download[%k] = %this.download[%i];
      %k++;
   }
   %this.totalDownloads--;
   
   return 1;
}
function RTBSI_DownloadQueueSO::poke(%this)
{
   if(RTBSI_DownloadQueue.totalDownloads >= 1)
   {
      if(isObject(RTBSI_FC))
         return;
      RTBSI_InitFC();
      
      RTBSI_FC.setBinary(0);
      RTBSI_FC.queueItem = RTBSI_DownloadQueue.download[0];
      RTBSI_FC.connect($RTB::CModManager::HostSite@":80");
      
      %container = RTBSI_FC.queueItem.gui_container;
      %dots = %container.getObject(1).getObject(3);
      for(%i=0;%i<20;%i++)
      {
         %dots.getObject(%i).setBitmap($RTB::Path@"images/bullet_orange");
      }
   }
}

function RTBSI_InitFC()
{
   if(isObject(RTBSI_FC))
      return;
      
   new TCPObject(RTBSI_FC);
}
function RTBSI_FC::onConnected(%this)
{
   if(%this.queueItem.file_id $= "")
   {
      %this.delete();
      RTBSI_DownloadQueue.poke();
      return;
   }

   %this.send("GET /blockland/rtbModServer.php?c=GETFILEDL&n="@urlEnc($Pref::Player::NetName)@"&arg1="@%this.queueItem.file_id@" HTTP/1.1\r\nHost: "@$RTB::CModManager::HostSite@"\r\nUser-Agent: Torque/1.0\r\n\r\n");
}
function RTBSI_FC::onDNSFailed(%this)
{
}
function RTBSI_FC::onConnectFailed(%this)
{
}
function RTBSI_FC::onDisconnect(%this)
{
}
function RTBSI_FC::onBinChunk(%this,%bin)
{
   if(%bin >= %this.header_contentLength)
   {
      %container = %this.queueItem.gui_container;
      %dots = %container.getObject(1).getObject(3);
      for(%i=0;%i<20;%i++)
      {
         %dots.getObject(%i).setBitmap($RTB::Path@"images/bullet_green");
      }
      
      %container.getObject(1).getObject(1).setVisible(0);
      %container.getObject(2).getObject(0).setBitmap($RTB::Path@"images/accept");
      
      %filename = %this.header_contentSavename;
      if(%this.header_contentType $= "zip")
         %this.saveBufferToFile("Add-Ons/"@%filename);
      else
         %this.saveBufferToFile("Add-Ons/Music/"@%filename);

      RTBSI_DownloadQueue.popIndex(0);
      setModPaths(getModPaths());
      RTBMM_loadExistingRTBMods();
      
      if(%this.header_contentType $= "zip")
      {
         if(isFile("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/client.cs") && isFile("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/description.txt"))
         {
            exec("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/client.cs");
         }
      }
      %this.delete();
      RTBSI_DownloadQueue.poke();
   }
   else
   {
      %twohundredpercent = mCeil((%bin/%this.header_contentLength)*20);
      %container = %this.queueItem.gui_container;
      
      %dots = %container.getObject(1).getObject(3);
      for(%i=0;%i<%twohundredpercent;%i++)
      {
         %dots.getObject(%i).setBitmap($RTB::Path@"images/bullet_green");
      }
   }
}
function RTBSI_FC::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      //do something
   }
   
   if(strPos(%line,"Content-Length:") $= 0)
      %this.header_contentLength = getWord(%line,1);
   
   if(strPos(%line,"Content-Savename:") $= 0)
      %this.header_contentSavename = getWord(%line,1);
   
   if(strPos(%line,"Content-Type: application/download") $= 0)
      %this.header_contentType = "zip";
   else if(strPos(%line,"Content-Type: application/ogg") $= 0)
      %this.header_contentType = "ogg";
   
   if(%line $= "")
      %this.setBinarySize(%this.header_contentLength);
         
   %this.lastLine = %line;
   %this.timeStarted = "";
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTBC_ServerInformation
{
   function queryMasterTCPObj::connect(%this,%host)
   {
      Parent::connect(%this,%host);
      
      if($RTB::Options::EnableServerInfo)
         RTBSI_getRTBServers();
   }
   
   function ServerSO::serialize(%this)
   {
      %serialized =  Parent::serialize(%this);
      
      if(%this.hasRTB)
         %hasRTB = "Yes";
      else
         %hasRTB = "No";
         
      if(!$RTB::Options::EnableServerInfo)
         %hasRTB = "???";
      
      return %serialized@"\t"@%hasRTB;
   }
};
activatePackage(RTBC_ServerInformation);