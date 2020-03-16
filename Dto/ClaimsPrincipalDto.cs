using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;

namespace AspCoreGraphQL_Demo.Dto
{
    public class ClaimsPrincipalDto
    {
        public ClaimsPrincipalDto(ClaimsPrincipal cp)
        {
            Claims = cp.Claims.Select(c => new ClaimDto(c)).ToList();
            Identity = new IdentityDto(cp.Identity);
            Identities = cp.Identities.Select(i => new IdentityDto(i)).ToList();
        }
        public List<ClaimDto> Claims { get; set; }
        public IdentityDto? Identity { get; set; }
        public List<IdentityDto> Identities { get; set; } = new List<IdentityDto>();
    }

    public class IdentityDto
    {

        public IdentityDto(IIdentity identity)
        {
            IdentityType = identity.GetType().FullName;
            AuthenticationType = identity.AuthenticationType;
            IsAuthenticated = identity.IsAuthenticated;
            Name = identity.Name;

            if (identity is ClaimsIdentity ci)
            {
                Claims = ci.Claims.Select(c => new ClaimDto(c)).ToList();
                NameClaimType = ci.NameClaimType;
                RoleClaimType = ci.RoleClaimType;
            }
        }

        public string? IdentityType { get; set; }
        public string? AuthenticationType { get; }
        public bool IsAuthenticated { get; }
        public string? Name { get; }
        public List<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
        public string? NameClaimType { get; }
        public string? RoleClaimType { get; }
    }

    public class ClaimDto
    {
        public ClaimDto(Claim claim)
        {
            this.Issuer = claim.Issuer;
            this.Subject = claim.Subject.Name;
            this.Type = claim.Type;
            this.Value = claim.Value;
            this.ValueType = claim.ValueType;
        }

        public string Issuer { get; }
        public string Subject { get; }
        public string Type { get; }
        public string Value { get; }
        public string ValueType { get; }
    }
}