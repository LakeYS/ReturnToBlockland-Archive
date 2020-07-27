//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Control (RTBSC/CServerControl)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_ServerControl = 1;

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_ServerControl))
	exec("./RTB_ServerControl.gui");

//*********************************************************
//* Main Control
//*********************************************************
package RTBC_ServerControl
{
   function adminGui::onWake(%this)
   {
      Parent::onWake(%this);
      
      if(isObject(rtbServerControlBtn))
      {
         if($IamAdmin !$= 2 || $RTB::CServerControl::Cache::ServerHasRTB !$= 1)
            rtbServerControlBtn.delete();
         return;
      }
      
      if($IamAdmin !$= 2 || $RTB::CServerControl::Cache::ServerHasRTB !$= 1)
         return;
         
      %btn = new GuiBitmapButtonCtrl(rtbServerControlBtn)
      {
         profile = BlockButtonProfile;
         horizSizing = "left";
         vertSizing = "bottom";
         position = "205 275";
         extent = "98 38";
         command = "canvas.pushDialog(RTB_ServerControl);";
         text = "Server Control";
         bitmap = "base/client/ui/button1";
         mcolor = "100 255 50 255";
      };
      adminGui.getObject(0).add(%btn);
   }
   
   function handleClientJoin(%a,%b,%c,%d,%e,%f,%g,%h,%i,%k,%l)
   {
      Parent::handleClientJoin(%a,%b,%c,%d,%e,%f,%g,%h,%i,%k,%l);
      
      if($RTB::CServerControl::Cache::currentTab $= 2)
         RTBSC_Pane2::onView(RTBSC_Pane2);
   }
};
activatePackage(RTBC_ServerControl);

function RTB_ServerControl::onWake(%this)
{
   for(%i=3;%i<%this.getObject(0).getCount();%i++)
   {
      %pane = %this.getObject(0).getObject(%i);
      if(%pane.visible)
      {
         %activePane = %pane.getName();
         break;
      }
   }
   
   if(!isObject(%activePane))
   {
      RTB_ServerControl.setTab(1);
      %activePane = "RTBSC_Pane1";
   }
   %call = %activePane@"::onView("@%activePane.getID()@");";
   eval(%call);
}

function RTB_ServerControl::setTab(%this,%id)
{
   for(%i=5;%i<%this.getObject(0).getCount();%i++)
   {
      %pane = %this.getObject(0).getObject(%i);
      %pane.visible = 0;
   }
   %pane = "RTBSC_Pane"@%id;
   %pane.visible = 1;
   %call = %pane@"::onView("@%pane@");";
   eval(%call);
   
   $RTB::CServerControl::Cache::currentTab = %id;
}

//*********************************************************
//* Server Options (SO) Pane 1
//*********************************************************
function RTBSC_Pane1::onView(%this)
{
   RTBSC_SO_MaxPlayers.clear();
   for(%i=1;%i<33;%i++)
   {
      if(%i > 1)
         %s = "s";
      RTBSC_SO_MaxPlayers.add(%i@" Player"@%s,%i);
   }
   RTBSC_SO_MaxPlayers.setSelected(12);
   commandtoserver('RTB_getServerOptions');
   RTBSC_SO_Tip.setText("Click a Setting Name to view some information about it.");
}

