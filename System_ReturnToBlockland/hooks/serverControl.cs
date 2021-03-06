//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 492 $
//#      $Date: 2013-04-21 12:36:33 +0100 (Sun, 21 Apr 2013) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/trunk/hooks/serverControl.cs $
//#
//#      $Id: serverControl.cs 492 2013-04-21 11:36:33Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Hooks / Server Control
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Hooks::ServerControl = 1;

//*********************************************************
//* Variable Declarations
//*********************************************************
$RTB::MSSC::Prefs = 0;

//*********************************************************
//* Requirements
//*********************************************************
if(isFile("config/server/rtb/modPrefs.cs"))
   exec("config/server/rtb/modPrefs.cs");
   
//*********************************************************
//* The Meat
//*********************************************************
//- RTB_registerPref (Registers a pref to be sent to clients)
function RTB_registerPref(%name,%cat,%pref,%vartype,%mod,%default,%requiresRestart,%hostOnly,%callback)
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
      
   if(%hostOnly !$= 1)
      %hostOnly = 0;
   
   if(%mod $= "")
      %mod = "Unknown";
   
   for(%i=0;%i<$RTB::MSSC::Prefs;%i++)
   {
      %checkpref = getField($RTB::MSSC::Pref[%i],1);
      if(%pref $= %checkpref)
      {
         echo("\c2ERROR: $"@%pref@" pref has already been registered to add-on: "@getField($RTB::MSSC::Pref[%i],4)@" in RTB_registerPref");
         return 0;
      }
   }
   
   if(%cat $= "")
      %cat = "Misc.";
   
   %pType = firstWord(%vartype);
   if(%pType $= "int")
   {
      if(getWordCount(%vartype) $= 3)
      {
         %max = getWord(%vartype,2);
         if(%max <= %min)
         {
            echo("\c2ERROR: Integer max value supplied for pref ("@%pref@") is less than or equal to min value in RTB_registerPref");
            return 0;
         }
      }
      else
      {
         echo("\c2ERROR: Integer variable type expects 2 parameters in RTB_registerPref");
         return 0;
      }
   }
   else if(%pType $= "num")
   {
      if(getWordCount(%vartype) $= 3)
      {
         %max = getWord(%vartype,2);
         if(%max <= %min)
         {
            echo("\c2ERROR: Number max value supplied for pref ("@%pref@") is less than or equal to min value in RTB_registerPref");
            return 0;
         }
      }
      else
      {
         echo("\c2ERROR: Number variable type expects 2 parameters in RTB_registerPref");
         return 0;
      }
   }
   else if(%pType $= "float")
   {
      if(getWordCount(%vartype) $= 3)
      {
         %max = getWord(%vartype,2);
         if(%max <= %min)
         {
            echo("\c2ERROR: Float max value supplied for pref ("@%pref@") is less than or equal to min value in RTB_registerPref");
            return 0;
         }
      }
      else
      {
         echo("\c2ERROR: Float variable type expects 2 parameters in RTB_registerPref");
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
   else if(%pType $= "datablock")
   {
      %type = getWord(%vartype,1);
      if(%type $= "")
      {
         echo("\c2ERROR: Invalid datablock type supplied for pref ("@%pref@") in RTB_registerPref");
         return 0;
      }
      if(%default !$= "")
      {
         echo("\c2ERROR: You cannot specify a default datablock value for pref ("@%pref@") in RTB_registerPref");
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
   
   $RTB::MSSC::Pref[$RTB::MSSC::Prefs] = %name TAB %pref TAB %cat TAB %vartype TAB %mod TAB %requiresRestart TAB %hostOnly TAB %callback;
   $RTB::MSSC::PrefDefault[$RTB::MSSC::Prefs] = %default;
   $RTB::MSSC::Prefs++;
   return 1;
}

RTB_registerPref("Test 1","Tests","$Pref::Test1","num 1 100","Test",5,0,0);
RTB_registerPref("Test 2","Tests","$Pref::Test2","float 1 100","Test",5,0,0);