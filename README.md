# SensorBlockFix
A torch plugin that fix MySensorBlock data validate issue.

If player use modified client to send a very big value of LeftExtend, RightExtend or other options in MySensorBlock UI. Than call `IMySensorBlock.DetectedEntities(List)` method in ProgramBlock, player will get as many grids' position as he want.

This plugin fix it.

# License
Apache 2.0
