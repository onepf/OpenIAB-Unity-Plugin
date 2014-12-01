OpenIAB Plugin. Open In-App Billing for Android, iOS and Windows Phone 8
====================

##About
Supporting in-app purchases for different platforms and stores is not a simple process. The developer has to study new API for each new store that he/she wants to use in an application.<br /><br />
OpenIAB plugin enables Unity developers to reduce integration and maintenance time. The plugin uses one common interface for 3 mobile platforms: Android, iOS and Windows Phone 8. It’s based on [OpenIAB library](https://github.com/onepf/OpenIAB "OpenIAB") developed by One Platform Foundation team. <br /><br />
OpenIAB plugin comes with full source code under Apache 2.0 license.<br /><br />
Supported Stores:<br />
**Android:**<br />
    - [Google Play](https://play.google.com/store "Google Play")<br />
    - [Amazon Appstore](http://www.amazon.com/mobile-apps/b?node=2350149011 "Amazon Appstore")<br />
    - [Yandex.Store](http://store.yandex.com/ "Yandex.Store")<br />
    - [Samsung Apps](http://apps.samsung.com/earth/main/getMain.as?COUNTRY_CODE=BLR "Samsung Apps")<br />
    - [Nokia Store](http://developer.nokia.com/nokia-x/publish-your-app "Nokia Store")<br />
    - [SlideMe](http://slideme.org/ "SlideMe")<br />
    - [Appland](http://www.applandinc.com/app-store/ "Appland")<br />
    - [Aptoide](http://m.aptoide.com/ "Aptoide")<br />
    - [AppMall](http://www.openmobileww.com/#!appmall/cunq "AppMall")<br />

**iOS:**   [Apple Store](https://itunes.apple.com/en/genre/mobile-software-applications/id36?mt=8 "Apple Store")

**Windows phone 8:**   [Windows Phone Store](http://www.windowsphone.com/en-us/store "Windows Phone Store")

##Version
The current version is 0.9.7.1.

##Tutorial
For a comprehensive tutorial, Check-out the complete turorial on [master](https://github.com/onepf/OpenIAB-Unity-Plugin/blob/master/unity_plugin/unity_src/Assets/Plugins/OpenIAB_manual.pdf?raw=true) here.

##Sample project
You can check out this sample project [here](https://github.com/GrimReio/OpenIAB-sample-game).

##How to build plugin
Plugin build view [Gradle][2]. You no need to download binary package of gradle tools, only requirements
is installed JDK version 1.6 or great and setup JAVA_HOME environment variable.<br>
<br>
Before build plugin you must: <br>
1. Install [Unity][1], start it and accept license agreement.
2. Unity must be closed while build is running.

For build project you must run from terminal in `unity_plugin` directory of project<br>
```groovy
../gradlew clean buildPlugin
```
On Windows run<br>
```groovy
..\gradlew.bat clean buildPlugin
```

If build was successfully, you can find output unitypackage file in directory<br>

`unity_plugin/build/outputs`

Build tools search unity by default path for OS. If you change default path, you need to set
path `unityExecutable` property  in `gradle.properties` file. 

[1]: https://unity3d.com/unity/download
[2]: http://www.gradle.org

##Options
Create and set up an Options object.
```
var options = new Options();
options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;
```
Set available stores to restrict the set of stores to check.
```
options.availableStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_Android.STORE_YANDEX };
```
Set preferred store names (works only for store search strategy ```OpenIabHelper.Options.SEARCH_STRATEGY_BEST_FIT``` and ```OpenIabHelper.Options.SEARCH_STRATEGY_INSTALLER_THEN_BEST_FIT```).
```
options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_Android.STORE_YANDEX };
```
Set verifying mode (applicable only for Google Play, Appland, Aptoide, AppMall, SlideMe, Yandex.Store).
```
options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
```
Init with specified options.
```
OpenIAB.init(options);
```

##Suggestions/Questions
We seek to constantly improve our project and give Unity developers more power when working with in-app purchases. We are open to any comments and suggestions you may have regarding the additional features to be implemented in our plugin.

If you know about issues we missed, please let us know in  
	Issues on GitHub: https://github.com/onepf/OpenIAB/issues<br />
or       by email: unitysupport@onepf.org

If you detect some issues with integration into your project, please let us know in 
	http://stackoverflow.com/questions/ask/advice?
	
```
When you post a question on stackoverlow, mark your post with the following tags
“in-app purchase”, “unity3d”, “openiab”. It will help you to get a faster response
from our community.
```

##License
The source code of the OpenIAB Unity plugin and OpenIAB library are available under the terms of the Apache License, Version 2.0:
http://www.apache.org/licenses/LICENSE-2.0

The OpenIAB API specification and the related texts are available under the terms of the Creative Commons Attribution 2.5 license:
http://creativecommons.org/licenses/by/2.5/

	
