//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 187 $
//#      $Date: 2010-01-21 23:51:47 +0000 (Thu, 21 Jan 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/modules/client/guiControl.cs $
//#
//#      $Id: guiControl.cs 187 2010-01-21 23:51:47Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Modules / Client / Info Tips
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::InfoTips = 1;

//*********************************************************
//* Requirements
//*********************************************************
if(!$RTB::Hooks::InfoTips)
   exec($RTB::Path@"hooks/infoTips.cs");

//*********************************************************
//* Default Tips
//*********************************************************
$RTB::MCIT::Tips = 0;
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can clear all of your own bricks by typing <color:FF0000>/clearbricks<color:000000> in the chat box.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Right clicking on a brick on the brick menu (the <key:openBSD> key) will close the menu and allow you to build with that brick immediatley.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Customize your controls! Press <key:openOptionsWindow> to open the options menu." TAB "Assign commands to your keyboard using the options menu on the main menu!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Server administrators, did you know that you can type <color:FF0000>/reloadbricks<color:000000> to load the most recently uploaded save?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Server administrators, did you know that you can use the <color:FF0000>/fetch <name><color:000000> and <color:FF0000>/find <name><color:000000> commands to bring or go to players instantly?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Save often! Don't let your build get lost!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can load saves from other maps? Click the map name on the load menu to pick another map.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Press <key:doDofScreenShot> to take a screenshot with depth of field!" TAB "Did you know that you can take depth of field screenshots? Set the key on the options menu.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Press <key:showPlayerList> to bring up the player trust box." TAB "It looks like you can't open the player menu! Open the options menu and assign a key to it!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Transparent bricks are weaker than normal bricks in a mini-game.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Lights on a 1x1 flat brick or cylinder can be broken with a shot from the pistol.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Holding a tool or brick will reveal invisible bricks.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can choose to play as different types of players when you create a mini-game!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know you can tap the <key:toggleSuperShift> key to enable Super Shift when moving bricks?" TAB "If you set the Toggle Super Shift control, you can super shift bricks when you move them!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "If you get stuck somewhere, you can use <key:Suicide> to kill yourself and respawn!" TAB "If you get stuck somewhere, you can type <color:FF0000>/suicide<color:000000> to kill yourself and respawn!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can remove your temp brick by pressing the <key:cancelBrick> key.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "If you make a mistake, you can press <key:undoBrick> to undo it." TAB "If you set the Undo Brick control, you will be able to reverse any mistakes you make!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "If you find yourself in a dark room, press <key:useLight> to toggle your personal light!" TAB "If you find yourself in a dark room, type <color:FF0000>/light<color:000000> to toggle your personal light!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can whisper to people in the same minigame as you by using the <key:TeamChat> key?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that if you jump and jet at the same time, you'll fly faster?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can push a vehicle if you click on it without holding any tools?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can swap bricks around in your cart by clicking on one slot, and then another?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can press <key:ToggleBuildMacroRecording> to start recording the bricks you place!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can get a special wand tool by typing <color:FF0000>/wand<color:000000> which can destroy any of your bricks!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can press a letter on your keyboard when you are setting a brick's print, instead of selecting it?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can express your emotions in Blockland by typing <color:FF0000>/love<color:000000>, <color:FF0000>/hate<color:000000> and <color:FF0000>/hug<color:000000> in the chat box!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know you can get a friends' attention by typing <color:FF0000>/alarm<color:000000> in the chat box?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that many of the maps on Blockland have secret passages and building areas?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can press <key:showPlayerList> to show a list of players in the server?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know you can scroll your mousewheel when zooming in to adjust the zoom level?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Server administrators, did you know that you can look at a location and type <color:FF0000>/warp<color:000000> to teleport there?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can throw your current item away by pressing <key:dropTool>?" TAB "If you set the Drop Tool control, you'll be able to drop the tool that you're holding!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can use the jeep to perform stunts made from bricks?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can walk or drive slower by holding the <key:Walk> key?" TAB "You can walk and drive slower by setting the Walk control in your options.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can switch seats in a vehicle by pressing the <key:NextSeat> key?" TAB "You can switch seats in the vehicle you're in by setting the Next and Previous Seat controls in your options!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Server administrators, did you know that you can press <key:DropCameraAtPlayer> to go into Fly Mode?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You will need your friends' <color:FF0000>Build Trust<color:000000> in order to build on their bricks.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You will need your friends' <color:FF0000>Full Trust<color:000000> in order to spray or hammer their bricks.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that certain weapons can destroy bricks in Mini-games?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that the clocks in the Bedroom and Kitchen show how long the server has been running in hours and minutes?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can place water bricks from the Baseplates category which simulate being in water?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Server administrators, did you know you can type <color:FF0000>/timescale number<color:000000> (with a number between 0.2 and 2) to put the game into slow-motion or high-speed?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can switch between 1st and 3rd person by pressing the <key:toggleFirstPerson> key." TAB "You can switch between 1st and 3rd person by setting the Toggle First Person control!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can find out how many bricks are on the server by typing <color:FF0000>/brickcount<color:000000>";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can take a screenshot by pressing <key:doScreenShot>" TAB "You'll be able to take screenshots if you set the Screenshot key in your controls!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can look at previous chats by pressing <key:PageUpNewChatHud> and <key:PageDownNewChatHud>";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "You can hold the <key:Crouch> key to dive when in water." TAB "Set your crouch key to allow you to dive in water!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that a person's Blockland ID (BL ID) is unique to them only?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can type <color:FF0000>/zombie<color:000000> to raise your hands infront of you like a zombie?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Do you know what happens when the clock in the Bedroom reaches 99:59? I do, but I won't tell!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can press <color:FF0000>tab<color:000000> when in the Brick Selector to switch categories?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that if you spray someone with your paint can, they will temporarily turn the color of your paint can?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that the Hammer can be used as a melee weapon in a mini-game?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can flip a vehicle by hitting it with the hammer?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that the Rocket Launcher can be used to get to high places by shooting a rocket at your feet as you jump?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that setting the output event disappear to -1 will cause a brick to disappear forever! Setting disappear to 0 will make it reappear again.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that turning raycasting off of a brick will cause bullets to travel straight through it?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that turning collision off of a brick will let you walk through it?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that turning rendering off of a brick will make it invisible?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that the delay time of an event is measured in milliseconds? Although it may be inaccurate sometimes if the server is lagging!";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can change between WASD steering and mouse steering for vehicles on the Advanced Configuration section of the options menu?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can double-click a person's name in the IRC to have a private chat with them?";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know you can type /me to perform an action in the IRC? For example /me gives a tip would be *Infomaniac gives a tip.";
$RTB::MCIT::Tip[$RTB::MCIT::Tips++] = "Did you know that you can open the RTB Server Control using <key:RTBSC_ToggleSC>?" TAB "Did you know that you can bind the Server Control window to a key in your Controls?";
$RTB::MCIT::RTBTips = $RTB::MCIT::Tips;

