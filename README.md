# NetUtilities
A bunch of extension methods that may be useful for someone.

Someday available on NuGet.

[![Build status](https://ci.appveyor.com/api/projects/status/xomp3jvculiodyd3/branch/master?svg=true)](https://ci.appveyor.com/project/JustNrik/netutilities/branch/master)

## Installation

To install the NetUtilities library in your C# project, you can use NuGet Package Manager:

1. Open the NuGet Package Manager in Visual Studio Code
2. Search for "NetUtilities" and click "Install"

Alternatively, you can install via the .NET CLI:

```bash
dotnet add package NetUtilities

### Example for Usage:

Below are some examples of how you can use the utilities in NetUtilities:

#### Validating an IP Address

```csharp
using NetUtilities;

bool isValid = IPAddressValidator.Validate("192.168.1.1");
Console.WriteLine(isValid); // Output: true

#### Checking Network Status

using NetUtilities;

bool isOnline = NetworkUtilities.IsNetworkAvailable();
Console.WriteLine(isOnline); // Output: true if network is available, false otherwise