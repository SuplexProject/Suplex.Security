using System;

namespace Suplex.Security.AclModel
{
    public interface IAccessControlEntryConverter : ICloneable<IAccessControlEntryConverter>
    {
        Guid UId { get; set; }

        void SetSourceRightValue(string value);
        Type SourceRightType { get; }
        string SourceRightName { get; }
        int SourceRightValue { get; }
        string FriendlySourceRightTypeNameValue { get; }


        void SetTargetRightValue(string value);
        Type TargetRightType { get; }
        string TargetRightName { get; }
        int TargetRightValue { get; }
        string FriendlyTargetRightTypeNameValue { get; }

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