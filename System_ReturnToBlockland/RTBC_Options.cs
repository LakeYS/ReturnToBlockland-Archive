//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBC_Options.cs $
//#
//#      $Id: RTBC_Options.cs 48 2009-03-14 13:47:40Z Ephialtes $
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
      bitmap = $RTB::Path@"images/buttons/btnOptions";
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
	applyRadioOption(RTBO_OptGen_Auth);
	applyRadioOption(RTBO_OptGen_PostServer);

	applyRadioOption(RTBO_OptMan_Login);
	applyRadioOption(RTBO_OptMan_CheckUpdate);
	applyRadioOption(RTBO_OptMan_DownloadScreenshot);

	applyRadioOption(RTBO_OptDis_EnableInfoTips);
	applyRadioOption(RTBO_OptDis_EnableServerInfo);
	applyRadioOption(RTBO_OptDis_EnableAutoUpdate);

	applyRadioOption(RTBO_OptIrc_AutoConnect);
	applyRadioOption(RTBO_OptIrc_AllowPM);
	applyRadioOption(RTBO_OptIrc_AudioNotify);
	applyRadioOption(RTBO_OptIrc_VisualNotify);

	applyRadioOption(RTBO_OptPPS_ShowOnline);
	applyRadioOption(RTBO_OptPPS_ShowServer);

	applyRadioOption(RTBO_OptSPS_ShowPlayers);
	applyRadioOption(RTBO_OptSPS_ShowPassworded);
	applyRadioOption(RTBO_OptSPS_ShowOwnership);
}

function RTB_Options::onSleep(%this)
{
   echo("Exporting rtb prefs");
   export("$RTB::Options*","config/client/RTB/prefs.cs");
	RTBCA_SendPrefs();
	RTBCA_Post();
}

//*********************************************************
//* Support Functions
//*********************************************************
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