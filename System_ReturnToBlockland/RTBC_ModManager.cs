//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBC_ModManager.cs $
//#
//#      $Id: RTBC_ModManager.cs 48 2009-03-14 13:47:40Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Mod Manager (RTBMM/CModManager)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_ModManager = 1;

//*********************************************************
//* GUI Modification
//*********************************************************
if(!isObject(MM_RTBModManagerButton))
{
   %btn = new GuiBitmapButtonCtrl(MM_RTBModManagerButton)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "416 120";
      extent = "224 40";
      minExtent = "8 2";
      visible = "1";
      text = " ";
      groupNum = "-1";
      buttonType = "PushButton";
      bitmap = $RTB::Path@"images/buttons/btnMODManager";
      command = "canvas.pushdialog(RTB_ModManager);";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%btn);
}
function MM_RTBModManagerButton::onMouseEnter(%this)
{
   if($Pref::Audio::MenuSounds)
	   alxPlay(Note8Sound);
}

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::CModManager::HostSite = "returntoblockland.com";
$RTB::CModManager::EnginePath = "/blockland/rtbModEngine.php";
$RTB::CModManager::GUI::YSpacing = 1;

$RTB::CModManager::Style::ColorDARKRED = "8B0000";
$RTB::CModManager::Style::ColorRED = "FF0000";
$RTB::CModManager::Style::ColorORANGE = "FFA500";
$RTB::CModManager::Style::ColorBROWN = "A52A2A";
$RTB::CModManager::Style::ColorYELLOW = "FFFF00";
$RTB::CModManager::Style::ColorGREEN = "008000";
$RTB::CModManager::Style::ColorOLIVE = "808000";
$RTB::CModManager::Style::ColorCYAN = "00FFFF";
$RTB::CModManager::Style::ColorBLUE = "0000FF";
$RTB::CModManager::Style::ColorDARKBLUE = "00008B";
$RTB::CModManager::Style::ColorINDIGO = "4B0082";
$RTB::CModManager::Style::ColorVIOLET = "EE82EE";
$RTB::CModManager::Style::ColorWHITE = "FFFFFF";
$RTB::CModManager::Style::ColorBLACK = "000000";

//*********************************************************
//* Initialisation of Required Elements
//*********************************************************
if(!isObject(RTB_ModManager))
	exec("./RTB_ModManager.gui");

//*********************************************************
//* Transmission Protocol
//*********************************************************
//- RTBMM_InitSC (Prepares TCPObject)
function RTBMM_InitSC()
{
   if(!isObject(RTBMM_SC))
   {
      new TCPObject(RTBMM_SC)
      {
         site = $RTB::CModManager::HostSite;
         port = 80;
         cmd = "";
         filePath = $RTB::CModManager::EnginePath;
         
         connected = 0;
         transmitting = 0;
         queueSize = 0;
         
         lineCallback = "RTBMM_onCommLine";
         defaultFailHandle = "RTBMM_onCommFail";
         
         isRTBObject = 1;
      };
      
      //Auth
      RTBMM_SC.addResponseHandle("AUTH","RTBMM_Auth_HandleResponse");
      
      //General Errors
      RTBMM_SC.addResponseHandle("ERROR","RTBMM_onError");
      RTBMM_SC.addResponseHandle("MESSAGE","RTBMM_onMessage");
      RTBMM_SC.addResponseHandle("SQLERROR","RTBMM_onSQLError");
      RTBMM_SC.addResponseHandle("BARRED","RTBMM_onBarred");
      
      //Multifunctional Calls
      RTBMM_SC.addResponseHandle("PAGINATION","RTBMM_Pagination");
      
      //Category View
      RTBMM_SC.addResponseHandle("GETCATEGORIES","RTBMM_CategoryView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETCATEGORIES","RTBMM_CategoryView_onTransmitEnd");
      
      //Section View
      RTBMM_SC.addResponseHandle("GETSECTION","RTBMM_SectionView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETSECTION","RTBMM_SectionView_onTransmitEnd");
      
      //File View
      RTBMM_SC.addResponseHandle("GETFILE","RTBMM_FileView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETFILE","RTBMM_FileView_onTransmitFileEnd");
      
      //Presets View
      RTBMM_SC.addResponseHandle("GETPRESETS","RTBMM_PresetsView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETPRESETS","RTBMM_PresetsView_onTransmitEnd");
      
      //Preset View
      RTBMM_SC.addResponseHandle("GETPRESETINFO","RTBMM_PresetView_onTransmit");
      RTBMM_SC.addResponseHandle("GETPRESET","RTBMM_PresetView_onTransmitRow");
      RTBMM_SC.addResponseHandle("ENDGETPRESET","RTBMM_PresetView_onTransmitEnd");
      
      //All Files View
      RTBMM_SC.addResponseHandle("GETALLFILES","RTBMM_AllFilesView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETALLFILES","RTBMM_AllFilesView_onTransmitEnd");
      
      //User Favourite Files View
      RTBMM_SC.addResponseHandle("GETUSERFAVS","RTBMM_FavsView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETUSERFAVS","RTBMM_FavsView_onTransmitEnd");
      
      //Search View
      RTBMM_SC.addResponseHandle("GETSEARCHDATA","RTBMM_SearchView_onTransmit");
      RTBMM_SC.addResponseHandle("ENDGETSEARCHDATA","RTBMM_SearchView_onTransmitEnd");
      RTBMM_SC.addResponseHandle("GETSEARCHRESULTS","RTBMM_SearchView_onResult");
      RTBMM_SC.addResponseHandle("FINISHGETSEARCHRESULTS","RTBMM_SearchView_onResultEnd");
      
      //Commenting and Rating
      RTBMM_SC.addResponseHandle("GETCOMMENT","RTBMM_Comments_onRetrieve");
      RTBMM_SC.addResponseHandle("ADDCOMMENT","RTBMM_Comments_onSend");
      RTBMM_SC.addResponseHandle("ADDRATING","RTBMM_Ratings_onSend");
      
      //Download Managing
      RTBMM_SC.addResponseHandle("GETFILEDATA","RTBMM_DownloadsView_retrieveFileData");
      
      //File Update Checking
      RTBMM_SC.addResponseHandle("GETFILEUPDATES","RTBMM_ModsView_retrieveUpdates");
      
      //News Feed
      RTBMM_SC.addResponseHandle("GETNEWSFEED","RTBMM_NewsFeedView_onTransmit");
      
      //Sync Mods
      RTBMM_SC.addResponseHandle("SYNCMODS","RTBMM_ModsView_syncResults");
      RTBMM_SC.addResponseHandle("ENDSYNCMODS","RTBMM_ModsView_endSyncResults");
      RTBMM_SC.addFailHandle("SYNCMODS","RTBMM_ModsView_failSyncResults");
   }
}
//- RTBMM_SendRequest (Handles Encoding for TCP Transmission)
function RTBMM_SendRequest(%cmd,%layer,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6,%arg7,%arg8,%arg9,%arg10)
{
   if(!isObject(RTBMM_SC))
      RTBMM_InitSC();

   for(%i=1;%i<11;%i++)
   {
      %arg = urlEnc(%arg[%i]);
      if(%argString $= "")
         %argString = "arg1="@%arg;
      else
         %argString = %argString@"&arg"@%i@"="@%arg;
   }
   RTBMM_SC.sendRequest(%cmd,%argString,%layer);
}

//*********************************************************
//* GUI Support
//*********************************************************
function RTB_ModManager::onWake(%this)
{
   if(!isObject(RTBMM_MODManifest))
      RTBMM_loadExistingRTBMods();
      
	if(!$RTB::CModManager::Session::LoggedIn && !$RTB::CModManager::Session::TimedOut)
	   RTBMM_beginAuth();
	   
   if(!isObject(RTBMM_WindowSwatch))
      RTBMM_getNewsFeedView();
   else
   {
      if(!$RTB::CModManager::Cache::DirectOpen)
      {
         RTBMM_SendRequest("GETNEWSFEED",7,RTBMM_getDelimitedMods(),RTBMM_FeedControl.latestFeed);
      }
   }
      
   $RTB::CModManager::Cache::DirectOpen = 0;
   if($RTB::CModManager::Cache::CurrentZone $= "modview")
      RTBMM_getModsView();
}
//- GuiMouseEventCtrl::onMouseEnter (Hack)
function GuiMouseEventCtrl::onMouseEnter(%this)
{
   if(%this.eventHover)
   {
      if(isObject(%this.eventSwatch))
      {
         %this.eventSwatch.visible = 1;
         %this.eventSwatch.color = "255 200 200 100";
      }
   }
   
   if(%this.eventType !$= "")
   {
      if(strLen(%this.eventCallbacks) == 4)
      {
         if(getSubStr(%this.eventCallbacks,0,1) $= 1)
         {
            %command = "Event_"@%this.eventType@"::onMouseEnter("@%this@");";
            eval(%command);
         }
      }
   }
}
//- GuiMouseEventCtrl::onMouseLeave (Hack)
function GuiMouseEventCtrl::onMouseLeave(%this)
{
   if(%this.eventHover)
   {
      if(isObject(%this.eventSwatch))
         %this.eventSwatch.visible = 0;
   }
   
   if(%this.eventType !$= "")
   {
      if(strLen(%this.eventCallbacks) == 4)
      {
         if(getSubStr(%this.eventCallbacks,1,1) $= 1)
         {
            %command = "Event_"@%this.eventType@"::onMouseLeave("@%this@");";
            eval(%command);
         }
      }
   }
}
//- GuiMouseEventCtrl::onMouseDown (Hack)
function GuiMouseEventCtrl::onMouseDown(%this)
{
   if(%this.eventHover)
   {
      if(isObject(%this.eventSwatch))
         %this.eventSwatch.color = "255 100 100 100";
   }
   
   if(%this.eventType !$= "")
   {
      if(strLen(%this.eventCallbacks) == 4)
      {
         if(getSubStr(%this.eventCallbacks,2,1) $= 1)
         {
            %command = "Event_"@%this.eventType@"::onMouseDown("@%this@");";
            eval(%command);
         }
      }
   }
}
//- GuiMouseEventCtrl::onMouseUp (Hack)
function GuiMouseEventCtrl::onMouseUp(%this)
{
   if(%this.eventHover)
   {
      if(isObject(%this.eventSwatch))
         %this.eventSwatch.color = "255 200 200 100";
   }
   
   if(%this.eventType !$= "")
   {
      if(strLen(%this.eventCallbacks) == 4)
      {
         if(getSubStr(%this.eventCallbacks,3,1) $= 1)
         {
            %command = "Event_"@%this.eventType@"::onMouseUp("@%this@");";
            eval(%command);
         }
      }
   }
}
//- RTBMM_OpenDirect()
function RTBMM_OpenDirect()
{
   $RTB::CModManager::Cache::DirectOpen = 1;
   canvas.pushDialog(RTB_ModManager);
}

//*********************************************************
//* Support Functions
//*********************************************************
//- RTBMM_getFieldFromContents (gets a tag from a contents var)
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

//*********************************************************
//* Movement Tracking
//*********************************************************
//- RTBMM_trackZone (logs where client browses for back button referral)
function RTBMM_trackZone(%zoneName,%returnEval)
{
   if(!isObject(RTBMM_ZoneTracker))
   {
      new ScriptObject(RTBMM_ZoneTracker)
      {
         queueSize = 0;
      };
   }
   
   if(RTBMM_ZoneTracker.queueItem[RTBMM_ZoneTracker.queueSize-1] !$= %returnEval)
   {
      RTBMM_ZoneTracker.queueItem[RTBMM_ZoneTracker.queueSize] = %returnEval;
      RTBMM_ZoneTracker.queueSize++;
   }
   
   $RTB::CModManager::Cache::CurrentZone = %zoneName;
}
//- RTBMM_goBack (pops off tracking queue)
function RTBMM_goBack()
{
   %returnEval = RTBMM_ZoneTracker.queueItem[RTBMM_ZoneTracker.queueSize-2];
   RTBMM_ZoneTracker.queueItem[RTBMM_ZoneTracker.queueSize-1] = "";
   RTBMM_ZoneTracker.queueSize--;
   eval(%returnEval);
}

//*********************************************************
//* Communication with RTB Mods Server
//*********************************************************
//- RTBMM_onCommLine (On communication from server)
function RTBMM_onCommLine(%this, %line)
{		
   if($RTB::CModManager::Session::TimedOut && !$RTB::CModManager::Session::LoggedIn)
      RTBMM_beginAuth();
      
   $RTB::CModManager::Session::TimedOut = 0;
}
//- RTBMM_onCommFail (On failure to communicate with server)
function RTBMM_onCommFail(%this,%type)
{
   RTB_ModManager.getObject(0).setText("Mod Manager - Unable to Connect");
   $RTB::CModManager::Session::TimedOut = 1;
   $RTB::CModManager::Session::LoggedIn = 0;

   if(isObject(RTBMM_LoadingContentSwatch))
   {
      RTBMM_stopLoadingRing(RTBMM_LoadingContentSwatch.getObject(0));
      RTBMM_LoadingContentSwatch.getObject(0).setBitmap($RTB::Path@"images/loadRingFail");
      RTBMM_LoadingContentSwatch.getObject(1).setText("<just:center><color:555555>Unable to Connect");
   }
   MessagePopup("","",1);
   
   if(%type $= "DNS")
      MessageBoxOK("ERROR","It would appear that you are not connected to the internet at the moment. Please check your connection and try again.");
   else
      MessageBoxOK("ERROR","A connection cannot be established with the RTB Server. Either this is a problem with your internet connection or the server.\n\nCheck your internet and retry in a few minutes.");
}

//*********************************************************
//* Authentication with RTB Server
//*********************************************************
//- RTBMM_beginAuth (start auth process)
function RTBMM_beginAuth()
{
   $RTB::CModManager::Session::LoggedIn = false;
   $RTB::CModManager::Session::LogInName = "";
   
   if(!$RTB::Options::LoginWithProfile)
   {
      RTB_ModManager.getObject(0).setText("Mod Manager");
      return;
   }
   
   RTBMM_SendRequest("AUTH",1);
   RTB_ModManager.getObject(0).setText("Mod Manager - Logging In...");
}
//- RTBMM_Auth_HandleResponse (handle auth response from server)
function RTBMM_Auth_HandleResponse(%this,%line)
{
   %result = getField(%line,0);
   %accountName = getField(%line,1);
   
   if(%result $= "FAIL")
   {
      RTB_ModManager.getObject(0).setText("Mod Manager - Logged Out (No RTB Account)");
      $RTB::CModManager::Session::LoggedIn = false;
      $RTB::CModManager::Session::LogInName = "";
   }
   else if(%result $= "WIN")
   {
      RTB_ModManager.getObject(0).setText("Mod Manager - Logged in as "@%accountName);
      $RTB::CModManager::Session::LoggedIn = true;
      $RTB::CModManager::Session::LogInName = %accountName;
   }
   else if(%result $= "BARRED")
   {
      RTB_ModManager.getObject(0).setText("Mod Manager - Barred from Authenticating");
      $RTB::CModManager::Session::LoggedIn = false;
      $RTB::CModManager::Session::LogInName = "";
   }
}

//*********************************************************
//* Support functions for gui generation
//*********************************************************
//- RTBMM_ConstructWindow (Set up frame to construct in)
function RTBMM_ConstructWindow()
{
   if(isObject(RTBMM_WindowSwatch))
		RTBMM_WindowSwatch.delete();

   $RTB::CModManager::GUI::LengthOverride = 0;
	$RTB::CModManager::GUI::YSpacing = 1;
	%swatch = new GuiSwatchCtrl(RTBMM_WindowSwatch)
	{
		position = "0 0";
		color = "255 255 255 255";
		extent = "498 421";
	};
	RTBMM_ModWindow.add(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		position = "0 0";
		color = "255 255 255 255";
		extent = RTBMM_ModWindow.extent;
	};
	RTBMM_WindowSwatch.add(%swatch);
}
//- RTBMM_PushWindowElement (Move created controls to main window and update y-pos)
function RTBMM_PushWindowElement(%ctrl,%updateYSpacing)
{
   if(%ctrl.fixedPosition !$= "")
      %ctrl.position = %ctrl.fixedPosition;
   else
      %ctrl.position = vectorAdd("0 "@$RTB::CModManager::GUI::YSpacing,%ctrl.offset);
      
   if(%updateYSpacing)
   {
      %cPosY = getWord(%ctrl.position,1);
      %cExtY = getWord(%ctrl.extent,1);
      $RTB::CModManager::GUI::YSpacing = %cPosY + %cExtY + 1; 
   }
   RTBMM_WindowSwatch.add(%ctrl);
   RTBMM_ResizeSwatch();
}
//- RTBMM_OffsetWindow (manual tweaking of Y offsets)
function RTBMM_OffsetWindow(%ammount)
{
   $RTB::CModManager::GUI::YSpacing += %ammount+1;
}
//- RTBMM_ResizeSwatch (recalculating how far down scrollctrl should go)
function RTBMM_ResizeSwatch()
{
	%PosX = getWord(RTBMM_ModWindow.position,0);
	%PosY = getWord(RTBMM_ModWindow.position,1);
	%ExtX = "498";

   %targetExtent = 421;
   for(%i=1;%i<RTBMM_WindowSwatch.getCount();%i++)
   {
      %ctrl = RTBMM_WindowSwatch.getObject(%i);
      %extent = getWord(%ctrl.position,1) + getWord(%ctrl.extent,1) + 1;
      if(%extent > %targetExtent)
         %targetExtent = %extent;
   }
   %targetExtent = %targetExtent + $RTB::CModManager::GUI::LengthOverride;
	RTBMM_WindowSwatch.resize(%PosX, %PosY, %ExtX, %targetExtent);
}

//*********************************************************
//* Scripted GUI Generation - Libraries
//*********************************************************
function RTBMM_createLoadingContent(%text)
{
   if(%text $= "")
      %text = "Downloading Content";
      
	%swatch = new GuiSwatchCtrl(RTBMM_LoadingContentSwatch)
	{
	   fixedPosition = "1 1";
		extent = "498 419";
		color = "235 235 235 255";
		
		new GuiBitmapCtrl()
		{
		   horizSizing = "center";
		   vertSizing = "bottom";
		   position = "233 174";
		   extent = "31 31";
		   bitmap = "./images/loadRing";
		};
		
		new GuiMLTextCtrl()
		{
		   horizSizing = "center";
		   position = "190 209";
		   extent = "400 12";
		   profile = RTB_Verdana12Pt;
		   text = "<just:center><color:555555>Downloading Content...";
		};
	};
   RTBMM_PushWindowElement(%swatch);
   RTBMM_createLoadingRing(%swatch.getObject(0));
}
function RTBMM_createCenterMessage(%message)
{
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "7 3";
		extent = "483 21";
		text = "\c1"@%message;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch,1);
}
function RTBMM_overrideMainSwatchLength(%override)
{
   $RTB::CModManager::GUI::LengthOverride = %override;
   RTBMM_ResizeSwatch();
}
function RTBMM_createGoBackButton()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "216 3";
		extent = "66 21";
		text = "\c1<< Back >>";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 -1";
		extent = "501 29";
		color = "255 200 200 100";
		visible = 0;
	};
	RTBMM_PushWindowElement(%swatch);
	
	%mouseevent = new GuiMouseEventCtrl()
	{
		offset = "1 0";
		extent = "501 27";
		
		eventType = "backButtonSelect";
		eventCallbacks = "0001";
		
		eventHover = 1;
		eventSwatch = %swatch;
	};
	RTBMM_PushWindowElement(%mouseevent);

   RTBMM_overrideMainSwatchLength(-1);
   RTBMM_OffsetWindow(27);
}
function Event_backButtonSelect::onMouseUp(%this)
{
   RTBMM_goBack();
}
function RTBMM_moveBitmap(%gui)
{
	if(!isObject(%gui))
		return;

	%moves = %gui.moves+1;
	if(%moves > 16)
		%moves = 1;
   %positionX = -16+%moves;
   %gui.moves = %moves;
   %gui.position = %positionX SPC getWord(%gui.position,1);

	%gui.moveSchedule = schedule(30,0,"RTBMM_moveBitmap",%gui);
}
function RTBMM_runLoadingDots(%gui)
{
	if(!isObject(%gui))
		return;

	%dots = %gui.dots+1;
	if(%dots > 3)
		%dots = 1;
	for(%i=0;%i<%dots;%i++)
	{
		%dotString = %dotString@".";
	}

	%gui.setText("\c1"@%gui.textVal@%dotString);
	%gui.dots = %dots;
	%gui.dotSchedule = schedule(500,0,"RTBMM_runLoadingDots",%gui);
}
function RTBMM_createLoadingRing(%gui)
{
   if(!isObject(%gui))
      return;
      
   for(%i=1;%i<9;%i++)
   {
      %bmp = new GuiBitmapCtrl()
      {
         position = "0 0";
         extent = "31 31";
         bitmap = "./images/loadRing"@%i;
         visible = 0;
      };
      %gui.add(%bmp);
   }
   RTBMM_animateLoadingRing(%gui);
}
function RTBMM_animateLoadingRing(%gui)
{
   if(!isObject(%gui))
      return;
      
   if(%gui.ringNumber > 8 || %gui.ringNumber $= "")
      %gui.ringNumber = 1;
      
   for(%i=0;%i<%gui.getCount();%i++)
   {
      %gui.getObject(%i).setVisible(0);
   }
   %gui.getObject(%gui.ringNumber-1).setVisible(1);
   %gui.ringNumber++;
   
   %gui.ringSc = schedule(80,0,"RTBMM_animateLoadingRing",%gui);
}
function RTBMM_stopLoadingRing(%gui)
{
   %gui.clear();
   cancel(%gui.ringSc);
}
function RTBMM_Pagination(%this,%line)
{
   %numPages = getField(%line,0);
   %currPage = getField(%line,1);
   
   if(%numPages <= 1)
      return;
   
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		bitmap = "./images/cellpic3";
		
		new GuiMLTextCtrl()
		{
		   profile = RTBMM_PaginationProfile;
		   vertSizing = "center";
		   horizSizing = "center";
		   extent = "496 14";
		};
	};
	RTBMM_PushWindowElement(%bitmap,1);
	
	if(%currPage > 1)
	   %pagination = "<a:pagination-"@%currPage-1@">Previous</a> ";
	
	for(%i=1;%i<%numPages+1;%i++)
	{
	   if(%i $= %currPage)
	      %pagination = %pagination@"["@%i@"] ";
      else
         %pagination = %pagination@"<a:pagination-"@%i@">["@%i@"]</a> ";
	}
	%pagination = getSubStr(%pagination,0,strLen(%pagination)-1);
	
	if(%currPage !$= %numPages)
	{
	   %pagination = %pagination@" <a:pagination-"@%currPage+1@">Next</a>";
	}
	%bitmap.getObject(0).setText("<just:center>"@%pagination);
}
function RTBMM_handlePagination(%page)
{
   %lastCmd = RTBMM_SC.t_command[2];
   %lastString = RTBMM_SC.t_string[2];
   
   if($RTB::CModManager::Cache::CurrentZone $= "allfilesview")
   {
      RTBMM_ConstructWindow();
      RTBMM_SectionView_createHeader();
      RTBMM_createLoadingContent();
      RTBMM_SC.sendRequest(%lastCmd,%string,2,"&p="@%page);
   }
}

