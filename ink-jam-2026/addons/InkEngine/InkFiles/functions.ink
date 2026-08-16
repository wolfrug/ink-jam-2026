// Inventories

LIST portraits = Player

LIST backgrounds = Bck_None

LIST characters = NoChar, Jeanne, Amar, Marcus

LIST characterStates = Available, Unavailable, Busy, Ready

LIST items = None, Multimeter, Metasocket, VoltometricPump, SonicScrewdriver, Ansible, Neurojack, CombatBiosoft, UtilityBiosoft, Ubik, HeatSink

LIST locations = LocControl, LocDock, LocMess, LocTool, LocEngineering, LocBarrack, LocArmory, LocHead, LocAdmin, LocRecroom

LIST tasks = NoTask, TaskMain, TaskRepair, TaskTrade, TaskSteal

LIST themes = Theme_Label_Default, Theme_Label_Dialogue, Theme_Label_Narrator

VAR global_temporary_variable = ()

VAR inventory_stack_dictionary = ""

VAR crew_stack_dictionary = ""

VAR task_stack_dictionary = ""

VAR curItem = ()
VAR curChar = ()
VAR curTask = ()

VAR startingInventory = ()

EXTERNAL EXT_AddToList(x,y)
EXTERNAL EXT_RemoveFromList(x,y)
EXTERNAL EXT_AddToDictionary(x,y,z)
EXTERNAL EXT_RemoveFromDictionary(x,y)
EXTERNAL EXT_HasValue(x,y)
EXTERNAL EXT_GetValue(x,y)
EXTERNAL EXT_GetValueInt(x,y)
EXTERNAL EXT_GetValueFloat(x,y)
EXTERNAL EXT_Count(x)

===function EXT_AddToList(x,y)
//[Added {x} to the list {y}]
~return y
===function EXT_RemoveFromList(x,y)
//[Removed {x} from the list {y}]
~return y
===function EXT_AddToDictionary(x,y,z)
//[Adds the Key Value Pair ({x},{y}) to {z}]
~return z

===function EXT_RemoveFromDictionary(x,y)
//[Removes the key and value of key {x} from {y}]
~return y
===function EXT_HasValue(x,y)
//[Checks if {x} exists in list {y}]
~return true
===function EXT_GetValue(x,y)
//[Gets either the index {x} or the key {x} from list {y}]
~return x
===function EXT_GetValueInt(x,y)
//[Gets either the index {x} or the key {x} from list {y} and tries to parse it to int]
~return 0
===function EXT_GetValueFloat(x,y)
//[Gets either the index {x} or the key {x} from list {y} and tries to parse it to float]
~return 0
===function EXT_Count(x)
//[Gets the number of entries in list x]
~return 0

===function AddToList(key, ref list)
~list = EXT_AddToList(key, list)
===function RemoveFromList(key, ref list)
~list = EXT_RemoveFromList(key, list)
===function AddToDictionary(key, value, ref list)
~list = EXT_AddToDictionary(key, value, list)
===function RemoveFromDictionary(key, ref list)
~list = EXT_RemoveFromDictionary(key, list)
===function HasValue(key, ref list)
~return EXT_HasValue(key, list)
===function GetValue(key, ref list)
~return EXT_GetValue(key, list)
===function GetValueInt(key, ref list)
~return EXT_GetValueInt(key, list)
===function GetValueFloat(key, ref list)
~return EXT_GetValueFloat(key, list)
===function Count(ref list)
~return EXT_Count(list)

===function CustomUI(tag)
#{tag}

===function SetActive(obj, active)
#{obj}^{active}

===function SetTheme(theme)
#SET_THEME:{GetTheme(theme)}

===function SetButtonIcon(icon)
#SET_ICON:{GetIcon(icon)}

===function SetCameraTarget(target, zoom, speed)
// NB: target can either be an ID set on Ink Background or coordinates in format "45,-45"
#SET_CAMERA_TARGET:{target}^{zoom}^{speed}

===function SetPortrait(portrait)
#SET_PORTRAIT: {GetIcon(portrait)}

===function SetSpriteBackground(background)
#SET_SPRITEBACKGROUND:{GetIcon(background)}

===function SetBackground(background)
#SET_BACKGROUND:{GetIcon(background)}

===function PlayMusic(music)
#PLAY_MUSIC:{music}


// Dialogues
===function Say(character)
 [right]{GetDisplayName(character)}[/right][hr][br][left]<>

===function SetCrewStatus(crew, status)
~temp crewStatus = GetCrewStatus(crew)
{not (crewStatus?status):
{AddToDictionary(crew, status, crew_stack_dictionary)}
{GetDisplayName(crew)} is now {status}.
}

===function GetCrewStatus(crew)
~temp stringstatus = GetValue(crew, crew_stack_dictionary)
{type_of(stringstatus)?List:
~return stringstatus
}
{stringstatus:
- "Available":
~return Available
- "Unavailable":
~return Unavailable
- "Busy":
~return Busy
- "Ready":
~return Ready
- else:
~return Unavailable
}
// TASK!
===function AddTask(item, amount, ref inventory)===
{not (inventory?item):
~inventory+=item
{AddToDictionary(item, amount, task_stack_dictionary)}
New Task Received: {GetDisplayName(item)}.
- else:
~temp currentValue = CountTask(item, inventory)
~currentValue+=amount
{AddToDictionary(item, currentValue, task_stack_dictionary)}
Task ({GetDisplayName(item)}) Progress: {currentValue}.
}

