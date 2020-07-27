//#############################################################################
//#
//#   Return to Blockland - Version 3.0
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 64 $
//#      $Date: 2009-07-08 20:24:38 +0100 (Wed, 08 Jul 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/trunk/RTBC_Options.cs $
//#
//#      $Id: RTBC_Options.cs 64 2009-07-08 19:24:38Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Options
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_Options = 1;

//*********************************************************
//* GUI Modification
//*********************************************************
if(!isObject(MM_RTBOptionsButton))
{
   %btn = new GuiBitmapButtonCtrl(MM_RTBOptionsButton)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "416 200";
      extent = "224 40";
      minExtent = "8 2";
      visible = "1";
      text = " ";
      groupNum = "-1";
      buttonType = "PushButton";
      bitmap = $RTB::Path@"images/buttons/menu/btnOptions";
      command = "canvas.pushdialog(rtb_options);";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%btn);
   
   %btn = new GuiBitmapButtonCtrl()
   {
      profile = "BlockButtonProfile";
      position = "10 433";
      extent = "111 25";
      command = "canvas.pushdialog(rtb_options);";
      text = "RTB Options";
      bitmap = "base/client/ui/button1";
   };
   optionsDlg.getObject(0).add(%btn);
}
function MM_RTBOptionsButton::onMouseEnter(%this)
{
   if($Pref::Audio::MenuSounds)
	   alxPlay(Note10Sound);
}

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_Options))
	exec("./RTB_Options.gui");

//*********************************************************
//* GUI Methods
//*********************************************************
function RTB_Options::onWake(%this)
{
	applyRadioOption(RTBO_OptSA_PostServer);
	applyRadioOption(RTBO_OptSA_ShowPlayers);

	applyRadioOption(RTBO_OptCA_AuthWithRTB);
	applyRadioOption(RTBO_OptCA_ShowOnline);
	applyRadioOption(RTBO_OptCA_ShowServer);
	
	applyRadioOption(RTBO_OptMM_AnimateGUI);
	applyRadioOption(RTBO_OptMM_WarnFailed);
	applyRadioOption(RTBO_OptMM_DisableFailed);
	applyRadioOption(RTBO_OptMM_CheckForUpdates);
	applyRadioOption(RTBO_OptMM_DownloadScreenshots);
	
	applyRadioOption(RTBO_OptIC_AutoConnect);
	applyRadioOption(RTBO_OptIC_AllowPM);
	applyRadioOption(RTBO_OptIC_AudioNotify);
	applyRadioOption(RTBO_OptIC_VisualNotify);
	
	applyRadioOption(RTBO_OptIT_EnableInfoTips);
	applyRadioOption(RTBO_OptIT_ShowAddonTips);
	
	applyRadioOption(RTBO_OptGT_EnableGUITransfer);
	
	applyRadioOption(RTBO_OptAU_EnableAutoUpdate);
}

function RTB_Options::onSleep(%this)
{
   echo("Exporting rtb prefs");
   export("$RTB::Options*","config/client/rtb/prefs.cs");
	RTBCA_SendPrefs();
	RTBCA_Post();
}

//*********************************************************
//* Support Functions
//*********************************************************
//- applyRadioOption (Finds the value of an rtb pref and applies it)
function applyRadioOption(%radio)
{
	eval("%pref = $"@(%radio@"Y").optionVariable@";");

	if(%pref $= 1)
		%radio = %radio@"Y";
	else if(%pref $= 2)
		%radio = %radio@"A";
	else
		%radio = %radio@"N";

	if(isObject(%radio.optionBlock))
	{
		%radio.optionBlock.setVisible(%radio.optionBlockVisible);
		%radio.optionBlock.isVisible = %radio.optionBlockVisible;
		if(isObject(%radio.optionBlock.effectorBlock))
		{
			if(%radio.optionBlockVisible $= 1)
				%radio.optionBlock.effectorBlock.setVisible(0);
			else
				%radio.optionBlock.effectorBlock.setVisible(%radio.optionBlock.effectorBlock.isVisible);
		}
	}
	%radio.setValue(1);
}

//- setRadioOption (Sets a specific radio to a value)
function setRadioOption(%radio,%value)
{
	if(%value $= 1)
		%radio = %radio@"Y";
	else if(%pref $= 2)
		%radio = %radio@"A";
	else
		%radio = %radio@"N";

	if(isObject(%radio.optionBlock))
	{
		%radio.optionBlock.setVisible(%radio.optionBlockVisible);
		%radio.optionBlock.isVisible = %radio.optionBlockVisible;
		if(isObject(%radio.optionBlock.effectorBlock))
		{
			if(%radio.optionBlockVisible $= 1)
				%radio.optionBlock.effectorBlock.setVisible(0);
			else
				%radio.optionBlock.effectorBlock.setVisible(%radio.optionBlock.effectorBlock.isVisible);
		}
	}

	if(%radio.optionVariable $= "")
		error("Error: Rogue option variable for radio: "@%radio);
	else
		eval("$"@%radio.optionVariable@" = "@%value@";");
}