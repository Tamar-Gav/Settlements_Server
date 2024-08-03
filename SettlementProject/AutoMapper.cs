using Entities;
using DTO;
using AutoMapper;

namespace SettlementProject;

public class AutoMapper: Profile
{
    public AutoMapper()
    {
        CreateMap<CreateSettlementDTO, Settlement>();
    }
}
