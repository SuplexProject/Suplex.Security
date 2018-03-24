using System;

namespace Suplex.Security.AclModel
{
    public interface IRightInfo
    {
        string FriendlyTypeName { get; }
        Type RightType { get; }
        int Value { get; }
        string Name { get; }
    }
}