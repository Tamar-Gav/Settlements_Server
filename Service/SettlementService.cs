using Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class SettlementService : ISettlementService
{
    private readonly ISettlementRepository _settlementRepository;

    public SettlementService(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository;
    }

    public async Task<int> AddSettlementAsync( string settlementName)
    {
        int isDuplicateElement = await _settlementRepository.IsDuplicateNameInDBAsync(settlementName); 
        if(isDuplicateElement != 0)
        {
            return -1;   
        }
        return await _settlementRepository.AddSettlementAsync(settlementName); 
    }

    public async Task<List <Settlement>> GetSettlementsAsync()
    {
        return await _settlementRepository.GetSettlementsAsync();
            
    }
    public async Task<Settlement> UpdateSettlementAsync(int settlementId, string nameToUpdate)
    {
        bool isvalidSettlement = await _settlementRepository.IsValidIdAsync(settlementId);

        if(!isvalidSettlement)
        {
            return new Settlement();
        }
        int isDuplicateSettlementName = await _settlementRepository.IsDuplicateNameInDBAsync(nameToUpdate);

        if (isDuplicateSettlementName != 0 && isDuplicateSettlementName != settlementId)
        {
            return new Settlement() { SettlementId = -1};
        }

        return  await _settlementRepository.UpdateSettlementAsync(settlementId, nameToUpdate);  
    }
    public async Task<int> DeleteSettlementAsync(int settlementId)
    {
        bool isvalidSettlement = await _settlementRepository.IsValidIdAsync(settlementId);

        if (!isvalidSettlement)
        {
            return -1;
        }
        return await _settlementRepository.DeleteSettlementAsync(settlementId);
    }
}
