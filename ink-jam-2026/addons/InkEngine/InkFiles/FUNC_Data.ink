// All of the Big Switch Statements go here

===function GetDisplayName(target)
{type_of(target)?String:
~return target
}
{target:
- None:
~return "No Selection"
- NoChar:
~return "No Selection"
- NoTask:
~return "No Selection"

- TaskMain:
~return "Acquire Heatsink"
- TaskRepair:
~return "Repair"
- TaskTrade:
~return "Trade"
- TaskSteal:
~return "Steal"

- Jeanne:
~return "[color=red]Jeanne Marten[/color]"
- Amar:
~return "[color=green]Amar Khalasi[/color]"
- Marcus:
~return "[color=yellow]Marcus Paattinen[/color]"
- Player:
~return "[color=grey]Current User[/color]"

- Multimeter:
~return "Multimeter"
- Metasocket:
~return "Metasocket"
- VoltometricPump:
~return "Voltometric Pump"
- SonicScrewdriver:
~return "Sonic Screwdriver"
- Ansible:
~return "Ansible"
- Neurojack:
~return "Neurojack"
- CombatBiosoft:
~return "Combat Biosoft"
- UtilityBiosoft:
~return "Utility Biosoft"
- Ubik:
~return "Ubik"
- HeatSink:
~return "Heat Sink"
- LocControl:
~return "Control"
- LocDock:
~return "Docks"
- LocMess:
~return "Mess Hall/Kitchen"
- LocTool:
~return "Tool Storage"
- LocEngineering:
~return "Engineering"
- LocBarrack:
~ return "Barracks"
- LocArmory:
~return "Armory"
- LocHead:
~return "Head"
- LocAdmin:
~return "Admin"
- LocRecroom:
~return "Rec Room"
- else:
~return target
}
===function GetDescription(target)
{type_of(target)?String:
~return target
}
{target:

- TaskMain:
~return "Acquire a Mawker-Gleeson Thruster Heat Sink Mk. 3.[br]Assigned by: Cmdr. Amada.[br]Priority: [color=red]Red[/color]"
- TaskRepair:
~return "Repair the thing."
- TaskTrade:
~return "Trade the thing."
-TaskSteal:
~return "Steal the thing."

- Jeanne:
~return "Jeanne Marten - Engineer"
- Amar:
~return "Amar Khalasi - Jr. Engineer"
- Marcus:
~return "Marcus Paattinen - Mechanic"
- None:
~return "No item selected."
- NoChar:
~return "No personnell selected."
- NoTask:
~return "No task selected."
- Multimeter:
~return "Multimeter[br]Detects short-circuits, power-surges and measures everything electric you might need."
- Metasocket:
~return "Metasocket[br]An all-purpose, infinitely readjustable socket wrench. Indispensable to space mechanics everywhere."
- VoltometricPump:
~return "Voltometric Pump[br]Need a fluid (or air) moved? This is your tool."
- SonicScrewdriver:
~return "Sonic Screwdriver[br]Uses micro-vibrations to fit any screw head. Even ancient ones, like the Phill-Ips."
- Ansible:
~return "Ansible[br]For all your intergalactic communication needs. Don't leave home without it."
- Neurojack:
~return "Neurojack[br]Some computers require your biocomputer, and this is the tool to jack into them."
- CombatBiosoft:
~return "Combat Biosoft[br]How did you find this? This is for jaegers!"
- UtilityBiosoft:
~return "Utility Biosoft[br]A databank of useful information and skills, neatly attached to your biocomputer."
- Ubik:
~return "Ubik[br]Comes in spray, pill or biosoft form. Warning: may cause mild disassocation from reality."
- HeatSink:
~return "Heat Sink[br]SSP-907 (Mawker-Gleeson Thruster Heat Sink Mk. 3).[br]THIS IS WHAT YOU NEED!"
- else:
~return "No description."
}
===function GetIcon(target)
{type_of(target)?String:
~return target
}
{target:
-None:
- else:
~return target
}

===function GetTheme(target)
{type_of(target)?String:
~return target
}
{target:
- Theme_Label_Default:
~return "default_invisible_label_box.tres"
- Theme_Label_Dialogue:
~return "dialog_label_box.tres"
- Theme_Label_Narrator:
~return "narrator_label_box.tres"
- else:
~return target
}

===function GetListValue(target)
{target:
- else:
~return 0
}

===function GetDictionary(target)
{LIST_ALL(items)?target:
~return inventory_stack_dictionary
}
{LIST_ALL(characters)?target:
~return crew_stack_dictionary
}
{LIST_ALL(tasks)?target:
~return task_stack_dictionary
}
// Backup
~return inventory_stack_dictionary

===function GetTaskVariable(target)
// External function
~target = ToListItem(target, tasks)
{target:
- TaskMain:
~return task_main
- else:
~return ""
}

===function ToString(target)
~return "{target}"

===function ToListItem(target, originlist)
~temp fullList = LIST_ALL(originlist)
~target = ToString(target)
~return _ToListItem(target, fullList)

===function _ToListItem(target, ref list)
~temp entry = pop(list)
//Entry: {entry}
{ToString(entry)==target:
~return entry
- else:
~return _ToListItem(target, list)
}