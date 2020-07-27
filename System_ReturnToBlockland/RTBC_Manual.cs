//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Manual (RTBMA/CManual)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_Manual = 1;
if(!isObject(MM_RTBHelpButton))
{
   %btn = new GuiBitmapButtonCtrl(MM_RTBHelpButton)
   {
      profile = "GuiDefaultProfile";
      horizSizing = "relative";
      vertSizing = "relative";
      position = "416 280";
      extent = "224 40";
      minExtent = "8 2";
      visible = "1";
      text = " ";
      groupNum = "-1";
      buttonType = "PushButton";
      bitmap = $RTB::Path@"images/buttons/btnHelp";
      command = "canvas.pushdialog(RTB_Manual);";
      lockAspectRatio = "1";
      alignLeft = "1";
      overflowImage = "0";
      mKeepCached = "1";
      mColor = "255 255 255 255";
   };
   MainMenuGui.add(%btn);
}
function MM_RTBHelpButton::onMouseEnter(%this)
{
	alxPlay(Note11Sound);
}

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_Manual))
	exec("./RTB_Manual.gui");

//*********************************************************
//* Operational Functions
//*********************************************************
function RTBMA_openManual(%page)
{
   canvas.pushDialog(RTB_Manual);
   for(%i=0;%i<RTBMA_Contents.rowCount();%i++)
   {
      %content = RTBMA_Contents.getRowText(%i);
      if(getSubStr(getField(%content,0),7,strLen(getField(%content,0))) $= %page)
      {
         RTBMA_Contents.setSelectedRow(%i);
         return;
      }
   }
}

function RTB_Manual::onWake(%this)
{
   if(!isFile("./manual/Contents.txt"))
   {
      MessageBoxOK("Ooops","Missing Contents.txt file for Manual\n\nProbably shouldn't screw around, huh?");
      return;
   }
   
   RTBMA_Contents.clear();
   RTBMA_ContentIcons.clear();
   
   %contents = new FileObject();
   %contents.openForRead($RTB::Path@"manual/Contents.txt");
   while(!%contents.isEOF())
   {
      %line = %contents.readLine();
      %line = strReplace(%line,"|","\t");
      
      if(%line $= "<sep>")
      {
         RTBMA_Contents.addRow(0,"-------------------------------");
         continue;
      }
      
      if(getFieldCount(%line) !$= 3)
         continue;
         
      %icon = getField(%line,0);
      %title = getField(%line,1);
      %file = getField(%line,2);
      
      if(!isFile($RTB::Path@%icon@".png"))
         %icon = "images/exclamation";
      
      RTBMA_Contents.addRow(0,"       "@%title TAB %file);
      %rows = RTBMA_Contents.rowCount()-1;
      
      %bit = new GuiBitmapCtrl()
      {
         position = "0" SPC ((%rows*16)+2)+%rows*4;
         extent = "16 16";
         bitmap = "./"@%icon;
      };
      RTBMA_ContentIcons.add(%bit);
   }
   %contents.delete();
   
   if(RTBMA_Content.getText() $= "")
      RTBMA_Contents.setSelectedRow(0);
}

function RTBMA_Contents::onSelect(%this,%id,%text)
{
   %file = $RTB::Path@"manual/"@getField(%text,1);
   if(isFile(%file))
   {
      RTBMA_Content.setText("");
      %fo = new FileObject();
      %fo.openForRead(%file);
      while(!%fo.isEOF())
      {
         if(RTBMA_Content.getText() $= "")
            RTBMA_Content.setText(%fo.readLine());
         else
            RTBMA_Content.setText(RTBMA_Content.getText()@"<br>"@%fo.readLine());
      }
      %fo.delete();
   }
}