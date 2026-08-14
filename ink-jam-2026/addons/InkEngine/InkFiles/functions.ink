// Inventories

LIST portraits = Player

LIST backgrounds = Bck_None

LIST items = ItemNone, Test

LIST themes = Theme_Label_Default, Theme_Label_Dialogue, Theme_Label_Narrator

VAR global_temporary_variable = ()

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
//=continuePoint(itemChosen)
//~global_temporary_variable = player_current_inventory
//~player_current_inventory = targetInventory
~temp selectedItem = ()
~temp copyList = targetInventory
<-loop(continuePoint, copyList)

+ [Cancel #SET_INVENTORY:INVENTORY_CANCEL]
~selectedItem = ItemNone
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
//Use: ->ShowAnyItem(->continuePoint, targetinventory)
//=continuePoint(itemChosen)
//~global_temporary_variable = player_current_inventory
//~player_current_inventory = targetInventory
~temp copyList = targetInventory
<-loop(selectedItem, copyList)

+ [Cancel #SET_INVENTORY:INVENTORY_CANCEL]
~selectedItem = ItemNone
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
