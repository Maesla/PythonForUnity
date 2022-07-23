# PythonForUnity
Python Plugin System for Unity

This project implements a full plugin system with python as scripting language.
You can create your own python scripts and run them on Unity.
You can:
* Run python scripts
* Run Unity Code from python scripts
* Use your own project classes inside the python scripts




https://user-images.githubusercontent.com/25863696/180600448-75b5e017-27b4-4fad-8236-c4baf40ae216.mp4


# External Tools
## Extenject
This project uses advanced options of Extenject.
https://github.com/Mathijs-Bakker/Extenject
That said, the plugin system, the microkernel and the Python runner and all the core systems are quite decoupled from Extenject. Extenject is mainly used for setting the scene and the components

## Pythonnet
Pythonnet is the library used to run python code inside the .net environment
https://github.com/pythonnet/pythonnet

# Setup
Go to Assets/StreamingAssets/Plugins/MicrokernelSystemSettings.json and set your python path

# Known issues
* The system is prepared to implement a ui part, but it's not implemented right now. The example provided is a json ui but probably using the new uxml unity ui would be a better option
* Python virtual environment are not supported, but pythonnet supports them so it should be possible to add that feature
