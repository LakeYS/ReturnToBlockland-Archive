//#############################################################################
//#
//#   Return to Blockland - Version 2.03
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 48 $
//#      $Date: 2009-03-14 13:47:40 +0000 (Sat, 14 Mar 2009) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.ephialtes.co.uk/RTBSVN/branches/2030/RTBR_InfoTips_Hook.cs $
//#
//#      $Id: RTBR_InfoTips_Hook.cs 48 2009-03-14 13:47:40Z Ephialtes $
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