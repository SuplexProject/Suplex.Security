using System;
using System.Collections.Generic;

namespace Suplex.Security.DaclModel
{
    public interface ISecureContainer : ISecureObject, IContainer<ISecureObject>
    {
    }
}