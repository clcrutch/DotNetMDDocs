# AssemblyDocumentation Class
> Parses a *.dll or *.exe file and generates it's documentation.

**Namespace:** DotNetDocs

**Assembly:** DotNetDocs (in DotNetDocs.dll)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;System.Object

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;DotNetDocs.AssemblyDocumentation

## Syntax
```csharp
public class AssemblyDocumentation : System.IDisposable
```
## Constructors
|Name|Description|
|---|---|
|[.ctor(AssemblyDefinition, PEFile, XDocument, FileInfo)](/docs/DotNetDocs/AssemblyDocumentation/Constructors/.ctor_AssemblyDefinition%2c%20PEFile%2c%20XDocument%2c%204892.md)|Initializes a new instance of the  class.|
## Properties
|Name|Description|
|---|---|
|[Decompiler](/docs/DotNetDocs/AssemblyDocumentation/Properties/Decompiler.md)|Gets an instance of the C# decompiler used to generate the declarations.|
|[FileName](/docs/DotNetDocs/AssemblyDocumentation/Properties/FileName.md)|Gets the file name for the underlying assembly file.|
|[Name](/docs/DotNetDocs/AssemblyDocumentation/Properties/Name.md)|Gets the assembly name.|
|[PEFile](/docs/DotNetDocs/AssemblyDocumentation/Properties/PEFile.md)|Gets a representation of the underlying assembly from System.Reflection.Metadata.|
|[Types](/docs/DotNetDocs/AssemblyDocumentation/Properties/Types.md)|Gets a list of all of the documented types.|
|[AssemblyDefinition](/docs/DotNetDocs/AssemblyDocumentation/Properties/AssemblyDefinition.md)|Gets the representation of the underlying assembly from Mono.Cecil.|
|[AssemblyFileInfo](/docs/DotNetDocs/AssemblyDocumentation/Properties/AssemblyFileInfo.md)|Gets the  for the underlying assembly.|
## Methods
|Name|Description|
|---|---|
|[Parse(String, String)](/docs/DotNetDocs/AssemblyDocumentation/Methods/Parse_String%2c%20String_.md)|Parses the assembly given its assembly and XML paths.|
