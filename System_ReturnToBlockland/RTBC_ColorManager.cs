//#############################################################################
//#
//#   Return to Blockland - Version 2.0
//#
//#   -------------------------------------------------------------------------
//#
//#   Color Manager (RTBCM/CColorManager)
//#
//#############################################################################
//Register that this module has been loaded
$RTB::RTBC_ColorManager = 1;

//*********************************************************
//* Initialisation of required objects
//*********************************************************
if(!isObject(RTB_ColorManager))
	exec("./RTB_ColorManager.gui");

//*********************************************************
//* GUI Modification
//*********************************************************
if(!isObject(btnColorManager))
{
   for(%inc=0;%inc<startMissionGui.getObject(0).getCount();%inc++)
   {
      %ctrl = startMissionGui.getObject(0).getObject(%inc);
      if(%ctrl.text $= "Add-Ons")
      {
         %btn = new GuiBitmapButtonCtrl(btnColorManager)
         {
            profile = BlockButtonProfile;
            horizSizing = "right";
            vertSizing = "top";
            position = vectorAdd(%ctrl.position,"120 0");
            extent = "113 19";
            command = "canvas.pushDialog(RTB_ColorManager);";
            text = "Color Manager";
            bitmap = "base/client/ui/button1";
            mColor = "255 255 255 255";
         };
         startMissionGui.getObject(0).add(%btn);
         break;
      }
   }
}

if(!isFile("Add-Ons/Colorset_Default/colorSet.txt"))
{
   %foA = new FileObject();
   %foB = new FileObject();
   %foA.openForWrite("Add-Ons/Colorset_Default/colorSet.txt");
   %foB.openForRead($RTB::Path@"Colorset_Default.txt");
   while(!%foB.isEOF())
   {
      %foA.writeLine(%foB.readLine());
   }
   %foA.close();
   %foA.delete();
   %foB.close();
   %foB.openForWrite("Add-Ons/Colorset_Default/description.txt");
   %foB.writeLine("Title: Blockland Default");
   %foB.writeLine("Author: Eric Hartman");
   %foB.close();
   %foB.delete();
}

//*********************************************************
//* The Meat
//*********************************************************
function RTB_ColorManager::onWake(%this)
{
   for(%i=RTBCM_Sets.getCount()-1;%i>0;%i--)
   {
      %obj = RTBCM_Sets.getObject(%i);
      %obj.delete();
   }

   RTBCM_Sets.getObject(0).extent = vectorSub(RTBCM_Sets.extent,"0 2");
   RTBCM_ColorsetPreview.color = "0 0 0 0";
	%colorset = FindFirstFile("Add-Ons/Colorset_*/colorSet.txt");
	while(strLen(%colorset) > 0)
	{
      %zip = getSubStr(%colorset,0,strLen(%colorset)-12);
      %fo = new FileObject();
      %fo.openForRead(%zip@"description.txt");
      %name = %fo.readLine();
      %name = getSubStr(%name,strPos(%name,": ")+2,strLen(%name));
      %fo.delete();
      
      if(isFile(%zip@"description.txt"))
      {
         if((%files % 2) $= 0)
         {
            %bg = new GuiSwatchCtrl()
            {
               profile = GuiDefaultProfile;
               position = "0" SPC %files*20;
               extent = "150 20";
               color = "200 200 200 255";
            };
            RTBCM_Sets.add(%bg);
         }
         
         %ctrl = new GuiRadioCtrl()
         {
            profile = GuiRadioProfile;
            position = "4" SPC %files*20;
            extent = "140 20";
            text = " "@%name;
            group = 1;
            command = "RTBCM_PreviewSet(\""@%zip@"colorSet.txt\");$RTB::CColorManager::SelectedSet = \""@%zip@"colorSet.txt\";";
         };
         RTBCM_Sets.add(%ctrl);
         
         %files++;
      }
      
	   if(isFile("config/server/colorSet.txt") && %foundMatch $= "")
	   {
         %master = new FileObject();
         %master.openForRead("config/server/colorSet.txt");
         %check = new FileObject();
         %check.openForRead(%zip@"colorSet.txt");
         %foundMatch = %zip@"colorSet.txt";
         %foundMatchRadio = %ctrl;
         while(!%check.isEOF() && %foundMatch !$= "")
         {
            if(%check.readLine() !$= %master.readLine())
               %foundMatch = "";
            else if(%master.isEOF() && !%check.isEOF() || !%master.isEOF() && %check.isEOF())
               %foundMatch = "";
            
            if(%foundMatch $= "")
               break;
         }
         %check.delete();
         %master.delete();
	   }
	   
		%colorset = FindNextFile("Add-Ons/Colorset_*/colorSet.txt");
	}
	
	if(%foundMatch !$= "")
	{
	   $RTB::CColorManager::SelectedSet = %foundMatch;
	   %foundMatchRadio.performClick();
	}
}

