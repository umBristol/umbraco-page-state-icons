# Page State Icons

## Install

Use the Umbraco package installer which will add all the image/config/dlls for the standard page state icons.

You will need to refresh (`CTRL+R`) the window running Umbraco in order for the package to start working.

## Usage

There are 3 icons included by default, picking up on some Umbraco best practice properties

* Page hidden from menu (`umbracoNaviHide`)
* Redirect to another page (`umbracoRedirect`)
* Transfer to another page (`umbracoInternalRedirectId`)

If you want to add your own icons then you will need to add another rule to the config file. 
You will find the config file at `/config/PageStateIcons.config` and the rules need to be in the following format:

	<add name="umbracoNaviHide"
		 description="Adds overlay to show the page is hidden from navigation."
		 xPath="umbracoNaviHide = '1'"
		 overlayIconPath="/Umbraco/Plugins/PageStateIcons/UmbracoNaviHide.png"
		 left="11"
		 top="7" />

All of the attributes are **MANDATORY** and can be explained as follows...

`name`: Name your rule!

`description`: This doesn't get used anywhere but reminds you what the rule does...

`xPath`: This is the expression which needs to match in order for the icon to display (this works against published and unpublished content)

`overlayIconPath`: This is the path to the icon you wish to be displayed.

`left`: Pixels from the left of the `<li>` element that the icon should be placed.

`top`: Pixels from the top of the `<li>` element that the icon should be placed.
