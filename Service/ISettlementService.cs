using Entities;

namespace Service
{
    public interface ISettlementService
    {
        Task<int> AddSettlementAsync(string settlementName);
        Task<List<Settlement>> GetSettlementsAsync();
        Task<int> DeleteSettlementAsync(int settlementId);
        Task<Settlement> UpdateSettlementAsync(int settlementId, string nameToUpdate);
    }
}