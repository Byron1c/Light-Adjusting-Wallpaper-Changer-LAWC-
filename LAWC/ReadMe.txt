
LAWC 0.9.9.1 BETA
=====================================================================
Light Adjusting Wallpaper Changer
https://www.strangetimez.com/Blog/light-adjusting-wallpaper-changer-application/

This FREE app is designed to enhance your wallpapers' display in Windows. It will display the images 
in the way you want, how you want, and how often you want. You can filter images by brightness, size, 
and shape (aspect). LAWC can be set to minimise the amount of repeated wallpapers, and you can see 
which ones have been displayed already.

It can adjust the wallpaper brightness/tint (and others) to minimise glare, all the time, or at night, 
or whenever you choose to set it. You can also set the wallpaper to have a bright sky blue border 
during the day/light period to help with alertness.

Additionally you can display various information such as the weather, or information about your PC, 
and alerts for triggered events that you can set, including changing the wallpaper based on weather,
computer sensors, and others. There is also a tool to quickly rename wallpapers. 

See the Feature List below for all of the things LAWC can do.


Requirements
=====================================================================
* Windows 10 32bit or 64bit
* 60mb free disk space
* 100mb Free Memory (for approximately 10,000 images). Depends on the number of screens, and their sizes 
* (Optional) Administrator access to read some of the hardware sensors


Feature List
=====================================================================
General
* Free, and Open source. The code will be available once I have released V1.0 (and have cleaned it up!). If you would like it sooner, please contact me via my blog.
* Add and use multiple separate folders of images
* Random or sequential order for displaying the wallpapers, including prioritising wallpapers that have not been displayed before
* Use hot keys to do a wallpaper change (The default is CTRL+ALT+C)
* Keeps track of how often each wallpaper is displayed
* Adjust the overall tint, brightness, gamma, transparency, and contrast for wallpapers
* Set the adjustments based on the time of day (ie. set a dark time and a light time, and transition between them)
* Automatically set the Light and Dark times based on your chosen location's sunrise and sunset times
* Set how often the wallpaper will change
* Filter the list of images by brightness, pixel size, image file size, and aspect ratio
* Turn the wallpaper display on/off on for each screen, When using the "Different on All" setting for multiple monitors
* View the current weather report
* Settings are stored in an XML file for portability, easy editing
* LAWC can be run as a normal windows application (where settings are stored in the %appdata% folder),
    or it can be set (via command line, or the Advanced Settings) as Portable, where settings are stored in the same folder as the program
* Load and save settings to/from any location you want. Ie. You can have multiple configurations and load the one you want
* Automatic Backup, and ability to Restore settings
* A tool to maintain a list of websites cataloging where you can download more wallpapers, and when you last visited the sites
* 5700 images (4.9gb) added/processed in 12m20s on Dual Core i7 1.7ghz, win10, 8gb. (2010 model) 
* 5700 images uses approximately 80mb RAM
* Can handle 25,000+ images
* Adjust the image scale for the wallpaper to show a border, or zoom in more
* Detects and uses Dark Mode in Windows 10 to adjust the interface colours accordingly
* Turn off / on wallpapers for each screen
* A custom wallpaper mode (LAWC Mode) to automatically choose the best mode for each image
* Wallpapers are enhanced, and the enhancement is also adjusted based on the time of day - Dark or light 

Image Management
* Displays the resolution, average brightness, aspect ratio, and Display count from the list
* View / delete images
* Open or delete the current wallpaper file (via the notification icon in the system tray) 
* A tool to quickly and easily rename images, with a larger preview (right click on a Wallpaper to get started)
* Rename images in place (like File Explorer does)
* View the metadata for each image
* Enhance the Saturation, Brightness, and Colour balance - bluer and brighter during the day, and darker, and more orange at night

Events
* Can use data for weather, location, and computer hardware to display and alert you visually
* Display messages and values of sensors, on the desktop (embedded in the wallpaper)
* Display an image as the wallpaper, when sensor conditions are met (eg. When the CPU temperature is over 65 degrees C, display a chosen image)
* Trigger a notification / balloon message when the sensor conditions are met


Limitations / Issues
=====================================================================
* Tested with (only) 2 screens 
* Maximum update speed for most sensors etc is 1 second. 30 Seconds for weather, to reduce the load on OpenWeather.org's free service.
* All statistics on performance may vary depending on your PC, Screen(s) Resolution(s), OS version, each image's resolution, and your collection size.
* Images with odd pixel sizes (eg. 1000x1073) may cause a partial border 