//*********************************************************
//* Public GUI Handlers
//*********************************************************
function RTBMM_createCustomHeader(%title)
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 3";
		extent = "498 21";
		text = "\c1"@%title;
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_createFooter()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		bitmap = "./images/cellpic3";
	};
	RTBMM_PushWindowElement(%bitmap,1);
}

//#############################################################################
//#
//#   Transmission Layers
//#
//#############################################################################
//#
//# Layer 1: Authentication
//# Layer 2: Browsing
//# Layer 3: Download Queue Data Retrieval
//# Layer 4: Rating Submitting
//# Layer 5: Commenting
//# Layer 6: File Update Checking
//#
//#############################################################################

//#############################################################################
//#
//#   Functions for real-time system usage
//#
//#############################################################################
//#
//# + Error Handling                         //> Messages or Errors from server
//# ---------------------------------------------------------------------------
//# - RTBMM_onMessage()                      //> Center Message
//# - RTBMM_onError()                        //> Error Box
//# - RTBMM_onSQLError()                     //> Bug Report Push & Msg
//# - RTBMM_onBarred()                       //> Incase you get barred (BAD!)
//#
//# + Category View                          //> Viewing categories of database
//# ---------------------------------------------------------------------------
//# - RTBMM_getCategoryView()                //> Action Call
//# - RTBMM_CategoryView_onTransmit()        //> Handle return from Database
//# - RTBMM_CategoryView_createHeader()      //> Header Call
//# - RTBMM_CategoryView_createCatRow()      //> Category Row Generation
//# - RTBMM_CategoryView_createSecRow()      //> Section Row Generation
//# - RTBMM_CategoryView_onTransmitEnd()     //> Handle end of Database
//#
//# + Section View                           //> Viewing sections of category
//# ---------------------------------------------------------------------------
//# - RTBMM_getSectionView()                 //> Action Call
//# - RTBMM_SectionView_onTransmit()         //> Handle return from Database
//# - RTBMM_SectionView_createHeader()       //> Header Call
//# - RTBMM_SectionView_createRow()          //> Add row to section listing
//# - RTBMM_SectionView_onTransmitEnd()      //> Process back button
//#
//# + File View                              //> Viewing files of section
//# ---------------------------------------------------------------------------
//# - RTBMM_getFileView()                    //> Action Call
//# - RTBMM_FileView_onTransmit()            //> Handle return from Database
//# - RTBMM_FileView_createGenericField()    //> Generic field property
//# - RTBMM_FileView_createAuthorField()     //> Author field with mousectrl
//# - RTBMM_FileView_createScreenshotField() //> Screenshot field with weblink
//# - RTBMM_FileView_createRatingField()     //> Rating Field with stars
//# - RTBMM_FileView_createFileOptions()     //> File options (download/rate)
//# - RTBMM_FileView_createCommentHeader()   //> Comment header for comments
//# - RTBMM_FileView_createCommentLine()     //> Comment line for file
//# - RTBMM_FileView_onTransmitEnd()         //> End Transmit from Database
//#
//# + Presets View                           //> View preset listing
//# ---------------------------------------------------------------------------
//# - RTBMM_getPresetsView()                 //> Action Call
//# - RTBMM_PresetsView_onTransmit()         //> Handle return from Database
//# - RTBMM_PresetsView_createHeader()       //> Header Call
//# - RTBMM_PresetsView_createRow()          //> Preset Row Generation
//# - RTBMM_PresetsView_onTransmitEnd()      //> Handle end of return
//#
//# + Preset View                            //> View single preset
//# ---------------------------------------------------------------------------
//# - RTBMM_getPresetView()                  //> Action Call
//# - RTBMM_PresetView_onTransmit()          //> Handle info from Database
//# - RTBMM_PresetView_onTransmitRow()       //> Handle row from Database
//# - RTBMM_PresetView_createRow()           //> Preset Row Generation
//# - RTBMM_PresetView_createOptions()       //> Create Options Listing
//# - RTBMM_PresetView_onTransmitEnd()       //> Handle return from Database
//# - RTBMM_PresetView_DownloadPreset()      //> Download Preset Handling
//#
//# + View All Files                         //> View all files in Database
//# ---------------------------------------------------------------------------
//# - RTBMM_getAllFilesView()                //> Action Call
//# - RTBMM_AllFilesView_onTransmit()        //> Handle return from Database
//# - RTBMM_AllFilesView_onTransmitEnd()     //> Handle end of return
//#
//# + View User Favourites                   //> View top 10 rated in database
//# ---------------------------------------------------------------------------
//# - RTBMM_getUserFavouritesView()          //> Action Call
//# - RTBMM_FavsView_onTransmit()            //> Handle return from Database
//# - RTBMM_FavsView_onTransmitEnd()         //> Handle end of return
//#
//# + Search View                            //> Window for doing a search
//# ---------------------------------------------------------------------------
//# - RTBMM_getSearchView()                  //> Action Call
//# - RTBMM_SearchView_onTransmit()          //> Handle return from database
//# - RTBMM_SearchView_createSearchGui()     //> Create gui with return from db
//# - RTBMM_SearchView_populateLists()       //> Populate search lists from db
//# - RTBMM_SearchView_performSearch()       //> Begin search
//# - RTBMM_SearchView_onResult()            //> Handle database return
//# - RTBMM_SearchView_createNoResults()     //> Handle no results
//# - RTBMM_SearchView_onResultEnd()         //> Handle end of return
//#
//# + News Feed View                         //> News Feed
//# ---------------------------------------------------------------------------
//# - RTBMM_getNewsFeedView()                //> Action Call
//# - RTBMM_NewsFeedView_onTransmit()        //> Handle return from database
//# - RTBMM_NewsFeedView_createFeedItem()    //> Creates news feed item
//#
//#############################################################################

//*********************************************************
//* Error Handling
//*********************************************************
function RTBMM_onMessage(%this, %line)
{
   RTBMM_createCenterMessage(getField(%line,0));
}
function RTBMM_onError(%this, %line)
{
   MessageBoxOK(getField(%line,0),getField(%line,1));
   MessagePopup("","",0);
}
function RTBMM_onSQLError(%this, %line)
{
   RTBBT_pushBugReporter("Mod Manager","SQL Error",2,%line);
   MessageBoxOK("ERROR","There has been a critical error. We would appreciate it if you could send this Bug Report so it can be fixed ASAP.");
}
function RTBMM_onBarred(%this, %line)
{
   canvas.pushDialog(RTB_BarredScreen);
   if(getField(%line,1) $= "")
      RTBB_Text.setText("No reason has been specified.");
   else
      RTBB_Text.setText(getField(%line,1));
      
   RTBMM_ConstructWindow();
   RTBMM_WindowSwatch.color = "0 0 0 0";
   RTBMM_WindowSwatch.getObject(0).color = "0 0 0 0";
   RTB_ModManager.getObject(0).setText("Mod Manager - Barred");
}

//*********************************************************
//* CategoryView: View Mod Categories+Sections
//*********************************************************
function RTBMM_getCategoryView()
{
	RTBMM_ConstructWindow();
	RTBMM_CategoryView_createHeader();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETCATEGORIES",2);
	RTBMM_trackZone("categoryview","RTBMM_getCategoryView();");
}
function RTBMM_CategoryView_onTransmit(%this, %line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "categoryview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   if(getField(%line,0) $= "CAT")
   {
      RTBMM_CategoryView_createCatRow(getField(%line,2));
   }
	else if(getField(%line,0) $= "SEC")
	{
		RTBMM_CategoryView_createSecRow(getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
	}
}
function RTBMM_CategoryView_createHeader()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "348 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "111 3";
		extent = "55 21";
		text = "\c1Category";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "350 0";
		extent = "44 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "8 3";
		extent = "28 21";
		text = "\c1Files";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "395 0";
		extent = "102 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "5 3";
		extent = "95 21";
		text = "\c1Last Submission";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_CategoryView_createCatRow(%name)
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "496 30";
		bitmap = "./images/cellpic1";
	};
	%text = new GuiTextCtrl()
	{
		profile = RTBMM_MainText;
		position = "4 4";
		horizSizing = "right";
		vertsizing = "center";
		extent = "430 22";
		text = "\c0"@%name;
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_CategoryView_createSecRow(%sectionid,%name,%description,%files,%lastsubmit)
{
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "32 32";
		color = "235 235 235 255";
	};
	%icon = new GuiBitmapCtrl()
	{
		position = "3 3";
		extent = "25 25";
		bitmap = "./images/folder_new";
	};
	%swatch.add(%icon);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "34 0";
		extent = "315 32";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "1 -1";
		extent = "307 18";
		text = "\c0"@%name;
		profile = "RTBMM_MainText";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "3 14";
		extent = "307 18";
		text = "\c0"@%description;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "350 0";
		extent = "44 32";
		color = "200 200 200 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 7";
		extent = "44 18";
		text = "\c1"@%files;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "395 0";
		extent = "102 32";
		color = "200 200 200 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 7";
		extent = "102 18";
		text = "\c1"@%lastsubmit;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "0 -1";
		extent = "501 34";
		color = "255 200 200 100";
		visible = 0;
	};
	RTBMM_PushWindowElement(%swatch);
	
	%mouseevent = new GuiMouseEventCtrl()
	{
		offset = "1 0";
		extent = "501 32";
		
		eventType = "sectionSelect";
		eventCallbacks = "0001";
		
		eventHover = 1;
		eventSwatch = %swatch;
		
		sectionID = %sectionid;
	};
	RTBMM_PushWindowElement(%mouseevent);
	RTBMM_OffsetWindow(32);
}
function RTBMM_CategoryView_onTransmitEnd(%this)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "categoryview")
      return;
      
   RTBMM_createFooter();
}
function Event_sectionSelect::onMouseUp(%this)
{
   RTBMM_getSectionView(%this.sectionID);
}

//*********************************************************
//* SectionView: Viewing a specific section, looking at files
//*********************************************************
function RTBMM_getSectionView(%section)
{
   //Init Temp Vars
   $RTB::CModManager::Cache::TotalSectionFiles = 0;
   
   RTBMM_ConstructWindow();
	RTBMM_SectionView_createHeader();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETSECTION",2,%section);
	RTBMM_trackZone("sectionview","RTBMM_getSectionView("@%section@");");
}
function RTBMM_SectionView_onTransmit(%this, %line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "sectionview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   RTBMM_SectionView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5),getField(%line,6));
}
function RTBMM_SectionView_createHeader()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "326 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "137 3";
		extent = "51 21";
		text = "\c1File Title";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "328 0";
		extent = "73 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "17 3";
		extent = "38 21";
		text = "\c1Author";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "402 0";
		extent = "95 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "28 3";
		extent = "39 21";
		text = "\c1Rating";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_SectionView_createRow(%fileid,%name,%description,%image,%author,%rating,%approved)
{
   if(strLen(%description) > 110)
      %description = getSubStr(%description,0,110)@"...";
      
   if(%approved $= 2)
   {
      %name = "\c1"@%name;
      %description = "This file failed inspection by moderators.";
      %image = "./images/exclamation";
   }
   else
      %image = "./images/icons/"@%image;
      
   $RTB::CModManager::Cache::TotalSectionFiles++;
	%rating1 = getSubStr(%rating,0,1);
	%rating2 = getSubStr(%rating,1,1);
	%rating3 = getSubStr(%rating,2,1);
	%rating4 = getSubStr(%rating,3,1);
	%rating5 = getSubStr(%rating,4,1);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "25 46";
		color = "235 235 235 255";
	};
	%icon = new GuiBitmapCtrl()
	{
		position = "4 15";
		extent = "16 16";
		bitmap = %image;
	};
	%swatch.add(%icon);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "27 0";
		extent = "300 46";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "2 0";
		extent = "307 18";
		text = "\c0"@%name;
		profile = "RTBMM_MainText";
	};
	%swatch.add(%text);
	
	%text = new GuiMLTextCtrl()
	{
		position = "2 18";
		extent = "295 26";
		text = "\c0"@%description;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "328 0";
		extent = "73 46";
		color = "200 200 200 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 14";
		extent = "73 18";
		text = "\c1"@%author;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "402 0";
		extent = "95 46";
		color = "230 230 230 255";
	};
	%star1 = new GuiBitmapCtrl()
	{
		position = "3 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating1;
	};
	%swatch.add(%star1);
	%star2 = new GuiBitmapCtrl()
	{
		position = "21 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating2;
	};
	%swatch.add(%star2);
	%star3 = new GuiBitmapCtrl()
	{
		position = "39 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating3;
	};
	%swatch.add(%star3);
	%star4 = new GuiBitmapCtrl()
	{
		position = "57 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating4;
	};
	%swatch.add(%star4);
	%star5 = new GuiBitmapCtrl()
	{
		position = "75 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating5;
	};
	%swatch.add(%star5);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 -1";
		extent = "501 48";
		color = "255 200 200 100";
		visible = 0;
	};
	RTBMM_PushWindowElement(%swatch);
	
	%mouseevent = new GuiMouseEventCtrl()
	{
		offset = "1 0";
		extent = "501 46";
		
		eventType = "fileSelect";
		eventCallbacks = "0001";
		
		eventHover = 1;
		eventSwatch = %swatch;
		
		fileID = %fileid;
	};
	RTBMM_PushWindowElement(%mouseevent);
   RTBMM_OffsetWindow(46);
}
function RTBMM_SectionView_onTransmitEnd(%this, %line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "sectionview")
      return;
      
   RTBMM_createGoBackButton("RTBMM_getCategoryView();");
}
function Event_fileSelect::onMouseUp(%this)
{
   RTBMM_getFileView(%this.fileID);
}

