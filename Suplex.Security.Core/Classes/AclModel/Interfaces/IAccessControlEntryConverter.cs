using System;

namespace Suplex.Security.AclModel
{
    public interface IAccessControlEntryConverter
    {
        Type SourceRightType { get; }
        string SourceRightName { get; }
        int SourceRightValue { get; }

        Type TargetRightType { get; }
        string TargetRightName { get; }
        int TargetRightValue { get; }

        bool Inheritable { get; set; }
    }

    public interface IAccessControlEntryConverter<TSource, TTarget> : IAccessControlEntryConverter
        where TSource : struct, IConvertible
        where TTarget : struct, IConvertible
    {
        TSource SourceRight { get; set; }
        TTarget TargetRight { get; set; }
    }
}