//*********************************************************
//* Info Tip Rendering
//*********************************************************
//- RTBIT_drawInfoTip (draws an info tip on the loading gui)
function RTBIT_drawInfoTip()
{
   if(isEventPending($RTB::MCIT::Schedule))
      cancel($RTB::MCIT::Schedule);
      
   if(isObject(LOAD_TipTop))
      LOAD_TipTop.delete();
   if(isObject(LOAD_TipMiddle))
      LOAD_TipMiddle.delete();
   if(isObject(LOAD_TipBottom))
      LOAD_TipBottom.delete();
   if(isObject(LOAD_TipText))
      LOAD_TipText.delete();

   if($RTB::MCIT::Tips < 1)
      return;

   while(%msg $= "")
   {
      if(RTBCO_getPref("IT::ShowAddons"))
         %tipnum = getRandom(1,$RTB::MCIT::Tips);
      else
         %tipnum = getRandom(1,$RTB::MCIT::RTBTips);
      
      if($RTB::MCIT::LastTip $= %tipnum)
         continue;      
      
      %tip = $RTB::MCIT::Tip[%tipnum];
      %msg = getField(%tip,0);
      while(strPos(%msg,"<key:") >= 0)
      {
         %key = getSubStr(%msg,strPos(%msg,"<key:")+5,strLen(%msg));
         %key = getSubStr(%key,0,strPos(%key,">"));
         if(getKeyBind(%key) $= -1)
            %msg = getField(%tip,1);
         else
            %msg = strReplace(%msg,"<key:"@%key@">","<spush><color:FF0000>"@getKeyBind(%key)@"<spop>");
      }
      
      %k++;
      if(%k > 500)
         %msg = "Did you know that I just malfunctioned?\n\nPlease Report It!";
   }
   $RTB::MCIT::LastTip = %tipnum;

   %bottom = new GuiBitmapCtrl(LOAD_TipBottom)
   {
      profile = GuiDefaultProfile;
      horizSizing = "left";
      vertSizing = "top";
      position = getWord(LOAD_TipSwatch.extent,0)-271 SPC getWord(LOAD_TipSwatch.extent,1)-161;
      extent = "271 161";
      bitmap = $RTB::Path@"images/ui/tipBottom";
   };
   LOAD_TipSwatch.add(%bottom);

   %text = new GuiMLTextCtrl(LOAD_TipText)
   {
      profile = RTB_TipTextProfile;
      position = "14 0";
      extent = "190 14";
      text = %msg;
   };
   LOAD_TipSwatch.add(%text);
   %text.forceReflow();
   
   %middle = new GuiBitmapCtrl(LOAD_TipMiddle)
   {
      profile = GuiDefaultProfile;
      horizSizing = "left";
      vertSizing = "top";
      position = getWord(LOAD_TipSwatch.extent,0)-271 SPC getWord(%bottom.position,1)-getWord(%text.extent,1);
      extent = "218" SPC getWord(%text.extent,1);
      bitmap = $RTB::Path@"images/ui/tipMiddle";
   };
   LOAD_TipSwatch.add(%middle);
   %middle.add(%text);

   %top = new GuiBitmapCtrl(LOAD_TipTop)
   {
      profile = GuiDefaultProfile;
      horizSizing = "left";
      vertSizing = "top";
      position = getWord(LOAD_TipSwatch.extent,0)-271 SPC getWord(%middle.position,1)-33;
      extent = "218 33";
      bitmap = $RTB::Path@"images/ui/tipTop";
   };
   LOAD_TipSwatch.add(%top);
   
   %msgTime = strLen(%msg)*160;
   if(%msgTime < 8000)
      %msgTime = 8000;
      
   $RTB::MCIT::Schedule = schedule(%msgTime,0,"RTBIT_CreateInfoTip");
}

