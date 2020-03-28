# Major Scientist

MajorScientist(o) is a SCP:SL Plugin. It requires [EXILED] to work.

Adds 'Major Scientist' to the game. Every round, all Scientists(or Class-Ds) have a chance to spawn as a Major Scientist. Major Scientist starts with better items and SCP-207 effects. What's more, if the major scientist is alive, round doesn't end! No more escape failure because of the extermination of scps. But there is a penalty - if major scientist dies, the MTF can't win. It will only make draw or SCP win or D-class win.
Also when you spawn as a major scientist, you will have a badge named 'Major Scientist'.

For now, this is very initial version of the plugin. I will add some configurable options to it in future.

# Installation

Just place .dll file in your Plugin folder.

# Configs

| Config        | Value Type | Default Value | Description |
| :-------------: | :---------: | :------: | :--------- |
| ms_health | Int | 150 | Max health of major scientist. |
| ms_spawn_chance | Int | 20 | It's a percent chance for a Major Scientist to spawn. This is not a chance for each Class-Ds or Scientists. It is calculated only once when a round starts. |
| ms_spawn_items | List | 2, 13, 17 | The ID list of items which the major scientist has when a round starts. | 
| ms_classd_replace_scientist | Bool | false | If this is set to true, one of the Class-Ds can be replaced with major scientist. It allows you to test this when you're alone in the server! |
| ms_getlog | Bool | False | If this is set to true, your console will send some logs: "MajorScientist has spawned", "MajorScientist has escaped", "MajorScientist has died". |























[EXILED]: https://github.com/galaxy119/EXILED
