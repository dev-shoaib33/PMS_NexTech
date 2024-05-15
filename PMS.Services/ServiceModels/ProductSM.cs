using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.DB.Model.EF.Models;

namespace PMS.Services.ServiceModels;
public class ProductSM
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    public int CategoryLkpId { get; set; }
    public string CategoryLkpName { get; set; }
    public string ImageName { get; set; } = string.Empty;
    public string Uom { get; set; } = string.Empty; 
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string LastUpdatedBy { get; set; } = null!;
    public DateTime LastUpdatedDate { get; set; }
    public bool ActiveFlag { get; set; }
    public virtual PmsLookup CategoryLkp { get; set; } = null!;
}
