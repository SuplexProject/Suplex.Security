using System;

namespace Suplex.Security.AclModel
{
    public interface IRightInfo
    {
        string FriendlyTypeName { get; }
        string FriendlyTypeNameValue { get; }
        Type RightType { get; }
        int Value { get; }
        string Name { get; }
    }
}