function RTBSC_Pane1::getSettingTip(%this,%id)
{
   %settingTip0 = "The Server Name is what is displayed to people who are browsing for servers to join.";
   %settingTip1 = "The Welcome Message is what is sent to users when they join the server.\n\n%1 is replaced with the player's name.";
   %settingTip2 = "The Max Players is the max ammount of people allowed in the server.\n\nThis includes the Server Host and the Admin.\n\nYou can set this to less than the current number of players in the server.";
   %settingTip3 = "The Server Password prevents people without the password from joining the server.";
   %settingTip4 = "The Admin Password allows people to enter a password to become Admin.";
   %settingTip5 = "The Super Admin Password allows people to enter a password to become a Super Admin.";
   %settingTip6 = "The E-Tard Filter prevents people from saying words that are in the box to the right.\n\nWords should be separated by commas. A space before and after means it must be a whole word, not a part of a bigger one.";
   %settingTip7 = "The Flood Protection stops people from spamming the same messages over and over again.";
   %settingTip8 = "The Max Bricks per Second is how many bricks users are allowed to place per second.\n\nFor fast macroing, this should be set to 0.";
   %settingTip9 = "Falling Damage means falling from large heights will kill players.";
   %settingTip10 = "The Too Far distance is how close people have to be to their ghost brick to plant it.";
   %settingTip11 = " Wrench Events can be made Admin Only by using this setting.";
   %settingTip12 = "This will make bricks lose their ownership if the owner is gone for more than the number of minutes in the box. This means any player will be able to build on or destroy those bricks.\n\n-1 disables this.";
   
   %settingTip13 = "The total number of Player-based Vehicles or Bots that can be in the server at one time.";
   %settingTip14 = "The total number of Physics-based Vehicles that can be in the server at one time.";
   %settingTip15 = "The total number of Player-based Vehicles or Bots that a single player can have at one time.";
   %settingTip16 = "The total number of Physics-based Vehicles that a single player can have at one time.";
   
   %settingTip17 = "The total number of schedules that can be used at once per player.";
   %settingTip18 = "You know, I honestly don't know what this one does.";
   %settingTip19 = "The total number of projectiles that can exist as a result of events per player.";
   %settingTip20 = "The total number of spawned items that can exist as a result of events per player.";
   %settingTip21 = "The total number of lights/emitters that can exist as a result of events per player.";
   
   if(%settingTip[%id] !$= "")
   {
      RTBSC_SO_Tip.setText(%settingTip[%id]);
   }
}

function clientCmdRTB_getServerOptions(%msString,%csString,%gsString,%vlString,%eqString)
{
   RTBSC_SO_ServerName.setValue(getField(%msString,0));
   RTBSC_SO_WelcomeMessage.setValue(getField(%msString,1));
   RTBSC_SO_MaxPlayers.setSelected(getField(%msString,2));
   RTBSC_SO_ServerPassword.setValue(getField(%msString,3));
   RTBSC_SO_AdminPassword.setValue(getField(%msString,4));
   RTBSC_SO_SuperAdminPassword.setValue(getField(%msString,5));
   
   RTBSC_SO_EtardFilter.setValue(getField(%csString,0));
   RTBSC_SO_EtardList.setValue(getField(%csString,1));
   RTBSC_SO_FloodProtection.setValue(getField(%csString,2));

   RTBSC_SO_MaxBPerS.setValue(getField(%gsString,0));
   RTBSC_SO_FallDamage.setValue(getField(%gsString,1));
   RTBSC_SO_TooFar.setValue(getField(%gsString,2));
   RTBSC_SO_WrenchEventsAdminOnly.setValue(getField(%gsString,3));
   RTBSC_SO_PublicDomain.setValue(getField(%gsString,4));
   
   RTBSC_SO_TotalPlayerVehicles.setValue(getField(%vlString,0));
   RTBSC_SO_TotalPhysicsVehicles.setValue(getField(%vlString,1));
   RTBSC_SO_TotalPlayerVehiclesPerPlayer.setValue(getField(%vlString,2));
   RTBSC_SO_TotalPhysicsVehiclesPerPlayer.setValue(getField(%vlString,3));
   
   RTBSC_SO_EventSchedules.setValue(getField(%eqString,0));
   RTBSC_SO_EventMiscellaneous.setValue(getField(%eqString,1));
   RTBSC_SO_EventProjectiles.setValue(getField(%eqString,2));
   RTBSC_SO_EventItems.setValue(getField(%eqString,3));
   RTBSC_SO_EventLightsAndEmitters.setValue(getField(%eqString,4));
}

