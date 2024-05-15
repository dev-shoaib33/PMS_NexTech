using PMS.Common.Models;

namespace PMS.Services.Interfaces;

public interface IMasterDataService
{
    List<DropDownModel> GetLookupsByType(string lkpType);
}
