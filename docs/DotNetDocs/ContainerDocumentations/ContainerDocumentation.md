# ContainerDocumentation Class
> Represents a container documentation.  This includes assemblies, namespaces, and types.

**Namespace:** DotNetDocs.ContainerDocumentations

**Assembly:** DotNetDocs (in DotNetDocs.dll v1.0.0)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[System.Object](https://www.google.com/search?q=System.Object&btnI=)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Documentation](/docs/DotNetDocs/Documentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ContainerDocumentations.ContainerDocumentation](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation.md)

## Syntax
```csharp
public abstract class ContainerDocumentation : DotNetDocs.Documentation, DotNetDocs.Mixins.Contracts.IContainerDocumentation, DotNetDocs.Mixins.Contracts.IDocumentation
```
## Constructors
|Name|Description|
|---|---|
|[Constructor()](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation/Constructors/Constructor__.md)||
## Properties
|Name|Description|
|---|---|
|[Children](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation/Properties/Children.md)|Gets the children for the current container.|
|[ContainerDocumentationMixin](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation/Properties/ContainerDocumentationMixin.md)|Gets the[DotNetDocs.ContainerDocumentations.ContainerDocumentation.ContainerDocumentationMixin](https://www.google.com/search?q=DotNetDocs.ContainerDocumentations.ContainerDocumentation.ContainerDocumentationMixin&btnI=)which backs the current container.|
|[Item](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation/Properties/Item.md)||
## Methods
|Name|Description|
|---|---|
|[Contains(String)](/docs/DotNetDocs/ContainerDocumentations/ContainerDocumentation/Methods/Contains_String_.md)|Get a value indicating if the current container contains an object represented byfullName.|
