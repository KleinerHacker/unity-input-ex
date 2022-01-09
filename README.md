# unity-input-ex
Extension for new input system of Unity

# install
Use this repository in Unity directly.

### Dependencies
* New Input System
* https://github.com/KleinerHacker/unity-editor-ex

### Open UPM
URL: https://package.openupm.com

Scope: org.pcsoft

# usage
Inherite from EventInputSystem class. This component is loaded automatically at startup.

### Extensions
* Common: You can check input device is available via `isAvailable`. It can work with `null` values.
* Keyborad: You can read Vector2 values for `Arrows`, `Numpad` and `WASD`
