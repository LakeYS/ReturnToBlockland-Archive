//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 266 $
//#      $Date: 2010-08-04 07:29:41 +0100 (Wed, 04 Aug 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/support/xmlParser.cs $
//#
//#      $Id: xmlParser.cs 266 2010-08-04 06:29:41Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Support / Overlay
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Support::Overlay = 1;

//*********************************************************
//* Functionality
//*********************************************************
//- RTB_toggleOverlay (toggles the rtb overlay)
function RTB_toggleOverlay(%trigger)
{
   if(%trigger)
      return;
   
   RTB_Overlay.toggleOverlay();
}

//- RTB_escapeOverlay (escapes the rtb overlay)
function RTB_escapeOverlay(%trigger)
{
   if(%trigger)
      return;
   
   RTB_Overlay.escapeOverlay();
}

//- RTB_Overlay::toggleOverlay (toggles the rtb overlay)
function RTB_Overlay::toggleOverlay(%this)
{
   if(%this.isAwake())
      %this.fadeOut();
   else
      %this.fadeIn();
}

//- RTB_Overlay::fadeIn (fades the overlay into view)
function RTB_Overlay::fadeIn(%this)
{
   Canvas.pushDialog(%this);
   
   for(%i=0;%i<%this.getCount();%i++)
   {
      %ctrl = %this.getObject(%i);
      if(%ctrl.getName() !$= "")
         if(isFunction(%ctrl.getName(),"onWake"))
            %ctrl.onWake();
   }
}

//- RTB_Overlay::fadeOut (fades the overlay out of view)
function RTB_Overlay::fadeOut(%this)
{
   Canvas.popDialog(%this);
   
   for(%i=0;%i<%this.getCount();%i++)
   {
      %ctrl = %this.getObject(%i);
      if(%ctrl.getName() !$= "")
         if(isFunction(%ctrl.getName(),"onWake"))
            %ctrl.onSleep();
   }
}

//- RTB_Overlay::escape (closes windows/overlay)
function RTB_Overlay::escapeOverlay(%this)
{
   if(RTB_Options.isAwake() && RTBCO_OV_RemapSwatch.isVisible())
   {
      RTBCO_OV_RemapSwatch.setVisible(false);
      RTBCO_OV_Remap.delete();
      return;
   }
   
   if(!RTBCO_getPref("OV::EscapeClose"))
   {
      %this.fadeOut();
      return;
   }
   
   for(%i=%this.getCount()-1;%i>=0;%i--)
   {
      %window = %this.getObject(%i);
      if(%window.getClassName() $= "GuiWindowCtrl" && %window.isVisible())
         break;
   }
   
   if(%i < 0)
   {
      %this.fadeOut();
      return;
   }
   
   if(%window.overlayCloseCommand !$= "")
      eval(%window.overlayCloseCommand);
   else
      eval(%window.closeCommand);
}

//*********************************************************
//* Callbacks
//*********************************************************
//- RTB_Overlay::onWake (makes sure all ctrls fit on the overlay)
function RTB_Overlay::onWake(%this)
{
   %xDim = getWord(getRes(), 0);
   %yDim = getWord(getRes(), 1);

   for(%i=0;%i<%this.getCount();%i++)
   {
      %ctrl = %this.getObject(%i);
      if(%ctrl.getClassName() !$= "GuiWindowCtrl")
         continue;
         
      if(%ctrl.session && %ctrl.session.class $= "RTBCC_RoomSession" && %ctrl.session.positionOnWake)
      {
         %ctrl.session.positionWindow();
         continue;
      }
      
      %xCurr = getWord(%ctrl.position,0);
      %yCurr = getWord(%ctrl.position,1);
      %xExt = getWord(%ctrl.extent,0);
      %yExt = getWord(%ctrl.extent,1);
      if($RTB::Options::WindowPosition[%ctrl.getName()] !$= "")
      {
         %xCurr = getWord($RTB::Options::WindowPosition[%ctrl.getName()],0);
         %yCurr = getWord($RTB::Options::WindowPosition[%ctrl.getName()],1);
         %xExt = getWord($RTB::Options::WindowExtent[%ctrl.getName()],0);
         %yExt = getWord($RTB::Options::WindowExtent[%ctrl.getName()],1);
      }
      %xBound = %xCurr + %xExt;
      %yBound = %yCurr + %yExt;
      
      if(%xExt > %xDim)
         %xExt = %xDim;
      if(%yExt > %yDim)
         %yExt = %yDim;
      if(%xBound > %xDim)
         %xCurr = %xDim - %xExt;
      if(%yBound > %yDim)
         %yCurr = %yDim - %yExt;
      if(%xCurr < 0)
         %xCurr = 0;
      if(%yCurr < 0)
         %yCurr = 0;
         
      %ctrl.resize(%xCurr,%yCurr,%xExt,%yExt);
   }
   
   GlobalActionMap.bind("keyboard","escape","RTB_escapeOverlay");
}

//- RTB_Overlay::onSleep (does random things)
function RTB_Overlay::onSleep(%this)
{
   if(AddOnsGui.isAwake())
      AddOnsGui.onWake();
      
   GlobalActionMap.unbind("keyboard","escape");
   
   for(%i=0;%i<%this.getCount();%i++)
   {
      %ctrl = %this.getObject(%i);
      if(%ctrl.getClassName() !$= "GuiWindowCtrl")
         continue;
         
      if(%ctrl.getName() !$= "")
      {
         $RTB::Options::WindowPosition[%ctrl.getName()] = %ctrl.position;
         $RTB::Options::WindowExtent[%ctrl.getName()] = %ctrl.extent;
      }
      else if(%ctrl.session && %ctrl.session.class $= "RTBCC_RoomSession")
      {
         %store = RTBCC_RoomOptionsManager.getRoomStore(%ctrl.session.name);
         %store.window_position = %ctrl.position;
         %store.window_extent = %ctrl.extent;
      }
   }
   RTBCO_Save();
   RTBCC_RoomOptionsManager.store();
}

//*********************************************************
//* Usage
//*********************************************************
//- RTB_Overlay::push (sets ctrl to visible on overlay)
function RTB_Overlay::push(%this,%ctrl)
{
   if(%this.isMember(%ctrl))
   {
      %ctrl.setVisible(true);
      %ctrl.onWake();
      
      %this.pushToBack(%ctrl);
   }
}

//- RTB_Overlay::toggle (toggles ctrl invisibility)
function RTB_Overlay::toggle(%this,%ctrl)
{
   if(%this.isMember(%ctrl))
   {
      if(%ctrl.isVisible())
      {
         %ctrl.setVisible(false);
         %ctrl.onSleep();
      }
      else
      {
         %ctrl.setVisible(true);
         %ctrl.onWake();
         
         %this.pushToBack(%ctrl);
      }
   }
}

//- RTB_Overlay::pop (sets ctrl to invisible on overlay)
function RTB_Overlay::pop(%this,%ctrl)
{
   if(%this.isMember(%ctrl))
   {
      %ctrl.setVisible(false);
      %ctrl.onSleep();
   }
}

//*********************************************************
//* Key Binds
//*********************************************************
GlobalActionMap.bind(getField(RTBCO_getPref("OV::OverlayKeybind"),0),getField(RTBCO_getPref("OV::OverlayKeybind"),1),"RTB_toggleOverlay");

//- Unbind RTB v3 IRC
%binding = moveMap.getBinding("RTBIC_toggleIRC");
if(%binding)
   moveMap.unbind(getField(%binding,0),getField(%binding,1));