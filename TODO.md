As some things do not work, this is what we have to do:

* Bug: Output Window:
	Code of ConsoleService was changed to async/await, somewhere in that process the output windows was gone.
	No Exception, just "ActivityWatch" Output Window isn't selectable, and no output is shown.
* Feature: NLog
	provide infos using NLog. Adding it for non-working Console Output first, so that we have a working output
* Bug: DisableTrackingCommand does nothing (maybe incoporate in Tool Window? Make a Command that opens the Tool Window?)
* Feature: Remote UI Framework / Tool Window
	A tool window seems to be the easies way to add configuration, do we want a tool window?
	Other options could be: Some external Window (not in VS Options)
	Feature described in https://devblogs.microsoft.com/visualstudio/visualstudio-extensibility/
* Some minor //TODO comments
* Some monor Warings in Code
* Update README from old project