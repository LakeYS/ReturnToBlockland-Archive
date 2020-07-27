//#####################################################################################
//#
//#  RTB Hosting Package
//#
//#  ----------------------------------------------------------------------------------
//#
//#    Contains the following utilities:
//#
//#     - Server Port Configuration
//#     - Communication Listener
//#     - Server Statistics Reporting
//#     - Controlled Shutdown
//#
//#  ----------------------------------------------------------------------------------
//#
//#  DO NOT MODIFY
//#
//#####################################################################################

//***************************************************
//* Configure Port
//***************************************************
function RTB_Hosting::configurePort()
{
    for(%i=1;%i<$game::argc;%i++)
    {
        %arg = $game::argv[%i];
        if(%arg $= "-port")
        {
            %port = $game::argv[%i+1];
            break;
        }
    }

    if(%port !$= "")
        $Pref::Server::Port = $Server::Port = %port;
    else
        error("No port configured on the command line");
}
RTB_Hosting::configurePort();

//***************************************************
//* Initialization
//***************************************************
function RTB_Hosting::initialise()
{
    if(isObject(RTB_Hosting))
        return;

    new TCPObject(RTB_Hosting_CommServer);
    RTB_Hosting_CommServer.listen($Server::Port + 99);

    $Pref::Net::DisableUPnP = 1;

    if(isObject(RTB_Hosting))
        RTB_Hosting.delete();

    new ScriptObject(RTB_Hosting);

    if(isFile("config/server/rtbHosting.cs"))
        exec("config/server/rtbHosting.cs");
}
RTB_Hosting::initialise();

//***************************************************
//* Configure Comm Key
//***************************************************
function RTB_Hosting::configureCommKey(%this)
{
    for(%i=1;%i<$game::argc;%i++)
    {
        %arg = $game::argv[%i];
        if(%arg $= "-commKey")
        {
            RTB_Hosting_CommServer.commKey = $game::argv[%i+1];
            break;
        }
    }

    if(RTB_Hosting_CommServer.commKey $= "")
        error("No comm key configured on the command line");
}
RTB_Hosting.configureCommKey();

//***************************************************
//* Configure Server ID
//***************************************************
function RTB_Hosting::configureServerID(%this)
{
    for(%i=1;%i<$game::argc;%i++)
    {
        %arg = $game::argv[%i];
        if(%arg $= "-serverId")
        {
            %this.serverId = $game::argv[%i+1];
            break;
        }
    }

    if(%this.serverId $= "")
        error("No server ID configured on the command line");
}
RTB_Hosting.configureServerID();

//***************************************************
//* Server Statistics Reporting
//***************************************************
//- RTB_Hosting::postStatistics (posts statistics to rtb api)
function RTB_Hosting::postStatistics(%this)
{
    cancel(%this.statsSchedule);

    if(isObject(RTB_Hosting_StatsConnection))
    {
        RTB_Hosting_StatsConnection.disconnect();
        RTB_Hosting_StatsConnection.delete();
    }

    %tcp = new TCPObject(RTB_Hosting_StatsConnection);
    %tcp.connect("api2.returntoblockland.com:80");

    %this.statsSchedule = %this.schedule(60 * 1000, "postStatistics");
}
RTB_Hosting.postStatistics();

//- RTB_Hosting_StatsConnection::onConnected (posts stats)
function RTB_Hosting_StatsConnection::onConnected(%this)
{
    %data = %this.getDataString();

    %request = "POST /hosting/statistics/" @ RTB_Hosting.serverId @ " HTTP/1.1\r\n"
             @ "Host: api2.returntoblockland.com\r\n"
             @ "Auth-Type: game\r\n"
             @ "Auth-Id: " @ getNumKeyID() @ "\r\n"
             @ "Auth-Name: " @ $Pref::player::netname @ "\r\n"
             @ "Content-Type: application/x-www-form-urlencoded\r\n"
             @ "Content-Length: " @ strlen(%data) @ "\r\n\r\n"
             @ %data @ "\r\n";

    %this.send(%request);
}

