INCLUDE functions.ink
INCLUDE FUNC_Data.ink

VAR debug = false

VAR testClock = ()

VAR testarray = ()
VAR testresiduapuzzle = ()

VAR inventory_stack_dictionary = ""

VAR testItem = ()


->init
==init

{CustomUI("SystemText")} What is Lorem Ipsum?

{CustomUI("SystemText")}Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James Mosley, the librarian at St Bride Printing Library in London, took a 1914 Cicero translation and scrambled it to make dummy text for Letraset's Body Type sheets.

{CustomUI("SystemText")}It has survived not only many decades, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised thanks to these sheets and more recently with desktop publishing software like Aldus PageMaker and Microsoft Word including versions of Lorem Ipsum.

* And they

* Lived happily

* And they

* Lived happily

* And they

* Lived happily

- Ever after.

->DONE