function RTBSC_Pane1::saveOptions(%this)
{
   %title = RTBSC_SO_ServerName.getValue();
   %welcome = RTBSC_SO_WelcomeMessage.getValue();
   %maxplayers = RTBSC_SO_MaxPlayers.getSelected();
   %serverpass = RTBSC_SO_ServerPassword.getValue();
   %adminpass = RTBSC_SO_AdminPassword.getValue();
   %sadminpass = RTBSC_SO_SuperAdminPassword.getValue();
   %msString = %title TAB %welcome TAB %maxplayers TAB %serverpass TAB %adminpass TAB %sadminpass;
   
   %etardFilter = RTBSC_SO_EtardFilter.getValue();
   %etardList = RTBSC_SO_EtardList.getValue();
   %floodProtect = RTBSC_SO_FloodProtection.getValue();
   %csString = %etardFilter TAB %etardList TAB %floodProtect;
   
   %maxBpS = RTBSC_SO_MaxBPerS.getValue();
   %fallDamage = RTBSC_SO_FallDamage.getValue();
   %toofar = RTBSC_SO_TooFar.getValue();
   %wrenchadminonly = RTBSC_SO_WrenchEventsAdminOnly.getValue();
   %pdomain = RTBSC_SO_PublicDomain.getValue();
   %gsString = %maxBpS TAB %fallDamage TAB %toofar TAB %wrenchadminonly TAB %pdomain;
   
   %totalPlayerVehicles = RTBSC_SO_TotalPlayerVehicles.getValue();
   %totalPhysicsVehicles = RTBSC_SO_TotalPhysicsVehicles.getValue();
   %totalPlayerVehiclesPerPlayer = RTBSC_SO_TotalPlayerVehiclesPerPlayer.getValue();
   %totalPhysicsVehiclesPerPlayer = RTBSC_SO_TotalPhysicsVehiclesPerPlayer.getValue();
   %vlString = %totalPlayerVehicles TAB %totalPhysicsVehicles TAB %totalPlayerVehiclesPerPlayer TAB %totalPhysicsVehiclesPerPlayer;
   
   %eventSchedules = RTBSC_SO_EventSchedules.getValue();
   %eventMiscellaneous = RTBSC_SO_EventMiscellaneous.getValue();
   %eventProjectiles = RTBSC_SO_EventProjectiles.getValue();
   %eventItems = RTBSC_SO_EventItems.getValue();
   %eventLightsAndEmitters = RTBSC_SO_EventLightsAndEmitters.getValue();
   %eqString = %eventSchedules TAB %eventMiscellaneous TAB %eventProjectiles TAB %eventItems TAB %eventLightsAndEmitters;
   
   //Arg validation.
   if(strLen(%title) <= 0)
      %error = "You need to enter a Server Name.";
   else if(%maxBpS < 0)
      %error = "The max bricks per second must be more than -1.";
   else if(%toofar < 0)
      %error = "The Too Far limit must be more than 0.";
   else if(%pdomain < -1)
      %error = "The Ownership Timeout must be greater than -1. (-1 disables it)";
      
   if(%error !$= "")
   {
      MessageBoxOK("Whoops","You made a mistake:\n\n"@%error);
      return;
   }
   commandtoserver('RTB_setServerOptions',%msString,%csString,%gsString,%vlString,%eqString,RTBSC_SO_Notify.getValue());
}

//*********************************************************
//* Auto Admin (AA) Pane 2
//*********************************************************
function RTBSC_Pane2::onView(%this)
{
	commandtoserver('RTB_getAutoAdminList');

	RTBSC_AA_RemoveAuto.setVisible(1);
	RTBSC_AA_Add_Status.clear();
	RTBSC_AA_Add_Status.add("Admin",0);
	RTBSC_AA_Add_Status.add("Super Admin",1);
	RTBSC_AA_Add_Status.setSelected(0);
	RTBSC_AA_Add_ID.setValue("");
	RTBSC_AA_AddStatus.setVisible(0);
	
	RTBSC_AA_PlayerList.clear();
	for(%i=0;%i<NPL_List.rowCount();%i++)
	{
	   %row = getFields(NPL_List.getRowText(%i),0,1) TAB getField(NPL_List.getRowText(%i),3);
	   %id = NPL_List.getRowId(%i);
      RTBSC_AA_PlayerList.addRow(%id,%row);
	}
}

