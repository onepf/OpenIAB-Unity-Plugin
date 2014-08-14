OpenIAB-Unity-Plugin
====================

How to build plugin
====================

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
