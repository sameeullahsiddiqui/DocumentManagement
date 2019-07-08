namespace DocumentManagement.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentsTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DocumentType = c.String(),
                        Remark = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileAllocations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        FolderName = c.String(),
                        Remark = c.String(),
                        RackBlockId = c.Guid(nullable: false),
                        DocumentTypeId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentsTypes", t => t.DocumentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RackBlockMasters", t => t.RackBlockId, cascadeDelete: true)
                .Index(t => t.RackBlockId)
                .Index(t => t.DocumentTypeId);
            
            CreateTable(
                "dbo.RackBlockMasters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BlockNumber = c.String(),
                        Remark = c.String(),
                        RackId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RackMasters", t => t.RackId, cascadeDelete: true)
                .Index(t => t.RackId);
            
            CreateTable(
                "dbo.RackMasters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RackNumber = c.String(),
                        Remark = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileAllocations", "RackBlockId", "dbo.RackBlockMasters");
            DropForeignKey("dbo.RackBlockMasters", "RackId", "dbo.RackMasters");
            DropForeignKey("dbo.FileAllocations", "DocumentTypeId", "dbo.DocumentsTypes");
            DropIndex("dbo.RackBlockMasters", new[] { "RackId" });
            DropIndex("dbo.FileAllocations", new[] { "DocumentTypeId" });
            DropIndex("dbo.FileAllocations", new[] { "RackBlockId" });
            DropTable("dbo.RackMasters");
            DropTable("dbo.RackBlockMasters");
            DropTable("dbo.FileAllocations");
            DropTable("dbo.DocumentsTypes");
        }
    }
}
