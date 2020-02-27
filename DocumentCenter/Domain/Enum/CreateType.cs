﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Enum
{
    public enum CreateType
    {
        [Description("新建")]
        newFile,
        [Description("上传")]
        uploadFile
    }
}