// All of the Big Switch Statements go here


===function GetDisplayName(target)
{type_of(target)?String:
~return target
}
{target:
- None:
~return "No Selection"
- Jeanne:
~return "[color=red][hint=\"Jeanne Marten - Worker \# 91\"]Jeanne Marten[/hint][/color]"
- Amar:
~return "[color=green][hint=\"Amar Khalasi - Worker \#567\"]Amar Khalasi[/hint][/color]"
- Marcus:
~return "[color=yellow][hint=\"Marcus Paattinen - Worker \#12 \"]Marcus Paattinen[/hint][/color]"
- Player:
~return "[color=grey][hint=\"Worker \#278\"]Current User[/hint][/color]"

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
- None:
~return "No item selected."
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

===function ToString(target)
~return "{target}"