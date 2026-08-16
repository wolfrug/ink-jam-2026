INCLUDE functions.ink
INCLUDE FUNC_Data.ink
INCLUDE rooms.ink


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

Okay we got a sort of inventory that's nice right.
*/

VAR debug = false

->init


==init

~curItem = None
~startingInventory = (Multimeter, Metasocket, SonicScrewdriver, VoltometricPump, Neurojack)
~locations = (LocDock, LocControl, LocAdmin)
->addStartingItem(startingInventory)->
->debugStart

#hideWriter
{SetActive("SystemBox", false)}
->start

=addStartingItem(ref itemsLeft)
~temp randomItem = pop(itemsLeft)
~temp randomAmount = RANDOM(1,3)
{AddToDictionary(randomItem, randomAmount, inventory_stack_dictionary)}
{LIST_COUNT(itemsLeft)>0:
->addStartingItem(itemsLeft)
- else:
->->
}

==debugStart
This is the debug start. We need to add this here, or it doesn't work.

+ [Show map]
->ShowMap


==start

<Initializing GCCTL v. 1.00567. rev. 56> #SystemText
<>[br]<>
<Initializing Inventory System> #SystemText
<>[br]<>
<Initializing Chat System> #SystemText

... #wait.3 #SystemText

<Chat System Initialized> #showWriter #SystemText

... #wait.1.5 #SystemText

<Inventory System Initialized> {SetActive("SystemBox", true)} #SystemText

... #wait.1.5 #SystemText

<Initialization done.> #SystemText
<>[br]<>
<Good morning, Worker\#278.>
<>[br]<>
<Have a productive day!>

... #wait.3 #SystemText

<Incoming messages from Worker\#12, Worker \#567, Worker \#91> #SystemText

... #wait.2 #SystemText

->intro_dialogues

==intro_dialogues
{Say(Jeanne)}
Good morning, my lovelies! 

#wait.1

{Say(Marcus)}
Morning.

#wait.1.2

{Say(Amar)}
Another day on the clock.

* [Good morning.]
{Say(Player)}
Good morning.
* (silence1) [(Say nothing)]

* [How is everyone?]
{Say(Player)}
How is everyone?

{Say(Jeanne)}
Oh, just grand. I got my tea, I got an empty sheet of work orders, I got my sleep.
{Say(Marcus)}
I am still alive.
{Say(Amar)}
Thank you for asking, my friend! I am well.

- {Say(Jeanne)} So everyone's still alive then? {silence1: Except for our brave leader, who seems to still be asleep.}

{Say(Amar)} {silence1: Boss is probably just getting some coffee.|Have no doubt. S-Corp workers are hardy like that.}

{Say(Marcus)} Got into a fight.

* [Yep, still alive.]
{Say(Player)} Yep, still alive.

{silence1: {Say(Jeanne)} Oh, the Boss speaks! Thank Helix.}

* [You did what, Marcus?]
{Say(Player)} You did what, Marcus?
* (silence2) [(Say nothing)]

- {Say(Amar)} Marc, are you okay? What happened?

{Say(Marcus)} Fine. Other guy is in medbay, though.

{Say(Jeanne)} Helix save us...Boss? What do we do?

{silence1 && silence2:
{Say(Amar)} If the Boss is even there?

{Say(Jeanne)} Well it'll be a fun surprise for 'em when they read this.
}

* {silence1 && silence2} [I'm here. And it IS so fun.]
{Say(Player)} I'm here. And it IS so fun.

* [I'll handle it.]
{Say(Player)} I'll handle it.

* (tellme) [Tell me what happened.]
{Say(Player)} Tell me what happened.
-
{silence1 && silence2:
{Say(Jeanne)} Oh, thank Helix. I was getting worried.

{Say(Amar)} Morning, Boss. I won't say 'good' morning though.
}

{not tellme: {Say(Player)} Tell me what happened, Marcus.}

{Say(Marcus)} Not much to say. He was grunt from the Jaegers. Got tired of not having gravy and synthmeat every day I guess.

So he starts ranting 'bout the Corp and how we're being treated worse than animals and yada yada. I am trying to have a drink, so I tell him to shut his hole.

{Say(Jeanne)} Oh, Marcus...

{Say(Marcus)} He is fine. Little black, little blue, in a few days.

{Say(Amar)} If we don't get off this rock soon, it is only going to get worse...

<Incoming Requisition Request>[br]<Priority: Red>[br]<Stand by...> #SystemText

* [We have an incoming request.]
{Say(Player)} We have an incoming request.
* [You and me will have a talk later, Marcus.]
{Say(Player)} You and me will have a talk later, Marcus.

But we have to see to this new request now.

- {Say(Amar)} Ah, I do so love work. Lay it on us, Boss.

<Requisition Request For: SSP-907 (Mawker-Gleeson Thruster Heat Sink Mk. 3)>[br]<Requested By: Cmdr. Amada>[br]<Priority: Red> #SystemText

{Say(Jeanne)} Ohh...this is good news, isn't it? That's the thing we need.

{Say(Amar)} But...Boss...if this was something we had all along, why didn't they requisition this earlier?

* [We DON'T have it. System, update inventory.]

{Say(Player)}We DON'T have it. System, update inventory.




->DONE