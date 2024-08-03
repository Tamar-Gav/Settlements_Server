using Entities;

namespace Repository
{
    public interface ISettlementRepository
    {
        Task<int> AddSettlementAsync(string settlementName);
        Task<List<Settlement>> GetSettlementsAsync();
        Task<int> IsDuplicateNameInDBAsync(string settlementName);
        Task<int> DeleteSettlementAsync(int settlementId);
        Task<Settlement> UpdateSettlementAsync(int settlementId, string nameToUpdate);

        Task<bool> IsValidIdAsync(int id);
    }
}