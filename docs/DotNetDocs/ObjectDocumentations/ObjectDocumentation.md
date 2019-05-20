# ObjectDocumentation Class
> Represents a object documentation.  This includes types and members.

**Namespace:** DotNetDocs.ObjectDocumentations

**Assembly:** DotNetDocs (in DotNetDocs.dll v1.0.0)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[System.Object](https://www.google.com/search?q=System.Object&btnI=)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Documentation](/docs/DotNetDocs/Documentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ObjectDocumentations.ObjectDocumentation](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation.md)

## Syntax
```csharp
public class ObjectDocumentation : DotNetDocs.Documentation, DotNetDocs.Mixins.Contracts.IObjectDocumentation, DotNetDocs.Mixins.Contracts.IDocumentation
```
## Constructors
|Name|Description|
|---|---|
|[Constructor(IMemberDefinition, XElement, TypeDocumentation, IDeclarationProvider)](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Constructors/Constructor_IMemberDefinition%2c%20XElement%2c%20Type7837.md)||
|[Constructor(ObjectDocumentationMixin)](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Constructors/Constructor_ObjectDocumentationMixin_.md)|Initializes a new instance of the[DotNetDocs.ObjectDocumentations.ObjectDocumentation](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation.md)class.|
|[Constructor()](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Constructors/Constructor__.md)||
## Properties
|Name|Description|
|---|---|
|[Declaration](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/Declaration.md)|Gets the declaration for the current object.|
|[DeclaringType](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/DeclaringType.md)|Gets the[DotNetDocs.ObjectDocumentations.TypeDocumentation](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation.md)which contains the current[DotNetDocs.ObjectDocumentations.ObjectDocumentation](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation.md).|
|[FullName](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/FullName.md)|Gets the full name of the current member.|
|[Name](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/Name.md)|Gets the name for the current member.|
|[Remarks](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/Remarks.md)|Gets objects representing the remarks comment.|
|[Summary](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/Summary.md)|Gets objects representing the summary comment.|
|[ObjectDocumentationMixin](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation/Properties/ObjectDocumentationMixin.md)|Gets or sets the[DotNetDocs.ObjectDocumentations.ObjectDocumentation.ObjectDocumentationMixin](https://www.google.com/search?q=DotNetDocs.ObjectDocumentations.ObjectDocumentation.ObjectDocumentationMixin&btnI=)that backs this type.|
