namespace AXA.Middleware.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Active = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.ClientClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ClientLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Clients", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AmountInsured = c.Double(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100),
                        InceptionDate = c.DateTime(nullable: false),
                        InstallmentPayment = c.Boolean(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientRoles", "UserId", "dbo.Clients");
            DropForeignKey("dbo.ClientLogins", "UserId", "dbo.Clients");
            DropForeignKey("dbo.ClientClaims", "UserId", "dbo.Clients");
            DropForeignKey("dbo.ClientRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Policies", "ClientId", "dbo.Clients");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ClientRoles", new[] { "RoleId" });
            DropIndex("dbo.ClientRoles", new[] { "UserId" });
            DropIndex("dbo.Policies", new[] { "ClientId" });
            DropIndex("dbo.ClientLogins", new[] { "UserId" });
            DropIndex("dbo.ClientClaims", new[] { "UserId" });
            DropIndex("dbo.Clients", "UserNameIndex");
            DropTable("dbo.Roles");
            DropTable("dbo.ClientRoles");
            DropTable("dbo.Policies");
            DropTable("dbo.ClientLogins");
            DropTable("dbo.ClientClaims");
            DropTable("dbo.Clients");
        }
    }
}
