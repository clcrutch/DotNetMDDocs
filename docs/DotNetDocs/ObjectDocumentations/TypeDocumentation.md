# TypeDocumentation Class
> Parses a type.

**Namespace:** DotNetDocs.ObjectDocumentations

**Assembly:** DotNetDocs (in DotNetDocs.dll)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[System.Object](https://www.google.com/search?q=System.Object&btnI=)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Documentation](/docs/DotNetDocs/Documentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ObjectDocumentations.ObjectDocumentation](/docs/DotNetDocs/ObjectDocumentations/ObjectDocumentation.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.ObjectDocumentations.TypeDocumentation](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation.md)

## Syntax
```csharp
public class TypeDocumentation : DotNetDocs.ObjectDocumentations.ObjectDocumentation, DotNetDocs.Mixins.Contracts.IContainerDocumentation, DotNetDocs.Mixins.Contracts.IDocumentation
```
## Constructors
|Name|Description|
|---|---|
|[Constructor(TypeDefinition, XElement, NamespaceDocumentation, AssemblyDocumentation)](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Constructors/Constructor_TypeDefinition%2c%20XElement%2c%20Namespa8233.md)||
## Properties
|Name|Description|
|---|---|
|[BaseType](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/BaseType.md)|Gets the base type for the type.|
|[ConstructorDocumentations](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/ConstructorDocumentations.md)|Gets a list of constructors for the current type.|
|[DeclaringAssembly](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/DeclaringAssembly.md)|Gets the assembly that declares the current type.|
|[FieldDocumentations](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/FieldDocumentations.md)|Gets a list of fields for the current type.|
|[IsClass](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/IsClass.md)|Gets a value indicating whether the type is a class.|
|[IsEnum](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/IsEnum.md)|Gets a value indicating whether the type is a enum.|
|[IsInterface](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/IsInterface.md)|Gets a value indicating whether the type is an interface.|
|[MethodDocumentations](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/MethodDocumentations.md)|Gets a list of methods for the current type.|
|[NamespaceDocumentation](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/NamespaceDocumentation.md)|Gets the parent namespace.|
|[PropertyDocumentations](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/PropertyDocumentations.md)|Gets a list of the properties for the current type.|
|[ReflectionTypeDefinition](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/ReflectionTypeDefinition.md)|Gets the underlying[System.Reflection.Metadata.TypeDefinition](https://www.google.com/search?q=System.Reflection.Metadata.TypeDefinition&btnI=)for the type.|
|[Item](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Properties/Item.md)||
## Methods
|Name|Description|
|---|---|
|[Contains(String)](/docs/DotNetDocs/ObjectDocumentations/TypeDocumentation/Methods/Contains_String_.md)|Gets a value indicating whether this type contains a member referenced byfullName.|
