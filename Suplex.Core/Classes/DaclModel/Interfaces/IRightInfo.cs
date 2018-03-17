using System;

namespace Suplex.Security.DaclModel
{
    public interface IRightInfo
    {
        string FriendlyTypeName { get; }
        Type RightType { get; }
        int Value { get; }
        string Name { get; }
    }
}