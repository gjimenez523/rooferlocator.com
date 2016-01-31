namespace rooferlocator.com.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class RooferLocator_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RlCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        ServiceArea = c.String(),
                        YearEstablished = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlLeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Budget = c.String(),
                        Comment = c.String(),
                        TimeOfRepair = c.DateTime(nullable: false),
                        State = c.String(),
                        RoofTypeRefId = c.Int(nullable: false),
                        ServiceTypeRefId = c.Int(nullable: false),
                        LocationTypeRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Lead_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlLocationType", t => t.LocationTypeRefId, cascadeDelete: true)
                .ForeignKey("dbo.RlRoofType", t => t.RoofTypeRefId, cascadeDelete: true)
                .ForeignKey("dbo.RlServiceType", t => t.ServiceTypeRefId, cascadeDelete: true)
                .Index(t => t.RoofTypeRefId)
                .Index(t => t.ServiceTypeRefId)
                .Index(t => t.LocationTypeRefId);
            
            CreateTable(
                "dbo.RlLocationType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(),
                        City = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LocationType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlRoofType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RoofType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlServiceType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ServiceType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        JobTitle = c.String(),
                        Phone = c.String(),
                        CellPhone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Credential = c.String(),
                        LicenseNumber = c.String(),
                        LicenseDescription = c.String(),
                        UserRefId = c.Long(nullable: false),
                        CompanyRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Member_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlCompany", t => t.CompanyRefId, cascadeDelete: true)
                .ForeignKey("dbo.AbpUsers", t => t.UserRefId, cascadeDelete: true)
                .Index(t => t.UserRefId)
                .Index(t => t.CompanyRefId);
            
            CreateTable(
                "dbo.RlPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionRefId = c.String(),
                        TransactionDateEntered = c.String(),
                        TransactionDateResponse = c.String(),
                        TransactionAmount = c.String(),
                        PONumber = c.String(),
                        ReturnTransId = c.String(),
                        ReturnRefNumber = c.String(),
                        ReturnAuth = c.String(),
                        ReturnAvsCode = c.String(),
                        ReturnCVV2Response = c.String(),
                        ReturnNotes = c.String(),
                        MemberRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Payment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlMembers", t => t.MemberRefId, cascadeDelete: true)
                .Index(t => t.MemberRefId);
            
            CreateTable(
                "dbo.RlMembersRoofType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberRefId = c.Int(nullable: false),
                        RoofTypeRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MembersRoofType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlMembers", t => t.MemberRefId, cascadeDelete: true)
                .ForeignKey("dbo.RlRoofType", t => t.RoofTypeRefId, cascadeDelete: true)
                .Index(t => t.MemberRefId)
                .Index(t => t.RoofTypeRefId);
            
            CreateTable(
                "dbo.RlMembersServiceType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberRefId = c.Int(nullable: false),
                        ServiceTypeRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MembersServiceType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlMembers", t => t.MemberRefId, cascadeDelete: true)
                .ForeignKey("dbo.RlRoofType", t => t.ServiceTypeRefId, cascadeDelete: true)
                .Index(t => t.MemberRefId)
                .Index(t => t.ServiceTypeRefId);
            
            CreateTable(
                "dbo.RlNews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        NewsCategoriesRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_News_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlNewsCategories", t => t.NewsCategoriesRefId, cascadeDelete: true)
                .Index(t => t.NewsCategoriesRefId);
            
            CreateTable(
                "dbo.RlNewsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NewsCategories_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlQuoteType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quote = c.String(),
                        DisplayName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QuoteType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RlTestimonial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Details = c.String(),
                        ImageUri = c.String(),
                        MemberRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Testimonial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlMembers", t => t.MemberRefId, cascadeDelete: true)
                .Index(t => t.MemberRefId);
            
            CreateTable(
                "dbo.RlTips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        TipsCategoriesRefId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tips_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RlTipsCategories", t => t.TipsCategoriesRefId, cascadeDelete: true)
                .Index(t => t.TipsCategoriesRefId);
            
            CreateTable(
                "dbo.RlTipsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipsCategories_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RlTips", "TipsCategoriesRefId", "dbo.RlTipsCategories");
            DropForeignKey("dbo.RlTestimonial", "MemberRefId", "dbo.RlMembers");
            DropForeignKey("dbo.RlNews", "NewsCategoriesRefId", "dbo.RlNewsCategories");
            DropForeignKey("dbo.RlMembersServiceType", "ServiceTypeRefId", "dbo.RlRoofType");
            DropForeignKey("dbo.RlMembersServiceType", "MemberRefId", "dbo.RlMembers");
            DropForeignKey("dbo.RlMembersRoofType", "RoofTypeRefId", "dbo.RlRoofType");
            DropForeignKey("dbo.RlMembersRoofType", "MemberRefId", "dbo.RlMembers");
            DropForeignKey("dbo.RlMembers", "UserRefId", "dbo.AbpUsers");
            DropForeignKey("dbo.RlPayments", "MemberRefId", "dbo.RlMembers");
            DropForeignKey("dbo.RlMembers", "CompanyRefId", "dbo.RlCompany");
            DropForeignKey("dbo.RlLeads", "ServiceTypeRefId", "dbo.RlServiceType");
            DropForeignKey("dbo.RlLeads", "RoofTypeRefId", "dbo.RlRoofType");
            DropForeignKey("dbo.RlLeads", "LocationTypeRefId", "dbo.RlLocationType");
            DropIndex("dbo.RlTips", new[] { "TipsCategoriesRefId" });
            DropIndex("dbo.RlTestimonial", new[] { "MemberRefId" });
            DropIndex("dbo.RlNews", new[] { "NewsCategoriesRefId" });
            DropIndex("dbo.RlMembersServiceType", new[] { "ServiceTypeRefId" });
            DropIndex("dbo.RlMembersServiceType", new[] { "MemberRefId" });
            DropIndex("dbo.RlMembersRoofType", new[] { "RoofTypeRefId" });
            DropIndex("dbo.RlMembersRoofType", new[] { "MemberRefId" });
            DropIndex("dbo.RlPayments", new[] { "MemberRefId" });
            DropIndex("dbo.RlMembers", new[] { "CompanyRefId" });
            DropIndex("dbo.RlMembers", new[] { "UserRefId" });
            DropIndex("dbo.RlLeads", new[] { "LocationTypeRefId" });
            DropIndex("dbo.RlLeads", new[] { "ServiceTypeRefId" });
            DropIndex("dbo.RlLeads", new[] { "RoofTypeRefId" });
            DropTable("dbo.RlTipsCategories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipsCategories_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlTips",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tips_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlTestimonial",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Testimonial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlQuoteType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QuoteType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlNewsCategories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NewsCategories_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlNews",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_News_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlMembersServiceType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MembersServiceType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlMembersRoofType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MembersRoofType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlPayments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Payment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlMembers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Member_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlServiceType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ServiceType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlRoofType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RoofType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlLocationType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LocationType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlLeads",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Lead_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RlCompany",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
