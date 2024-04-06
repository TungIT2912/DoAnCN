using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class ChanDoan
{
    public string IdchanDoan { get; set; } = null!;

    public string TenChanDoan { get; set; } = null!;

    public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();
}