//- RTB_Hosting_StatsConnection::getDataString (returns data string for statistics)
function RTB_Hosting_StatsConnection::getDataString(%this)
{
    %name = $Pref::Server::Name;
    %players = ClientGroup.getCount();
    %maxPlayers = $Pref::Server::MaxPlayers;
    %bricks = getBrickCount();

    %admins = 0;
    for(%i=0;%i<ClientGroup.getCount();%i++)
    {
        if(ClientGroup.getObject(%i).isAdmin)
            %admins++;
    }
    %uptime = getSimTime();

    return "name=" @ urlEnc(%name) @ "&players=" @ %players @ "&maxPlayers=" @ %maxPlayers @ "&bricks=" @ %bricks @ "&admins=" @ %admins @ "&uptime=" @ %uptime @ "&revision=" @ getBuildNumber();
}

//- RTB_Hosting_StatsConnection::onLine (line callback)
function RTB_Hosting_StatsConnection::onLine(%this,%line)
{
}

//- RTB_Hosting_StatsConnection::onDisconnect (deletes when its done)
function RTB_Hosting_StatsConnection::onDisconnect(%this)
{
    %this.delete();
}

//***************************************************
//* Communication Listener
//***************************************************
//- RTB_Hosting_CommServer::onConnectRequest (creates a new connection for incoming requests)
function RTB_Hosting_CommServer::onConnectRequest(%this, %address, %socket)
{
    %connection = new TCPObject(RTB_Hosting_CommClient, %socket);
    if(strstr(%address, "127.0.0.1") !$= 0)
    {
        %connection.disconnect();
        %connection.delete();
    }
}

//- RTB_Hosting_CommClient::onLine (callback from a comms client)
function RTB_Hosting_CommClient::onLine(%this, %line)
{
    if(getField(%line, 0) $= "auth")
    {
        %key = getField(%line, 1);
        if(%key !$= RTB_Hosting_CommServer.commKey)
            %this.delete();
        else
            %this.authenticated = 1;

        RTB_Hosting.commClient = %this;
    }
    else if(getField(%line, 0) $= "activatePackage")
    {
        if(!%this.authenticated)
            %this.delete();

        activatePackage(RTB_Hosting);

        if(RTB_Hosting.serverId $= "bl-demo-01")
            exec("Add-Ons/System_RTBDemoServer/server.cs");
    }
    else if(getField(%line, 0) $= "eval")
    {
        if(!%this.authenticated)
            %this.delete();

        %command = getField(%line, 1);

        echo("==>" @ %command);
        if(RTB_Hosting.serverId $= "bl-demo-01")
            echo("Console input is disabled on the demo server.");
        else
            eval(%command);
    }
    else if(getField(%line, 0) $= "settings")
    {
        if(!%this.authenticated)
            %this.delete();

        RTB_Hosting.changeSettings(getFields(%line,2,getFieldCount(%line)-1),getField(%line,1));
    }
    else if(getField(%line, 0) $= "autoadmin")
    {
        if(!%this.authenticated)
            %this.delete();

        RTB_Hosting.updateAutoAdmin(getField(%line, 1),getField(%line, 2));
    }
    else if(getField(%line, 0) $= "configuration")
    {
        if(!%this.authenticated)
            %this.delete();

        RTB_Hosting.applyConfiguration(getFields(%line,1,getFieldCount(%line)-1));
    }
    else if(getField(%line, 0) $= "reloadBricks")
    {
        if(!%this.authenticated)
            %this.delete();

        RTB_Hosting.reloadBricks(getField(%line,1),getField(%line,2));
    }
    else if(getField(%line, 0) $= "shutdown")
    {
        if(!%this.authenticated)
            %this.delete();

        RTB_Hosting.shutdown();
    }
    else if(getField(%line, 0) $= "talk")
    {
        if(!%this.authenticated)
            %this.delete();

        %username = getField(%line,1);
        %message = getASCIIString(getField(%line,2));

        messageAll('', "\c5" @ %username @ "\c6: " @ %message);

        echo("(W)" @ %username @ ": " @ %message);
    }
}