Possible Enhancements (In no particular order):
=====================================================================
* Add an alternative wizard to set LAWC up with "Blue Light" settings
* Set a different fit/mode per wallpaper folder (eg. keep extra wide images in a separate folder to 16:9 etc)
* Track image history, so users can go back to the previous image
* Auto re-scan on content changes - detect file number and size changes to the folders 
* Set max limit per minute for how often Notifications can appear
* Web cam light detector
* Multiple time changes beyond dark / light
* Adjust image settings during the light stage? Ie. adjust brightness and contrast etc during light time
* Randomize / fuzzy light and dark times?  Any point?
* Highlight controls when there is an error (eg. Events - highlight empty fields)
* Blurred edges to pics that do not fill the screen
* Setting option to start as administrator, and/or a button
* Blur the wallpapers by a chosen amount. The idea being that it will make desktop icons and text easier to read with "busy" images
* Animate some values? (eg. animate the scale of a wallpaper over time - zoom in and out)
* Interface improvement - Change to multi value Sliders for aspect values, and LAWC mode settings
* Check for extremely large images, compare to all screen sizes (well, largest widest screen) - warn user if its excessive
* Event - Use a category dropdown in Events, to make getting the right sensor easier. eg, pick between Weather, HDD, Hardware, etc
* Event - option for graph instead of text (or text with graph)
* Event - to use image folder (multiple images) instead of single image?
* Event - Option to show more wallpaper metadata
* Event - Choose which screen shows each event message
* Event - Add option to put event messages and images on a selected monitor, all monitors, etc


TIPS
=====================================================================
* If you have started a folder scan, click on the progress bar to cancel it. Note: if you have told LAWC to scan *all* folders, then you will need to click on the progress bar for each one, to cancel it.
* Right click on the icon in the System Tray for options like deleting the current wallpaper, opening it, switching to Dark time, etc.
* Double click on the icon in the System Tray to change the wallpaper(s).
* Single click on the icon in the System Tray to show or hide LAWC.
* If you cannot see an image in the list, or it never appears as a wallpaper, have a look at the Filter options in Advanced Settings. Make sure you images are not being filtered out by 
* To quickly/easily delete the current wallpaper, right click on the LAWC icon in the System Tray, and choose Delete Image, and pick which screen.
* If the main window has dissapeared or is not looking correct, you can reset LAWC's size and position by right clicking on the icon in the system tray, and choosing "Reset Size and Position""
* If resetting the Settings (Options menu) does not work / do what you want, you can delete the settings.xml file (or named whatever you have selected)
* Clicking on the "Info" text at tbe bottom of the main screen will show you some useful details about your screens, and about LAWCs current settings
* Using a border with Tile fit will srink the image and tile it at the smaller size
* Command line parameters:
	-DebugOn 		This will record more verbose debug imformation into the error.log file
	-PortableOn  	This will force LAWC to operate as a Portable application, and 
					will create and use a Settings file in the same folder as the executable
    -PortableOff  	(Default) This will set LAWC to create and use a settings file in the %appdata% path, or whichever one you have loaded


Wallpaper Modes Explained
=====================================================================
https://www.andyrathbone.com/2010/01/13/filling-fitting-stretching-tiling-or-centering-your-desktop-background/

Center: This centers your image on the screen. If the image is small, you’ll see a solid-colored border around the edges. 
On large images, you’ll see the very center of the image, with the rest outside of view. 
Choose this for small logos, close-ups of spider webs, and similar things where the focus belongs on the center.

Fill Width: This shrinks or enlarges your photo to fit your monitor’s width. 
That keeps everything in perspective, but chops off the top and bottom of large images and often stretches tiny images beyond recognition.  
Choose this for horizon shots, where a photo’s top and bottom often aren’t missed.

Fit / Fill Height: Misleadingly named, the "Fit" option shrinks or enlarges your photo to fit your monitor’s height. 
Everything stays in perspective, but it lops off the sides of large images, and puts borders along the left and right sides of small images. I rarely use this one.

Stretch: Choose this when you must fit all of the photo on the screen, perspective be damned. This might work for some abstract or outdoors shots, 
but any easily recognizable objects like people or animals may look distorted as Windows stretches or shrinks them to meet your monitor’s confines.

Tile: Meant for small images or textures, this repeats the same image across the background like tile along a bathroom wall. You’ll find zillions 
of downloadable textures for tiling across your desktop.



History
=====================================================================
0.9.9.1  2019-11-24    BETA.
* Fixed issue with Open Weather not always reading data
* Added option to use own Open Weather API Key
* Improved messages when there is a problem with Open Weather
* Added / fixed compatibility with Win 8.1, 7 
* Improved basic settings layout for usability


