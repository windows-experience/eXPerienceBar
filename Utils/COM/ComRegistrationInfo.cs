using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eXPerienceBar.Utils.COM;

public class ComRegistrationInfo
{
    public string? DisplayName { get; set; }

    public List<Guid> ImplementedCategories { get; set; } = new();
}