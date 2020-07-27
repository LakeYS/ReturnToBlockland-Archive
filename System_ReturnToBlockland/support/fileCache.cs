//#############################################################################
//#
//#   Return to Blockland - Version 4
//#
//#   -------------------------------------------------------------------------
//#
//#      $Rev: 205 $
//#      $Date: 2010-04-10 22:53:37 +0100 (Sat, 10 Apr 2010) $
//#      $Author: Ephialtes $
//#      $URL: http://svn.returntoblockland.com/code/branches/4000/support/functions.cs $
//#
//#      $Id: functions.cs 205 2010-04-10 21:53:37Z Ephialtes $
//#
//#   -------------------------------------------------------------------------
//#
//#   Support / File Cache
//#
//#############################################################################
//Register that this module has been loaded
$RTB::Support::FileCache = 1;

//*********************************************************
//* Cache Instantiation
//*********************************************************
//- RTB_createFileCache (creates a new file cache)
function RTB_createFileCache()
{
   if(isObject(RTB_FileCache))
      RTB_FileCache.delete();
      
   %cache = new ScriptGroup(RTB_FileCache);
   RTBGroup.add(%cache);
   
   return %cache;
}
RTB_createFileCache();

//*********************************************************
//* Cache Functionality
//*********************************************************
//- RTB_FileCache::addPath (adds a path to the file cache)
function RTB_FileCache::addPath(%this,%path)
{
   if(!isFile(%path) && !isFile(strReplace(%path,".zip","")@"/description.txt"))
      return false;
      
   if(%path $= "Add-Ons/System_ReturnToBlockland.zip" || %path $= "Add-Ons/Colorset_Default.zip")
      return false;
      
   if(%this.hasPath(%path))
      %this.removeByPath(%path);
      
   %directory = getSubStr(%path,0,strPos(%path,".zip"))@"/";
   %zip = fileBase(%path)@".zip";
   
   if(!isFile(%directory@"description.txt"))
      return false;
   if(isFile(%directory@"rtbContent.txt"))
      %isContent = true;
   if(isFile(%directory@"rtbInfo.txt"))
      %isRTB = true;
      
   %fo = new FileObject();
   if(%fo.openForRead(%directory@"description.txt"))
   {
      while(!%fo.isEOF())
      {
         %l = %fo.readLine();
         if(striPos(%l,"Title:") $= 0)
            %title = trim(getSubStr(%l,6,strLen(%l)));
         else if(striPos(%l,"Author:") $= 0)
            %author = trim(getSubStr(%l,7,strLen(%l)));
         else if(%title !$= "" && %author !$= "")
            %description = %l;
      }
      %fo.close();
   }
   
   if(%title $= "")
      %title = %zip;
      
   if(%author $= "")
      %author = "Unknown Author";
      
   %isOldRTB = false;
   if(%isRTB)
   {
      if(%fo.openForRead(%directory@"rtbInfo.txt"))
      {
         while(!%fo.isEOF())
         {
            %l = %fo.readLine();
            if(striPos(%l,"id:") $= 0)
               %id = getWord(%l,1);
            else if(striPos(%l,"icon:") $= 0)
               %icon = getWord(%l,1);
            else if(striPos(%l,"type:") $= 0)
               %type = trim(getSubStr(%l,5,strLen(%l)));
            else if(striPos(%l,"version:") $= 0)
               %version = getWord(%l,1);
            else if(striPos(%l,"name:") $= 0)
               %isOldRTB = true;
         }
         %fo.close();
      }
   }
   
   if(%isContent)
   {
      if(%fo.openForRead(%directory@"rtbContent.txt"))
      {
         while(!%fo.isEOF())
         {
            %l = %fo.readLine();
            if(striPos(%l,"id:") $= 0)
               %id = getWord(%l,1);
         }
         %fo.close();
      }
   }
   %fo.delete();
   
   if(%isRTB)
      %platform = "rtb";
   else
      %platform = "bl";
      
   if(%isOldRTB)
      %platform = "rtb2";

   %file = new ScriptObject()
   {
      zip = %zip;
      path = %path;
      directory = %directory;
      zipname = fileBase(%zip);
      title = %title;
      author = %author;
      description = %description;
      filesize = getFileLength(%path);
      variableName = getSafeVariableName(fileBase(%zip));
      
      platform = %platform;
      isContent = %isContent;
      
      type = %type;      
      
      id = %id;
      icon = %icon;
      version = %version;
   };
   %this.add(%file);
   
   return %file;
}

//- RTB_FileCache::hasPath (checks if cache has a path)
function RTB_FileCache::hasPath(%this,%path)
{
   for(%i=0;%i<%this.getCount();%i++)
   {
      if(%this.getObject(%i).path $= %path)
         return true;
   }
   return false;
}

//- RTB_FileCache::getByPath (returns a cache object by path)
function RTB_FileCache::getByPath(%this,%path)
{
   for(%i=0;%i<%this.getCount();%i++)
   {
      %cache = %this.getObject(%i);
      if(%cache.path $= %path)
         return %cache;
   }
   return false;
}

//- RTB_FileCache::removeByPath (removes path from cache)
function RTB_FileCache::removeByPath(%this,%path)
{
   for(%i=0;%i<%this.getCount();%i++)
   {
      %cache = %this.getObject(%i);
      if(%cache.path $= %path)
      {
         %cache.delete();
         return true;
      }
   }
   return false;
}

//- RTB_FileCache::refresh (refreshes the file cache)
function RTB_FileCache::refresh(%this)
{
   %path = findFirstFile("Add-Ons/*_*/description.txt");
   while(strLen(%path) > 0)
   {
      %zip = getSubStr(%path,0,strPos(%path,"/description.txt")) @ ".zip";
      %this.addPath(%zip);
      
      %path = findNextFile("Add-Ons/*_*/description.txt");
   }
   %this.reverse();
}

//- RTB_FileCache::reverse (reverses the order of the file cache)
function RTB_FileCache::reverse(%this)
{
   for(%i=0;%i<%this.getCount();%i++)
   {
      %this.bringToFront(%this.getObject(%i));
   }
}

//- RTB_FileCache::list (lists the contents of the cache)
function RTB_FileCache::list(%this)
{
   echo(%this.getCount());
   echo("--------------------------------------");
   for(%i=0;%i<%this.getCount();%i++)
   {
      %file = %this.getObject(%i);
      echo(%file.path@"\c2 "@%file.zipname);
   }
   echo("");
}