//*********************************************************
//* FileView: Viewing an individual file
//*********************************************************
function RTBMM_getFileView(%file)
{
   RTBMM_ConstructWindow();
	RTBMM_createLoadingContent();
	$RTB::CModManager::Cache::CurrentFile = %file;
	
	RTBMM_SendRequest("GETFILE",2,%file);
	RTBMM_trackZone("fileview","RTBMM_getFileView("@%file@");");
	
   $RTB::CModManager::Cache::CurrentMusic = %file;
   $RTB::CModManager::Cache::TotalMusic = 0;
}
function RTBMM_FileView_onTransmit(%this, %line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "fileview")
      return;
      
   %transType = getField(%line,0);
   if(%transType $= 0)
   {
      RTBMM_stopLoadingRing(RTBMM_LoadingContentSwatch.getObject(0));
      RTBMM_LoadingContentSwatch.getObject(0).setBitmap($RTB::Path@"images/loadRingFail");
      RTBMM_LoadingContentSwatch.getObject(1).setText("<just:center><color:555555>File not Found");
      return;
   }
   
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
   
   switch$(%transType)
   {
      case "HEADER":
      RTBMM_createCustomHeader(getField(%line,1));
      case "FIELD":
      RTBMM_FileView_createGenericField(getField(%line,1),getField(%line,2));
      case "AUTHOR":
      RTBMM_FileView_createAuthorField(getField(%line,1),getField(%line,2),getField(%line,3));
      case "SCREENSHOTS":
      RTBMM_FileView_createScreenshotsField(getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5),getField(%line,6),getField(%line,7),getField(%line,8),getField(%line,9),getField(%line,10),getField(%line,11),getField(%line,12),getField(%line,13),getField(%line,14),getField(%line,15));
      case "FOOTER":
      RTBMM_createFooter();
      case "RATING":
      RTBMM_FileView_createRatingField(getField(%line,1),getField(%line,2),getField(%line,3));
      case "BEGINOLDVERS":
      RTBMM_OffsetWindow(10);
      RTBMM_createCustomHeader("Outdated Versions");
      case "OLDVERS":
      RTBMM_SectionView_createRow(getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5),getField(%line,6));
      case "ENDOLDVERS":
      RTBMM_createFooter();
      case "BEGINCOMMENTS":
      RTBMM_OffsetWindow(10);
      RTBMM_createCustomHeader("Comments");
      RTBMM_FileView_createCommentHeader();
      case "COMMENT":
      RTBMM_FileView_createCommentLine(getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5),getField(%line,6),getField(%line,7),getField(%line,8),getField(%line,9),getField(%line,10));
      case "ENDCOMMENTS":
      RTBMM_createGoBackButton();
      case "MUSIC":
      RTBMM_FileView_createMusicRow(getField(%line,1),getField(%line,2),getField(%line,3));
   }
}
function RTBMM_FileView_createGenericField(%name,%value)
{
	%swatch2 = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1"@%name@":";
		profile = "RTBMM_FieldText";
	};
	%swatch2.add(%text);
	RTBMM_PushWindowElement(%swatch2);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%text = new GuiMLTextCtrl()
	{
		position = "4 5";
		extent = "305 14";
		text = "\c1"@%value;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%text.forcereflow();
	%newheight = getWord(%text.extent,1)+11;
	%swatch.extent = "315 "@%newheight;
	%swatch2.extent = "180 "@%newheight;
	
	RTBMM_OffsetWindow(%newheight);
}
function RTBMM_FileView_createAuthorField(%image,%author,%totals)
{
   $RTB::CModManager::Cache::FileAuthor = %author;
	$RTB::CModManager::CurrentAuthorCount = %totals;
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Author:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%bitmap = new GuiBitmapCtrl()
	{
		position = "4 4";
		extent = "16 16";
		bitmap = "./images/linkman"@%image;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%text = new GuiMLTextCtrl()
	{
		position = "24 6";
		extent = "305 14";
		text = %author;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.linkText = %text;
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	
   RTBMM_OffsetWindow(25);
}
function RTBMM_FileView_createScreenshotsField(%presence,%collage,%crc,%ss1,%caption1,%crc1,%ss2,%caption2,%crc2,%ss3,%caption3,%crc3,%ss4,%caption4,%crc4)
{
   if(!$RTB::Options::DownloadScreenshots)
      return;
      
   if(%presence $= "0000")
      return;
      
   $RTB::CModManager::Cache::FileCollage = %collage;
   $RTB::CModManager::Cache::FileSSPresence = %presence;
   RTBMM_createCustomHeader("Screenshots");
   
   %numSS = 0;
   for(%i=0;%i<4;%i++)
   {
      if(getSubStr(%presence,%i,1) $= 1)
      {
         %screen[%numSS] = %i+1;
         %numSS++;
      }
   }
   
   for(%i=0;%i<%numSS;%i++)
   {
      %screenID = %screen[%i];
      %screenBitmap = "RTBMM_FileThumb"@%i;
      %eventName = "RTBMM_FileThumbME"@%i;
      %swatch = new GuiSwatchCtrl()
      {
		   offset = 1+%i*mFloor(495/%numSS)+(%i*1) SPC "0";
		   extent = mFloor(495/%numSS) SPC "92";
		   color = "240 240 240 255";
		   
		   new GuiSwatchCtrl()
		   {
		      horizSizing = "center";
		      vertSizing = "center";
		      extent = "116 88";
		      color = "100 100 100 255";
            
            new GuiSwatchCtrl()
            {
               horizSizing = "center";
               vertSizing = "center";
               extent = "114 86";
               color = "255 255 255 255";
               
               new GuiSwatchCtrl(%screenBitmap)
               {
                  horizSizing = "center";
                  vertSizing = "center";
                  extent = "112 84";
                  color = "0 0 0 0";
                  visible = 0;
                  
                  new GuiBitmapCtrl()
                  {
                     position = "0 0";
                     extent = "224 168";
                  };
               };
               
               new GuiSwatchCtrl(%eventName)
               {
                  position = "0 0";
                  extent = "114 86";
                  color = "0 0 0 0";
               };
               
               new GuiMouseEventCtrl()
               {
                  position = "0 0";
                  extent = "114 86";
		            eventType = "screenSelect";
            		eventCallbacks = "0001";
		
		            eventHover = 1;
		            eventSwatch = %eventName;
		            
		            ssCaption = %caption[%screenID];
		            ssPath = %ss[%screenID];
		            ssCRC = %crc[%screenID];
               };
            };
		   };
      };
      if(%i $= %numSS-1)
         %add = 1;
      RTBMM_PushWindowElement(%swatch,%add);
      
      if(getFileCRC("config/client/RTB/cache/screenCollage.png") $= %crc)
      {
         %k=0;
         for(%j=0;%j<4;%j++)
         {
            %pres = getSubStr($RTB::CModManager::Cache::FileSSPresence,%j,1);
            if(%pres)
            {
               %ctrl = "RTBMM_FileThumb"@%k;
               if(isObject(%ctrl))
               {
                  %ctrl.setVisible(1);
                  %ctrl.getObject(0).setBitmap("config/client/RTB/cache/screenCollage");
                  
                  if(%j $= 1)
                     %ctrl.getObject(0).position = "-112 0";
                  else if(%j $= 2)
                     %ctrl.getObject(0).position = "0 -84";
                  else if(%j $= 3)
                     %ctrl.getObject(0).position = "-112 -84";
                  %k++;
               }
            }
         }
      }
      else
      {
         %loadingSwatch = new GuiSwatchCtrl()
         {
            horizSizing = "center";
            vertSizing = "center";
            extent = "112 84";
            color = "220 220 220 255";
            
            new GuiBitmapCtrl()
            {
               horizSizing = "center";
               vertSizing = "center";
               extent = "31 31";
               bitmap = "./images/loadRing";
            };
         };
         %screenBitmap.loadSwatch = %loadingSwatch;
         %screenBitmap.getGroup().add(%loadingSwatch);
         
         RTBMM_createLoadingRing(%loadingSwatch.getObject(0));
      }
   }
   RTBMM_ScreenshotGrabber.getCollage();
}
function Event_screenSelect::onMouseUp(%this)
{
   RTBMM_PushScreenshotViewer(%this.ssPath,%this.ssCaption,%this.ssCRC);
}
function RTBMM_FileView_createRatingField(%rating,%total,%user)
{
	%star1 = getSubStr(%rating,0,1);
	%star2 = getSubStr(%rating,1,1);
	%star3 = getSubStr(%rating,2,1);
	%star4 = getSubStr(%rating,3,1);
	%star5 = getSubStr(%rating,4,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Current Rating:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};

	%bitmap = new GuiBitmapCtrl()
	{
		position = "4 4";
		extent = "16 16";
		bitmap = "./images/star"@%star1;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%bitmap = new GuiBitmapCtrl()
	{
		position = "22 4";
		extent = "16 16";
		bitmap = "./images/star"@%star2;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%bitmap = new GuiBitmapCtrl()
	{
		position = "40 4";
		extent = "16 16";
		bitmap = "./images/star"@%star3;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%bitmap = new GuiBitmapCtrl()
	{
		position = "58 4";
		extent = "16 16";
		bitmap = "./images/star"@%star4;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%bitmap = new GuiBitmapCtrl()
	{
		position = "76 4";
		extent = "16 16";
		bitmap = "./images/star"@%star5;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%text = new GuiTextCtrl()
	{
		position = "100 4";
		extent = "150 18";
		text = "\c1("@%total@" Ratings)";
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
   if(%user !$= "")
      %text.setText("\c1("@%total@" Ratings - You gave a "@%user@")");
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl(RTBMM_FileRatingApply)
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%swatch.setVisible(0);
	%bitmap = new GuiBitmapCtrl(RTBMM_FileStar1)
	{
		position = "4 4";
		extent = "16 16";
		bitmap = "./images/star"@%star1;
		oldBitmap = %star1;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%mouseevent = new GuiMouseEventCtrl()
	{
		position = "3 4";
		extent = "18 16";
		
		eventType = "ratingAdd";
		eventCallbacks = "1101";
		
		eventNumber = 1;
		eventOld = %star1;
	};
	%swatch.add(%mouseevent);
	%bitmap = new GuiBitmapCtrl(RTBMM_FileStar2)
	{
		position = "22 4";
		extent = "16 16";
		bitmap = "./images/star"@%star2;
		oldBitmap = %star2;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%mouseevent = new GuiMouseEventCtrl()
	{
		position = "21 4";
		extent = "18 16";
		
		eventType = "ratingAdd";
		eventCallbacks = "1101";
		
		eventNumber = 2;
		eventOld = %star2;
	};
	%swatch.add(%mouseevent);
	%bitmap = new GuiBitmapCtrl(RTBMM_FileStar3)
	{
		position = "40 4";
		extent = "16 16";
		bitmap = "./images/star"@%star3;
		oldBitmap = %star3;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%mouseevent = new GuiMouseEventCtrl()
	{
		position = "39 4";
		extent = "18 16";
		
		eventType = "ratingAdd";
		eventCallbacks = "1101";
		
		eventNumber = 3;
		eventOld = %star3;
	};
	%swatch.add(%mouseevent);	
	%bitmap = new GuiBitmapCtrl(RTBMM_FileStar4)
	{
		position = "58 4";
		extent = "16 16";
		bitmap = "./images/star"@%star4;
		oldBitmap = %star4;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%mouseevent = new GuiMouseEventCtrl()
	{
		position = "57 4";
		extent = "18 16";
		
		eventType = "ratingAdd";
		eventCallbacks = "1101";
		
		eventNumber = 4;
		eventOld = %star4;
	};
	%swatch.add(%mouseevent);
	%bitmap = new GuiBitmapCtrl(RTBMM_FileStar5)
	{
		position = "76 4";
		extent = "16 16";
		bitmap = "./images/star"@%star5;
		oldBitmap = %star5;
		profile = "GuiDefaultProfile";
	};
	%swatch.add(%bitmap);
	%mouseevent = new GuiMouseEventCtrl()
	{
		position = "75 4";
		extent = "18 16";
		
		eventType = "ratingAdd";
		eventCallbacks = "1101";
		
		eventNumber = 5;
		eventOld = %star5;
	};
	%swatch.add(%mouseevent);
	%text = new GuiTextCtrl()
	{
		position = "100 4";
		extent = "200 18";
		text = "\c2 << Click a Star to give a Rating";
		profile = "RTBMM_BlockText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch,1);
}
function Event_ratingAdd::onMouseEnter(%this)
{
   for(%i=1;%i<6;%i++)
   {
      %starBitmap = "RTBMM_FileStar"@%i;
      if(%this.eventNumber < %i)
         %starBitmap.setBitmap($RTB::Path@"images/star0");
      else
         %starBitmap.setBitmap($RTB::Path@"images/star4");
   }
}
function Event_ratingAdd::onMouseLeave(%this)
{
   for(%i=1;%i<6;%i++)
   {
      %starBitmap = "RTBMM_FileStar"@%i;
      %starBitmap.setBitmap($RTB::Path@"images/star"@%starBitmap.oldBitmap);
   }
}
function Event_ratingAdd::onMouseUp(%this)
{
   %rating = %this.eventNumber;
   RTBMM_SendRequest("ADDRATING",4,$RTB::CModManager::Cache::CurrentFile,%rating);
   MessagePopup("Transmitting...","Your Rating of "@%rating@" is being Submitted...");
}
function RTBMM_FileView_createFileOptions(%disableDL,%canRate,%canComment)
{
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "500 40";
		color = "240 240 240 255";
	};
	
	if($RTB::CModManager::Cache::TotalMusic >= 1)
	{
      %btn = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "206 7";
         extent = "82 25";
         bitmap = "./images/buttons/link_download";
         text = "";
         command = "RTBMM_FileView_downloadMusic();";
      };
	}
	else
	{
      %btn = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "206 7";
         extent = "82 25";
         bitmap = "./images/buttons/link_download";
         text = "";
         command = "RTBMM_DownloadsView_addDownload("@$RTB::CModManager::Cache::CurrentFile@");";
      };
	}
	%swatch.add(%btn);
	
   if(%disableDL $= 2)
   {
      %btn.command = "MessageBoxOK(\"Whoops!\",\"You cannot Download this File since it failed inspection by our Add-On Moderators.\");";
      %btn.setColor("0.8 0.8 0.8 1");
   }
	
	if($RTB::CModManager::Session::LoggedIn)
	{
      %btn = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "58 7";
         extent = "82 25";
         bitmap = "./images/buttons/link_rating";
         text = "";
         command = "RTBMM_FileRatingApply.setVisible(1);";
      };
      %swatch.add(%btn);

      if($RTB::CModManager::Session::LogInName $= $RTB::CModManager::Cache::FileAuthor)
      {
         %btn.command = "MessageBoxOK(\"Whoops!\",\"You cannot rate your own Files!\");";
         %btn.setColor("0.8 0.8 0.8 1");
      }
	   if(!%canRate)
	   {
         %btn.command = "MessageBoxOK(\"Whoops!\",\"You have been barred from rating files.\");";
         %btn.setColor("0.8 0.8 0.8 1");
	   }
      
      %btn = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "358 7";
         extent = "82 25";
         bitmap = "./images/buttons/link_comment";
         text = "";
         command = "canvas.pushDialog(RTBMM_AddComment);RTBMM_SendCommentBtn.command = \"RTBMM_SendComment();\";";
      };
      %swatch.add(%btn);
      
      if(!%canComment)
      {
         %btn.command = "MessageBoxOK(\"Whoops!\",\"You have been barred from commenting on files.\");";
         %btn.setColor("0.8 0.8 0.8 1");
      }
	}
	RTBMM_PushWindowElement(%swatch,1);
}
function RTBMM_FileView_createCommentHeader()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "149 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "54 3";
		extent = "40 21";
		text = "\c1Author";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);
	
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "151 0";
		extent = "346 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "144 3";
		extent = "57 21";
		text = "\c1Comment";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_FileView_createCommentLine(%oddEven,%name,%title,%blid,%posts,%reppoints,%date,%comment,%canedit,%commentid)
{
   $RTB::CModManager::Cache::NumCommentQuotes = 0;
   if(%oddEven $= 1)
      %bgColor = "240 240 240 255";
   else
      %bgColor = "220 220 220 255";
      
   %comment = RTBMM_parseComment(%comment,%oddEven);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "149 96";
		color = %bgColor;
	};
	%text = new GuiTextCtrl()
	{
		position = "3 2";
		extent = "142 18";
		text = "\c1"@%name;
		profile = "RTBMM_BlockText";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "4 18";
		extent = "142 18";
		text = "\c1"@%title;
		profile = "RTBMM_GenText";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "4 45";
		extent = "66 18";
		text = "\c1Posts:";
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "37 45";
		extent = "66 18";
		text = "\c1"@%posts;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "4 60";
		extent = "70 18";
		text = "\c1Blockland ID:";
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "73 60";
		extent = "66 18";
		text = "\c1"@%blid;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "4 75";
		extent = "66 18";
		text = "\c1Rep Points:";
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	%text = new GuiTextCtrl()
	{
		position = "62 75";
		extent = "66 18";
		text = "\c1"@%reppoints;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatchB = new GuiSwatchCtrl()
	{
		offset = "151 0";
		extent = "346 96";
		color = %bgColor;
	};
	%horiz = new GuiSwatchCtrl()
	{
		position = "0 21";
		extent = "350 1";
		minExtent = "8 1";
		color = "255 255 255 255";
		profile = "GuiDefaultProfile";
	};
	%swatchB.add(%horiz);
	%text = new GuiTextCtrl()
	{
		position = "3 1";
		extent = "250 18";
		text = "\c1Date: "@%date;
		profile = "RTB_Verdana12Pt";
	};
	%swatchB.add(%text);
	%text = new GuiMLTextCtrl()
	{
		position = "3 25";
		extent = "339 18";
		text = "<color:777777>"@%comment;
		profile = "GuiMLTextCtrl";
	};
	%swatchB.add(%text);
	
	if(%canedit)
   {
      %bitmap = new GuiBitmapButtonCtrl()
      {
         position = "266 1";
         extent = "59 18";
         bitmap = "./images/buttons/btnEdit";
         profile = GuiDefaultProfile;
         text = "\c1";
         command = "RTBMM_EditComment("@%commentid@");";
      };
      %swatchB.add(%bitmap);
      
      %bitmap = new GuiBitmapCtrl()
      {
         position = "328 1";
         extent = "16 18";
         bitmap = "./images/icon_delete";
         profile = GuiDefaultProfile;
      };
      %swatchB.add(%bitmap);
   }
	RTBMM_PushWindowElement(%swatchB);
	
	for(%i=0;%i<$RTB::CModManager::Cache::NumCommentQuotes;%i++)
	{
	   %sw = new GuiSwatchCtrl()
	   {
	      position = "8" SPC 25+$RTB::CModManager::Cache::QuoteStart[%i];
	      extent = "330 40";
	      color = "150 150 150 255";
	   };
	   %swatchB.add(%sw);
	   
	   %sw2 = new GuiSwatchCtrl()
	   {
	      position = "1 1";
	      extent = "328 38";
	      color = "255 255 255 255";
	   };
	   %sw.add(%sw2);
	   
      %ml = new GuiMLTextCtrl()
      {
         position = "3 3";
         extent = "320 2";
         text = "<color:777777>"@RTBMM_parseComment($RTB::CModManager::Cache::QuoteText[%i]);
      };
      %sw2.add(%ml);
      
      %ml.forceReflow();      
      %sw2.resize(1,1,328,getWord(%ml.extent,1)+3);
      %sw.resize(8,getWord(%sw.position,1),330,getWord(%ml.extent,1)+5);
	}
	
	%text.forcereflow();
	%newheight = getWord(%text.extent,1)+30;
	if(%newheight < 96)
	   %newheight = 96;
	%swatch.extent = "149 "@%newheight;
	%swatchB.extent = "356 "@%newheight;

	RTBMM_OffsetWindow(%newheight);

	%swatch = new GuiSwatchCtrl()
	{
	   offset = "1 0";
	   extent = "500 12";
	   color = "220 220 220 255";
	   profile = GuiDefaultProfile;
	};
	RTBMM_PushWindowElement(%swatch,1);
}
function RTBMM_FileView_onTransmitFileEnd(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "fileview")
      return;
      
   RTBMM_createCustomHeader("File Menu");
   if(getField(%line,0) $= 1)
   {
      %swatch = new GuiSwatchCtrl()
      {
         offset = "1 0";
         extent = "498 27";
         color = "240 240 240 255";
      };
      %text = new GuiMLTextCtrl()
      {
         position = "7 3";
         extent = "483 21";
         text = "\c1<just:center>This file has <spush><font:Verdana Bold:12>not<spop> been approved by our download moderators.\n\nUnapproved add-ons are not guaranteed to work properly or be safe to your Blockland folder until they are tested by our moderators.";
         profile = "RTB_Verdana12PtCenter";
      };
      %swatch.add(%text);
      RTBMM_PushWindowElement(%swatch);
      
	   %text.forcereflow();
	   %newheight = getWord(%text.extent,1)+11;
	   %swatch.extent = "498 "@%newheight;
      RTBMM_OffsetWindow(%newheight);
   }
   else if(getField(%line,0) $= 0)
   {
      %swatch = new GuiSwatchCtrl()
      {
         offset = "1 0";
         extent = "495 28";
         color = "50 240 50 255";
      };
      %swatch2 = new GuiSwatchCtrl()
      {
         position = "1 1";
         extent = "493 26";
         color = "200 240 200 255";
      };
      %text = new GuiMLTextCtrl()
      {
         position = "7 3";
         extent = "483 21";
         text = "\c1<just:center>This file has been approved by a member of our moderation team. Approved add-ons work properly, and appear to be safe when installed.";
         profile = "RTB_Verdana12PtCenter";
      };
      %swatch2.add(%text);
      %swatch.add(%swatch2);
      RTBMM_PushWindowElement(%swatch);
      
	   %text.forcereflow();
	   %newheight = getWord(%text.extent,1)+11;
	   %swatch.extent = "495 "@%newheight;
	   %swatch2.extent = "493 "@%newheight-2;
      RTBMM_OffsetWindow(%newheight);
   }
   else if(getField(%line,0) $= 2)
   {
      %swatch = new GuiSwatchCtrl()
      {
         offset = "1 0";
         extent = "495 28";
         color = "240 50 50 255";
      };
      %swatch2 = new GuiSwatchCtrl()
      {
         position = "1 1";
         extent = "493 26";
         color = "240 200 200 255";
      };
      %text = new GuiMLTextCtrl()
      {
         position = "7 3";
         extent = "483 21";
         text = "\c1<just:center>Approval of this file has been denied by a member of our moderation team. This means the add-on was either broken or unsafe to run.\n\nSee file comments.";
         profile = "RTB_Verdana12PtCenter";
      };
      %swatch2.add(%text);
      %swatch.add(%swatch2);
      RTBMM_PushWindowElement(%swatch);
      
	   %text.forcereflow();
	   %newheight = getWord(%text.extent,1)+11;
	   %swatch.extent = "495 "@%newheight;
	   %swatch2.extent = "493 "@%newheight-2;
      RTBMM_OffsetWindow(%newheight);
   }
   RTBMM_FileView_createFileOptions(getField(%line,0),getField(%line,1),getField(%line,2));
   RTBMM_createGoBackButton();
}
function RTBMM_FileView_createMusicRow(%index,%name,%filesize)
{
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "25 32";
		color = "235 235 235 255";
	};
	%icon = new GuiBitmapCtrl()
	{
		position = "4 8";
		extent = "16 16";
		bitmap = "./images/music";
	};
	%swatch.add(%icon);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "27 0";
		extent = "278 32";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 7";
		extent = "272 18";
		text = "\c0"@%name;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "306 0";
		extent = "73 32";
		color = "222 222 222 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 7";
		extent = "73 18";
		text = "\c1"@%filesize;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "380 0";
		extent = "95 32";
		color = "230 230 230 255";
		
      new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "5 3";
         extent = "82 25";
         bitmap = "./images/buttons/link_download";
         text = "";
         command = "RTBMM_DownloadsView_addMusic("@$RTB::CModManager::Cache::CurrentMusic@","@%index@");";
      };
	};
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "476 0";
		extent = "22 32";
		color = "222 222 222 255";
	};
	%cbox = new GuiCheckBoxCtrl()
	{
		position = "4 1";
		extent = "140 30";
		text = " ";
		profile = "GuiCheckBoxProfile";
		variable = "$RTB::CModManager::MusicDL"@$RTB::CModManager::Cache::TotalMusic;
	};
	%swatch.add(%cbox);
	%cbox.setValue(1);
	RTBMM_PushWindowElement(%swatch,1);
   
   $RTB::CModManager::Cache::MusicIndex[$RTB::CModManager::Cache::TotalMusic] = %index;
   $RTB::CModManager::Cache::MusicDL[$RTB::CModManager::Cache::TotalMusic] = 1;
   $RTB::CModManager::Cache::TotalMusic++;
}
function RTBMM_FileView_downloadMusic()
{
   if($RTB::CModManager::Cache::TotalMusic <= 0)
   {
      MessageBoxOK("Ooops","There is no music available to download.");
      return;
   }
   
   for(%i=0;%i<$RTB::CModManager::Cache::TotalMusic;%i++)
   {
      %fileID = $RTB::CModManager::Cache::MusicIndex[%i];
      %selected = $RTB::CModManager::Cache::MusicDL[%i];
      if(%selected $= 1)
      {
         %totalSelections++;
         RTBMM_DownloadQueue.pushMusic($RTB::CModManager::Cache::CurrentMusic,%fileID);
      }
   }
   
   if(%totalSelections >= 1)
      MessageBoxYesNo("Whoopee","A total of "@%totalSelections@" music files have been added to your downloads queue.\n\nWould you like to view it now?","RTBMM_getDownloadsView();","");
   else
      MessageBoxOK("Whoops","No music files have been added to your download queue. You should try ticking some maybe.");
}

//*********************************************************
//* PresetsView: View of presets
//*********************************************************
function RTBMM_getPresetsView()
{
	RTBMM_ConstructWindow();
	RTBMM_PresetsView_createHeader();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETPRESETS",2);
	RTBMM_trackZone("presetsview","RTBMM_getPresetsView();");
}
function RTBMM_PresetsView_onTransmit(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "presetsview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   RTBMM_PresetsView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
}
function RTBMM_PresetsView_createHeader()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "300 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "117 3";
		extent = "65 21";
		text = "\c1Preset Title";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "302 0";
		extent = "73 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "7 3";
		extent = "58 21";
		text = "\c1File Count";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "376 0";
		extent = "121 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "28 3";
		extent = "65 21";
		text = "\c1Date Added";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_PresetsView_createRow(%presetId,%name,%icon,%description,%date,%filecount)
{
   if(strLen(%description) > 110)
      %description = getSubStr(%description,0,110)@"...";

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "25 46";
		color = "235 235 235 255";
	};
	%icon = new GuiBitmapCtrl()
	{
		position = "4 15";
		extent = "16 16";
		bitmap = "./images/icons/"@%icon;
	};
	%swatch.add(%icon);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "27 0";
		extent = "274 46";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "2 0";
		extent = "307 18";
		text = "\c0"@%name;
		profile = "RTBMM_MainText";
	};
	%swatch.add(%text);
	%text = new GuiMLTextCtrl()
	{
		position = "2 18";
		extent = "295 26";
		text = "\c0"@%description;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "302 0";
		extent = "73 46";
		color = "200 200 200 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 14";
		extent = "73 18";
		text = "\c1"@%filecount;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "376 0";
		extent = "123 46";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 14";
		extent = "114 18";
		text = "\c1"@%date;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 -1";
		extent = "501 48";
		color = "255 200 200 100";
		visible = 0;
	};
	RTBMM_PushWindowElement(%swatch);
	
	%mouseevent = new GuiMouseEventCtrl()
	{
		offset = "1 0";
		extent = "501 46";

		eventType = "presetSelect";
		eventCallbacks = "0001";
		
		eventHover = 1;
		eventSwatch = %swatch;
		
		presetID = %presetId;
	};
	RTBMM_PushWindowElement(%mouseevent);
   RTBMM_OffsetWindow(46);
}
function RTBMM_PresetsView_onTransmitEnd()
{
   if($RTB::CModManager::Cache::CurrentZone !$= "presetsview")
      return;
      
   RTBMM_createFooter();
}
function Event_presetSelect::onMouseUp(%this)
{
   RTBMM_getPresetView(%this.presetID);
}

//*********************************************************
//* PresetView: Single Preset
//*********************************************************
function RTBMM_getPresetView(%id)
{
   RTBMM_ConstructWindow();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETPRESET",2,%id);
	RTBMM_trackZone("presetview","RTBMM_getPresetView("@%id@");");
   $RTB::CModManager::Cache::CurrentPreset = %id;
   $RTB::CModManager::Cache::TotalPresets = 0;
}
function RTBMM_PresetView_onTransmit(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "presetview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   RTBMM_createCustomHeader("Preset Information");
   RTBMM_FileView_createGenericField("Name",getField(%line,0));
   RTBMM_FileView_createGenericField("Description",getField(%line,1));
   RTBMM_FileView_createGenericField("Date of Submission",getField(%line,2));
   RTBMM_createCustomHeader("Preset Items");
   RTBMM_createCenterMessage("You can untick files you do not wish to download, and then click the Download Button.");
}
function RTBMM_PresetView_onTransmitRow(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "presetview")
      return;
      
   RTBMM_PresetView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
}
function RTBMM_PresetView_createRow(%fileid,%name,%description,%image,%author,%rating)
{
   if(strLen(%description) > 110)
      %description = getSubStr(%description,0,110)@"...";

	%rating1 = getSubStr(%rating,0,1);
	%rating2 = getSubStr(%rating,1,1);
	%rating3 = getSubStr(%rating,2,1);
	%rating4 = getSubStr(%rating,3,1);
	%rating5 = getSubStr(%rating,4,1);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "25 46";
		color = "235 235 235 255";
	};
	%icon = new GuiBitmapCtrl()
	{
		position = "4 15";
		extent = "16 16";
		bitmap = "./images/icons/"@%image;
	};
	%swatch.add(%icon);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "27 0";
		extent = "278 46";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "2 0";
		extent = "272 18";
		text = "\c0"@%name;
		profile = "RTBMM_MainText";
	};
	%swatch.add(%text);
	%text = new GuiMLTextCtrl()
	{
		position = "2 18";
		extent = "272 26";
		text = "\c0"@%description;
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "306 0";
		extent = "73 46";
		color = "200 200 200 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "0 14";
		extent = "73 18";
		text = "\c1"@%author;
		profile = "RTB_Verdana12PtCenter";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "380 0";
		extent = "95 46";
		color = "230 230 230 255";
	};
	%star1 = new GuiBitmapCtrl()
	{
		position = "3 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating1;
	};
	%swatch.add(%star1);
	%star2 = new GuiBitmapCtrl()
	{
		position = "21 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating2;
	};
	%swatch.add(%star2);
	%star3 = new GuiBitmapCtrl()
	{
		position = "39 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating3;
	};
	%swatch.add(%star3);
	%star4 = new GuiBitmapCtrl()
	{
		position = "57 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating4;
	};
	%swatch.add(%star4);
	%star5 = new GuiBitmapCtrl()
	{
		position = "75 15";
		extent = "16 16";
		bitmap = "./images/star"@%rating5;
	};
	%swatch.add(%star5);
	RTBMM_PushWindowElement(%swatch);
	
	%swatch = new GuiSwatchCtrl()
	{
		offset = "476 0";
		extent = "22 46";
		color = "200 200 200 255";
	};
	%cbox = new GuiCheckBoxCtrl()
	{
		position = "4 9";
		extent = "140 30";
		text = " ";
		profile = "GuiCheckBoxProfile";
		variable = "$RTB::CModManager::Cache::PresetDL"@$RTB::CModManager::Cache::TotalPresets;
	};
	%swatch.add(%cbox);
	%cbox.setValue(1);
	RTBMM_PushWindowElement(%swatch);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 -1";
		extent = "475 48";
		color = "255 200 200 100";
		visible = 0;
	};
	RTBMM_PushWindowElement(%swatch);
	
	%mouseevent = new GuiMouseEventCtrl()
	{
		offset = "1 0";
		extent = "475 46";
		linkSwatch = %swatch;
		linkType = "file";
		linkValue = %fileid;
	};
	RTBMM_PushWindowElement(%mouseevent);
   RTBMM_OffsetWindow(46);
   
   $RTB::CModManager::Cache::Preset[$RTB::CModManager::Cache::TotalPresets] = %fileid;
   $RTB::CModManager::Cache::PresetDL[$RTB::CModManager::Cache::TotalPresets] = 1;
   $RTB::CModManager::Cache::TotalPresets++;
}
function RTBMM_PresetView_createOptions()
{
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "500 40";
		color = "215 215 215 255";
	};
	%btn = new GuiBitmapButtonCtrl()
	{
	   profile = GuiDefaultProfile;
	   position = "205 7";
	   extent = "82 25";
	   bitmap = "./images/buttons/link_download";
	   text = "";
	   command = "RTBMM_PresetView_DownloadPreset();";
	};
	%swatch.add(%btn);
	RTBMM_PushWindowElement(%swatch,1);
}
function RTBMM_PresetView_onTransmitEnd()
{
   if($RTB::CModManager::Cache::CurrentZone !$= "presetview")
      return;
      
   RTBMM_PresetView_createOptions();
   RTBMM_createGoBackButton();
}
function RTBMM_PresetView_DownloadPreset()
{
   if($RTB::CModManager::Cache::TotalPresets <= 0)
   {
      MessageBoxOK("Ooops","There are no items in this Preset that are available for download.");
      return;
   }
   
   for(%i=0;%i<$RTB::CModManager::Cache::TotalPresets;%i++)
   {
      %fileID = $RTB::CModManager::Cache::Preset[%i];
      %selected = $RTB::CModManager::Cache::PresetDL[%i];
      if(%selected $= 1)
      {
         %totalSelections++;
         RTBMM_DownloadQueue.pushFile(%fileID);
      }
   }
   
   if(%totalSelections >= 1)
      MessageBoxYesNo("Whoopee","A total of "@%totalSelections@" files have been added to your downloads queue.\n\nWould you like to view it now?","RTBMM_getDownloadsView();","");
   else
      MessageBoxOK("Whoops","No files have been added to your download queue. Either you already have them, or you did not tick them.");
}

