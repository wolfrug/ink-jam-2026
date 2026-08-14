// All of the Big Switch Statements go here


===function GetDisplayName(target)
{type_of(target)?String:
~return target
}
{target:
- ItemNone:
~return "Cancel"
- else:
~return target
}
===function GetDescription(target)
{type_of(target)?String:
~return target
}
{target:
- ItemNone:
~return "Cancel interaction."
- else:
~return "No description."
}
===function GetIcon(target)
{type_of(target)?String:
~return target
}
{target:
-ItemNone:
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