function RTBSC_AA_AdminList::onSelect(%this,%id,%text)
{
	RTBSC_AA_RemoveAuto.setVisible(0);
}

function RTBSC_Pane2::addAutoStatus(%this)
{
	%bl_id = RTBSC_AA_Add_ID.getValue();
	%status = RTBSC_AA_Add_Status.getValue();

	if(%bl_id $= "" || %bl_id < 0)
		messageBoxOK("Whoopsie Daisy","You haven't entered a valid BL_ID.");
	else if(%status !$= "Admin" && %status !$= "Super Admin")
		messageBoxOK("Whoopsie Daisy","You haven't selected a valid Status.");
	else if(!isInt(%bl_id))
		messageBoxOK("Whoopsie Daisy","You have entered an invalid BL_ID.");
	else
	{
		RTBSC_AA_Add_Status.setSelected(0);
		RTBSC_AA_Add_ID.setValue("");
		RTBSC_AA_AddStatus.setVisible(0);
		commandToServer('RTB_addAutoStatus',%bl_id,%status);
	}
}

function RTBSC_Pane2::removeAutoStatus(%this)
{
	%bl_id = getField(RTBSC_AA_AdminList.getValue(),0);
	if(%bl_id  $= "")
		messageBoxOK("Whoopsie Daisy","You haven't selected a valid BL_ID.");
	else
		commandToServer('RTB_removeAutoStatus',%bl_id);
}

function clientCmdRTB_getAutoAdminList(%adminList,%superAdminList)
{
	RTBSC_AA_AdminList.clear();
	for(%i=0;%i<getWordCount(%superAdminList);%i++)
	{
		%id = getWord(%superAdminList,%i);
		if(%id $= "")
		   continue;
		if(RTBSC_AA_AdminList.findTextIndex(%id TAB "Super Admin") $= -1 && %id >= 0)
			RTBSC_AA_AdminList.addRow(%k++,%id TAB "Super Admin");
	}
	for(%i=0;%i<getWordCount(%adminList);%i++)
	{
		%id = getWord(%adminList,%i);
		if(%id $= "")
		   continue;
		if(RTBSC_AA_AdminList.findTextIndex(%id TAB "Admin") $= -1 && RTBSC_AA_AdminList.findTextIndex(%id TAB "Super Admin") $= -1 && %id >= 0)
			RTBSC_AA_AdminList.addRow(%k++,%id TAB "Admin");
	}
	RTBSC_AA_AdminList.sortNumerical(0,1);
	RTBSC_AA_RemoveAuto.setVisible(1);
}

function RTBSC_Pane2::clearAll(%this,%confirm)
{
	if(!%confirm)
	{
		MessageBoxYesNo("Really?","Are you sure you want to delete ALL the Auto Admin Entries?","RTBSC_Pane2::clearAll("@%this@",1);","");
		return;
	}
	commandtoserver('RTB_clearAutoAdminList');
}

function RTBSC_Pane2::addFromList(%this)
{
   RTBSC_AA_AddStatus.setVisible(1);
   RTBSC_AA_Add_ID.setValue(getField(RTBSC_AA_PlayerList.getValue(),2));
}

function RTBSC_Pane2::deAdmin(%this)
{
   %sel = RTBSC_AA_PlayerList.getSelectedID();
   if(%sel $= "-1")
   {
      messageBoxOK("Ooops","You need to select someone from the right list to De-Admin.");
      return;
   }
   commandtoserver('RTB_DeAdminPlayer',%sel);
}