0.9.9.0  2019-10-01    BETA.
* MAJOR FIX: Issue with multiple screens / different DPI settings not displaying correctly is now FIXED!!
* Improved preset dark settings to not look so terrible
* Set the "working" messages to show until the wallpaper change is complete
* Improved change speed during daylight hours
* Faster overall startup and performance improvements
* Fixed Reset Settings function issues
* Added the log file to the email that can be sent when an error is found
* Improved functionality in the Image Renaming tool
* Fixed issues with error reporting
* Updated to .NET 4.8


0.9.8.1  2019-06-18    BETA.
* More error handling
* Minor UI Improvements
* Updated to .NET 4.7.2


0.9.8.0  2019-05-13    BETA.
* Added HSV adjustment to wallpapers, making them bluer in light times, and more orange in dark times 
* Improved / fixed borders in different wallpaper modes
* Performance improvements
* Limited Weather updates to run a minimum of 2 minutes apart, to stop overloading the server
* Other sensors limited to run a minimum of 30 seconds apart
* Error handling for corrupted XML settings
* Fixed inconsistencies in the image adjustment sliders in Advanced Settings
* Interface and wizard cleanup, more error handling


0.9.7.4  2019-04-06    BETA.
* Editing wallpaper filenames in place is improved / fixed
* UI Improvements - Colours and indicators
* Improved / fixed centering, borders, and span position for wallpapers
* Added manual adjustment / offset for wallpaper images position
* Fixed issue with Loading and Saving images not using the correct folder
* Imroved technical information display (click on the "Info" label at the bottom of the main window)
* Shortened the image/wallpaper xml tags to speed up loading and saving
* Added "Internet Connection" availability, and "Website" check sensor (pings the server/website)


0.9.7.3  2019-03-08    BETA.
* What else... Many bug fixes!
* Added ability to edit individual filenames "in place" (in the wallpaper list, with a second mouse click)
* "Save Settings As" and "Load Settings" implemented
* Portable setting is fixed - it did not always save to the appropriate location
* Added an setting option "Multi Monitor Wallpaper Display" - Same wallpaper on all screens, or Different wallpapers on each screen
* Stopped errors incorrectly being logged with a fresh install, or reset settings
* UI Cleanup


0.9.7.2  2019-02-15    BETA.
* Option to send error reports and notes via email
* Code analysis and cleanup
* Show error messages in a better layout
* Added command line parameters
	-DebugOn This will record more verbose debug imformation into the error.log file
	-PortableOn  This will force LAWC to operate as a Portable application, and 
	           will create and use a Settings file in the same folder as the executable
    -PortableOff  This will set LAWC to create and use a settings file in the %appdata% path
* A bunch of minor UI and bug fixes


0.9.7.1  2019-02-05	   BETA.
* Fixed sorting on the wallpaper list
* Cleanup of error messages
* Improve the LAWC Mode and settings


0.9.7.0  2019-01-27	   BETA.
* UI cleanup and improvements, including "Working" messages
* Many minor bug fixes
* Re-implemented better error handling
* Added functionality to toggle Win10's Dark Mode and Transparency
* Fixed wallpapers displaying incorrectly when using LAWC mode


0.9.6.0  2019-01-01	   BETA.
* Performance issues with displaying the Images list (ListView)
* Converted LAWC to use OpenListView - faster and more features
* App testing with new OpenListView
* Performance issues with displaying/hiding main window - fixed.


0.9.5.0  2018-12-18	   BETA.
* Polishing interface, and cleaning up code
* Performance testing
* Changed default sample image to my beloved dog Miki, who passed away on the 9th of December :'(

0.9.2.0	 2018-11-27    BETA.
* Numerous improvements and fixes


0.9.1.0  2016-01-04    BETA.  
* Stopped the Donate message from popping up
* Hopefully fixed problem when changing the number of active screens


0.9.0.9  2015-10-25    BETA.  
* Added ability to blur edges of image
* Cleaned up interface issues
* Hopefully fixed a shutdown problem


0.9.0.8  2015-08-22    BETA.  
* Fixed problem with Brightness adjustments
* Removed "Recent Images" history function (until its fixed)


0.9.0.7  2015-05-29    BETA.  
* Trying to fix repeating changing of wallpaper


0.9.0.6  2014-09-10    BETA.  
* Fixed Ordered Images Option
* Added: Reset Settings
* Added: Clear Recent History


0.1.0.0  2014-06-17  
* Started