using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Enum
{
    public enum SystemType
    {
        [Description("生产协同")]
        cdpm,
        [Description("新生产协同")]
        pms
    }
}