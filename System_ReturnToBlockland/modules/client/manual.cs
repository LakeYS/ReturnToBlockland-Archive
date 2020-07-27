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
//#   Modules / Client / Manual
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Modules::Client::Manual = 1;

//- RTB_Manual::onWake (wake)
function RTB_Manual::onWake(%this)
{
}

//- RTB_Manual::onSleep (sleep)
function RTB_Manual::onSleep(%this)
{
}

//- RTB_Manual::setPage (sets the manual page - simples)
function RTB_Manual::setPage(%this,%page)
{
   %swatch = %this.getObject(0).getObject(0);
   for(%i=1;%i<%swatch.getCount();%i++)
   {
      %pg = %swatch.getObject(%i);
      %pg.setVisible(false);
   }
   
   if(%page < 0 || %page >= %swatch.getCount())
      %page = 0;
      
   %swatch.getObject(%page).setVisible(true);
}