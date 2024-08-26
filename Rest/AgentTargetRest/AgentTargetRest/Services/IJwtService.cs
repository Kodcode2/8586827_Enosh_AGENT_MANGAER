using AgentTargetRest.Dto;

namespace AgentTargetRest.Services
{
    public interface IJwtService
    {
        string CreateToken(string name);

    }
}