//*********************************************************
//* Grab all files in downloads system
//*********************************************************
function RTBMM_getAllFilesView()
{
	RTBMM_ConstructWindow();
	RTBMM_SectionView_createHeader();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETALLFILES",2);
	RTBMM_trackZone("allfilesview","RTBMM_getAllFilesView();");
}
function RTBMM_AllFilesView_onTransmit(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "allfilesview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   RTBMM_SectionView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5),getField(%line,6));
}
function RTBMM_AllFilesView_onTransmitEnd(%this)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "allfilesview")
      return;
      
   RTBMM_createFooter();
}

//*********************************************************
//* Grab list of top 10 high-rated files
//*********************************************************
function RTBMM_getUserFavouritesView()
{
	RTBMM_ConstructWindow();
	RTBMM_SectionView_createHeader();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETUSERFAVS",2);
	RTBMM_trackZone("favouritesview","RTBMM_getUserFavouritesView();");
}
function RTBMM_FavsView_onTransmit(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "favouritesview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   RTBMM_SectionView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
}
function RTBMM_FavsView_onTransmitEnd(%this)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "favouritesview")
      return;
      
   RTBMM_createFooter();
}

//*********************************************************
//* Search window generation
//*********************************************************
function RTBMM_getSearchView(%oldData)
{
   if(%oldData)
   {
      RTBMM_ConstructWindow();
      RTBMM_createCustomHeader("Search Results");
      RTBMM_SectionView_createHeader();
      for(%i=0;%i<$RTB::CModManager::Cache::TotalSearchResults;%i++)
      {
         %line = $RTB::CModManager::Cache::SearchResultLine[%i];
         RTBMM_SectionView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
      }
      RTBMM_trackZone("searchresultsview","RTBMM_getSearchView(1);");
      RTBMM_createGoBackButton();
      return;
   }
   
   $RTB::CModManager::Cache::TotalSearchResults = 0;
   $RTB::CModManager::Search::CategoryListCount = 0;
   $RTB::CModManager::Search::SectionListCount = 0;
   
	RTBMM_ConstructWindow();
	RTBMM_createLoadingContent();
	
	RTBMM_SendRequest("GETSEARCHDATA",2);
	RTBMM_trackZone("searchview","RTBMM_getSearchView();");
}
function RTBMM_SearchView_onTransmit(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "searchview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   if(getField(%line,0) $= "CAT")
   {
      $RTB::CModManager::Search::CategoryListID[$RTB::CModManager::Search::CategoryListCount] = getField(%line,1);
      $RTB::CModManager::Search::CategoryListName[$RTB::CModManager::Search::CategoryListCount] = getField(%line,2);
      $RTB::CModManager::Search::CategoryListCount++;
   }
   else if(getField(%line,0) $= "SEC")
   {
      $RTB::CModManager::Search::SectionListID[$RTB::CModManager::Search::SectionListCount] = getField(%line,1);
      $RTB::CModManager::Search::SectionListName[$RTB::CModManager::Search::SectionListCount] = getField(%line,2);
      $RTB::CModManager::Search::SectionListCat[$RTB::CModManager::Search::SectionListCount] = getField(%line,3);
      $RTB::CModManager::Search::SectionListCount++;
   }
}
function RTBMM_SearchView_createSearchGui()
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "495 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "72 3";
		extent = "351 21";
		text = "\c1Search MOD Database";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Search for Keywords:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%edit = new GuiTextEditCtrl(RTBMM_SearchView_Keywords)
	{
		position = "4 3";
		extent = "309 18";
		profile = "GuiTextEditProfile";
	};
	%swatch.add(%edit);
	RTBMM_PushWindowElement(%swatch,1);
   
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Search for Author:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%edit = new GuiTextEditCtrl(RTBMM_SearchView_Author)
	{
		position = "4 3";
		extent = "309 18";
		profile = "GuiTextEditProfile";
	};
	%swatch.add(%edit);
	RTBMM_PushWindowElement(%swatch,1);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "495 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "72 3";
		extent = "351 21";
		text = "\c1Search Options";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1File Category:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%menu = new GuiPopupMenuCtrl(RTBMM_SearchView_Category)
	{
		position = "4 3";
		extent = "309 18";
		profile = "GuiPopupMenuProfile";
	};
	%swatch.add(%menu);
	RTBMM_PushWindowElement(%swatch,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1File Section:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%menu = new GuiPopupMenuCtrl(RTBMM_SearchView_Section)
	{
		position = "4 3";
		extent = "309 18";
		profile = "GuiPopupMenuProfile";
	};
	%swatch.add(%menu);
	RTBMM_PushWindowElement(%swatch,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Search Alternatives:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%check = new GuiCheckBoxCtrl(RTBMM_SearchView_ShortDesc)
	{
		position = "4 -2";
		extent = "140 30";
		profile = "GuiCheckBoxProfile";
		text = "Search Short Description";
	};
	%swatch.add(%check);
	%check = new GuiCheckBoxCtrl(RTBMM_SearchView_LongDesc)
	{
		position = "178 -2";
		extent = "140 30";
		profile = "GuiCheckBoxProfile";
		text = "Search Long Description";
	};
	%swatch.add(%check);
	RTBMM_PushWindowElement(%swatch,1);

	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "180 25";
		color = "240 240 240 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "4 3";
		extent = "171 18";
		text = "\c1Sort By:";
		profile = "RTBMM_FieldText";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch);
	%swatch = new GuiSwatchCtrl()
	{
		offset = "182 0";
		extent = "315 25";
		color = "222 222 222 255";
	};
	%menu = new GuiPopupMenuCtrl(RTBMM_SearchView_SortBy)
	{
		position = "4 3";
		extent = "148 18";
		profile = "GuiPopupMenuProfile";
	};
	%swatch.add(%menu);
	%radio = new GuiRadioCtrl(RTBMM_SearchView_SortDir_Asc)
	{
		position = "161 -3";
		extent = "140 30";
		profile = "GuiRadioProfile";
		text = "Ascending";
	};
	%swatch.add(%radio);
	%radio = new GuiRadioCtrl(RTBMM_SearchView_SortDir_Desc)
	{
		position = "236 -3";
		extent = "140 30";
		profile = "GuiRadioProfile";
		text = "Descending";
	};
	%swatch.add(%radio);
	RTBMM_PushWindowElement(%swatch,1);
	RTBMM_createFooter();
	
	%button = new GuiBitmapButtonCtrl()
	{
	   profile = BlockButtonProfile;
	   fixedPosition = "174 246";
	   extent = "149 30";
	   text = "Perform Search";
	   command = "RTBMM_SearchView_PerformSearch();";
	   bitmap = "base/client/ui/button1";
	   mColor = "100 255 100 255";
	};
	RTBMM_PushWindowElement(%button);
	
	RTBMM_SearchView_populateLists();
	
	RTBMM_SearchView_SortBy.add("Name",0);
	RTBMM_SearchView_SortBy.add("Rating",1);
	RTBMM_SearchView_SortBy.add("Downloads",2);
	RTBMM_SearchView_SortBy.add("Date Submitted",3);
   //RTBMM_Search_SortBy.setSelected(0);
	RTBMM_SearchView_SortBy.setSelected(1);
	RTBMM_SearchView_SortDir_Desc.setValue(1);
	//RTBMM_Search_SortDir_Asc.setValue(1);
	RTBMM_Searchview_ShortDesc.setValue(1);
	RTBMM_SearchView_Keywords.makeFirstResponder(1);
}
function RTBMM_SearchView_populateLists()
{
   %catList = RTBMM_SearchView_Category;
   %secList = RTBMM_SearchView_Section;
   if(%catList.size() $= 0 && %secList.size() $= 0)
   {
      %catList.add("All Available",0);
      for(%i=0;%i<$RTB::CModManager::Search::CategoryListCount;%i++)
      {
         %ID = $RTB::CModManager::Search::CategoryListID[%i];
         %name = $RTB::CModManager::Search::CategoryListName[%i];
         %catList.add(%name,%ID);
      }
      
      %secList.add("All Available",0);
      for(%i=0;%i<$RTB::CModManager::Search::SectionListCount;%i++)
      {
         %ID = $RTB::CModManager::Search::SectionListID[%i];
         %name = $RTB::CModManager::Search::SectionListName[%i];
         %secList.add(%name,%ID);
      }
      
      %catList.setSelected(0);
      %secList.setSelected(0);
   }
   else
   {
      %catSelection = %catList.getSelected();
      if(%catSelection $= 0)
      {
         %secList.clear();
         %secList.add("All Available",0);
         for(%i=0;%i<$RTB::CModManager::Search::SectionListCount;%i++)
         {
            %ID = $RTB::CModManager::Search::SectionListID[%i];
            %name = $RTB::CModManager::Search::SectionListName[%i];
            %secList.add(%name,%ID);
         }
         %secList.setSelected(0);
      }
      else
      {
         %secList.clear();
         %secList.add("All Available",0);
         for(%i=0;%i<$RTB::CModManager::Search::SectionListCount;%i++)
         {
            %ID = $RTB::CModManager::Search::SectionListID[%i];
            %name = $RTB::CModManager::Search::SectionListName[%i];
            %catID = $RTB::CModManager::Search::SectionListCat[%i];
            if(%catID $= %catSelection)
               %secList.add(%name,%ID);
         }
         %secList.setSelected(0);
      }
   }
}
function RTBMM_SearchView_onTransmitEnd(%this)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "searchview")
      return;
      
   RTBMM_SearchView_createSearchGui();
}
function RTBMM_SearchView_Category::onSelect(%this,%text,%id)
{
   RTBMM_SearchView_populateLists();
}
function RTBMM_SearchView_PerformSearch()
{
   %keywords = RTBMM_SearchView_Keywords.getValue();
   if(%keywords $= "")
   {
      MessageBoxOK("Ooops","You haven't entered any Keywords to Search for. Use * to find all Mods in the Database.");
      return;
   }
   %author = RTBMM_SearchView_Author.getValue();
   
   %category = RTBMM_SearchView_Category.getSelected();
   if(%category $= 0)
      %category = "ALL";
   %section = RTBMM_SearchView_Section.getSelected();
   if(%section $= 0)
      %section = "ALL";
   %searchShortDesc = RTBMM_SearchView_ShortDesc.getValue();
   %searchLongDesc = RTBMM_SearchView_LongDesc.getValue();
   %sortBy = RTBMM_SearchView_SortBy.getSelected();
   if(%sortBy $= 0)
      %sortBy = "file_title";
   else if(%sortBy $= 1)
      %sortBy = "rating";
   else if(%sortBy $= 2)
      %sortBy = "file_downloads";
   else if(%sortBy $= 3)
      %sortBy = "file_date";
   if(RTBMM_SearchView_SortDir_Asc.getValue() $= 1)
      %sortDir = "ASC";
   else
      %sortDir = "DESC";

   RTBMM_ConstructWindow();
   RTBMM_createCustomHeader("Search Results");
   RTBMM_SectionView_createHeader();
   RTBMM_createLoadingContent("Performing Search");
   RTBMM_SendRequest("GETSEARCHRESULTS",2,%keywords,%author,%category,%section,%searchShortDesc,%searchLongDesc,%sortBy,%sortDir);
	RTBMM_trackZone("searchresultsview","RTBMM_getSearchView(1);");
}
function RTBMM_SearchView_OnResult(%this,%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "searchresultsview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   $RTB::CModManager::Cache::SearchResultLine[$RTB::CModManager::Cache::TotalSearchResults] = getField(%line,0)TAB getField(%line,1) TAB getField(%line,2) TAB getField(%line,3) TAB getField(%line,4) TAB getField(%line,5);
   $RTB::CModManager::Cache::TotalSearchResults++;
   RTBMM_SectionView_createRow(getField(%line,0),getField(%line,1),getField(%line,2),getField(%line,3),getField(%line,4),getField(%line,5));
}
function RTBMM_SearchView_createNoResults()
{
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
		
	%swatch = new GuiSwatchCtrl()
	{
		offset = "1 0";
		extent = "498 27";
		color = "235 235 235 255";
	};
	%text = new GuiTextCtrl()
	{
		position = "166 3";
		extent = "165 21";
		text = "\c1Your Search returned no Results.";
		profile = "RTB_Verdana12Pt";
	};
	%swatch.add(%text);
	RTBMM_PushWindowElement(%swatch,1);
}
function RTBMM_SearchView_OnResultEnd(%line)
{
   if($RTB::CModManager::Cache::CurrentZone !$= "searchresultsview")
      return;
      
	if(isObject(RTBMM_LoadingContentSwatch))
		RTBMM_LoadingContentSwatch.delete();
      
   if($RTB::CModManager::Cache::TotalSearchResults <= 0)
      RTBMM_SearchView_createNoResults();
   RTBMM_createGoBackButton();
}