function RTBSC_Pane2::admin(%this)
{
   %sel = RTBSC_AA_PlayerList.getSelectedID();
   if(%sel $= "-1")
   {
      messageBoxOK("Ooops","You need to select someone from the right list to Admin.");
      return;
   }
   commandtoserver('RTB_AdminPlayer',%sel);
}

function RTBSC_Pane2::superAdmin(%this)
{
   %sel = RTBSC_AA_PlayerList.getSelectedID();
   if(%sel $= "-1")
   {
      messageBoxOK("Ooops","You need to select someone from the right list to Super Admin.");
      return;
   }
   commandtoserver('RTB_SuperAdminPlayer',%sel);
}

//*********************************************************
//* Preferences (PF) Pane 3
//*********************************************************
function RTBSC_Pane3::onView(%this)
{
   RTBSC_PF_PrefList.clear();
   
   %idA = 0;
   while($RTB::CServerControl::Server::Pref[%idA,0] !$= "")
   {
      RTBSC_PF_createCategory($RTB::CServerControl::Server::Pref[%idA,0]);
      
      %idB = 1;
      while($RTB::CServerControl::Server::Pref[%idA,%idB] !$= "")
      {
         %odd = %idB%2;
         $RTB::CServerControl::Server::PrefControl[%idA,%idB] = RTBSC_PF_createPref(getField($RTB::CServerControl::Server::Pref[%idA,%idB],0),getField($RTB::CServerControl::Server::Pref[%idA,%idB],1),%odd,$RTB::CServerControl::Server::PrefValue[%idA,%idB],getField($RTB::CServerControl::Server::Pref[%idA,%idB],2));
         %idB++;
      }
      %idA++;
   }
}

function clientCmdRTB_addPref(%idA,%idB,%string)
{
   $RTB::CServerControl::Server::Pref[%idA,%idB] = %string;
}

function clientCmdRTB_updatePrefs(%prefArray)
{
   for(%i=0;%i<getFieldCount(%prefArray);%i++)
   {
      %pref = strReplace(getField(%prefArray,%i),",","\t");
      %idA = getField(%pref,0);
      %idB = getField(%pref,1);
      %value = getField(%pref,2);
      
      $RTB::CServerControl::Server::PrefValue[%idA,%idB] = %value;
   }
}

function RTBSC_Pane3::saveOptions()
{
   %idA = 0;
   while($RTB::CServerControl::Server::Pref[%idA,0] !$= "")
   {
      %idB = 1;
      while($RTB::CServerControl::Server::Pref[%idA,%idB] !$= "")
      {
         %ctrl = $RTB::CServerControl::Server::PrefControl[%idA,%idB];
         if(%ctrl.getClassName() $= "GuiPopupMenuCtrl")
            %value = %ctrl.getSelected();
         else
            %value = %ctrl.getValue();
            
         if(%value !$= $RTB::CServerControl::Server::PrefValue[%idA,%idB])
         {
            %prefArray = %prefArray@%idA@","@%idB@","@%value@"\t";
            %numChanged++;
         }
            
         %idB++;
      }
      %idA++;
   }
   
   if(%numChanged <= 0)
   {
      canvas.popDialog(RTB_ServerControl);
      return;
   }
   
   %prefArray = getSubStr(%prefArray,0,strLen(%prefArray)-1);
   commandtoserver('RTB_updatePrefs',%prefArray);
}

function RTBSC_PF_createCategory(%category)
{
   %yPos = RTBSC_PF_getLowestPos()+1;
   
   %swatch = new GuiSwatchCtrl()
   {
      position = "2" SPC %yPos;
      extent = "330 30";
      color = "0 0 0 50";
      
      new GuiTextCtrl()
      {
         profile = BlockWindowProfile;
         position = "5 6";
         extent = "296 18";
         text = "\c2"@%category;
      };
   };
   RTBSC_PF_PrefList.add(%swatch);
   
   RTBSC_PF_PrefList.resize(getWord(RTBSC_PF_PrefList.position,0),getWord(RTBSC_PF_PrefList.position,1),getWord(RTBSC_PF_PrefList.extent,0),RTBSC_PF_getLowestPos()+1);
}