===function RemoveTask(item, amount, ref inventory)===
~temp change = 0
{inventory?item:
~temp currentValue = CountTask(item, inventory)
{currentValue>=amount:
~currentValue-=amount
~change = amount
- else:
~change = currentValue
~currentValue = 0
}
{currentValue>0:
{AddToDictionary(item, currentValue, task_stack_dictionary)}
Removed Task ({GetDisplayName(item)}) Progress: {currentValue}.
- else:
{RemoveFromDictionary(item, task_stack_dictionary)}
~inventory-=item
[color=red]Task Failed: {GetDisplayName(item)}![/color]
}
}

===function CountTask(item, inventory)===
{inventory?item:
~return GetValueInt(item, task_stack_dictionary)
-else:
~return 0
}

===function CompleteTask(task, inventory)
{inventory?task:
{AddToDictionary(task, 100, task_stack_dictionary)}
[color=green]Completed Task: {GetDisplayName(task)}[/color]
}

// INVENTORY!
===function AddToInventory(item, amount, ref inventory)===
{not (inventory?item):
~inventory+=item
{AddToDictionary(item, amount, inventory_stack_dictionary)}
Received {amount} {GetDisplayName(item)}.
- else:
~temp currentValue = CountItem(item, inventory)
~currentValue+=amount
{AddToDictionary(item, currentValue, inventory_stack_dictionary)}
Received {amount} {GetDisplayName(item)}.
}

===function RemoveFromInventory(item, amount, ref inventory)===
~temp change = 0
{inventory?item:
~temp currentValue = CountItem(item, inventory)
{currentValue>=amount:
~currentValue-=amount
~change = amount
- else:
~change = currentValue
~currentValue = 0
}
{currentValue>0:
{AddToDictionary(item, currentValue, inventory_stack_dictionary)}
- else:
{RemoveFromDictionary(item, inventory_stack_dictionary)}
~inventory-=item
}
Lost {amount} {GetDisplayName(item)}.
}

===function CountItem(item, inventory)===
{inventory?item:
~return GetValueInt(item, inventory_stack_dictionary)
-else:
~return 0
}

===function IsInteractable(b)
{not b:
#DISABLED
}

LIST Type = List, String, Number
=== function type_of(val)
    {"{val + val}":
        - "{val}{val}":
            {val ? val:
                ~ return String
            }
            ~ return List // empty
        - "{val}":
            {"{val}" == "0":
                ~ return Number // zero
            }
            ~ return List
        - else:
            ~ return Number
    }

===function UseItem(item, enabled, amount)
{GetDisplayName(item)} #SET_INVENTORY:{GetDescription(item)}^{GetIcon(item)}^{enabled}^{amount}

/// Functions
=== function pop(ref _list) 
    ~ temp el = LIST_MIN(_list) 
    ~ _list -= el
    ~ return el 

===ShowAnyItem(->continuePoint, targetInventory)
//Use: ->ShowAnyItem(->continuePoint, targetinventory)
~temp selectedItem = ()
~temp copyList = targetInventory
<-loop(continuePoint, copyList)

+ [Cancel #SET_INVENTORY:INVENTORY_CANCEL]
~selectedItem = None
->continuePoint(selectedItem)

=loop(->continuePoint, copyList)
{LIST_COUNT(copyList)>0:
~temp item = pop(copyList)
<-addItem(item, continuePoint)
->loop(continuePoint, copyList)
}

=addItem(targetItem, ->continuePoint)
~temp count = GetValueInt(targetItem, inventory_stack_dictionary)
+ [{UseItem(targetItem, true, count)}]
//~player_current_inventory = global_temporary_variable
->continuePoint(targetItem)

===ShowAnyItemTunnel(targetInventory, ref selectedItem)
//Use: ->ShowAnyItem( targetinventory, outitem)
~temp copyList = targetInventory
<-loop(selectedItem, copyList)

+ [Cancel #SET_INVENTORY:INVENTORY_CANCEL]
~selectedItem = None
->->

=loop(ref selectedItem, copyList)
{LIST_COUNT(copyList)>0:
~temp item = pop(copyList)
<-addItem(item, selectedItem)
->loop(selectedItem, copyList)
}

=addItem(targetItem, ref selectedItem)
~temp count = GetValueInt(targetItem, inventory_stack_dictionary)
+ [{UseItem(targetItem, true, count)}]
//~player_current_inventory = global_temporary_variable
~selectedItem = targetItem
->->

// Map!
===ShowMap
#hideWriter
{SetActive("MapControl", true)}
#INK_UI_DISABLE_ALL_BUTTONS
->ShowCustomButtonUI(->continue, locations)

=continue(room)
{SetActive("MapControl", false)}
#showWriter #continue #clear
{room:
- LocControl:
->Control
- LocDock:
->Dock
- LocMess:
->Mess
- LocTool:
->Tool
- LocEngineering:
->Engineering
- LocBarrack:
->Barrack
- LocArmory:
->Armory
- LocHead:
->Head
- LocAdmin:
->Admin
- LocRecroom:
->Recroom
- else:
(This should never happen. Sorry!)
->DONE
}


===ShowCustomButtonUI(->continuePoint, targetInventory)
//Use: ->ShowCustomButtonUI(->continuePoint, targetinventory)
~temp selectedItem = ()
~temp copyList = targetInventory
<-loop(continuePoint, copyList)

+ ->
~selectedItem = None
->continuePoint(selectedItem)

=loop(->continuePoint, copyList)
{LIST_COUNT(copyList)>0:
~temp item = pop(copyList)
<-addItem(item, continuePoint)
->loop(continuePoint, copyList)
}

=addItem(targetItem, ->continuePoint)
~temp count = GetValueInt(targetItem, inventory_stack_dictionary)
+ [{GetDisplayName(targetItem)}{CustomUI(targetItem)}]
->continuePoint(targetItem)