function RTBCM_PreviewSet(%filepath)
{
   RTBCM_ColorsetPreview.clear();
   
   %file = new FileObject();
   if(%file.openForRead(%filepath))
   {
      while(!%file.isEOF())
      {
         %line = %file.readLine();
         if(%line !$= "" && strPos(%line,"DIV:") !$= 0)
         {
            %r = getWord(%line,0);
            %g = getWord(%line,1);
            %b = getWord(%line,2);
            %a = getWord(%line,3);

            if(!isInt(getWord(%line,0)))
            {
               %r = mFloor(%r*255);
               %g = mFloor(%g*255);
               %b = mFloor(%b*255);
               %a = mFloor(%a*255);
            }
            %color = %r SPC %g SPC %b SPC %a;

            %xPos = (%currCol*16)+%currCol+1;
            %yPos = (%currRow*16)+%currRow+1;
            
            if(%a < 255)
            {
               %b = new GuiBitmapCtrl()
               {
                  profile = "GuiDefaultProfile";
                  position = %xPos SPC %yPos;
                  extent = "16 16";
                  bitmap = "./images/colorbg";
               };
               RTBCM_ColorsetPreview.add(%b);
            }
            
            %c = new GuiSwatchCtrl()
            {
               profile = "GuiDefaultProfile";
               position = %xPos SPC %yPos;
               extent = "16 16";
               color = %color;
            };
            RTBCM_ColorsetPreview.add(%c);
            %currRow++;
            
            if(%currRow > %maxRow)
               %maxRow = %currRow;
         }
         else if(strPos(%line,"DIV:") $= 0)
         {
            %currRow = 0;
            %currCol++;
         }
      }
   }
   RTBCM_ColorsetPreview.extent = (%currcol*16+%currCol+1) SPC (%maxRow*16+%maxRow+1);
   
   %file.close();
   %file.delete();
   
   %xpos = (mFloor(getWord(RTBCM_ColorSetPreview.getGroup().extent,0)/2))-(mFloor(getWord(RTBCM_ColorSetPreview.extent,0)/2));
   %ypos = (mFloor(getWord(RTBCM_ColorSetPreview.getGroup().extent,1)/2))-(mFloor(getWord(RTBCM_ColorSetPreview.extent,1)/2));
   RTBCM_ColorSetPreview.position = %xpos SPC %ypos;
}

function RTB_ColorManager::saveSet()
{
   %sel = $RTB::CColorManager::SelectedSet;
   if(%sel $= "" || !isFile(%sel))
   {
      MessageBoxOK("Whoops","Please make a valid selection.");
      return;
   }
   
   canvas.popDialog(RTB_ColorManager);
   
   %input = new FileObject();
   %output = new FileObject();
   %input.openForRead(%sel);
   %output.openForWrite("config/server/colorSet.txt");
   while(!%input.isEOF())
   {
      %output.writeLine(%input.readLine());
   }
   %input.close();
   %output.close();
   %input.delete();
   %output.delete();
}