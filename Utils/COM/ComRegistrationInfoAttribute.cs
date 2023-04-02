using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eXPerienceBar.Utils.COM;

/// <summary>
///     An attribute which is used to specify metadata about a COM class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ComRegistrationInfoAttribute : Attribute
{
    public string? DisplayName { get; set; }

    public class ImplementsCategoryAttribute : Attribute
    {
        public Guid Guid { get; private set; }

        public ImplementsCategoryAttribute(Guid guid)
        {
            Guid = guid;
        }
    }
}