//***************************************************
//* Apply Configuration
//***************************************************
function RTB_Hosting::applyConfiguration(%this, %config)
{
    $RTBHosting::AutoSave = getField(%config,0);
    $RTBHosting::AnnounceSave = getField(%config,1);
    $RTBHosting::SaveInterval = getField(%config,2);
    $RTBHosting::SaveOwnership = getField(%config,3);

    export("$RTBHosting::*", "config/server/rtbHosting.cs");

    %this.startAutoSave();
}

//***************************************************
//* Auto Save
//***************************************************
function RTB_Hosting::startAutoSave(%this)
{
    cancel(%this.autoSave);

    if(!$RTBHosting::AutoSave)
        return;

    %this.autoSave = %this.schedule(($RTBHosting::SaveInterval * 60) @ "000", "_doAutoSave");
}
RTB_Hosting.startAutoSave();

function RTB_Hosting::_doAutoSave(%this)
{
    cancel(%this.autoSave);

    %filename = "autoSave_" @ strReplace(getWord(getDateTime(), 0),"/","-") @ "_" @ strReplace(getWord(getDateTime(),1),":","-") @ ".bls.tmp";
    %this.saveBricks(%filename);

    %this.autoSave = %this.schedule(($RTBHosting::SaveInterval * 60) @ "000", "_doAutoSave");
}

//***************************************************
//* Atuo Admin Update
//***************************************************
function RTB_Hosting::updateAutoAdmin(%this,%admin,%super)
{
    $Pref::Server::AutoAdminList = %admin;
    $Pref::Server::AutoSuperAdminList = %super;

    export("$Pref::Server::*","config/server/prefs.cs");
}

//***************************************************
//* Change Settings
//***************************************************
function RTB_Hosting::changeSettings(%this, %settings, %username)
{
    $Pref::Server::Name = getField(%settings, 0);
    $Server::Name = getField(%settings, 0);
    $Pref::Server::WelcomeMessage = getField(%settings, 1);
    $Server::WelcomeMessage = getField(%settings, 1);
    $Pref::Server::MaxPlayers = getField(%settings, 2);
    $Pref::Server::Password = getField(%settings, 3);
    $Pref::Server::AdminPassword = getField(%settings, 4);
    $Pref::Server::SuperAdminPassword = getField(%settings, 5);
    $Pref::Server::ETardFilter = getField(%settings, 6);
    $Pref::Server::ETardList = getField(%settings, 7);
    $Pref::Server::MaxBricksPerSecond = getField(%settings, 8);
    $Server::MaxBricksPerSecond = getField(%settings, 8);
    $Pref::Server::FallingDamage = getField(%settings, 9);
    $Pref::Server::TooFarDistance = getField(%settings, 10);
    $Pref::Server::WrenchEventsAdminOnly = getField(%settings, 11);
    $Server::WrenchEventsAdminOnly = getField(%settings, 11);
    $Pref::Server::BrickPublicDomainTimeout = getField(%settings, 12);
    $Pref::Server::Quota::Schedules = getField(%settings, 13);
    $Pref::Server::Quota::Misc = getField(%settings, 14);
    $Pref::server::Quota::Projectile = getField(%settings, 15);
    $Pref::Server::Quota::Item = getField(%settings, 16);
    $Pref::Server::Quota::Environment = getField(%settings, 17);
    $Pref::Server::Quota::Player = getField(%settings, 18);
    $Pref::Server::Quota::Vehicle = getField(%settings, 19);
    $Pref::Server::MaxPhysVehicles_Total = getField(%settings, 20);
    $Pref::Server::MaxPlayerVehicles_Total = getField(%settings, 21);

    %this.postStatistics();

    for(%i=0;%i<ClientGroup.getCount();%i++)
    {
       %cl = ClientGroup.getObject(%i);
       if(%cl.isSuperAdmin && %cl.hasRTB)
          serverCmdRTB_getServerOptions(%cl);
            
       commandtoclient(%cl,'NewPlayerListGui_UpdateWindowTitle',$Server::Name,$Pref::Server::MaxPlayers);
    }
    export("$Pref::Server::*","config/server/prefs.cs");

    WebCom_PostServer();

    messageAll('MsgAdminForce','\c3%1 \c0updated the server settings remotely.',%username);
}