//*********************************************************
//* News Feeder
//*********************************************************
function RTBMM_getNewsFeedView(%nocheck)
{
   RTBMM_ConstructWindow();
   RTBMM_WindowSwatch.color = "0 0 0 0";
   RTBMM_WindowSwatch.getObject(0).color = "0 0 0 0";
   
   RTBMM_NewsFeedView_createStatusBar(%nocheck);
   
   RTBMM_NewsFeedView_createExistingFeeds();
   
   RTBMM_btnNewsFeed.setBitmap($RTB::Path@"images/buttons/link_newsFeed");
   RTBMM_trackZone("newsfeedview","RTBMM_getNewsFeedView();");
   
   if(%nocheck)
      return;
      
   RTBMM_SendRequest("GETNEWSFEED",6,RTBMM_getDelimitedMods(),RTBMM_FeedControl.latestFeed);
   $RTB::CModManager::Cache::NumNewFeeds = 0;
}
function RTBMM_NewsFeedView_onTransmit(%this,%line)
{
   if(%line $= "END")
   {
      if(isObject(RTBMM_FeedCheck))
         RTBMM_FeedCheck.delete();
      return;
   }
      
   %feedId = getField(%line,0);
   %feedType = getField(%line,1);
   %feedDate = getField(%line,2);
   %feedSubject = getField(%line,3);
   %feedMessage = getField(%line,4);
   
   %feed = new ScriptObject()
   {
      class = RTBMM_NewsFeedItem;
      feed_id = %feedId;
      feed_type = %feedType;
      feed_date = %feedDate;
      feed_subject = %feedSubject;
      feed_message = %feedMessage;
      feed_read = false;
   };
   RTBMM_FeedControl.add(%feed);
   RTBMM_FeedControl_Save(RTBMM_FeedControl);
   
   if(!isObject(RTBMM_NumNewFeeds))
      RTBMM_btnNewsFeed.setBitmap($RTB::Path@"images/buttons/link_newsFeedNew");
   else
   {
      RTBMM_NewsFeedView_createFeedItem(%feedId);
      
      if(%feedId > RTBMM_FeedControl.latestFeed)
      {
         $RTB::CModManager::Cache::NumNewFeeds++;
         RTBMM_NumNewFeeds.setText("New: "@$RTB::CModManager::Cache::NumNewFeeds);
      }
   }
   
   if(%feedId > RTBMM_FeedControl.latestFeed)
      RTBMM_FeedControl.latestFeed = %feedId;
}
function RTBMM_NewsFeedView_createStatusBar(%noload)
{
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "400 27";
		bitmap = "./images/cellpic3";
	};
	
   if(!%noload)
   {
      %text = new GuiTextCtrl(RTBMM_FeedCheck)
      {
         position = "3 3";
         extent = "200 21";
         text = "\c1Checking for News";
         profile = "RTBMM_BlockText";
         textVal = "\c1Checking for News";
      };
      %bitmap.add(%text);
      RTBMM_runLoadingDots(%text);
   }
   
   %button = new GuiBitmapButtonCtrl()
   {
      position = "321 3";
      extent = "73 20";
      profile = "BlockButtonProfile";
      text = "Clear All";
      bitmap = "base/client/ui/button1";
      command = "RTBMM_NewsFeedView_clearFeeds();";
   };
   %bitmap.add(%button);
   
   RTBMM_PushWindowElement(%bitmap);
	
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "402 0";
		extent = "100 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl(RTBMM_NumNewFeeds)
	{
		position = "0 3";
		extent = "100 21";
		text = "\c1New: 0";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_NewsFeedView_createExistingFeeds(%onlyLive)
{
   for(%i=0;%i<RTBMM_FeedControl.getCount();%i++)
   {
      if(%onlyLive && RTBMM_FeedControl.getObject(%i).feed_loaded)
         continue;
      RTBMM_NewsFeedView_createFeedItem(RTBMM_FeedControl.getObject(%i).feed_id);
   }
}
function RTBMM_NewsFeedView_deleteFeed(%feed)
{
   if(isObject(%feed))
   {
      %feed.delete();
      RTBMM_FeedControl_reqSave(RTBMM_FeedControl);
      schedule(10,0,"RTBMM_getNewsFeedView",1);
   }
}
function RTBMM_NewsFeedView_clearFeeds()
{
   RTBMM_FeedControl.clear();
   RTBMM_FeedControl_reqSave(RTBMM_FeedControl);
   schedule(10,0,"RTBMM_getNewsFeedView",1);
}
function RTBMM_NewsFeedView_createFeedItem(%feedId)
{
   for(%i=0;%i<RTBMM_FeedControl.getCount();%i++)
   {
      %feed = RTBMM_FeedControl.getObject(%i);
      if(%feed.feed_id $= %feedId)
         break;
   }
   
   if(%feed.feed_type $= 1)
   {
      %borderColor = "100 100 250 255";
      %backgroundColor = "70 70 255 50";
      %feedIcon = "feed";
   }
   else if(%feed.feed_type $= 2)
   {
      %borderColor = "255 100 100 255";
      %backgroundColor = "255 70 70 50";
      %feedIcon = "exclamation";
   }
   else if(%feed.feed_type $= 3)
   {
      %borderColor = "100 255 100 255";
      %backgroundColor = "70 255 70 50";
      %feedIcon = "newaddon";
   }
   else if(%feed.feed_type $= 4)
   {
      %borderColor = "200 200 50 255";
      %backgroundColor = "255 255 70 50";
      %feedIcon = "newupdate";
   }
   
	%container = new GuiSwatchCtrl()
	{
		position = "3 32";
		extent = "491 100";
		color = %backgroundColor;
		
		isContainer = 1;
	};
	%topswatch = new GuiSwatchCtrl()
	{
	   position = "0 0";
	   extent = "491 1";
	   minExtent = "1 1";
	   color = %borderColor;
	};
	%container.add(%topswatch);
	%bottomswatch = new GuiSwatchCtrl()
	{
	   position = "0 99";
	   extent = "491 1";
	   minExtent = "1 1";
	   color = %borderColor;
	};
	%container.add(%bottomswatch);
	%leftswatch = new GuiSwatchCtrl()
	{
	   position = "0 0";
	   extent = "1 100";
	   minExtent = "1 1";
	   color = %borderColor;
	};
	%container.add(%leftswatch);
	%rightswatch = new GuiSwatchCtrl()
	{
	   position = "490 0";
	   extent = "1 100";
	   minExtent = "1 1";
	   color = %borderColor;
	};
	%container.add(%rightswatch);
	%swatch = new GuiSwatchCtrl()
	{
	   position = "3 27";
	   extent = "485 1";
	   minExtent = "1 1";
	   color = %borderColor;
	};
	%container.add(%swatch);
	%icon = new GuiBitmapCtrl()
	{
	   position = "6 6";
	   extent = "16 16";
	   bitmap = $RTB::Path@"images/"@%feedIcon;
	};
	%container.add(%icon);
	%subject = new GuiTextCtrl()
	{
	   profile = RTB_Verdana12PtAuto;
	   position = "26 6";
	   text = %feed.feed_subject;
	};
	%container.add(%subject);
	%date = new GuiMLTextCtrl()
	{
	   profile = RTB_Verdana12Pt;
	   position = "190 6";
	   extent = "297 16";
	   text = "<just:right>"@%feed.feed_date;
	};
	%container.add(%date);
	//RTBMM_PushWindowElement(%container);
	
   RTBMM_WindowSwatch.add(%container);
   RTBMM_ResizeSwatch();
	
	if(!%feed.feed_read)
	{
      %icon = new GuiBitmapCtrl()
      {
         position = getWord(%subject.extent,0)+32 SPC 7;
         extent = "16 16";
         bitmap = $RTB::Path@"images/new";
      };
      %container.add(%icon);
	}
	
	%message = new GuiMLTextCtrl(RTBMM_FeedMessage)
	{
	   profile = RTB_Verdana12Pt;
	   position = "4 32";
	   extent = "482 65";
	   text = "<color:444444>"@%feed.feed_message;
	};
	%container.add(%message);
	%message.forceReflow();
	
	%container.extent = getWord(%container.extent,0) SPC getWord(%message.extent,1)+38;
	%bottomswatch.position = getWord(%bottomswatch.position,0) SPC getWord(%message.extent,1)+37;
	%rightswatch.extent = "1" SPC getWord(%container.extent,1);
	%leftswatch.extent = "1" SPC getWord(%container.extent,1);
	
	%delBtn = new GuiBitmapButtonCtrl()
	{
	   extent = "11 11";
	   position = "476" SPC getWord(%message.extent,1)+getWord(%message.position,1)-8;
	   bitmap = "./images/buttons/btnDelete";
	   command = "RTBMM_NewsFeedView_DeleteFeed("@%feed@");";
	};
	%container.add(%delBtn);
	
	for(%i=0;%i<RTBMM_WindowSwatch.getCount();%i++)
	{
	   %ctrl = RTBMM_WindowSwatch.getObject(%i);
	   if(%ctrl.isContainer && %ctrl !$= %container)
	      %ctrl.position = getWord(%ctrl.position,0) SPC getWord(%ctrl.position,1)+getWord(%container.extent,1)+4;
	}
   RTBMM_overrideMainSwatchLength(2);
   
   %feed.feed_read = 1;
   RTBMM_FeedControl_Save(RTBMM_FeedControl);
}
function RTBMM_FeedMessage::onURL(%this,%url)
{
   if(strPos(%url,"file-") $= 0)
      schedule(1,0,"RTBMM_getFileView",getSubStr(%url,5,strLen(%url)));
   if(strPos(%url,"download-") $= 0)
      schedule(1,0,"RTBMM_DownloadsView_AddDownload",getSubStr(%url,9,strLen(%url)));
}

//*********************************************************
//* News Feed Interaction
//*********************************************************
function RTBMM_InitateFeedControl()
{
   if(isObject(RTBMM_FeedControl))
      RTBMM_FeedControl.delete();
      
   new SimGroup(RTBMM_FeedControl);
   RTBMM_FeedControl::load(RTBMM_FeedControl);
}
function RTBMM_FeedControl::load(%this)
{
   if(!isFile("config/client/RTB/newsFeed.feed"))
   {
      %this.latestFeed = 0;
      return;
   }
      
   %file = new FileObject();
   %file.openForRead("config/client/RTB/newsFeed.feed");
   %this.latestFeed = %file.readLine();
   
   while(!%file.isEOF())
   {
      %line = %file.readLine();

      if(!%file.readingMessage)
      {
         if(%line $= "")
            continue;

         if(strPos(%line,"---") $= 0)
         {
            %file.readingMessage = 1;
            %feedMessage = "";
            continue;
         }
         
         %feedData = strReplace(%line,"|","\t");
         %feedId = getField(%feedData,0);
         %feedType = getField(%feedData,1);
         %feedDate = getField(%feedData,2);
         %feedSubject = getField(%feedData,3);
         %feedRead = getField(%feedData,4);
      }
      else
      {
         if(strPos(%line,"---") $= 0)
         {
            %file.readingMessage = 0;
            
            %feed = new ScriptObject()
            {
               class = RTBMM_NewsFeedItem;
               feed_id = %feedId;
               feed_type = %feedType;
               feed_date = %feedDate;
               feed_subject = %feedSubject;
               feed_message = %feedMessage;
               feed_read = %feedRead;
               feed_loaded = 1;
            };
            RTBMM_FeedControl.add(%feed);
         }
         else
         {
            %feedMessage = (%feedMessage $= "") ? %line : %feedMessage@"\n"@%line;
         }
      }
   }
   %file.close();
   %file.delete();
}
function RTBMM_FeedControl_reqSave(%this)
{
   if(isEventPending(%this.saveSchedule))
      cancel(%this.saveSchedule);
      
   %this.saveSchedule = schedule(1000,0,"RTBMM_FeedControl_Save",%this);
}
function RTBMM_FeedControl_Save(%this)
{
   %highestFeedID = %this.latestFeed;
   for(%i=0;%i<%this.getCount();%i++)
   {
      %feedId = %this.getObject(%i).feed_id;
      if(%feedId > %highestFeedID)
         %highestFeedID = %feedId;
   }
   
   %file = new FileObject();
   %file.openForWrite("config/client/RTB/newsFeed.feed");
   %file.writeLine(%highestFeedID);
   %file.writeLine("");
   
   for(%i=0;%i<%this.getCount();%i++)
   {
      %feed = %this.getObject(%i);
      %file.writeLine(%feed.feed_id@"|"@%feed.feed_type@"|"@%feed.feed_date@"|"@%feed.feed_subject@"|"@%feed.feed_read);
      %file.writeLine("--------------------------------------------------------");
      %file.writeLine(%feed.feed_message);
      %file.writeLine("--------------------------------------------------------");
      %file.writeLine("");
   }
   %file.close();
   %file.delete();
}

//*********************************************************
//* User Interaction with Files (Ratings/Comments/Reports)
//*********************************************************
function RTBMM_AddComment::onWake(%this)
{
   RTBMM_CommentText.setValue("");
}
function RTBMM_EditComment(%comment)
{
   if(!$RTB::CModManager::Session::LoggedIn)
      return;
      
   canvas.pushdialog(RTBMM_AddComment);
   RTBMM_AddComment.getObject(0).setText("Edit Comment");
   RTBMM_AddComment.getObject(0).getObject(0).command = "RTBMM_SendComment("@%comment@");";
   RTBMM_CommentText.setValue("Downloading Comment...");
   RTBMM_SendRequest("GETCOMMENT",5,%comment);
}
function RTBMM_Comments_onRetrieve(%this,%line)
{
   if(getField(%line,0) $= 1)
      RTBMM_CommentText.setValue(getField(%line,1));
   else if(getField(%line,0) $= "ERROR")
   {
      MessageBoxOK("Failure!",getField(%line,1));
      canvas.popDialog(RTBMM_AddComment);
   }
   else
   {
      MessageBoxOK("Failure!","Your Comment could not be retrieved due to a Server Error.");
      canvas.popDialog(RTBMM_AddComment);
   }
}
function RTBMM_SendComment(%commentID)
{
   if(!$RTB::CModManager::Session::LoggedIn)
      return;
      
   %comment = RTBMM_CommentText.getValue();
   if(%comment !$= "")
   {
      %file = $RTB::CModManager::Cache::CurrentFile;
      canvas.popDialog(RTBMM_AddComment);
      MessagePopup("Transmitting...","Your Comment is being Submitted...");
      RTBMM_SendRequest("ADDCOMMENT",5,%file,%comment,%commentID);
   }
   else
      MessageBoxOK("Ooops","You forgot to enter a Comment.");
}
function RTBMM_Comments_onSend(%this,%line)
{
   MessagePopup("","",1);
   if(getField(%line,0) $= 1)
      MessageBoxOK("Success!","Your comment was posted successfully.");
   else if(getField(%line,0) $= 2)
      MessageBoxOK("Success!","Your comment was edited successfully.");
   else if(getField(%line,0) $= "ERROR")
      MessageBoxOK("Failure!",getField(%line,1));
   else
      MessageBoxOK("Failure","Your Comment was not added due to a Server Error.");
      
   RTBMM_getFileView($RTB::CModManager::Cache::CurrentFile);
}
function RTBMM_DeleteComment(%comment)
{
   MessagePopup("Transmitting...","Your Comment is being Deleted...");
   RTBMM_SendRequest("DELETECOMMENT",5,%comment);
}
function RTBMM_Comments_onRemove(%this,%line)
{
   if(getField(%line,0) $= 1)
      MessageBoxOK("Success!","Your Comment has been removed.");
   else if(getField(%line,0) $= "ERROR")
      MessageBoxOK("Failure!",getField(%line,1));
   else
      MessageBoxOK("Failure!","Your Comment was not removed due to a Server Error.");
      
   RTBMM_getFileView($RTB::CModManager::Cache::CurrentFile);
}
function RTBMM_Ratings_onSend(%this,%line)
{
   MessagePopup("","",1);
   if(%line $= 1)
      MessageBoxOK("Success!","Your Rating has been added.");
   else if(%line $= 2)
      MessageBoxOK("Success!","Your Rating has been edited.");
   else if(%line $= 0)
      MessageBoxOK("Oops!","Your BL_ID ("@getNumKeyID()@") is not linked to an RTB Account, therefore you cannot Comment or Rate on Files.");
   else if(%line $= "ERROR")
      MessageBoxOK("Failure!",getField(%line,1));
   else
      MessageBoxOK("Failure!","Your Rating was not added due to a Server Error.");
   
   RTBMM_getFileView($RTB::CModManager::Cache::CurrentFile);
}

//*********************************************************
//* Download Queue Managing
//*********************************************************
function RTBMM_InitiateDownloadQueue()
{
   if(!isObject(RTBMM_DownloadManifest))
      new SimGroup(RTBMM_DownloadManifest);
      
   %dQueue = new ScriptObject(RTBMM_DownloadQueue)
   {
      class = RTBMM_DownloadQueueSO;
      totalDownloads = 0;
   };
   RTBMM_DownloadManifest.add(%dQueue);
      
   if(isFile("config/client/RTB/dlQueue.dat"))
   {
      %file = new FileObject();
      if(%file.openForRead("config/client/RTB/dlQueue.dat"))
      {
         while(!%file.isEOF())
         {
            %queuedFile = %file.readLine();
            %dQueue.pushFile(%queuedFile);
         }
      }
      %file.delete();
   }   
   RTBMM_InitiateFileConnection();
   
   return 1;
}
function RTBMM_DownloadQueueSO::hasFile(%this,%file)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      %fileID = %this.download[%i].file_id;
      if(%fileID $= %file)
         return 1;
   }
   return 0;
}
function RTBMM_DownloadQueueSO::pushFile(%this,%file)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
   if(%file $= "" || %file < 0 || !isint(%file))
      return 0;
      
   if(%this.hasFile(%file))
      return 0;
      
   %fileSO = new ScriptObject()
   {
      class = RTBMM_DownloadFileSO;
      file_id = %file;
   };
   %this.getGroup().add(%fileSO);
   %this.download[%this.totalDownloads] = %fileSO;
   %this.totalDownloads++;
   
   %this.export();
   
   %this.grabFileData();
   return 1;
}
function RTBMM_DownloadQueueSO::pushMusic(%this,%file,%index)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
   if(%file $= "" || %file < 0 || !isint(%file))
      return 0;
      
   %fileSO = new ScriptObject()
   {
      class = RTBMM_DownloadFileSO;
      file_id = %file@"-"@%index;
      file_music = 1;
   };
   %this.getGroup().add(%fileSO);
   %this.download[%this.totalDownloads] = %fileSO;
   %this.totalDownloads++;
   
   %this.export();
   
   %this.grabFileData();
   return 1;
}
function RTBMM_DownloadQueueSO::popFile(%this,%file)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
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
   
   %this.export();
   
   return 1;
}
function RTBMM_DownloadQueueSO::popIndex(%this,%index)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
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
   
   %this.export();
   
   return 1;
}
function RTBMM_DownloadQueueSO::dump(%this)
{
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
      
   echo("");
   echo("Download Queue SO: Dump ("@%this.totalDownloads@")");
   echo("***************************************");
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      echo(" "@%i@" - "@%this.download[%i]);
   }
   echo("");
}
function RTBMM_DownloadQueueSO::export(%this)
{
   %file = new FileObject();
   %file.openForWrite("config/client/RTB/dlQueue.dat");
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      %file.writeLine(%this.download[%i].file_id);
   }
   %file.close();
   %file.delete();
}
function RTBMM_DownloadQueueSO::grabFileData(%this)
{
   for(%i=0;%i<%this.totalDownloads;%i++)
   {
      if(%this.download[%i].liveData $= "")
      {
         if(%this.download[%i].file_music $= 1)
            %fileList = %fileList@"M"@%this.download[%i].file_id@",";
         else
            %fileList = %fileList@"F"@%this.download[%i].file_id@",";
      }
   }
   
   if(%fileList !$= "")
      RTBMM_SendRequest("GETFILEDATA",3,getSubStr(%fileList,0,strLen(%fileList)-1));
}
function RTBMM_DownloadFileSO::dump(%this)
{
   echo("");
   echo("Download File SO: Dump");
   echo("***************************************");
   echo(" file_id:      "@%this.file_id);
   echo(" file_name:    "@%this.file_name);
   echo(" file_author:  "@%this.file_author);
   echo(" file_version: "@%this.file_version);
   echo(" file_url:     "@%this.file_url);
   echo(" file_crc:     "@%this.file_crc);
   echo(" file_size:    "@%this.file_size);
   echo(" liveData:     "@%this.liveData);
   echo("");
}

