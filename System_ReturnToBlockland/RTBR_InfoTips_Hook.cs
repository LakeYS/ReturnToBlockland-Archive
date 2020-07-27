//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Info Tips Hook (RTBIT/RInfoTipsHook)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBR_InfoTips_Hook = 1;

//*********************************************************
//* The Meat
//*********************************************************
function RTB_addInfoTip(%tip,%nobindtip,%category)
{
   if(%tip $= "")
   {
      echo("\c2ERROR: No tip specified in RTB_addInfoTip");
      return 0;
   }
   
   //category is deprecated for 2.0
   
   $RTB::InfoTip[$RTB::InfoTips++] = %tip TAB %nobindtip;
   
   return 1;
}