function RTBSC_PF_createPref(%name,%variableType,%odd,%value,%requiresRestart)
{
   if(%requiresRestart)
      %star = "*";
   else
      %star = "";
      
   if(%odd)
      %color = "0 0 0 0";
   else
      %color = "0 0 0 10";
      
   %yPos = RTBSC_PF_getLowestPos()+1;
      
   %swatch = new GuiSwatchCtrl()
   {
      position = "2" SPC %yPos;
      extent = "330 29";
      color = %color;
      
      new GuiTextCtrl()
      {
         profile = GuiTextProfile;
         position = "5 5";
         extent = "162 18";
         text = " "@%name@%star;
      };
   };
   RTBSC_PF_PrefList.add(%swatch);
   
   if(getWord(%variableType,0) $= "bool")
   {
      %var = new GuiCheckboxCtrl()
      {
         position = "170 0";
         extent = "140 30";
         text = "";
      };
      %swatch.add(%var);
      if(%value)
         %var.setValue(1);
   }
   else if(getWord(%variableType,0) $= "list")
   {
      %var = new GuiPopupMenuCtrl()
      {
         position = "171 4";
         extent = "129 20";
      };
      for(%i=1;%i<getWordCount(%variableType);%i+=2)
      {
         %var.add(getWord(%variableType,%i),getWord(%variableType,%i+1));
      }
      %swatch.add(%var);
      if(%value !$= "")
         %var.setSelected(%value);
   }
   else if(getWord(%variableType,0) $= "int")
   {
      %var = new GuiTextEditCtrl()
      {
         position = "171 5";
         extent = "129 18";
         fieldMax = getWord(%variableType,2);
         fieldMin = getWord(%variableType,1);
      };
      %swatch.add(%var);
      %var.setValue(%value);
      %var.command = "RTBSC_PF_CheckForInt("@%var@");";
   }
   else if(getWord(%variableType,0) $= "string")
   {
      %var = new GuiTextEditCtrl()
      {
         position = "171 5";
         extent = "129 18";
         maxLength = getWord(%variableType,1);
      };
      %swatch.add(%var);
      %var.setValue(%value);
   }
   RTBSC_PF_PrefList.resize(getWord(RTBSC_PF_PrefList.position,0),getWord(RTBSC_PF_PrefList.position,1),getWord(RTBSC_PF_PrefList.extent,0),RTBSC_PF_getLowestPos()+1);
   
   return %var;
}

function RTBSC_PF_getLowestPos()
{
   %lowestPos = 0;
   for(%i=0;%i<RTBSC_PF_PrefList.getCount();%i++)
   {
      %ctrl = RTBSC_PF_PrefList.getObject(%i);
      %bottom = getWord(%ctrl.position,1)+getWord(%ctrl.extent,1);
      
      if(%bottom > %lowestPos)
         %lowestPos = %bottom;
   }
   
   return %lowestpos;
}

function RTBSC_PF_CheckForInt(%ctrl)
{
   %value = %ctrl.getValue();
   for(%i=0;%i<strLen(%value);%i++)
   {
      if(isInt(getSubStr(%value,%i,1)))
         %string = %string@getSubStr(%value,%i,1);
   }
   
   if(%string > %ctrl.fieldMax)
      %string = %ctrl.fieldMax;
      
   if(%string < %ctrl.fieldMin)
      %string = %ctrl.fieldMin;
      
   %ctrl.setValue(%string);
}

//*********************************************************
//* Version Establishment
//*********************************************************
function clientcmdsendRTBVersion(%version)
{
   $RTB::CServerControl::Cache::ServerHasRTB = 1;
   $RTB::CServerControl::Cache::ServerRTBVersion = %version;
   
   //reset server-based vars
   $RTB::CServerControl::Server::CatCount = 0;
   $RTB::CServerControl::Server::PrefCount = 0;
}