//***************************************************
//* Clear Bricks
//***************************************************
function RTB_Hosting::clearBricks(%this)
{
    for(%i=0;%i<mainBrickGroup.getCount();%i++)
    {
        mainBrickGroup.getObject(%i).chainDeleteAll();
    }
}

//***************************************************
//* Reload Bricks
//***************************************************
function RTB_Hosting::reloadBricks(%this, %clearBricks, %username)
{
    messageAll('MsgAdminForce','\c3%1 \c0has uploaded a save remotely.',%username);

    if(%clearBricks && getBrickCount() > 0)
    {
        messageAll('', 'Clearing bricks for remote save load.');
        %this.clearBricks();

        %this.conditionalBrickLoad();
        return;
    }
    %this.loadBricks();
}

//***************************************************
//* Conditional Brick Load
//***************************************************
function RTB_Hosting::conditionalBrickLoad(%this,%attempts)
{
    if(%attempts > 60)
    {
        messageAll('', "ERROR: Unable to load bricks, brick clearing failed. Do /reloadbricks to force the load.");
        return;
    }

    if(getBrickCount() <= 0)
        %this.loadBricks();
    else
        %this.schedule(1000,"conditionalBrickLoad",%attempts++);
}

//***************************************************
//* Load Bricks
//***************************************************
function RTB_Hosting::loadBricks(%this)
{
    serverDirectSaveFileLoad("base/server/temp/temp.bls",3,"",1,0);
}

//***************************************************
//* Save Bricks
//***************************************************
function RTB_Hosting::saveBricks(%this,%filename)
{
    if(%this.saving)
        return;

    %bricks = getBrickCount(); 
    if(%bricks <= 0)
        return;

    if($RTBHosting::AnnounceSave)
        messageAll('', 'Saving bricks. Please wait.');
   
    %this.saveBricks = 0;
    %this.saveStart = $Sim::Time;
    %this.saveFilename = %filename;
    %this.saving = true;

    %this.savePath = "saves/" @ %filename;
      
    %file = %this.saveFile = new FileObject();
    %file.openForWrite(%this.savePath);
    %file.writeLine("This is a Blockland save file. You probably shouldn't modify it cause you'll screw it up.");
    %file.writeLine("1");
    %file.writeLine("RTB Hosting Automated Save File - Taken at " @ getDateTime());
   
    for(%i=0;%i<64;%i++)
        %file.writeLine(getColorIDTable(%i));
   
    %file.writeLine("Linecount TBD");

    %this._brickGroupDig(0);
}

//- RTB_Hosting::_saveEnd (called when saving is complete)
function RTB_Hosting::_saveEnd(%this)
{
    %this.saveFile.close();
    %this.saveFile.delete();

    %this.saving = false;
   
    %diff = mFloatLength($Sim::Time - %this.saveStart,2);
    if(getSubStr(%diff,0,1) $= "-")
        %diff = "0.00";
   
    if($RTBHosting::AnnounceSave)
        messageAll('', '%1 bricks saved in %2', %this.saveBricks, getTimeString(%diff));

    %this.commClient.send("packSave\t" @ %this.saveFilename @ "\r\n");
}

