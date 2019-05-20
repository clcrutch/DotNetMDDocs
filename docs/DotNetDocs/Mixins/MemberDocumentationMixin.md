# MemberDocumentationMixin Class
> Represents a mixin for members.

**Namespace:** DotNetDocs.Mixins

**Assembly:** DotNetDocs (in DotNetDocs.dll)
## Inheritance Hierarchy
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[System.Object](https://www.google.com/search?q=System.Object&btnI=)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Mixins.ObjectDocumentationMixin](/docs/DotNetDocs/Mixins/ObjectDocumentationMixin.md)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DotNetDocs.Mixins.MemberDocumentationMixin](/docs/DotNetDocs/Mixins/MemberDocumentationMixin.md)

## Syntax
```csharp
public sealed class MemberDocumentationMixin : DotNetDocs.Mixins.ObjectDocumentationMixin, DotNetDocs.Mixins.Contracts.IMemberDocumentation, DotNetDocs.Mixins.Contracts.IObjectDocumentation, DotNetDocs.Mixins.Contracts.IDocumentation
```
## Constructors
|Name|Description|
|---|---|
|[Constructor(IMemberDefinition, XElement, TypeDocumentation, IDeclarationProvider)](/docs/DotNetDocs/Mixins/MemberDocumentationMixin/Constructors/Constructor_IMemberDefinition%2c%20XElement%2c%20Type7837.md)||
## Properties
|Name|Description|
|---|---|
|[Remarks](/docs/DotNetDocs/Mixins/MemberDocumentationMixin/Properties/Remarks.md)|Gets objects representing the remarks comment.  Handles inheritdoc elements as well.|
|[Summary](/docs/DotNetDocs/Mixins/MemberDocumentationMixin/Properties/Summary.md)|Gets objects representing the summary comment.  Handles inheritdoc elements as well.|
