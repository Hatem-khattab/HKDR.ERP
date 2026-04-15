namespace HKDR.DomainEntities.Entities.Core
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
