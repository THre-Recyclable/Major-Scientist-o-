# Major Scientist

MajorScientist(o) is a SCP:SL Plugin. It requires [EXILED] to work.

Adds 'Major Scientist' to the game. Every round, all Scientists(or Class-Ds) have a chance to spawn as a Major Scientist. Major Scientist starts with better items and SCP-207 effects. What's more, if the major scientist is alive, round doesn't end! No more escape failure because of the extermination of scps. But there is a penalty - if the major scientist dies, the MTF can't win. It will only make draw or SCP win or D-class win.
Also when you spawn as a major scientist, you will have a badge named 'Major Scientist'.

For now, this is very initial version of the plugin. I will add some configurable options to it in future.

# Installation

Just place .dll file in your Plugin folder.

# Configs

| Config        | Value Type | Default Value | Description |
| :-------------: | :---------: | :------: | :--------- |
| ms_enabled | Bool | true | Enables/disables the MajorScientist(o). |
| ms_health | Int | 150 | Max health of major scientist. |
| ms_spawn_chance | Int | 20 | It's a percent chance for a major Scientist to spawn. This is not a chance for each Class-Ds or Scientists. It is calculated only once when a round starts. |
| ms_spawn_items | List | [2, 13, 17] | The ID list of items which the major scientist has when a round starts. | 
| ms_classd_replace_scientist | Bool | false | If this is set to true, one of the Class-Ds can be replaced with major scientist. It allows you to test this when you're alone in the server! |
| ms_getlog | Bool | false | If this is set to true, your console will send some logs: "MajorScientist has spawned", "MajorScientist has escaped", "MajorScientist has died". |
| ms_use_207 | Bool | true | Should major scientist use SCP-207 effect on round start. |
| ms_round_continue | Bool | true | If this is set to true, the round won't end if the major scientist is alive. |
| ms_vip | Bool | true | If this is set to true, MTF can't win if the major scientist has died. |
| ms_ammo_box | String | 150:150:150 | The amount of ammo the major scientist has on round start. It should be a form of int:int:int. |
| ms_badge | String | Major Scientist | Name of badge. |
| ms_replace_string | String |  | String will be displayed when some class-D is replaced with major scientist. It has default value, but you can customize it too. You can use <color="yellow"></color>, etc. |
| ms_spawn_string | String |  | String will be displayed when someone spawn as a major scientist. It has default value, but you can customize it too. You can use <color="yellow"></color>, etc. |
| ms_use_dm | Bool | false | String will be displayed to every player when a major scientist dies. |
| ms_death_message | String |  | This string will be displayed when ms_use_dm is set to true. It has default value, but you can customize it too. You can use <color="yellow"></color>, etc. |





















[EXILED]: https://github.com/galaxy119/EXILED
