# SharpShell

[SharpShell](https://github.com/cobbr/SharpShell) is a proof-of-concept offensive C# scripting engine that utilizes the [Rosyln](https://github.com/dotnet/roslyn) C# compiler to quickly cross-compile .NET Framework console applications or libraries.

SharpShell is broken up into three distinct C# projects:

* **SharpShell** - `SharpShell` is the most straightforward of the three projects. It acts as an interactive shell and scripting engine for C# code compiled against chosen source code, references, and resources. The main caveat with `SharpShell` is that it depends upon .NET Framework 4.6 and 3.5/4.0 being installed on the system. This is because `SharpShell` depends upon the Roslyn API, which requires 4.6, and executes an assembly in memory cross-compiled for you choice of versions 3.5 or 4.0.
* **SharpShell.API** - `SharpShell.API` and `SharpShell.API.SharpShell` are two projects meant to be used in tandem. To avoid the opsec limitations and .NET Framework 4.6 requirements of `SharpShell`, `SharpShell.API` acts as a web-server that handles the compilation for `SharpShell.API.SharpShell`. `SharpShell.API` is a ASP.NET Core 2.1 application and is cross-platform.
* **SharpShell.API.SharpShell** - `SharpShell.API.SharpShell` provides the same interface as `SharpShell`, but doesn't have the .NET Framework 4.6 requirement. `SharpShell.API.SharpShell` runs on .NET Framework 3.5, but also requires network communication with a `SharpShell.API` server for handling compilation of assemblies.

### Intro

You'll find additional details about the SharpShell project in this [introductory blog post](https://cobbr.io/SharpShell.html).

### Quick Start

Start up the standalone `SharpShell` and execute C# one-liners that compile against [SharpSploit](https://github.com/cobbr/SharpSploit):

```
PS C:\Users\cobbr\Demos\SharpShell\SharpShell\bin\Release> .\SharpShell.exe
SharpShell > Shell.ShellExecute("whoami");
desktop-f9dq76g\cobbr

SharpShell > using (Tokens t = new Tokens()) { \
>>>            return t.WhoAmI(); \
>>>          }
DESKTOP-F9DQ76G\cobbr
SharpShell > Mimikatz.Command("coffee");

  .#####.   mimikatz 2.1.1 (x86) built on Oct 22 2018 16:27:15
 .## ^ ##.  "A La Vie, A L'Amour" - (oe.eo) ** Kitten Edition **
 ## / \ ##  /*** Benjamin DELPY `gentilkiwi` ( benjamin@gentilkiwi.com )
 ## \ / ##       > http://blog.gentilkiwi.com/mimikatz
 '## v ##'       Vincent LE TOUX             ( vincent.letoux@gmail.com )
  '#####'        > http://pingcastle.com / http://mysmartlogon.com   ***/

mimikatz(powershell) # coffee

    ( (
     ) )
  .______.
  |      |]
  \      /
   `----'

SharpShell >
```
