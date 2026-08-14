INCLUDE functions.ink
INCLUDE FUNC_Data.ink

/*
Notes and brainstorms
Theme: Let that SINK in.
Heatsink, kitchen sink, etc. Literal interpretation. Funny but maybe not.
More direct: a statement said to someone, let that (revelation, etc) sink in. What does that mean though, to let it sink in? Realize the horror, realize the consequences?
More generally, not as a direct statement, a story that inspires one to let it sink in, having perhaps a longer tail of a sort?
Can also mostly ignore the theme and just go with whatever.
Okay you are speaking through or to a computer. Why? Who are you? Maybe a good question to ask to begin with, idk. But we should know, so...we need characters. Character-driven above all. Okay.
Mechanics on a ship? Yeah. Sending messages between each other, chatting-like, while doing other work? Could work. Sort of a public bulletin board. But what is the mechanic (hah), what are we -doing-? We can be one of the mechanics, and whenever there's an opening we can say something, oxenfree-style?

Complicated coding that tho. Except I guess we can always go "...". Maybe make a time-out function? That could be useful anyway.

Okay, so what then? You say something, you don't say something, what's the difference? 

Maybe we finally do the 'lend tools' thing? Need an inventory function. Follow the same plot as before, but more hammers and shit. Idk. Maybe.
*/

VAR debug = false

VAR testarray = ()

VAR inventory_stack_dictionary = ""

VAR curItem = ""

VAR testItem = ()

->init

===function ST()
{CustomUI("SystemText")}


==init
//#hideWriter
{SetActive("SystemBox", false)}

->start

==start

Hupp. Currently selected item is: {curItem}
+ [Add another]
{AddToInventory(Test, 1, items)}
+ [Remove 1]
{RemoveFromInventory(Test, 1, items)}
- ->start


->DONE