//*********************************************************
//* View of current downloads queue
//*********************************************************
function RTBMM_getDownloadsView()
{
	RTBMM_ConstructWindow();
	RTBMM_DownloadsView_createHeader();
   if(!isObject(RTBMM_DownloadQueue))
      RTBMM_InitiateDownloadQueue();
	
	RTBMM_trackZone("downloadview","RTBMM_getDownloadsView();");
   RTBMM_DownloadsView_DrawQueue();
   RTBMM_DownloadQueue.grabFileData();
   
   RTBMM_InitiateFileConnection();
   
   if(RTBMM_DownloadQueue.totalDownloads $= 0)
      RTBMM_createCenterMessage("You do not have any Downloads.");
   
   RTBMM_createFooter();
}
function RTBMM_DownloadsView_createHeader()
{
	%bitmap = new GuiBitmapCtrl(MODDownloadHeader)
	{
		offset = "1 0";
		extent = "318 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "137 3";
		extent = "51 21";
		text = "\c1File Title";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "320 0";
		extent = "101 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "35 3";
		extent = "30 21";
		text = "\c1Stats";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "422 0";
		extent = "75 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "16 3";
		extent = "45 21";
		text = "\c1Options";
		profile = "RTBMM_MainMiddleText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_DownloadsView_drawQueue()
{
   for(%i=0;%i<RTBMM_DownloadQueue.totalDownloads;%i++)
   {
      %queuePosition = %i+1;
      %fileID = RTBMM_DownloadQueue.download[%i].file_id;
      %hasData = RTBMM_DownloadQueue.download[%i].liveData;
      %swatch = new GuiSwatchCtrl()
      {
         offset = "1 0";
         extent = "25 46";
         color = "200 200 200 255";
      };
      %textPos = new GuiTextCtrl()
      {
         position = "4 14";
         extent = "17 18";
         text = "\c0"@%queuePosition@":";
         profile = "BlockButtonProfile";
      };
      %swatch.add(%textPos);
      RTBMM_PushWindowElement(%swatch);

      %swatch = new GuiSwatchCtrl()
      {
         offset = "27 0";
         extent = "292 46";
         color = "235 235 235 255";
      };
      %text = new GuiTextCtrl("RTBMM_DownloadQueue_Title"@%queuePosition)
      {
         position = "2 0";
         extent = "327 18";
         text = "\c0File "@%fileID@" - Downloading Data";
         profile = "RTBMM_MainText";
      };
      %swatch.add(%text);
      %progress = new GuiProgressCtrl("RTBMM_DownloadQueue_Progress"@%queuePosition)
      {
         position = "2 19";
         extent = "286 24";
         profile = "GuiProgressProfile";
      };
      %swatch.add(%progress);

      %text = new GuiTextCtrl("RTBMM_DownloadQueue_Status"@%queuePosition)
      {
         position = "5 3";
         extent = "275 18";
         text = "\c0Connecting...";
         profile = "RTBMM_MiddleText";
      };
      %progress.add(%text);
      RTBMM_PushWindowElement(%swatch);

      %swatch = new GuiSwatchCtrl()
      {
         offset = "320 0";
         extent = "101 46";
         color = "235 235 235 255";
      };
      %text = new GuiTextCtrl("RTBMM_DownloadQueue_Size"@%queuePosition)
      {
         position = "3 1";
         extent = "95 18";
         text = "\c0Size: N/A";
         profile = "RTB_Verdana12Pt";
      };
      %swatch.add(%text);
      %text = new GuiTextCtrl("RTBMM_DownloadQueue_Done"@%queuePosition)
      {
         position = "3 14";
         extent = "95 18";
         text = "\c0Done: N/A";
         profile = "RTB_Verdana12Pt";
      };
      %swatch.add(%text);
      %text = new GuiTextCtrl("RTBMM_DownloadQueue_Speed"@%queuePosition)
      {
         position = "3 28";
         extent = "95 18";
         text = "\c0Speed: N/A";
         profile = "RTB_Verdana12Pt";
      };
      %swatch.add(%text);
      RTBMM_PushWindowElement(%swatch);

      %swatch = new GuiSwatchCtrl()
      {
         offset = "422 0";
         extent = "75 46";
         color = "230 230 230 255";
      };
      
      if(%queuePosition $= 1)
      {
         %button = new GuiBitmapButtonCtrl("RTBMM_DownloadQueue_ButtonA"@%queuePosition)
         {
            position = "8 3";
            extent = "59 18";
            text = "";
            bitmap = "./images/buttons/btnResume";
            profile = "BlockButtonProfile";
            mColor = "255 255 255 255";
            command = "RTBMM_FileConnection.promptDownload();";
         };
         %swatch.add(%button);
         
         %disableswatch = new GuiSwatchCtrl("RTBMM_DownloadQueue_ButtonABlock"@%queuePosition)
         {
            offset = "2 1";
            extent = "91 22";
            color = "230 230 230 150";
         };
      }
      else
      {
         %button = new GuiBitmapButtonCtrl("RTBMM_DownloadQueue_ButtonA"@%queuePosition)
         {
            position = "8 3";
            extent = "59 18";
            text = "";
            bitmap = "./images/buttons/btnMoveUp";
            profile = "BlockButtonProfile";
            mColor = "255 255 255 255";
            command = "RTBMM_DownloadsView_MoveFileUp("@%i@");";
         };
         %swatch.add(%button);
      }
      
      %button = new GuiBitmapButtonCtrl("RTBMM_DownloadQueue_ButtonB"@%queuePosition)
      {
         position = "8 22";
         extent = "59 18";
         text = "";
         command = "RTBMM_DownloadsView_RemoveDownload("@%i@");";
         bitmap = "./images/buttons/btnCancel";
         profile = "BlockButtonProfile";
         mColor = "255 255 255 255";
      };
      %swatch.add(%button);
      if(isObject(%disableswatch))
         %swatch.add(%disableswatch);
      RTBMM_PushWindowElement(%swatch,1);
      
      if(%hasData !$= "")
         RTBMM_DownloadsView_ApplyFileData(%fileID);
         
      if(RTBMM_DownloadQueue.download[%i].isDownloading)
      {
         ("RTBMM_DownloadQueue_ButtonA"@%queuePosition).setBitmap($RTB::Path@"images/buttons/btnStop");
         ("RTBMM_DownloadQueue_ButtonA"@%queuePosition).command = "RTBMM_FileConnection.stopDownload();";
      }
         
      if(RTBMM_DownloadQueue.download[%i].isArchive)
      {
         %progress.getObject(0).setText("Download Complete");
         %progress.setValue(1);
         %disableswatch.extent = "91 44";
         %disableswatch.setVisible(1);
         %size = "RTBMM_DownloadQueue_Size"@%queuePosition;
         %done = "RTBMM_DownloadQueue_Done"@%queuePosition;
         %size.setText("\c0Size: "@byteRound(RTBMM_DownloadQueue.download[%i].file_size));
         %done.setText("\c0Done: "@byteRound(RTBMM_DownloadQueue.download[%i].file_size));
      }
   }
}
function RTBMM_DownloadsView_retrieveFileData(%this,%line)
{
   //id-name-author-version-url-crc-size
   %fileID = getField(%line,0);
   for(%i=0;%i<RTBMM_DownloadQueue.totalDownloads;%i++)
   {
      if(RTBMM_DownloadQueue.download[%i].file_id $= %fileID)
      {
         if(RTBMM_DownloadQueue.download[%i].file_music)
         {
            RTBMM_DownloadQueue.download[%i].file_name = getField(%line,1);
            RTBMM_DownloadQueue.download[%i].file_size = getField(%line,2);
            RTBMM_DownloadQueue.download[%i].file_author = getField(%line,3);
         }
         else
         {
            RTBMM_DownloadQueue.download[%i].file_name = getField(%line,1);
            RTBMM_DownloadQueue.download[%i].file_author = getField(%line,2);
            RTBMM_DownloadQueue.download[%i].file_version = getField(%line,3);
            RTBMM_DownloadQueue.download[%i].file_url = getField(%line,4);
            RTBMM_DownloadQueue.download[%i].file_crc = getField(%line,5);
            RTBMM_DownloadQueue.download[%i].file_size = getField(%line,6);
            RTBMM_DownloadQueue.download[%i].file_savename = getField(%line,7);
         }
         RTBMM_DownloadQueue.download[%i].liveData = 1;
         
         if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
            RTBMM_DownloadsView_ApplyFileData(%fileID);
            
         RTBMM_FileConnection.schedule(1000,"promptDownload");
         return;
      }
   }
}
function RTBMM_DownloadsView_ApplyFileData(%fileID)
{
   for(%i=0;%i<RTBMM_DownloadQueue.totalDownloads;%i++)
   {
      if(RTBMM_DownloadQueue.download[%i].file_id $= %fileID)
      {
         %SO = RTBMM_DownloadQueue.download[%i];
         %i++;
         break;
      }
   }
   
   if(%SO.liveData $= "")
      return;
      
   %titleCtrl = "RTBMM_DownloadQueue_Title"@%i;
   %progressCtrl = "RTBMM_DownloadQueue_Progress"@%i;
   %statusCtrl = "RTBMM_DownloadQueue_Status"@%i;
   %sizeCtrl = "RTBMM_DownloadQueue_Size"@%i;
   %doneCtrl = "RTBMM_DownloadQueue_Done"@%i;
   %speedCtrl = "RTBMM_DownloadQueue_Speed"@%i;
   %buttonACtrl = "RTBMM_DownloadQueue_ButtonA"@%i;
   %buttonABCtrl = "RTBMM_DownloadQueue_ButtonABlock"@%i;
   %buttonBCtrl = "RTBMM_DownloadQueue_ButtonB"@%i;
      
   //Data like so:
   //name-author-version-url-crc-size
   %file_name = %SO.file_name;
   %file_author = %SO.file_author;
   %file_version = %SO.file_version;
   %file_url = %SO.file_url;
   %file_crc = %SO.file_crc;
   %file_size = %SO.file_size;
   %file_zip = %SO.file_savename;
   
   if(%file_name $= "0")
   {
      %titleCtrl.setText("File "@%fileID@" - File not Found");
      %sizeCtrl.setText("Size: N/A");
      %statusCtrl.setText("File cannot be Located");
      %doneCtrl.setText("Done: N/A");
   }
   else if(%file_name $= "2")
   {
      %titleCtrl.setText("File "@%fileID@" - Download Content Missing");
      %sizeCtrl.setText("Size: N/A");
      %statusCtrl.setText("File cannot be Located");
      %doneCtrl.setText("Done: N/A");
   }
   else if(isReadonly("Add-Ons/"@%file_zip))
   {
      %titleCtrl.setText(%file_name@" - ZIP is Readonly");
      %sizeCtrl.setText("Size: N/A");
      %statusCtrl.setText("Cannot overwrite existing "@%file_zip@"!");
      %doneCtrl.setText("Done: N/A");
      %SO.isArchive = 1;
      %SO.liveData = 0;
      %SO.file_name = 2;
   }
   else
   {
      if(%SO.file_music)
      {
         %titleCtrl.setText("Music File: "@%file_name@" by "@%file_author);
         %sizeCtrl.setText("Size: "@byteRound(%file_size));
         %statusCtrl.setText("Waiting to Download...");
         %doneCtrl.setText("Done: "@byteRound(0));
      }
      else
      {
         %titleCtrl.setText(%file_name@" v"@%file_version@" by "@%file_author);
         %sizeCtrl.setText("Size: "@byteRound(%file_size));
         %statusCtrl.setText("Waiting to Download...");
         %doneCtrl.setText("Done: "@byteRound(0));
      }
      
      if(%i $= 1)
         %buttonABCtrl.setVisible(0);
   }
}
function RTBMM_DownloadsView_MoveFileUp(%index,%conf)
{
   if(%index $= 0)
      return;
      
   if(%index >= RTBMM_DownloadQueue.totalDownloads)
      return;
      
   if(%index $= 1 && RTBMM_FileConnection.inTransfer)
   {
      if(!%conf)
      {
         MessageBoxYesNo("Oops...","Are you sure you want to move this file to the top of the Queue?\n\nThis will stop the current Download.","RTBMM_DownloadsView_MoveFileUp("@%index@",1);","");
         return;
      }
      
      RTBMM_FileConnection.stopDownload();
   }
      
   %tempDLW = RTBMM_DownloadQueue.download[%index-1];
   RTBMM_DownloadQueue.download[%index-1] = RTBMM_DownloadQueue.download[%index];
   RTBMM_DownloadQueue.download[%index] = %tempDLW;
   
   if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
      schedule(1,0,"RTBMM_getDownloadsView");
}
function RTBMM_DownloadsView_removeDownload(%index,%conf)
{
   if(!%conf)
   {
      MessageBoxYesNo("Really?","Are you sure you want to delete this file from your Downloads Queue?","RTBMM_DownloadsView_removeDownload("@%index@",1);","");
      return;
   }

   RTBMM_DownloadQueue.popIndex(%index);
   if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
      RTBMM_getDownloadsView();
}
function RTBMM_DownloadsView_addDownload(%file)
{
   if(RTBMM_DownloadQueue.pushFile(%file))
      MessageBoxOK("Huzzah!","This file has been added to your Download Queue.");
   else if(RTBMM_hasFile(%file))
   {
      %so = RTBMM_MODManifest.getEntryById(%file);
   }
   else
      MessageBoxYesNo("Failure!","It appears as though this file is already in your Download Queue...\n\nWould you like to see your Downloads?","RTBMM_getDownloadsView();","");
}
function RTBMM_DownloadsView_addMusic(%file,%index)
{
   if(RTBMM_DownloadQueue.pushMusic(%file,%index))
      MessageBoxOK("Huzzah!","This music has been added to your Download Queue.");
}
function RTBMM_DownloadsView_clearDownloadedFile(%file)
{
   if(RTBMM_DownloadQueue.download[0] $= %file)
   {
      RTBMM_DownloadQueue.popIndex(0);
      RTBMM_FileConnection.delete();
      RTBMM_InitiateFileConnection();
      
      if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
         RTBMM_getDownloadsView();
      
      RTBMM_FileConnection.promptDownload();
   }
}

//*********************************************************
//* View of existing downloaded mogs
//*********************************************************
function RTBMM_getModsView()
{
   RTBMM_ConstructWindow();
   RTBMM_trackZone("modview","RTBMM_getModsView();");
   RTBMM_ModsView_createHeader();
   
   if(!isObject(RTBMM_MODManifest))
      RTBMM_loadExistingRTBMods();
   
   if(RTBMM_MODManifest.getCount() >= 1)
   {
      for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
      {
         %SO = RTBMM_MODManifest.getObject(%i);
         RTBMM_ModsView_addMod(%SO);
      }
   }
   else
      RTBMM_createCenterMessage("You do not have any RTB-Downloaded Mods in your Add-Ons Folder.");
   
   RTBMM_createFooter();
}
function RTBMM_ModsView_addMod(%SO)
{
   %type = RTBMM_ModsView_addCategory(%SO.type);
   
   %categoryName = "RTBMM_ModCat_"@%type;
   %categoryName.extent = vectorAdd(%categoryName.extent,"0 41");
   %categoryName.newExtent = %categoryName.extent;
   
   %yHeight = 40*(%categoryName.getObject(0).getObject(1).catCount-1)+31+(%categoryName.getObject(0).getObject(1).catCount-1);
   %swatch = new GuiSwatchCtrl()
   {
      profile = GuiDefaultProfile;
      position = "0" SPC %yHeight;
      extent = "25 40";
      color = "220 220 220 255";
      
      new GuiBitmapCtrl()
      {
         profile = GuiDefaultProfile;
         vertSizing = "center";
         horizSizing = "center";
         position = "0 0";
         extent = "16 16";
         bitmap = "./images/icons/"@%SO.icon;
      };
   };
   %categoryName.add(%swatch);
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = GuiDefaultProfile;
      position = "26" SPC %yHeight;
      extent = "299 40";
      color = "220 220 220 255";
      
      new GuiTextCtrl()
      {
         profile = RTBMM_MainText;
         position = "3 1";
         extent = "284 18";
         text = %SO.name SPC "\c1v"@%SO.version;
      };
      
      new GuiTextCtrl()
      {
         profile = RTB_Verdana12Pt;
         position = "4 16";
         extent = "300 18";
         text = "by" SPC %SO.author;
      };
   };
   %categoryName.add(%swatch);
   
   if(%SO.isClientside)
   {
      %txt = "\c1Clientside";
      %swatchColor = "200 200 200 255";
      %categoryName.numEnabled++;
   }
   else if(%SO.isMap)
   {
      %txt = "\c1Map";
      %swatchColor = "200 200 200 255";
      %categoryName.numEnabled++;
   }
   else if($AddOn__[%SO.variableName] $= 1)
   {
      %txt = "Enabled";
      %swatchColor = "150 255 150 255";
      %categoryName.numEnabled++;
   }
   else
   {
      %txt = "Disabled";
      %swatchColor = "255 150 150 255";
   }
   %categoryName.getObject(0).getObject(1).setText("\c1"@%categoryName.catName@"  ("@%categoryName.numEnabled@"/"@%categoryName.getObject(0).getObject(1).catCount@" )");
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = GuiDefaultProfile;
      position = "326" SPC %yHeight;
      extent = "104 40";
      color = %swatchColor;
   };
   %categoryName.add(%swatch);

   %statusText = new GuiTextCtrl()
   {
      profile = RTBMM_MainMiddleText;
      position = "1 1";
      vertSizing = "center";
      extent = "103 18";
      text = %txt;
   };
   %swatch.add(%statusText);
   
   %swatch = new GuiSwatchCtrl()
   {
      profile = GuiDefaultProfile;
      position = "431" SPC %yHeight;
      extent = "74 40";
      color = "220 220 220 255";
      
      new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "3 20";
         vertSizing = "bottom";
         extent = "59 18";
         text = "\c1";
         bitmap = "./images/buttons/btnView";
         command = "RTBMM_getFileView("@%SO.id@");";
      };
   };
   %categoryName.add(%swatch);

   if(!%SO.isClientside && !%SO.isMap)
   {
      %buttonDisable = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "3 0";
         vertSizing = "top";
         extent = "59 18";
         text = "\c1";
         bitmap = "./images/buttons/btnDisable";
         command = "RTBMM_ModsView_DisableMod("@%SO@");";
      };
      
      %buttonEnable = new GuiBitmapButtonCtrl()
      {
         profile = GuiDefaultProfile;
         position = "3 0";
         vertSizing = "top";
         extent = "59 18";
         text = "\c1";
         bitmap = "./images/buttons/btnEnable";
         command = "RTBMM_ModsView_EnableMod("@%SO@");";
      };
      
      if($AddOn__[%SO.variableName] $= 1)
         %buttonEnable.setVisible(0);
      else
         %buttonDisable.setVisible(0);

      %swatch.add(%buttonEnable);
      %swatch.add(%buttonDisable);
      
      %SO.gui_buttonEnable = %buttonEnable;
      %SO.gui_buttonDisable = %buttonDisable;
   }
   
   %SO.gui_category = %categoryName;
   %SO.gui_statusBG = %statusText.getGroup();
   %SO.gui_statusText = %statusText;
   
	$RTB::CModManager::GUI::YSpacing = 55;
	for(%i=5;%i<RTBMM_WindowSwatch.getCount();%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      RTBMM_PushWindowElement(%obj,1);
	}
}
function RTBMM_ModsView_EnableMod(%SO)
{
   if($AddOn__[%SO.variableName] $= 1)
      return;
      
   %SO.gui_buttonDisable.setVisible(1);
   %SO.gui_buttonEnable.setVisible(0);
   %SO.gui_category.numEnabled++;   
   %SO.gui_category.getObject(0).getObject(1).setText("\c1"@%SO.gui_category.catName@"  ("@%SO.gui_category.numEnabled@"/"@%SO.gui_category.getObject(0).getObject(1).catCount@" )");
   %SO.gui_statusBG.color = "150 255 150 255";
   %SO.gui_statusText.setText("Enabled");
   
   $AddOn__[%SO.variableName] = 1;
   export("$AddOn__*","config/server/ADD_ON_LIST.cs");
}
function RTBMM_ModsView_DisableMod(%SO)
{
   if($AddOn__[%SO.variableName] !$= 1)
      return;
      
   %SO.gui_buttonDisable.setVisible(0);
   %SO.gui_buttonEnable.setVisible(1);
   %SO.gui_category.numEnabled--;   
   %SO.gui_category.getObject(0).getObject(1).setText("\c1"@%SO.gui_category.catName@"  ("@%SO.gui_category.numEnabled@"/"@%SO.gui_category.getObject(0).getObject(1).catCount@" )");
   %SO.gui_statusBG.color = "255 150 150 255";
   %SO.gui_statusText.setText("Disabled");
   
   $AddOn__[%SO.variableName] = 0;
   export("$AddOn__*","config/server/ADD_ON_LIST.cs");
}
function RTBMM_ModsView_collapseCategory(%name)
{
   %categoryName = "RTBMM_ModCat_"@%name;
   %categoryName.extent = "496 30";
   %categoryName.getObject(0).getObject(0).command = "RTBMM_ModsView_expandCategory(\""@%name@"\");";
   %categoryName.getObject(0).getObject(0).setBitmap($RTB::Path@"images/buttons/bullet_plus");
   
	$RTB::CModManager::GUI::YSpacing = 55;
	for(%i=5;%i<RTBMM_WindowSwatch.getCount();%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      RTBMM_PushWindowElement(%obj,1);
	}
}
function RTBMM_ModsView_expandCategory(%name)
{
   %categoryName = "RTBMM_ModCat_"@%name;
   %categoryName.extent = %categoryName.newExtent;
   %categoryName.getObject(0).getObject(0).command = "RTBMM_ModsView_collapseCategory(\""@%name@"\");";
   %categoryName.getObject(0).getObject(0).setBitmap($RTB::Path@"images/buttons/bullet_minus");
   
	$RTB::CModManager::GUI::YSpacing = 55;
	for(%i=5;%i<RTBMM_WindowSwatch.getCount();%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      RTBMM_PushWindowElement(%obj,1);
	}
}
function RTBMM_ModsView_addCategory(%name)
{
   if(%name $= "")
      %name = "Unsorted";
      
   %categoryName = "RTBMM_ModCat_"@%name;
   if(isObject(%categoryName))
   {
      %txt = %categoryName.getObject(0).getObject(1);
      %currNum = %txt.catCount++;
      %txt.setText("\c1"@%name@"  ("@%currNum@" )");
      return %name;
   }
   
   %swatch = new GuiSwatchCtrl(%categoryName)
   {
      offset = "1 0";
      extent = "496 30";
      color = "255 0 0 0";
      
      catName = %name;
      numEnabled = 0;
   };
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "496 30";
		bitmap = "./images/cellpic3";
	};
   %swatch.add(%bitmap);
	%toggle = new GuiBitmapButtonCtrl()
	{
		position = "1 8";
		extent = "16 16";
		bitmap = "./images/buttons/bullet_minus";
		text = " ";
		command = "RTBMM_ModsView_collapseCategory(\""@%name@"\");";
	};
	%bitmap.add(%toggle);
	%text = new GuiTextCtrl()
	{
		profile = RTBMM_MainText;
		position = "18 4";
		horizSizing = "right";
		vertsizing = "center";
		extent = "430 22";
		text = "\c1"@%name@"  (1/1 )";
		catCount = 1;
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%swatch,1);
	
	for(%i=1;%i<RTBMM_WindowSwatch.getCount();%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      if(%obj $= %swatch)
         continue;
         
      if(alphaCompare(%obj.catName,%name) $= 2)
      {
         %numPushBack++;
         %pushBack[%numPushBack] = %obj;
      }
	}
	for(%i=1;%i<%numPushBack+1;%i++)
	{
	   RTBMM_WindowSwatch.pushToBack(%pushBack[%i]);
	}
	
	return %name;
}
function RTBMM_ModsView_createHeader()
{
   %swatch = new GuiSwatchCtrl()
   {
      offset = "1 0";
      extent = "496 25";
      color = "220 220 220 255";
   };
   %sync = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "3 2";
      extent = "130 18";
      text = "\c1";
      bitmap = "./images/buttons/btnSyncMods";
      command = "RTBMM_ModsView_syncNonRTB();";
   };
   %swatch.add(%sync);
   %expand = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "166 3";
      extent = "68 18";
      text = "\c1";
      bitmap = "./images/buttons/btnExpand";
      command = "RTBMM_ModsView_expandAll();";
   };
   %swatch.add(%expand);
   %collapse = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "236 3";
      extent = "68 18";
      text = "\c1";
      bitmap = "./images/buttons/btnCollapse";
      command = "RTBMM_ModsView_collapseAll();";
   };
   %swatch.add(%collapse);
   %enable = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "307 2";
      extent = "59 18";
      text = "\c1";
      bitmap = "./images/buttons/btnEnable";
      command = "RTBMM_ModsView_enableAll();";
   };
   %swatch.add(%enable);
   %disable = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "370 2";
      extent = "59 18";
      text = "\c1";
      bitmap = "./images/buttons/btnDisable";
      command = "RTBMM_ModsView_disableAll();";
   };
   %swatch.add(%disable);
   %update = new GuiBitmapButtonCtrl()
   {
      profile = GuiDefaultProfile;
      position = "433 2";
      extent = "59 18";
      text = "\c1";
      bitmap = "./images/buttons/btnUpdate";
      command = "RTBMM_ModsView_checkForUpdates();";
   };
   %swatch.add(%update);
   RTBMM_PushWindowElement(%swatch,1);
   
	%bitmap = new GuiBitmapCtrl()
	{
		offset = "1 0";
		extent = "325 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "116 3";
		extent = "97 21";
		text = "\c1MOD Information";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "327 0";
		extent = "104 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "35 3";
		extent = "47 21";
		text = "\c1Status";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap);

	%bitmap = new GuiBitmapCtrl()
	{
		offset = "432 0";
		extent = "72 27";
		bitmap = "./images/cellpic3";
	};
	%text = new GuiTextCtrl()
	{
		position = "10 3";
		extent = "47 21";
		text = "\c1Options";
		profile = "RTBMM_MainText";
	};
	%bitmap.add(%text);
	RTBMM_PushWindowElement(%bitmap,1);
}
function RTBMM_ModsView_downloadUpdates()
{
   for(%i=0;%i<RTBMM_UpdateScroll.getObject(0).getCount();%i++)
   {
      %obj = RTBMM_UpdateScroll.getObject(0).getObject(%i);
      if(%obj.getObject(3).getObject(0).getValue() $= 1)
      {
         if(RTBMM_DownloadQueue.pushFile(%obj.getObject(3).getObject(0).file_id))
            %numAdded++;
         %numAttempted++;
      }
   }
   
   if(%numAttempted)
   {
      if(%numAdded)
      {
         MessageBoxYesNo("Huzzah","A total of "@%numAdded@" files have been added to your downloads queue.\n\nWould you like to view it now?","RTBMM_OpenDirect();RTBMM_getDownloadsView();","");
         canvas.popDialog(RTBMM_Updates);
      }
      else
         MessageBoxOK("Oh Dear","Looks like all the updates failed. You'd better report this.");
   }
   else
      MessageBoxOK("Ooops","You haven't selected any updates to download.");
}
function RTBMM_ModsView_checkForUpdates(%silent)
{
   $RTB::CModManager::Cache::SilentUpdate = %silent;
   if(!isObject(RTBMM_MODManifest))
      RTBMM_loadExistingRTBMods();
      
   if(RTBMM_MODManifest.getCount() >= 1)
   {
      if(!%silent)
         MessagePopup("Searching for Updates...","Please wait while updates are located for the Add-Ons you have...");
      for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
      {
         %SO = RTBMM_MODManifest.getObject(%i);
         %delimList = (%delimList $= "") ? %SO.id : %delimList@","@%SO.id;
      }
   }
   else
   {
      if(!%silent)
         MessageBoxOK("Whoops","You don't have any Add-Ons to update.");
      return;
   }
   
   RTBMM_SendRequest("GETFILEUPDATES",6,%delimList);
}
function RTBMM_ModsView_retrieveUpdates(%this,%line)
{
   MessagePopup("","",0);
   if(getField(%line,0) >= 1)
   {
      canvas.pushDialog(RTBMM_Updates);
      RTBMM_UpdateScroll.clear();
      
      %s = new GuiSwatchCtrl()
      {
         position = "0 0";
         extent = "338 2";
         color = "0 0 0 0";
      };
      RTBMM_UpdateScroll.add(%s);
      
      %idString = strReplace(getField(%line,1),",","\t");
      for(%i=0;%i<getFieldCount(%idString);%i++)
      {
         %idL = getField(%idString,%i);
         %idL = strReplace(%idL,"-","\t");
         RTBMM_ModsView_PushUpdate(getField(%idL,0),getField(%idL,1),getField(%idL,2));
      }
   }
   else if(getField(%line,0) $= 0)
   {
      if(!$RTB::CModManager::Cache::SilentUpdate)
         MessageBoxOK("No Updates Found!","No updates were found for the Add-Ons you have.");
   }
   else
   {
      if(!$RTB::CModManager::Cache::SilentUpdate)
         MessageBoxOK("Failure!","Updates could not be found due to a Server Error.");
   }
}
function RTBMM_ModsView_PushUpdate(%oldID,%newID,%newVers)
{
   for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
   {
      %SO = RTBMM_MODManifest.getObject(%i);
      if(%SO.id $= %oldID)
         break;
   }
   
   %s = new GuiSwatchCtrl()
   {
      position = "1" SPC 30*RTBMM_UpdateScroll.getObject(0).getCount()+(RTBMM_UpdateScroll.getObject(0).getCount()+1);
      extent = "336 30";
      color = "0 0 0 0";
      
      new GuiSwatchCtrl()
      {
         position = "0 0";
         extent = "30 30";
         color = "230 230 230 255";
         
         new GuiBitmapCtrl()
         {
            position = "7 7";
            extent = "16 16";
            bitmap = "./images/icons/"@%SO.icon;
         };
      };
      
      new GuiSwatchCtrl()
      {
         position = "31 0";
         extent = "223 30";
         color = "200 200 200 255";
         
         new GuiTextCtrl()
         {
            profile = RTBMM_MainText;
            position = "5 6";
            vertSizing = "center";
            extent = "284 18";
            text = "\c1"@%SO.title;
         };
      };

      new GuiSwatchCtrl()
      {
         position = "255 0";
         extent = "50 30";
         color = "220 220 220 255";
         
         new GuiTextCtrl()
         {
            profile = RTB_Verdana12PtCenter;
            position = "0 6";
            vertSizing = "center";
            extent = "50 18";
            text = "\c1v"@%newVers;
         };
      };
      
      new GuiSwatchCtrl()
      {
         position = "306 0";
         extent = "30 30";
         color = "200 200 200 255";
         
         new GuiCheckBoxCtrl()
         {
            position = "8 1";
            text = " ";
            file_id = %newID;
         };
      };
   };
   RTBMM_UpdateScroll.getObject(0).add(%s);
   %s.getObject(3).getObject(0).setValue(1);
   
   %bottom = getWord(%s.position,1)+31;
   RTBMM_UpdateScroll.getObject(0).extent = "338" SPC %bottom;
   RTBMM_UpdateScroll.scrollToTop();
}
function RTBMM_ModsView_syncNonRTB()
{
   clientUpdateAddOnsList();
   
	%mod = FindFirstFile("Add-Ons/*_*/description.txt");
	while(strLen(%mod) > 0)
	{
	   %modName = getSubStr(%mod,8,strLen(%mod)-24);
	   %modVarName = getSafeVariableName(%modName);

      if(!isFile("Add-Ons/"@%modName@"/rtbInfo.txt") && (isFile("Add-Ons/"@%modName@"/server.cs") || isFile("Add-Ons/"@%modName@"/client.cs")))
      {
         %isDefault = 0;
         for(%i=0;%i<$RTB::CModManager::DefaultBLMods+1;%i++)
         {
            if($RTB::CModManager::DefaultBLMod[%i] $= %modName)
            {
               %isDefault = 1;
               break;
            }
         }
         
         if(!%isDefault)
            %delimitedMods = (%delimitedMods $= "") ? %modName : %delimitedMods@","@%modName;
      }
		%mod = FindNextFile("Add-Ons/*_*/description.txt");
	}
	
	%mod = FindFirstFile("Add-Ons/*_*/*.mis");
	while(strLen(%mod) > 0)
	{
	   %parts = strReplace(%mod,"/","\t");
	   %modName = getField(%parts,1);
	   %modVarName = getSafeVariableName(%modName);

      if(!isFile("Add-Ons/"@%modName@"/rtbInfo.txt"))
      {
         %isDefault = 0;
         for(%i=0;%i<$RTB::CModManager::DefaultBLMods+1;%i++)
         {
            if($RTB::CModManager::DefaultBLMod[%i] $= %modName)
            {
               %isDefault = 1;
               break;
            }
         }
         
         if(!%isDefault)
            %delimitedMods = (%delimitedMods $= "") ? %modName : %delimitedMods@","@%modName;
      }
		%mod = FindNextFile("Add-Ons/*_*/*.mis");
	}
	
	if(%delimitedMods $= "")
	{
	   MessageBoxOK("Phew","Looks like you don't have any Add-Ons that weren't downloaded through RTB!");
	}
	else
	{
      canvas.pushDialog(RTBMM_SyncMods);
      RTBMM_SyncText.setText("Attempting to match your non-rtb add-ons with ones in the RTB system...");
      RTBMM_SyncScroll.clear();
      RTBMM_SyncButton.setVisible(0);
      
      %s = new GuiSwatchCtrl()
      {
         position = "0 0";
         extent = "384 2";
         color = "255 255 255 255";
      };
      RTBMM_SyncScroll.add(%s);
      
      %swatch = new GuiSwatchCtrl(RTBMM_SyncLoading)
      {
         position = "0 0";
         extent = "384 187";
         color = "235 235 235 255";
         
         new GuiBitmapCtrl()
         {
            position = "186 66";
            extent = "31 31";
            bitmap = "./images/loadRing";
         };
         
         new GuiMLTextCtrl()
         {
            position = "161 104";
            extent = "83 12";
            profile = RTB_Verdana12Pt;
            text = "<color:555555>Synchronising...";
         };
      };
      RTBMM_SyncScroll.add(%swatch);
      RTBMM_createLoadingRing(%swatch.getObject(0));
      
      RTBMM_SendRequest("SYNCMODS",2,%delimitedMods);
	}
	
	if($RTB::Debug)
	   echo("\c4Delimited Mods for Sync: "@%delimitedMods);
}
function RTBMM_ModsView_syncResults(%this,%line)
{
   if(getField(%line,0) $= 1)
   {
      %id = getField(%line,1);
      %title = getField(%line,2);
      %version = getField(%line,3);
      %icon = getField(%line,4);
      %zipname = getField(%line,5);
      RTBMM_ModsView_PushSync(%id,%title,%version,%icon,%zipname,1);
   }
   else
   {
      %zipname = getField(%line,1);
      RTBMM_ModsView_PushSync(%id,%title,%version,%icon,%zipname,0);
   }
}
function RTBMM_ModsView_PushSync(%id,%title,%version,%icon,%zipname,%found)
{
   if(isObject(RTBMM_SyncLoading))
      RTBMM_SyncLoading.delete();
      
   if(%found)
   {
      %s = new GuiSwatchCtrl()
      {
         position = "1" SPC 30*RTBMM_SyncScroll.getObject(0).getCount()+(RTBMM_SyncScroll.getObject(0).getCount()+1);
         extent = "384 30";
         color = "0 0 0 0";
         
         new GuiSwatchCtrl()
         {
            position = "0 0";
            extent = "30 30";
            color = "230 230 230 255";
            
            new GuiBitmapCtrl()
            {
               position = "7 7";
               extent = "16 16";
               bitmap = "./images/icons/"@%icon;
            };
         };
         
         new GuiSwatchCtrl()
         {
            position = "31 0";
            extent = "291 30";
            color = "200 200 200 255";
            
            new GuiTextCtrl()
            {
               profile = RTBMM_MainText;
               position = "5 0";
               extent = "284 18";
               text = "\c1"@%title;
            };
            
            new GuiTextCtrl()
            {
               profile = RTB_Verdana12Pt;
               position = "5 13";
               extent = "284 18";
               text = "\c1Add-Ons/"@%zipname;
            };
         };

         new GuiSwatchCtrl()
         {
            position = "313 0";
            extent = "40 30";
            color = "220 220 220 255";
            
            new GuiTextCtrl()
            {
               profile = RTB_Verdana12PtCenter;
               position = "0 6";
               vertSizing = "center";
               extent = "40 18";
               text = "\c1v"@%version;
            };
         };
         
         new GuiSwatchCtrl()
         {
            position = "354 0";
            extent = "30 30";
            color = "200 200 200 255";
            
            new GuiCheckBoxCtrl()
            {
               position = "8 1";
               text = " ";
               file_id = %id;
            };
         };
      };
      RTBMM_SyncScroll.getObject(0).add(%s);
      %s.getObject(3).getObject(0).setValue(1);
   }
   else
   {
      %s = new GuiSwatchCtrl()
      {
         position = "1" SPC 30*RTBMM_SyncScroll.getObject(0).getCount()+(RTBMM_SyncScroll.getObject(0).getCount()+1);
         extent = "384 30";
         color = "0 0 0 0";
         
         new GuiSwatchCtrl()
         {
            position = "0 0";
            extent = "30 30";
            color = "230 230 230 255";
            
            new GuiBitmapCtrl()
            {
               position = "7 7";
               extent = "16 16";
               bitmap = "./images/smallbllogo";
            };
         };
         
         new GuiSwatchCtrl()
         {
            position = "31 0";
            extent = "363 30";
            color = "200 200 200 255";
            
            new GuiTextCtrl()
            {
               profile = RTBMM_MainText;
               position = "5 0";
               extent = "284 18";
               text = "\c1"@%zipname;
            };
            
            new GuiTextCtrl()
            {
               profile = RTB_Verdana12Pt;
               position = "5 13";
               extent = "284 18";
               text = "Could not locate this file on RTB.";
            };
         };
      };
      RTBMM_SyncScroll.getObject(0).add(%s);
      %s.getObject(3).getObject(0).setValue(1);
   }
   
   %bottom = getWord(%s.position,1)+31;
   RTBMM_SyncScroll.getObject(0).resize(getWord(RTBMM_SyncScroll.getObject(0).position,0),getWord(RTBMM_SyncScroll.getObject(0).position,1),384,%bottom);
   RTBMM_SyncScroll.scrollToTop();
}
function RTBMM_ModsView_downloadSyncs(%conf)
{
   if(!%conf)
   {
      MessageBoxYesNo("Hmm","Are you sure you want to synchronise your add-ons?\n\nThis will download the add-ons listed above and overwrite the old non-rtb ones.","RTBMM_ModsView_downloadSyncs(1);","");
      return;
   }
   
   for(%i=0;%i<RTBMM_SyncScroll.getObject(0).getCount();%i++)
   {
      %obj = RTBMM_SyncScroll.getObject(0).getObject(%i);
      if(%obj.getObject(3).getObject(0).getValue() $= 1)
      {
         if(RTBMM_DownloadQueue.pushFile(%obj.getObject(3).getObject(0).file_id))
            %numAdded++;
         %numAttempted++;
      }
   }
   
   if(%numAttempted)
   {
      if(%numAdded)
         MessageBoxYesNo("Huzzah","A total of "@%numAdded@" files have been added to your downloads queue.\n\nWould you like to view it now?","RTBMM_getDownloadsView();","");
      else
         MessageBoxOK("Oh Dear","Looks like all the synchronizations failed. You'd better report this.");
      canvas.popDialog(RTBMM_SyncMods);
   }
   else
      MessageBoxOK("Ooops","You haven't selected any add-ons to synchronise.");
}
function RTBMM_ModsView_endSyncResults(%this,%line)
{
   %numSynced = getField(%line,0);
   %total = getField(%line,1);
   if(%numSynced $= 0)
   {
      MessageBoxOK("Oh Dear","RTB was unable to sync any of your "@%total@" non-rtb mod(s).");
   }
   else
   {
      MessageBoxOK("Woo","RTB managed to find positive matches for "@%numSynced@"/"@%total@" of your non-rtb mod(s).\n\nPlease make sure they are the correct ones before ticking them for Synchronization!");
      RTBMM_SyncButton.setVisible(1);
      RTBMM_SyncText.setText("RTB has found matches for "@%numSynced@"/"@%total@" of your non-rtb mod(s):");
   }
}
function RTBMM_ModsView_failSyncResults(%this,%line)
{
   canvas.popDialog(RTBMM_SyncMods);
   MessageBoxOK("Ooops","The RTB System could not be connected to, please try again later.");
}
function RTBMM_ModsView_expandAll()
{
	for(%i=5;%i<RTBMM_WindowSwatch.getCount()-1;%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      RTBMM_ModsView_expandCategory(%obj.catName);
	}
}
function RTBMM_ModsView_collapseAll()
{
	for(%i=5;%i<RTBMM_WindowSwatch.getCount()-1;%i++)
	{
      %obj = RTBMM_WindowSwatch.getObject(%i);
      RTBMM_ModsView_collapseCategory(%obj.catName);
	}
}
function RTBMM_ModsView_enableAll()
{
   for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
   {
      %SO = RTBMM_MODManifest.getObject(%i);
      if(!%SO.isClientside && !%SO.isMap)
         RTBMM_ModsView_EnableMod(%SO);
   }
}
function RTBMM_ModsView_disableAll()
{
   for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
   {
      %SO = RTBMM_MODManifest.getObject(%i);
      if(!%SO.isClientside && !%SO.isMap)
         RTBMM_ModsView_DisableMod(%SO);
   }
}
function RTBMM_SyncMods::onSleep(%this)
{
   if(isObject(RTBMM_SyncLoading))
      RTBMM_SyncLoading.delete();
}

