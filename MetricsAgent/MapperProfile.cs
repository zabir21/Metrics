using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Request;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<CpuMetricDtO, CpuMetricDto>();
            CreateMap<CpuMetricsCreateRequest, CpuMetricDtO>()
                .ForMember(x=> x.Value, opt=> opt.MapFrom(src => src.Value))
                .ForMember(x=> x.Time, opt=> opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<HddMetrics, HddMetricDto>();
            CreateMap<HddMetricsCreateRequest, HddMetrics>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<NetworkMetrics, NetworkMetricDto>();
            CreateMap<NetworkMetricsCreateRequest, NetworkMetrics>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<RamMetrics, RamMetricDto>();
            CreateMap<RamMetricsCreateRequest, RamMetrics>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));
        }
    }
}
