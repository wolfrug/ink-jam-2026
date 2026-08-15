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
~return "Multimeter This is a multi-line tooltip."
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