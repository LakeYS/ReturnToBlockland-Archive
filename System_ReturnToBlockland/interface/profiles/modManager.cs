//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 266 $
//#      $Date: 2010-08-04 07:29:41 +0100 (Wed, 04 Aug 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/interface/profiles/generic.cs $
//#
//#      $Id: generic.cs 266 2010-08-04 06:29:41Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Interface / Profiles / Mod Manager GUI Profiles
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Interface::Profiles::ModManager = 1;

// Row Styling
if(!isObject(RTBMM_CellLightProfile)) new GuiControlProfile(RTBMM_CellLightProfile : GuiBitmapBorderProfile)
{
   bitmap = $RTB::Path@"images/ui/cellArray_light";
};

// Row Styling
if(!isObject(RTBMM_CellYellowProfile)) new GuiControlProfile(RTBMM_CellYellowProfile : RTBMM_CellLightProfile)
{
   bitmap = $RTB::Path@"images/ui/cellArray_yellow";
};

// Row Styling
if(!isObject(RTBMM_CellDarkProfile)) new GuiControlProfile(RTBMM_CellDarkProfile : RTBMM_CellLightProfile)
{
   bitmap = $RTB::Path@"images/ui/cellArray_dark";
};

// Pagination Style
if(!isObject(RTBMM_PaginationProfile)) new GuiControlProfile(RTBMM_PaginationProfile)
{
   fontColor = "230 230 230 255";
   fontType = "Verdana Bold";
   fontSize = "12";
   justify = "Center";
   fontColors[1] = "230 230 230";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0"; 
   fontColorLink = "230 230 230 255";
   fontColorLinkHL = "255 255 255 255";
};

// News Content
if(!isObject(RTBMM_NewsContentProfile)) new GuiControlProfile(RTBMM_NewsContentProfile)
{
   fontColor = "230 230 230 255";
   fontType = "Verdana Bold";
   fontSize = "12";
   justify = "Center";
   fontColors[1] = "230 230 230";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0"; 
   fontColorLink = "150 150 150 255";
   fontColorLinkHL = "200 200 200 255";
};

// Main Text
if(!isObject(RTBMM_MainText)) new GuiControlProfile(RTBMM_MainText)
{
	fontColor = "30 30 30 255";
	fontSize = 18;
	fontType = "Impact";
	justify = "Left";
   fontColors[1] = "100 100 100";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0";   
};

// Middle Text
if(!isObject(RTBMM_MiddleText)) new GuiControlProfile(RTBMM_MiddleText)
{
	fontColor = "30 30 30 255";
	fontSize = 14;
	fontType = "Arial";
	justify = "center";
   fontColors[1] = "100 100 100";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0";  
};

// Block Text
if(!isObject(RTBMM_BlockText)) new GuiControlProfile(RTBMM_BlockText:BlockButtonProfile)
{
   fontColors[1] = "100 100 100";
	justify = "Left"; 
};

// Standard Text
if(!isObject(RTBMM_GenText)) new GuiControlProfile(RTBMM_GenText)
{
	fontColor = "30 30 30 255";
	fontSize = 14;
	fontType = "Arial";
	justify = "Left";
   fontColors[1] = "100 100 100";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0";  
};

// File View Field Style
if(!isObject(RTBMM_FieldText)) new GuiControlProfile(RTBMM_FieldText)
{
	fontColor = "30 30 30 255";
	fontSize = 12;
	fontType = "Verdana Bold";
	justify = "Left";
   fontColors[1] = "150 150 150";
   fontColors[2] = "0 255 0";  
   fontColors[3] = "0 0 255"; 
   fontColors[4] = "255 255 0";   
};

// Progress Style
if(!isObject(RTBMM_ProgressBar)) new GuiControlProfile(RTBMM_ProgressBar)
{
   fillColor = "0 200 0 100";
   border = 0; 
};