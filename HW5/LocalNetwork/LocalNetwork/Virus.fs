module Virus

open OS

type Virus() =
    member val WindowsToWindows = 0.9 with get, set
    member val WindowsToLinux = 0.5 with get, set
    member val WindowsToMacOS = 0.2 with get, set
    member val LinuxToLinux = 0.75 with get, set
    member val LinuxToWindows = 0.4 with get, set
    member val LinuxToMacOS = 0.67 with get, set
    member val MacOSToMacOS = 0.34 with get, set
    member val MacOSToWindows = 0.15 with get, set
    member val MacOSToLinux = 0.8 with get, set
    
    member this.Check = function
        | (Windows, Windows) -> this.WindowsToWindows
        | (Linux, Linux) -> this.LinuxToLinux
        | (MacOS, MacOS) -> this.MacOSToMacOS
        | (Windows, Linux) -> this.WindowsToLinux
        | (Linux, Windows) -> this.LinuxToWindows
        | (Windows, MacOS) -> this.WindowsToMacOS
        | (MacOS, Windows) -> this.MacOSToWindows
        | (MacOS, Linux) -> this.MacOSToLinux
        | (Linux, MacOS) -> this.LinuxToMacOS
