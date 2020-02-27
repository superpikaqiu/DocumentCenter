using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Enum
{
    public enum FileType
    {
        [Description("word")]
        docx,
        [Description("word")]
        doc,
        [Description("excel")]
        xlsx,
        [Description("excel")]
        xls,
    }
}