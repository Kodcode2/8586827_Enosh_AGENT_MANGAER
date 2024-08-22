using Microsoft.CodeAnalysis;
using AgentTargetRest.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using AgentTargetRest.Models;

namespace AgentTargetRest.Dto
{
    public class TargetDto
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}