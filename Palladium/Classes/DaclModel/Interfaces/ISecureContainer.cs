using System;
using System.Collections.Generic;

namespace Palladium.Security.DaclModel
{
    public interface ISecureContainer : ISecureObject
    {
        List<ISecureContainer> Children { get; set; }
    }
}