//*********************************************************
//* Support Functions
//*********************************************************
//- getKeyBind (gets a text-version of a keybind)
function getKeyBind(%bindName)
{
   %device = getField(movemap.getBinding(%bindName), 0);

   if(%device $= "")
	   return -1;

   %device = getSubStr(%device, 0, 5);
   %key = getField(movemap.getBinding(%bindName), 1);

   if(%device $= "mouse")
   {
      switch$(%key)
      {
         case "button0":
            %key = "left mouse button";
         case "button1":
            %key = "right mouse button";
         case "button2":
            %key = "middle mouse button";
         default:
            %key = "mouse " @ %key;
      }
   }
   else
   {
      switch$(%key)
      {
         case "space":
            %key = "spacebar";
         case "lshift":
            %key = "left shift";
         case "rshift":
            %key = "right shift";
         case "lalt":
            %key = "left alt";
         case "ralt":
            %key = "right alt";
      }
      
      if(strlen(%key) == 1)
         %key = strUpr(%key);
   }
   return %key;
}

//*********************************************************
//* Packaged Functions
//*********************************************************
package RTB_Modules_Client_InfoTips
{
   function LoadingGui::onWake(%this)
   {
      Parent::onWake(%this);
      if(RTBCO_getPref("IT::Enable"))
         RTBIT_drawInfoTip();
   }
   
   function LoadingGui::onSleep(%this)
   {
      Parent::onSleep(%this);
      
      cancel($RTB::MCIT::Schedule);
   }
};