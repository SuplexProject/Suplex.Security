using System;

namespace Suplex.Security.AclModel
{
    public class RightInfo<T> : IRightInfo where T : struct, IConvertible
    {

        public T Right { get; set; }
        public string FriendlyTypeName { get { return Right.GetFriendlyRightTypeName(); } }
        public Type RightType { get { return Right.GetType(); } }
        public int Value { get { return (int)Enum.Parse( Right.GetType(), Right.ToString() ); } }
        public string Name { get { return Right.ToString(); } }
    }
}