# AssemblyDocumentation Class
> Parses a *.dll or *.exe file and generates it's documentation.

**Namespace:** DotNetDocs.ContainerDocumentations

**Assembly:** DotNetDocs (in DotNetDocs.dll v1.0.0)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[System.Object](https://www.google.com/search?q=System.Object&btnI=)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Documentation](/docs/DotNetDocs/Documentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ContainerDocumentations.ContainerDocumentation](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ContainerDocumentations.AssemblyDocumentation](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation.md)

## Syntax
```csharp
public class AssemblyDocumentation : DotNetDocs.ContainerDocumentations.ContainerDocumentation, System.IDisposable
```
## Constructors
|Name|Description|
|---|---|
|[Constructor(AssemblyDefinition, PEFile, XDocument, Boolean)](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Constructors/Constructor_AssemblyDefinition%2c%20PEFile%2c%20XDocu5520.md)||
## Properties
|Name|Description|
|---|---|
|[AssemblyDefinition](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/AssemblyDefinition.md)|Gets the representation of the underlying assembly from Mono.Cecil.|
|[Children](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/Children.md)|Gets the children for the current container.|
|[FileName](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/FileName.md)|Gets the file name for the underlying assembly file.|
|[FilePath](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/FilePath.md)|Gets the file path to the documented assembly.|
|[FullName](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/FullName.md)||
|[Name](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/Name.md)|Gets the assembly name.|
|[Namespaces](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/Namespaces.md)|Gets a list of the namespaces contained within the assembly.|
|[Decompiler](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/Decompiler.md)|Gets an instance of the C# decompiler used to generate the declarations.|
|[DocumentationsToDispose](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/DocumentationsToDispose.md)|Gets a lists of the assembly documentations to dispose.|
|[PEFile](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Properties/PEFile.md)|Gets a representation of the underlying assembly from System.Reflection.Metadata.|
## Methods
|Name|Description|
|---|---|
|[Parse(String)](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Methods/Parse_String_.md)|Parses the assembly given its assembly and XML paths.|
|[Load(AssemblyDefinition, Boolean)](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Methods/Load_AssemblyDefinition%2c%20Boolean_.md)||
|[Dispose()](/docs/DotNetDocs/ContainerDocumentations/AssemblyDocumentation/Methods/Dispose__.md)||
