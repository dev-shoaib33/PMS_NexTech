using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PMS.DB.Model.EF.Models;

[Table("PMS_Product")]
public partial class PmsProduct
{
    [Key]
    public int ProductId { get; set; }

    [Column("Product_Name")]
    [StringLength(255)]
    public string ProductName { get; set; } = null!;

    [Column("Product_Code")]
    [StringLength(50)]
    public string ProductCode { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    [Column("Category_lkp_Id")]
    public int CategoryLkpId { get; set; }

    [StringLength(50)]
    public string? Uom { get; set; }

    [Column("Created_By")]
    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    [Column("Created_Date", TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column("Last_Updated_By")]
    [StringLength(50)]
    public string LastUpdatedBy { get; set; } = null!;

    [Column("Last_Updated_Date", TypeName = "datetime")]
    public DateTime LastUpdatedDate { get; set; }

    [Column("Active_Flag")]
    public bool ActiveFlag { get; set; }

    [ForeignKey("CategoryLkpId")]
    [InverseProperty("PmsProducts")]
    public virtual PmsLookup CategoryLkp { get; set; } = null!;
}
