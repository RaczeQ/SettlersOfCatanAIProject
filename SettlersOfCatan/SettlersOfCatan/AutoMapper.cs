using AutoMapper;
using SettlersOfCatan.SimplifiedModels;
using System.Linq;

namespace SettlersOfCatan
{
    public static class AutoMapperRegister
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Settlement, SimplifiedSettlement>()
                        .ForMember(dest => dest.AdjustedTiles, src => src.MapFrom(s => s.adjacentTiles))
                        .ForMember(dest => dest.ConnectedRoads, src => src.MapFrom(s => s.connectedRoads))
                        .ForMember(dest => dest.Id, src => src.MapFrom(s => s.id))
                        .ForMember(dest => dest.OwningPlayer, src => src.MapFrom(s => s.owningPlayer))
                        .ForMember(dest => dest.TitleWeight, src => src.MapFrom(s => s.adjacentTiles.Select(y => y.tileType).ToList()));
                }
            );
        }
    }
}
