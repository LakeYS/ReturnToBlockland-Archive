//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Server Control Hook (RTBSC/RServerControlHook)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBR_ServerControl_Hook = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::ServerPrefs = 0;

//*********************************************************
//* Requirements
//*********************************************************
if(isFile("config/server/RTB/modPrefs.cs"))
   exec("config/server/RTB/modPrefs.cs");

//*********************************************************
//* The Meat
//*********************************************************
function RTB_registerPref(%name,%cat,%pref,%vartype,%mod,%default,%requiresRestart)
{
   %pref = strReplace(%pref,"$","");
   
   if(%name $= "")
   {
      echo("\c2ERROR: No user-friendly name for pref supplied in RTB_registerPref");
      return 0;
   }
   else if(%pref $= "")
   {
      echo("\c2ERROR: No pref value supplied in RTB_registerPref");
      return 0;
   }
   else if(%vartype $= "")
   {
      echo("\c2ERROR: No pref variable type supplied in RTB_registerPref");
      return 0;
   }
      
   if(%requiresRestart !$= 1)
      %requiresRestart = 0;
   
   if(%mod $= "")
      %mod = "Unknown";
   
   for(%i=0;%i<$RTB::ServerPrefs;%i++)
   {
      %catcount = $RTB::ServerPrefCount[%i];
      for(%j=1;%j<%catcount+1;%j++)
      {
         %checkpref = getField($RTB::ServerPref[%i,%j],1);
         if(%pref $= %checkpref)
         {
            echo("\c2ERROR: $"@%pref@" pref has already been registered to add-on: "@getField($RTB::ServerPref[%i,%j],3)@" in RTB_registerPref");
            return 0;
         }
      }
   }
   
   if(%cat $= "")
      %cat = "Misc.";
   
   %pType = firstWord(%vartype);
   if(%pType $= "int")
   {
      %min = getWord(%vartype,1);
      if(%min <= -1)
      {
         echo("\c2ERROR: Invalid integer min value supplied for pref ("@%pref@") in RTB_registerPref");
         return 0;
      }
      
      %max = getWord(%vartype,2);
      if(%max <= %min)
      {
         echo("\c2ERROR: Integer max value supplied for pref ("@%pref@") is less than or equal to min value in RTB_registerPref");
         return 0;
      }
   }
   else if(%pType $= "string")
   {
      %length = getWord(%vartype,1);
      if(%length <= 0)
      {
         echo("\c2ERROR: Invalid string length supplied for pref ("@%pref@") in RTB_registerPref");
         return 0;
      }
   }
   else if(%pType $= "list")
   {
      if(getWordCount(%vartype)%2 $= 0)
      {
         echo("\c2ERROR: Invalid list values supplied for pref ("@%pref@") in RTB_registerPref");
         return 0;
      }
   }
   else if(%pType $= "bool")
   {
   }
   else
   {
      echo("\c2ERROR: Invalid pref type supplied ("@%pType@") for pref "@%pref@" in RTB_registerPref");
      return 0;
   }
   
   eval("%currVal = $"@%pref@";");
   if(%currVal $= "")
      eval("$"@%pref@" = \""@%default@"\";");
   
   for(%i=0;%i<$RTB::ServerPrefs;%i++)
   {
      if($RTB::ServerPref[%i,0] $= %cat)
      {
         %catcount = $RTB::ServerPrefCount[%i];
         $RTB::ServerPref[%i,%catcount] = %name TAB %pref TAB %vartype TAB %mod TAB %requiresRestart;
         $RTB::ServerPrefCount[%i]++;
         return 1;
      }
   }

   $RTB::ServerPrefCount[$RTB::ServerPrefs] = 1;
   $RTB::ServerPref[$RTB::ServerPrefs,0] = %cat;
   $RTB::ServerPref[$RTB::ServerPrefs,1] = %name TAB %pref TAB %vartype TAB %mod TAB %requiresRestart;
   $RTB::ServerPrefCount[$RTB::ServerPrefs]++;
   $RTB::ServerPrefs++;
   
   return 1;
}