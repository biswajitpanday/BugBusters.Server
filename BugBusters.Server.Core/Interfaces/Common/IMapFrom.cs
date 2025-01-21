using AutoMapper;

namespace BugBusters.Server.Core.Interfaces.Common;

public interface IMapFrom<T>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
        profile.CreateMap(GetType(), typeof(T));
    }
}