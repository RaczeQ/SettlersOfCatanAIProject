using System.Collections.Generic;
using System.Drawing;
using AutoMapper;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.SimplifiedModels;
using System.Linq;
using System.Windows.Forms;
using SettlersOfCatan.TransparencyFix;

namespace SettlersOfCatan.Utils
{
    public static class AutoMapperRegister
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Settlement, SimplifiedSettlement>()
                        .ForMember(dest => dest.AdjacentTiles, src => src.MapFrom(s => s.adjacentTiles))
                        .ForMember(dest => dest.ConnectedRoads, src => src.MapFrom(s => s.connectedRoads))
                        .ForMember(dest => dest.Id, src => src.MapFrom(s => s.id))
                        .ForMember(dest => dest.OwningPlayer, src => src.MapFrom(s => s.owningPlayer))
                        .ForMember(dest => dest.TileWeights, src => src.MapFrom(s => s.adjacentTiles.Select(y => y.tileType).ToList()));

                    cfg.CreateMap<Control, Control>()
                        .ForCtorParam("text", opt => opt.MapFrom(src => ""))
                        .ForMember(desc => desc.BackColor, opt => opt.MapFrom(src => Color.White));

                    cfg.CreateMap<Panel, Panel>()
                        .ForMember(desc => desc.BackColor, opt => opt.MapFrom(src => Color.White));

                    cfg.CreateMap<NumberChip, NumberChip>()
                        .IncludeBase<Control, Control>()
                        .ForCtorParam("numberValue", opt => opt.MapFrom(src => src.numberValue));

                    cfg.CreateMap<Control.ControlCollection, Control.ControlCollection>()
                        .ForCtorParam("owner", opt => opt.MapFrom<Control>(src => null));

                    cfg.CreateMap<ResourceCard, ResourceCard>()
                        .ForCtorParam("resource", opt => opt.MapFrom(src => src.getResourceType()));
                    cfg.CreateMap<DevelopmentCard, DevelopmentCard>();
                    cfg.CreateMap<TerrainTile, TerrainTile>()
                        .IncludeBase<Control, Control>()
                        .ForCtorParam("p", opt => opt.MapFrom(src => new BoardArea()))
                        .ForCtorParam("resourceType", opt => opt.MapFrom(src => src.tileType))
                        .ForCtorParam("image", opt => opt.MapFrom(src => src.BackgroundImage))
                        .ForMember(dest => dest.adjascentRoads, opt => opt.Ignore())
                        .ForMember(dest => dest.adjascentSettlements, opt => opt.Ignore());

                    cfg.CreateMap<Harbor, Harbor>()
                        .IncludeBase<Control, Control>()
                        .ForCtorParam("resource", opt => opt.MapFrom(src => src.getTradeOutputResource()))
                        .ForCtorParam("requiredCount", opt => opt.MapFrom(src => src.getRequiredResourceCount()))
                        .ForMember(dest => dest.validTradeLocations, opt => opt.Ignore());

                    cfg.CreateMap<Settlement, Settlement>()
                        .IncludeBase<Control, Control>()
                        .ForCtorParam("position", opt => opt.MapFrom(src => src.position))
                        .ForMember(dest => dest.connectedRoads, opt => opt.Ignore())
                        .ForMember(dest => dest.adjacentTiles, opt => opt.Ignore())
                        .ForMember(dest => dest.owningPlayer, opt => opt.Ignore());

                    cfg.CreateMap<Road, Road>()
                        .IncludeBase<Control, Control>()
                        .ForCtorParam("position", opt => opt.MapFrom(src => src.position))
                        .ForMember(dest => dest.connectedSettlements, opt => opt.Ignore())
                        .ForMember(dest => dest.owningPlayer, opt => opt.Ignore());

                    cfg.CreateMap<Bank, Bank>()
                        .ForMember(dest => dest.developmentCards,
                            src => src.MapFrom(s => s.developmentCards.CopyDeck()))
                        .ForMember(dest => dest.resources,
                            src => src.MapFrom(s => Mapper.Map<List<ResourceCard>>(s.resources)));

                    cfg.CreateMap<Player, Player>()
                        .IncludeBase<Control, Control>()
                        .ForMember(dest => dest.playerNumber,
                            src => src.MapFrom(s => s.playerNumber))
                        .ForMember(dest => dest.resources,
                            src => src.MapFrom(s => Mapper.Map<List<ResourceCard>>(s.resources)))
                        .ForMember(dest => dest.onHandDevelopmentCards,
                            src => src.MapFrom(s => Mapper.Map<List<DevelopmentCard>>(s.onHandDevelopmentCards)))
                        .ForMember(dest => dest.roads,  opt => opt.Ignore())
                        .ForMember(dest => dest.settlements, opt => opt.Ignore());

                }
            );

        }
    }
}
