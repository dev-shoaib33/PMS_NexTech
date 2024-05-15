using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PMS.DB.Model.EF.Models;

[Table("PMS_Lookup")]
public partial class PmsLookup
{
    [Key]
    [Column("Lookup_Id")]
    public int LookupId { get; set; }

    [Column("Lookup_Type")]
    [StringLength(50)]
    public string LookupType { get; set; } = null!;

    [Column("Hidden_Value")]
    [StringLength(50)]
    public string HiddenValue { get; set; } = null!;

    [Column("Visible_Value")]
    [StringLength(50)]
    public string VisibleValue { get; set; } = null!;

    [Column("Active_Flag")]
    public bool ActiveFlag { get; set; }

    [Column("Created_by")]
    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    [Column("Created_Date", TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("CategoryLkp")]
    public virtual ICollection<PmsProduct> PmsProducts { get; set; } = new List<PmsProduct>();
}
