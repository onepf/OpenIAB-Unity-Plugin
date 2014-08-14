OpenIAB-Unity-Plugin
====================

How to build plugin
====================

Before build plugin you must:

1. Setup JAVA_HOME environment variable
2. Install [Unity][1], start it and accept license agreement.
3. Unity must be closed while build is running.

For build project you must run from terminal in `unity_plugin` directory of project<br>
```groovy ../gradlew clean buildPlugin```<br>
On Windows run<br>
```groovy ..\gradlew.bat clean buildPlugin```

If build was successfully, you can find output unitypackage file in directory<br>

`unity_plugin/build/outputs`

[1]: https://unity3d.com/unity/download
