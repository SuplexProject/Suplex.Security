using System;
using System.Collections.Generic;

namespace Suplex.Security.AclModel
{
    public interface ISecureContainer : ISecureObject, IContainer<ISecureObject>
    {
    }
}