//*********************************************************
//* File Downloading Connection and Handling
//*********************************************************
function RTBMM_InitiateFileConnection()
{
   if(isObject(RTBMM_FileConnection))
      return;
      
   %connection = new TCPObject(RTBMM_FileConnection);
}
function RTBMM_FileConnection::onConnected(%this)
{
   if(!%this.SO.liveData || %this.SO.isArchive)
   {
      %this.disconnect();
      return;
   }
   
   %this.SO.isDownloading = 1;
   
   if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
   {
      RTBMM_DownloadQueue_ButtonA1.setBitmap($RTB::Path@"images/buttons/btnStop");
      RTBMM_DownloadQueue_ButtonA1.command = "RTBMM_FileConnection.stopDownload();";
      RTBMM_DownloadQueue_Status1.setText("Locating Download...");
   }
   
   %this.inTransfer = 1;
   %this.send("GET /blockland/rtbModServer.php?c=GETFILEDL&n="@urlEnc($Pref::Player::NetName)@"&arg1="@%this.SO.file_id@" HTTP/1.1\r\nHost: returntoblockland.com\r\nUser-Agent: Torque/1.0\r\n\r\n");
}
function RTBMM_FileConnection::onConnectFailed(%this)
{
}
function RTBMM_FileConnection::onDisconnect(%this)
{
   %this.setBinarySize(0);
}
function RTBMM_FileConnection::onBinChunk(%this,%bin)
{
   if(%this.timeStarted $= "")
      %this.timeStarted = getSimTime();
      
   if(%bin $= %this.fileSize)
      schedule(3000,0,"RTBMM_DownloadsView_ClearDownloadedFile",%this.SO);
      
   if(%bin >= %this.fileSize)
   {
      if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
      {
         RTBMM_DownloadQueue_Status1.setText("Download Complete");
         RTBMM_DownloadQueue_ButtonABlock1.extent = "91 44";
         RTBMM_DownloadQueue_ButtonABlock1.setVisible(1);
      }
      
      %this.SO.isArchive = 1;
      %filename = %this.fileName;

      %fo = new FileObject();
      %fo.openForWrite("Add-Ons/"@%filename);
      %fo.delete();

      if(%this.fileType $= "zip")
         %this.saveBufferToFile("Add-Ons/"@%filename);
      else
         %this.saveBufferToFile("Add-Ons/Music/"@%filename);
      
      %this.disconnect();
      setModPaths(getModPaths());
      RTBMM_loadExistingRTBMods();
      
      if(%this.fileType $= "zip")
      {
         if(isFile("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/client.cs") && isFile("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/description.txt"))
         {
            exec("Add-Ons/"@getSubStr(%filename,0,strLen(%filename)-4)@"/client.cs");
         }
      }
      buildIFLs();
   }
   else
   {
      if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
         RTBMM_DownloadQueue_Status1.setText("Downloading ("@mFloor((%bin/%this.fileSize)*100)@"%)...");
   }

   if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
   {
      RTBMM_DownloadQueue_Progress1.setValue(%bin/%this.fileSize);
      RTBMM_DownloadQueue_Done1.setText("Done: "@byteRound(%bin));
      RTBMM_DownloadQueue_Speed1.setText("Speed: "@mFloatLength(%bin/(getSimTime()-%this.timeStarted),2)@"kb/s");
   }
}
function RTBMM_FileConnection::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      MessageBoxOK("Error!","There appears to be an error with this file, and the ZIP cannot be located.");
      return;
   }
   
   if(firstWord(%line) $= "Content-Type:")
   {
      if(getWord(%line,1) $= "application/download")
         %this.fileType = "zip";
      else if(getWord(%line,1) $= "application/ogg")
         %this.fileType = "ogg";
   }
   
   if(firstWord(%line) $= "Content-Length:")
      %this.fileSize = getWord(%line,1);
      
   if(firstWord(%line) $= "Content-Savename:")
      %this.fileName = getWord(%line,1);
      
   if(%line $= "")
      %this.setBinarySize(%this.fileSize);
      
   %this.lastLine = %line;
   %this.timeStarted = "";
}
function RTBMM_FileConnection::promptDownload(%this)
{
   if(%this.inTransfer)
      return;

   %SO = RTBMM_DownloadQueue.download[0];
   if(!%SO.liveData || %SO.isArchive || %SO.file_name $= 0 || %SO.file_name $= 2)
      return;
   
   %this.setBinary(0);
   %this.lastLine = "";
   %this.SO = %SO;
   %this.connect($RTB::CModManager::HostSite@":80");
}
function RTBMM_FileConnection::stopDownload(%this,%conf)
{
   if(!%conf)
   {
      MessageBoxYesNo("Really?","Are you sure you want to stop this download?","RTBMM_FileConnection.stopDownload(1);","");
      return;
   }
   %this.SO.isDownloading = 0;
   
   %this.delete();
   if($RTB::CModManager::Cache::CurrentZone $= "downloadview")
      RTBMM_getDownloadsView();
}

//*********************************************************
//* Screenshot Downloading and Handling
//*********************************************************
function RTBMM_InitiateScreenshotGrabber()
{
   if(isObject(RTBMM_ScreenshotGrabber))
      RTBMM_ScreenshotGrabber.delete();
      
   new TCPObject(RTBMM_ScreenshotGrabber);
}
function RTBMM_ScreenshotGrabber::getCollage(%this)
{
   %this.setBinary(0);
   %this.grabMode = "collage";
   %this.connect($RTB::CModManager::HostSite@":80");
}
function RTBMM_ScreenshotGrabber::getScreenshot(%this,%sspath)
{
   %this.setBinary(0);
   %this.screenshotPath = %sspath;
   %this.grabMode = "screenshot";
   %this.connect($RTB::CModManager::HostSite@":80");
}
function RTBMM_ScreenshotGrabber::onConnected(%this)
{
   if(%this.grabMode $= "collage")
      %this.send("GET "@$RTB::CModManager::Cache::FileCollage@" HTTP/1.1\r\nHost: "@$RTB::CModManager::HostSite@"\r\n\r\n");
   else if(%this.grabMode $= "screenshot")
      %this.send("GET "@%this.screenshotPath@" HTTP/1.1\r\nHost: "@$RTB::CModManager::HostSite@"\r\n\r\n");
}
function RTBMM_ScreenshotGrabber::onLine(%this,%line)
{
   if(strPos(%line,"404 Not Found") >= 0)
   {
      MessageBoxOK("Error!","The Screenshot Collage could not be located!");
      return;
   }
   
   if(strPos(%line,"Content-Length:") $= 0)
      %this.contentSize = getWord(%line,1);
   
   if(%line $= "")
      %this.setBinarySize(%this.contentSize);
   
   %this.lastLine = %line;
}
function RTBMM_ScreenshotGrabber::onBinChunk(%this,%chunk)
{
   if(%chunk >= %this.contentSize)
   {
      if(%this.grabMode $= "collage")
      {
         %this.saveBufferToFile("config/client/RTB/cache/screenCollage.png");
         
         %k=0;
         for(%i=0;%i<4;%i++)
         {
            %pres = getSubStr($RTB::CModManager::Cache::FileSSPresence,%i,1);
            if(%pres)
            {
               %ctrl = "RTBMM_FileThumb"@%k;
               if(isObject(%ctrl))
               {
                  %ctrl.setVisible(1);
                  if(isObject(%ctrl.loadSwatch))
                     %ctrl.loadSwatch.delete();
                  %ctrl.getObject(0).setBitmap("config/client/RTB/cache/screenCollage");
                  
                  if(%i $= 1)
                     %ctrl.getObject(0).position = "-112 0";
                  else if(%i $= 2)
                     %ctrl.getObject(0).position = "0 -84";
                  else if(%i $= 3)
                     %ctrl.getObject(0).position = "-112 -84";
                  %k++;
               }
            }
         }
      }
      else if(%this.grabMode $= "screenshot")
      {
         if(isObject(RTBMM_VS_Loader))
            RTBMM_VS_Loader.delete();
            
         if(RTBMM_ViewScreenshot.isAwake())
         {
            %this.saveBufferToFile("config/client/RTB/cache/screenshot.png");
            RTBMM_VS_Bitmap.setBitmap("config/client/RTB/cache/screenshot");
         }
      }
      %this.disconnect();
      RTBMM_InitiateScreenshotGrabber();
   }
}
function RTBMM_ScreenshotGrabber::onDisconnect(%this)
{
}
function RTBMM_PushScreenshotViewer(%file,%caption,%crc)
{
   if(isObject(RTBMM_VS_Loader))
      RTBMM_VS_Loader.delete();
   
   RTBMM_VS_Bitmap.setBitmap("");
   canvas.pushDialog(RTBMM_ViewScreenshot);
   RTBMM_VS_Caption.setText(%caption);
   
   if(%crc $= getFileCRC("config/client/RTB/cache/screenshot.png"))
   {
      RTBMM_VS_Bitmap.setBitmap("config/client/RTB/cache/screenshot");
      return;
   }
   
   %swatch = new GuiSwatchCtrl(RTBMM_VS_Loader)
   {
      position = "0 0";
      extent = "550 413";
      color = "235 235 235 255";
   };
   RTBMM_VS_Bitmap.add(%swatch);
   
   %ring = new GuiBitmapCtrl()
   {
      vertSizing = "center";
      horizSizing = "center";
      position = "259 191";
      extent = "31 31";
      bitmap = "./images/loadRing";
   };
   %swatch.add(%ring);
   RTBMM_createLoadingRing(%ring);
   
   RTBMM_ScreenshotGrabber.getScreenshot(%file);
}
function RTBMM_ViewScreenshot::onSleep(%this)
{
   if(isObject(RTBMM_VS_Loader))
      RTBMM_VS_Loader.delete();
   RTBMM_ScreenshotGrabber.delete();
   RTBMM_InitiateScreenshotGrabber();
}

