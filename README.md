Code Dump - Map Maker
=====================

Resurrection of an old tile-based map editor.

Building
--------

### Requirements
| Operating System     | Version         |
|----------------------|-----------------|
| Microsoft Windows 7  | SP1             |
| Microsoft Windows 10 | 1607 or later   |

| Dependency                   | Version                                       |
|------------------------------|-----------------------------------------------|
| Microsoft Visual Studio 2017 | 15.9.3 Community, Professional, or Enterprise |
| Microsoft .NET Framework     | 4.6.1                                         |

### Installing Requirements
Download and run Microsoft Visual Studio Installer, and include these components:
- Workload: .NET desktop development
- Individual Component: .NET Framework 4-4.6 development tools

### Building Map Maker from command line
Open up Developer Command Prompt for VS 2017, navigate to the solution directory, and run this command:
```
msbuild MapMaker.sln /t:Build /p:Configuration=Release
```

### Building Map Maker from VS 2017
1. Open up Microsoft Visual Studio 2017.
2. Open the solution file.
3. In the main menu, click `Build -> Rebuild Solution`.
