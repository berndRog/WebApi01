namespace WebApi._3_Infrastructure._3_Security;

// Well-known claim names used by the Identity Provider (OIDC/OAuth).
// Acts as a shared contract between IdP and APIs.
public static class IdentityClaims {
   public const string Subject = "sub";
   public const string PreferredUsername = "preferred_username";
   public const string CreatedAt = "created_at";
   public const string AdminRights = "admin_rights";
}