//*********************************************************
//* New Add-On Window
//*********************************************************
function addonsGui::onWake()
{
   clientUpdateAddOnsList();
   
   %prefix["Brick"] = "Bricks";
   %prefix["Emote"] = "Emotes";
   %prefix["Event"] = "Events";
   %prefix["Gamemode"] = "Gamemodes";
   %prefix["Item"] = "Items";
   %prefix["Light"] = "Lights";
   %prefix["Particle"] = "Particles";
   %prefix["Player"] = "Players";
   %prefix["Print"] = "Prints";
   %prefix["Projectile"] = "Projectiles";
   %prefix["Script"] = "Scripts";
   %prefix["Server"] = "Server Mods";
   %prefix["Tool"] = "Tools";
   %prefix["Vehicle"] = "Vehicles";
   %prefix["Weapon"] = "Weapons";
   
   %AOG_CategoryCount = 0;
   AOG_Scroll.clear();
   %file = findFirstFile("Add-Ons/*_*/server.cs");
   while(strLen(%file) > 0)
   {
      %filename = getSubStr(%file,8,strLen(%file));
      %filename = getSubStr(%filename,0,strPos(%filename,"/"));
      //if(%filename $= "System_ReturnToBlockland" || !isFile("Add-Ons/"@%filename@"/description.txt"))
      if(!isFile("Add-Ons/"@%filename@"/description.txt"))
      {
         %file = findNextFile("Add-Ons/*_*/server.cs");
         continue;
      }
      %file_prefix = getSubStr(%filename,0,strPos(%filename,"_"));
      %new_prefix = %file_prefix;
      if(%prefix[%new_prefix] !$= "")
            %new_prefix = %prefix[%new_prefix];
      if(%AOG_hasCategory[%new_prefix])
      {
         %iC = %AOG_ItemCount[%AOG_CategoryCount-1];
         %AOG_Category[%AOG_CategoryCount-1,%iC] = %filename;
         %AOG_ItemCount[%AOG_CategoryCount-1]++;
      }
      else
      {
         %AOG_Category[%AOG_CategoryCount,0] = %new_prefix;
         %AOG_Category[%AOG_CategoryCount,1] = %filename;
         %AOG_ItemCount[%AOG_CategoryCount] = 2;
         %AOG_CategoryCount++;
         %AOG_hasCategory[%new_prefix] = 1;
      }
      %file = findNextFile("Add-Ons/*_*/server.cs");
   }
   
   %swatch = new GuiSwatchCtrl()
   {
      position = "0 0";
      extent = "261 1000";
      color = "0 0 0 0";
   };
   AOG_Scroll.add(%swatch);
   
   %AOG_nextPos = 1;
   for(%i=%AOG_CategoryCount-1;%i>-1;%i--)
   {
      %bg = new GuiSwatchCtrl()
      {
         position = "5" SPC %AOG_nextPos+2;
         extent = "13 13";
         color = "0 0 0 255";
      };
      %swatch.add(%bg);
      %AOG_CatCheck[%i] = new GuiCheckboxCtrl()
      {
         profile = GuiCheckBoxBoldProfile;
         position = "5" SPC %AOG_nextPos;
         extent = "256 18";
         text = " "@%AOG_Category[%i,0];
         category = %AOG_Category[%i,0];
      };
      %AOG_CatCheck[%i].command = "AOG_tickCategory("@%AOG_CatCheck[%i]@");";
      %swatch.add(%AOG_CatCheck[%i]);
      %AOG_nextPos += 18;
      %hr = new GuiSwatchCtrl()
      {
         position = "5" SPC %AOG_nextPos;
         extent = "256 2";
         color = "0 0 0 255";
      };
      %swatch.add(%hr);
      %AOG_nextPos += 5;
      for(%j=%AOG_ItemCount[%i]-1;%j>0;%j--)
      {
         %checkbox = new GuiCheckboxCtrl()
         {
            position = "5" SPC %AOG_nextPos;
            extent = "256 18";
            text = %AOG_Category[%i,%j];
            varName = getSafeVariableName(%AOG_Category[%i,%j]);
            parent = %AOG_CatCheck[%i];
         };
         %checkbox.command = "AOG_tickAddon("@%checkbox@");";
         %swatch.add(%checkbox);
         %AOG_CatCheck[%i].numChildren++;
         if($AddOn__[%checkbox.varName] $= 1)
         {
            %AOG_CatCheck[%i].numEnabled++;
            %checkbox.setValue(1);
         }
         %AOG_nextPos += 18;
         
         %childID = %AOG_CatCheck[%i].numChildren-1;
         %AOG_CatCheck[%i].child[%childID] = %checkbox;
      }
      
      if(%AOG_CatCheck[%i].numEnabled $= %AOG_CatCheck[%i].numChildren)
         %AOG_CatCheck[%i].setValue(1);
   }
   %swatch.resize(0,0,261,%AOG_nextPos);
}

function AOG_tickCategory(%checkbox)
{
   for(%i=0;%i<%checkbox.numChildren;%i++)
   {
      %child = %checkbox.child[%i];
      %child.setValue(%checkbox.getValue());
   }
   
   if(%checkbox.getValue() $= 1)
      %checkbox.numEnabled = %checkbox.numChildren;
   else
      %checkbox.numEnabled = 0;
}

function AOG_selectNone()
{
   for(%i=0;%i<AOG_Scroll.getObject(0).getCount();%i++)
   {
      %obj = AOG_Scroll.getObject(0).getObject(%i);
      if(%obj.getClassName() $= "GuiCheckboxCtrl" && %obj.varName !$= "System_ReturnToBlockland")
      {
         %obj.setValue(0);
         if(%obj.numChildren >= 1)
            %obj.numEnabled = 0;
      }
   }
}

function AOG_selectAll()
{
   for(%i=0;%i<AOG_Scroll.getObject(0).getCount();%i++)
   {
      %obj = AOG_Scroll.getObject(0).getObject(%i);
      if(%obj.getClassName() $= "GuiCheckboxCtrl")
      {
         %obj.setValue(1);
         if(%obj.numChildren >= 1)
            %obj.numEnabled = %obj.numChildren;
      }
   }
}

function AOG_selectDefault()
{
   AOG_selectNone();
   for(%i=0;%i<AOG_Scroll.getObject(0).getCount();%i++)
   {
      %obj = AOG_Scroll.getObject(0).getObject(%i);
      if(%obj.varName !$= "")
      {
         %isDefault = 0;
         for(%j=0;%j<$RTB::CModManager::DefaultBLMods;%j++)
         {
            if(%obj.varName $= $RTB::CModManager::DefaultBLMod[%j])
            {
               %isDefault = 1;
               break;
            }
         }
         
         if(%isDefault)
         {
            %obj.setValue(1);
            AOG_tickAddon(%obj);
         }
      }
   }
}

function AOG_selectMinimal()
{
   %minimalList = " Brick_Large_Cubes Light_Animated Light_Basic Particle_Basic Particle_FX_Cans Particle_Player Print_1x2f_Default Print_2x2f_Default Print_2x2r_Default Print_Letters_Default Sound_Synth4 Sound_Beeps Sound_Phone ";
   
   AOG_selectNone();
   for(%i=0;%i<AOG_Scroll.getObject(0).getCount();%i++)
   {
      %obj = AOG_Scroll.getObject(0).getObject(%i);
      if(%obj.varName !$= "")
      {
         if(strPos(%minimalList," "@%obj.varName@" ") >= 0)
         {
            %obj.setValue(1);
            AOG_tickAddon(%obj);
         }
      }
   }
}

function AOG_tickAddon(%checkbox)
{
   if(%checkbox.getValue() $= 1)
      %checkbox.parent.numEnabled++;
   else
      %checkbox.parent.numEnabled--;
      
   if(%checkbox.parent.numEnabled $= %checkbox.parent.numChildren)
      %checkbox.parent.setValue(1);
   else
      %checkbox.parent.setValue(0);
}

function addonsGui::onSleep()
{
   for(%i=0;%i<AOG_Scroll.getObject(0).getCount();%i++)
   {
      %obj = AOG_Scroll.getObject(0).getObject(%i);
      if(%obj.varName !$= "")
      {
         $AddOn__[%obj.varName] = %obj.getValue();
      }
   }
   export("$AddOn__*","config/server/ADD_ON_LIST.cs");
}

//*********************************************************
//* Server-Client MOD Support
//*********************************************************
function RTBMM_loadExistingRTBMods()
{
   clientUpdateAddOnsList();
   
   if(isObject(RTBMM_MODManifest))
      RTBMM_MODManifest.delete();
      
   new SimGroup(RTBMM_MODManifest);
   
	%mod = FindFirstFile("Add-Ons/*_*/rtbInfo.txt");
	while(strLen(%mod) > 0)
	{
	   %modName = getSubStr(%mod,8,strLen(%mod)-20);
	   %modVarName = getSafeVariableName(%modName);
	   if($AddOn__[%modVarName] !$= "" || (isFile("Add-Ons/"@%modName@"/client.cs") && isFile("Add-Ons/"@%modName@"/description.txt")) || %isValidMap)
	   {
	      %descriptionData = getFileContents("Add-Ons/"@%modName@"/description.txt");
	      %rtbData = getFileContents(%mod);
	      %title = RTBMM_getFieldFromContents(%descriptionData,"Title");
	      if(%title $= "0")
            %title = %modName;
         %author = RTBMM_getFieldFromContents(%descriptionData,"Author");
         if(%author $= "0")
            %author = "Unknown";
         %so = new ScriptObject()
         {
            class = "ModSO";
            variableName = %modVarName;
            zipName = %modName;
            name = RTBMM_getFieldFromContents(%rtbData,"Name");
            title = %title;
            author = %author;
            description = RTBMM_getFieldFromContents(%descriptionData);
            version = RTBMM_getFieldFromContents(%rtbData,"Version");
            id = RTBMM_getFieldFromContents(%rtbData,"ID");
            icon = RTBMM_getFieldFromContents(%rtbData,"Icon");
            type = RTBMM_getFieldFromContents(%rtbData,"Type");
            path = "Add-Ons/"@%modName@"/";
            zip = "Add-Ons/"@%modName@".zip";
         };
         RTBMM_MODManifest.add(%so);
         
         if(isFile("Add-Ons/"@%modName@"/client.cs") && isFile("Add-Ons/"@%modName@"/description.txt") && !isFile("Add-Ons/"@%modName@"/server.cs"))
            %so.isClientside = 1;
	   }
		%mod = FindNextFile("Add-Ons/*_*/rtbInfo.txt");
	}
	
	%mod = FindFirstFile("Add-Ons/*_*/*.mis");
	while(strLen(%mod) > 0)
	{
	   %misName = getSubStr(getField(strReplace(%mod,"/","\t"),2),0,strLen(getField(strReplace(%mod,"/","\t"),2))-4);
	   %modName = getField(strReplace(%mod,"/","\t"),1);
	   %modVarName = getSafeVariableName(%modName);
	   if(isFile("Add-Ons/"@%modName@"/"@%misName@".txt") && isFile("Add-Ons/"@%modName@"/rtbInfo.txt"))
	   {
	      %descriptionData = getFileContents("Add-Ons/"@%modName@"/"@%misName@".txt");
	      %rtbData = getFileContents("Add-Ons/"@%modName@"/rtbInfo.txt");
	      %title = RTBMM_getFieldFromContents(%descriptionData,"Title");
	      if(%title $= "0")
            %title = %modName;
         %author = RTBMM_getFieldFromContents(%descriptionData,"Author");
         if(%author $= "0")
            %author = "Unknown";
         %so = new ScriptObject()
         {
            class = "ModSO";
            variableName = %modVarName;
            zipName = %modName;
            name = RTBMM_getFieldFromContents(%rtbData,"Name");
            title = %title;
            author = %author;
            description = RTBMM_getFieldFromContents(%descriptionData);
            version = RTBMM_getFieldFromContents(%rtbData,"Version");
            id = RTBMM_getFieldFromContents(%rtbData,"ID");
            icon = RTBMM_getFieldFromContents(%rtbData,"Icon");
            type = RTBMM_getFieldFromContents(%rtbData,"Type");
            path = "Add-Ons/"@%modName@"/";
            zip = "Add-Ons/"@%modName@".zip";
            isMap = 1;
         };
         RTBMM_MODManifest.add(%so);
	   }
		%mod = FindNextFile("Add-Ons/*_*/*.mis");
	}
}
RTBMM_InitiateDownloadQueue();
RTBMM_InitiateScreenshotGrabber();
RTBMM_InitateFeedControl();

//*********************************************************
//* Public Support Functions
//*********************************************************
function RTBMM_getDelimitedMods()
{
   for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
   {
      %SO = RTBMM_MODManifest.getObject(%i);
      if(%delimitedString $= "")
         %delimitedString = %SO.id;
      else
         %delimitedString = %delimitedString@","@%SO.id;
   }
   return %delimitedString;
}
function RTBMM_hasFile(%id)
{
   if(RTBMM_DownloadQueue.hasFile(%id))
      return 1;
      
   for(%i=0;%i<RTBMM_MODManifest.getCount();%i++)
   {
      %SO = RTBMM_MODManifest.getObject(%i);
      if(%SO.id $= %id)
         return 1;
   }
   return 0;
}
function RTBMM_parseComment(%comment,%commentOdd)
{
   %comment = strReplace(%comment,"&quot;","\"");
   %comment = strReplace(%comment,"&amp;","&");
   %comment = strReplace(%comment,"&lt;","<<spush><spop>");
   %comment = strReplace(%comment,"&gt;","<spush><spop>>");
   %comment = strReplace(%comment,"[/color]","<spop>");
   
   %size = 14;
   for(%i=0;%i<strLen(%comment);%i++)
   {
      if(getSubStr(%comment,%i,7) $= "[color=")
      {
         %colorType = getSubStr(%comment,%i+7,strLen(%comment));
         %endBracket = strPos(%colorType,"]");
         %colorType = getSubStr(%colorType,0,strPos(%colorType,"]"));
         if(getSubStr(%colorType,0,1) $= "#")
         {
            %colorType = getSubStr(%colorType,1,6);
         }
         else
         {
            %colorType = strUpr(%colorType);
            if($RTB::CModManager::Style::Color[%colorType] !$= "")
               %colorType = $RTB::CModManager::Style::Color[%colorType];
         }
         %comment = getSubStr(%comment,0,%i)@"<spush><color:"@%colorType@">"@getSubStr(%comment,%i+%endBracket+8,strLen(%comment));
         %i = %i+%endBracket+8;
      }
      else if(getSubStr(%comment,%i,4) $= "[url")
      {
         if(getSubStr(%comment,%i+4,1) $= "=")
         {
            %remain = getSubStr(%comment,%i+5,strLen(%comment));
            %url = getSubStr(%remain,0,strPos(%remain,"]"));
            %remain = getSubStr(%remain,strPos(%remain,"]")+1,strLen(%remain));
            %text = getSubStr(%remain,0,strPos(%remain,"[/url]"));
            
            %comment = getSubStr(%comment,0,%i)@"<a:"@%url@">"@%text@"</a>"@getSubStr(%comment,%i+6+strLen(%url)+6+strLen(%text),strLen(%comment));
         }
         else
         {
            %remain = getSubStr(%comment,%i+5,strLen(%comment));
            %url = getSubStr(%remain,0,strPos(%remain,"[/url]"));
            
            %comment = getSubStr(%comment,0,%i)@"<a:"@%url@">"@%url@"</a>"@getSubStr(%comment,%i+5+strLen(%url)+6,strLen(%comment));
         }
      }
      else if(getSubStr(%comment,%i,6) $= "[size=")
      {
         %size = getSubStr(%comment,%i+6,strLen(%comment));
         %endBracket = strPos(%size,"]");
         %size = getSubStr(%size,0,%endBracket);
         %size += 2;
         
         %comment = getSubStr(%comment,0,%i)@"<spush><font:Arial"@%fontBold@%fontItalic@":"@%size@">"@getSubStr(%comment,%i+%endBracket+7,strLen(%comment));
         %i = %i+%endBracket+7;
      }
      else if(getSubStr(%comment,%i,7) $= "[/size]")
      {
         %size = 14;
         %comment = getSubStr(%comment,0,%i)@"<spush><font:Arial"@%fontBold@%fontItalic@":"@%size@">"@getSubStr(%comment,%i+7,strLen(%comment));
         %i = %i+3;
      }
      else if(getSubStr(%comment,%i,3) $= "[i]")
      {
         %fontItalic = " Italic";
         %comment = getSubStr(%comment,0,%i)@"<spush><font:Arial"@%fontBold@%fontItalic@":"@%size@">"@getSubStr(%comment,%i+3,strLen(%comment));
         %i = %i+3;
      }
      else if(getSubStr(%comment,%i,4) $= "[/i]")
      {
         %fontItalic = "";
         %comment = getSubStr(%comment,0,%i)@"<spop>"@getSubStr(%comment,%i+4,strLen(%comment));
         %i = %i+4;
      }
      else if(getSubStr(%comment,%i,3) $= "[b]")
      {
         %fontBold = " Bold";
         %comment = getSubStr(%comment,0,%i)@"<spush><font:Arial"@%fontBold@%fontItalic@":"@%size@">"@getSubStr(%comment,%i+3,strLen(%comment));
         %i = %i+3;
      }
      else if(getSubStr(%comment,%i,4) $= "[/b]")
      {
         %fontBold = "";
         %comment = getSubStr(%comment,0,%i)@"<spop>"@getSubStr(%comment,%i+4,strLen(%comment));
         %i = %i+4;
      }
      else if(getSubStr(%comment,%i,6) $= "[quote")
      {
         if(getSubStr(%comment,%i+6,1) $= "=")
         {
            %author = getSubStr(%comment,%i+7,strLen(%comment));
            %endBracket = strPos(%author,"]");
            %endQuote = strPos(%author,"[/quote]");
            %author = getSubStr(%author,0,%endBracket);
            %author = strReplace(%author,"\"","");
            %endBracket++;
         }
         else
         {
            %remaining = getSubStr(%comment,%i+7,strLen(%comment));
            %endBracket = 0;
            %endQuote = strPos(%remaining,"[/quote]");
         }

         //one of the worst hacks i've had to do ever :(
         %ml = new GuiMLTextCtrl()
         {
            extent = "339 0";
            text = "<color:FFFFFF>"@getSubStr(%comment,0,%i)@"<font:Arial Bold:14><br>";
         };
         RTBMM_WindowSwatch.add(%ml);
         %ml.forceReflow();
         %extent = getWord(%ml.extent,1);
         %ml.delete();
         
         %text = getSubStr(%comment,%i+%endBracket+7,strLen(%comment));
         %text = getSubStr(%text,0,strPos(%text,"[/quote]"));
         
         $RTB::CModManager::Cache::QuoteAuthor[$RTB::CModManager::Cache::NumCommentQuotes] = %author;
         $RTB::CModManager::Cache::QuoteText[$RTB::CModManager::Cache::NumCommentQuotes] = %text;
         $RTB::CModManager::Cache::QuoteStart[$RTB::CModManager::Cache::NumCommentQuotes] = %extent;
         $RTB::CModManager::Cache::NumCommentQuotes++;
         
         if(%commentOdd)
            %color = "F0F0F0";
         else
            %color = "DCDCDC";
         
         %ml = new GuiMLTextCtrl()
         {
            extent = "339 0";
            text = "<color:777777>"@%text;
         };
         RTBMM_WindowSwatch.add(%ml);
         %ml.forceReflow();
         %extent = getWord(%ml.extent,1);
         %ml.delete();
      
         %textadd = "<br><spush><color:FFFFFF>"@%text@"<spop><br>";
         if(%author $= "")
            %quoteText = "Quote:";
         else
            %quoteText = %author@" wrote:";
            
         %comment = getSubStr(%comment,0,%i)@"<spush><font:Arial Bold:14><br><color:000000>    "@%quoteText@%textadd@"<spop>"@getSubStr(%comment,%i+%endQuote+7+8,strLen(%comment));
      }
   }
   return %comment;
}

//*********************************************************
//* Module Package
//*********************************************************
package RTBC_ModManager
{
   function verTCPObj::connect(%this,%address)
   {
      Parent::connect(%this,%address);
      
      if($RTB::Options::CheckForUpdates)
         RTBMM_ModsView_checkForUpdates(1);
   }

   function SM_StartMission()
   {
      if(RTBMM_DownloadQueue.totalDownloads >= 1)
      {
         MessageBoxYesNo("Whoops!","You are not able to start a server while you have files in your Download Queue.\n\nWould you like to view your queue?","canvas.pushDialog(RTB_ModManager);RTBMM_getModsView();","");
         return;
      }
      Parent::SM_StartMission();
   }
   
   function GuiMLTextCtrl::onURL(%this,%url)
   {
      if(strPos(%url,"rtb-") $= 0)
      {
         RTBMM_OpenDirect();
         RTBMM_getFileView(getSubStr(%url,4,strLen(%url)-4));
      }
      else if(strPos(%url,"pagination-") $= 0)
      {
         %page = getSubStr(%url,11,strLen(%url)-11);
         schedule(1,0,"RTBMM_handlePagination",%page);
      }
      else
         Parent::onURL(%this,%url);
   }
};
activatePackage(RTBC_ModManager);