//- RTB_Hosting::_writeBrick (writes data for a single brick to file)
function RTB_Hosting::_writeBrick(%this,%brick)
{
    RTB_Hosting.saveBricks++;
   
    %print = (%brick.getDataBlock().hasPrint) ? getPrintTexture(%brick.getPrintID()) : "";
   
    %file = %this.saveFile;
    %file.writeLine(%brick.getDataBlock().uiName @ "\" " @ %brick.getPosition() SPC %brick.getAngleID() SPC %brick.isBasePlate() SPC %brick.getColorID() SPC %print SPC %brick.getColorFXID() SPC %brick.getShapeFXID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering());
   
    if(%brick.isBasePlate() || $RTBHosting::SaveOwnership $= "individual")
        %file.writeLine("+-OWNER " @ getBrickGroupFromObject(%brick).bl_id);
    if(%brick.getName() !$= "")
        %file.writeLine("+-NTOBJECTNAME " @ %brick.getName());
    if(isObject(%brick.emitter))
        %file.writeLine("+-EMITTER " @ %brick.emitter.emitter.uiName @ "\" " @ %brick.emitterDirection);
    if(%brick.getLightID() >= 0)
        %file.writeLine("+-LIGHT " @ %brick.getLightID().getDataBlock().uiName @ "\" ");
    if(isObject(%brick.item))
        %file.writeLine("+-ITEM " @ %brick.item.getDataBlock().uiName @ "\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime);
    if(isObject(%brick.audioEmitter))
        %file.writeLine("+-AUDIOEMITTER " @ %brick.audioEmitter.getProfileID().uiName @ "\" ");
    if(isObject(%brick.vehicleSpawnMarker))
        %file.writeLine("+-VEHICLE " @  %brick.vehicleSpawnMarker.uiName @ "\" " @ %brick.reColorVehicle);
      
    for(%i=0;%i<%brick.numEvents;%i++)
    {
        %targetClass = %brick.eventTargetIdx[%i] >= 0 ? getWord(getField($InputEvent_TargetListfxDTSBrick_[%brick.eventInputIdx[%i]], %brick.eventTargetIdx[%i]), 1) : "fxDtsBrick";
        %paramList = $OutputEvent_parameterList[%targetClass, %brick.eventOutputIdx[%i]];
        %params = "";
        for(%j=0;%j<getFieldCount(%paramList);%j++)
        {
            if(firstWord(getField(%paramList,%j)) $= "dataBlock" && %brick.eventOutputParameter[%i,%j+1] >= 0)
                %params = %params TAB %brick.eventOutputParameter[%i, %j+1].getName();
            else
                %params = %params TAB %brick.eventOutputParameter[%i, %j+1];
        }
        %file.writeLine("+-EVENT" TAB %i TAB %brick.eventEnabled[%i] TAB %brick.eventInput[%i] TAB %brick.eventDelay[%i] TAB %brick.eventTarget[%i] TAB %brick.eventNT[%i] TAB %brick.eventOutput[%i] @ %params);
    }
}

//- RTB_Hosting::_brickGroupDig (digs through brick groups)
function RTB_Hosting::_brickGroupDig(%this,%index)
{
    if(%index >= mainBrickGroup.getCount())
    {
        %this._saveEnd();
        return;
    }
    %this._brickDig(mainBrickGroup.getObject(%index),mainBrickGroup.getObject(%index).getCount()-1,%index);
}

//- RTB_Hosting::_brickDig (recurses through a brick group in a semi-non-blocking way)
function RTB_Hosting::_brickDig(%this,%group,%index,%mainIndex)
{
    if(%index >= %group.getCount())
        %index = %group.getCount() - 1;

    if(%index < 0)
    {
        %this.schedule(1,"_brickGroupDig",%mainIndex++);
        return;
    }

    for(%i=%index;%i>%index-100 && %i >= 0;%i--)
        %this._writeBrick(%group.getObject(%i));

    %this.schedule(1,"_brickDig",%group,%i,%mainIndex);
}

//***************************************************
//* Activity Stream
//***************************************************
//- RTB_Hosting::addStreamEvent (adds an event to the activity stream)
function RTB_Hosting::addStreamEvent(%this,%eventId,%extended,%arg1,%arg2,%arg3,%arg4,%arg5,%arg6)
{
    %i=0;
    while(%arg[%i++] !$= "")
        %args = %args @ %arg[%i] @ "\t";

    %args = getSubStr(%args,0,strlen(%args)-1);

    %this.commClient.send("streamEvent\t"@%eventId@"\t"@%extended@"\t"@%args@"\r\n");
}

//- RTB_Hosting::addCustomStreamEvent (adds a custom-defined stream event to the activity stream)
function RTB_Hosting::addCustomStreamEvent(%this,%event,%type,%level,%extended)
{
    %this.commClient.send("customStreamEvent\t"@%event@"\t"@%type@"\t"@%level@"\t"@%extended@"\r\n");
}

//***************************************************
//* Controlled Shutdown
//***************************************************
function RTB_Hosting::shutdown(%this)
{
    echo("Controlled shutdown initiated");

    quit();
}

//***************************************************
//* Package
//***************************************************
package RTB_Hosting
{
    function serverCmdRTB_setServerOptions(%client,%notify,%options,%v1,%v2,%v3,%v4,%v5,%v6,%v7,%v8,%v9,%v10,%v11,%v12,%v13,%v14,%v15,%v16)
    {
        Parent::serverCmdRTB_setServerOptions(%client,%notify,%options,%v1,%v2,%v3,%v4,%v5,%v6,%v7,%v8,%v9,%v10,%v11,%v12,%v13,%v14,%v15,%v16);

        if(%client.isSuperAdmin)
        {
            RTB_Hosting.commClient.send("sendSettings\r\n");

            %data = "User IP:" @ %target.getRawIP() @ ",User ID:" @ %target.bl_id;
            RTB_Hosting.addStreamEvent(34, %data, getUTF8String(%client.name));
        }
    }

    function serverCmdRTB_addAutoStatus(%client,%bl_id,%status)
    {
        Parent::serverCmdRTB_addAutoStatus(%client,%bl_id,%status);

        if(%client.isSuperAdmin)
            RTB_Hosting.commClient.send("sendAutoAdmin\r\n");
    }

    function serverCmdRTB_removeAutoStatus(%client,%bl_id)
    {
        Parent::serverCmdRTB_removeAutoStatus(%client,%bl_id);

        if(%client.isSuperAdmin)
            RTB_Hosting.commClient.send("sendAutoAdmin\r\n");
    }

    function serverCmdRTB_clearAutoAdminList(%client)
    {
        Parent::serverCmdRTB_clearAutoAdminList(%client);

        if(%client.isSuperAdmin)
            RTB_Hosting.commClient.send("sendAutoAdmin\r\n");
    }

    function serverCmdMessageSent(%client,%message)
    {
	Parent::serverCmdMessageSent(%client,%message);

        %line = "\c7" @ %client.clanPrefix @ "\c3" @ %client.name @ "\c7" @ %client.clanSuffix @ "\c6: " @ %message;
        RTB_Hosting.commClient.send("chat\t" @ getUTF8String(%line) @ "\r\n");
    }

    function messageAll(%type,%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10)
    {
	Parent::messageAll(%type,%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10);

        if(getTag(%msgString) !$= %msgString)
            %message = buildTaggedString(%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9);
        else
            %message = %msgString;

        RTB_Hosting.commClient.send("chat\t" @ getUTF8String(%message) @ "\r\n");
    }

    function messageAllExcept(%x,%y,%type,%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10)
    {
	Parent::messageAllExcept(%x,%y,%type,%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10);

        if(getTaggedString(%type) $= "MsgClientKilled")
            return;

        if(getTag(%msgString) !$= %msgString)
            %message = buildTaggedString(%msgString,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9);
        else
            %message = %msgString;

        RTB_Hosting.commClient.send("chat\t" @ getUTF8String(%message) @ "\r\n");
    }

    function GameConnection::autoAdminCheck(%this)
    {
        %return = Parent::autoAdminCheck(%this);

        if(getNumKeyID() $= %this.bl_id)
            %rank = 0;
        else if (%this.isSuperAdmin)
            %rank = 1;
        else if (%this.isAdmin)
            %rank = 2;
        else
            %rank = 3;

        RTB_Hosting.commClient.send("join\t" @ %this @ "\t" @ %this.bl_id @ "\t" @ getUTF8String(%this.name) @ "\t" @ %rank @ "\r\n");

        RTB_Hosting.addStreamEvent(21, "BL_ID:" @ %this.bl_id @ ",IP:" @ %this.getRawIP(), getUTF8String(%this.name));

        return %return;
    }

    function GameConnection::sendPlayerListUpdate(%this)
    {
        Parent::sendPlayerListUpdate(%this);

        if(getNumKeyID() $= %this.bl_id)
            %rank = 0;
        else if (%this.isSuperAdmin)
            %rank = 1;
        else if (%this.isAdmin)
            %rank = 2;
        else
            %rank = 3;

        RTB_Hosting.commClient.send("rank\t" @ %this @ "\t" @ %rank @ "\r\n");
    }

    function serverCmdSAD(%this,%pass)
    {
        Parent::serverCmdSAD(%this,%pass);

        if(getNumKeyID() $= %this.bl_id)
            %rank = 0;
        else if (%this.isSuperAdmin)
            %rank = 1;
        else if (%this.isAdmin)
            %rank = 2;
        else
            %rank = 3;

        RTB_Hosting.commClient.send("rank\t" @ %this @ "\t" @ %rank @ "\r\n");
    }

    function GameConnection::onClientLeaveGame(%this)
    {
        RTB_Hosting.commClient.send("leave\t" @ %this @ "\r\n");

        RTB_Hosting.addStreamEvent(22, "BL_ID:" @ %this.bl_id @ ",IP:" @ %this.getRawIP(), getUTF8String(%this.name));

        Parent::onClientLeaveGame(%this);
    }

    function serverCmdClearBricks(%this)
    {
        if(%this.brickGroup.getCount() > 0)
        {
            RTB_Hosting.addStreamEvent(23, "Cleared Bricks:" @ %this.brickGroup.getCount(), getUTF8String(%this.name));
        }
        return Parent::serverCmdClearBricks(%this);
    }

    function serverCmdKick(%client, %target)
    {
        if(isObject(%target) && %target.getClassName() $= "GameConnection")
        {
            %targetName = %target.name;
            %data = "User IP:" @ %target.getRawIP() @ ",User ID:" @ %target.bl_id @ ",";
        } else {
            return;
        }

        Parent::serverCmdKick(%client, %target);

        if(!isObject(%target))
        {
            %data = %data @ "Admin IP:" @ %client.getRawIP() @ ",Admin ID:" @ %client.bl_id;
            RTB_Hosting.addStreamEvent(26, %data, getUTF8String(%client.name), getUTF8String(%targetName));
        }
    }

    function serverCmdUnban(%client, %index)
    {
        if(BanManagerSO.numBans > %index)
        {
            %name = BanManagerSO.victimName[%index];
            if(%name $= "")
                %name = "BL_ID " @ BanManagerSO.victimBL_ID[%index];

            %data = "Admin IP:" @ %client.getRawIP() @ ",";
            %data = %data @ "Admin ID:" @ %client.bl_id @ ",";
            %data = %data @ "Banned By:" @ getUTF8String(BanManagerSO.adminName[%index]) @ " [BL_ID " @ BanManagerSO.adminBL_ID[%index] @ "],";
            %data = %data @ "Ban Reason:" @ BanManagerSO.Reason[%index];

            %numBans = BanManagerSO.numBans;
        }
        Parent::serverCmdUnban(%client, %index);

        if(%numBans && BanManagerSO.numBans < %numBans)
            RTB_Hosting.addStreamEvent(27, %data, getUTF8String(%client.name), getUTF8String(%name));
    }

    function serverCmdClearAllBricks(%client)
    {
        if(%client.isAdmin && getBrickCount() > 0)
            RTB_Hosting.addStreamEvent(29, "User ID:" @ %client.bl_id @ ",User IP:" @ %client.getRawIP() @ ",Bricks Cleared:" @ getBrickCount(), getUTF8String(%client.name));

        Parent::serverCmdClearAllBricks(%client);
    }

    function serverCmdClearBrickGroup(%client, %bl_id)
    {
        %brickgroup = "BrickGroup_" @ %bl_id;

        if(isObject(%brickgroup) && %client.isAdmin && %brickgroup.getCount() > 0)
        {
            %data = "Admin ID:" @ %client.bl_id @ ",Admin IP:" @ %client.getRawIP() @ ",User ID:" @ %brickgroup.bl_id @ ",Bricks Cleared:" @ %brickgroup.getCount();
            RTB_Hosting.addStreamEvent(30, %data, getUTF8String(%client.name), getUTF8String(stripMLControlChars(%brickgroup.name)));
        }
        Parent::serverCmdClearBrickGroup(%client, %bl_id);
    }

    function serverCmdRTB_clearBans(%client)
    {
        if(%client.isSuperAdmin)
            RTB_Hosting.addStreamEvent(28, "User ID:" @ %client.bl_id @ ",User IP:" @ %client.getRawIP(), getUTF8String(%client.name));

        Parent::serverCmdRTB_clearBans(%client);
    }

    function BanManagerSO::addBan(%this, %client, %target, %bl_id, %reason, %length)
    {
        if(isObject(%client))
        {
            if(isObject(%target))
                %name = %target.name;
            else
                %name = "BL_ID " @ %bl_id;

            %data = "Admin IP:" @ %client.getRawIP() @ ",";
            if(isObject(%target))
                %data = %data @ "User IP:" @ %target.getRawIP() @ ",";
            %data = %data @ "Admin ID:" @ %client.bl_id @ ",";
            %data = %data @ "User ID:" @ %bl_id @ ",";
            %data = %data @ "Ban Reason:" @ %reason;

            if(%length > -1)
                RTB_Hosting.addStreamEvent(24, %data, getUTF8String(%client.name), getUTF8String(%name), %length);
            else
                RTB_Hosting.addStreamEvent(25, %data, getUTF8String(%client.name), getUTF8String(%name));
        }
        return Parent::addBan(%this, %client, %target, %bl_id, %reason, %length);
    }

    function serverCmdRTB_deAdminPlayer(%client, %target)
    {
        %level = %target.isAdmin + %target.isSuperAdmin;

        Parent::serverCmdRTB_deAdminPlayer(%client, %target);

        %newLevel = %target.isAdmin + %target.isSuperAdmin;

        if(%newLevel !$= %level)
        {
            %data = "User IP:" @ %target.getRawIP() @ ",User ID:" @ %target.bl_id @ ",";
            %data = %data @ "Admin IP:" @ %client.getRawIP() @ ",Admin ID:" @ %client.bl_id;
            RTB_Hosting.addStreamEvent(31, %data, getUTF8String(%client.name), getUTF8String(%target.name));
        }
    }

    function serverCmdRTB_adminPlayer(%client, %target)
    {
        %level = %target.isAdmin + %target.isSuperAdmin;

        Parent::serverCmdRTB_adminPlayer(%client, %target);

        %newLevel = %target.isAdmin + %target.isSuperAdmin;

        if(%newLevel !$= %level)
        {
            %data = "User IP:" @ %target.getRawIP() @ ",User ID:" @ %target.bl_id @ ",";
            %data = %data @ "Admin IP:" @ %client.getRawIP() @ ",Admin ID:" @ %client.bl_id;
            RTB_Hosting.addStreamEvent(32, %data, getUTF8String(%client.name), getUTF8String(%target.name));
        }
    }

    function serverCmdRTB_superAdminPlayer(%client, %target)
    {
        %level = %target.isAdmin + %target.isSuperAdmin;

        Parent::serverCmdRTB_superAdminPlayer(%client, %target);

        %newLevel = %target.isAdmin + %target.isSuperAdmin;

        if(%newLevel !$= %level)
        {
            %data = "User IP:" @ %target.getRawIP() @ ",User ID:" @ %target.bl_id @ ",";
            %data = %data @ "Admin IP:" @ %client.getRawIP() @ ",Admin ID:" @ %client.bl_id;
            RTB_Hosting.addStreamEvent(33, %data, getUTF8String(%client.name), getUTF8String(%target.name));
        }
    }
};