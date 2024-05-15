using PMS.Common;
using PMS.Common.Models;
using PMS.DB.Model.EF.Models;
using PMS.Repositories.Interfaces;
using PMS.Services.Interfaces;

namespace PMS.Services.Services;

public class MasterDataService : IMasterDataService
{
    private readonly IRepository<PmsLookup> _repository;

    public MasterDataService(IRepository<PmsLookup> repository)
    {
        _repository = repository;
    }
    public List<DropDownModel> GetLookupsByType(string lkpType)
    {
        return _repository.GetAll().Where(x => x.LookupType.Equals(lkpType, StringComparison.OrdinalIgnoreCase)).Select(x=> new DropDownModel()
        {
            Id = x.LookupId,
            Name = x.VisibleValue
        }).ToList();
    }
}
