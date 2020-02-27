namespace DocumentCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentInfoId = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        PreviousVersion = c.Int(nullable: false),
                        ServerVersion = c.String(),
                        Key = c.String(),
                        Path = c.String(),
                        Changes = c.String(),
                        ChangesUrl = c.String(),
                        UserId = c.String(),
                        UserName = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        AutoSave = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileType = c.String(),
                        Size = c.Long(nullable: false),
                        CurrentVersion = c.Int(nullable: false),
                        Key = c.String(),
                        Path = c.String(),
                        RelativePath = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserID = c.String(),
                        CreateUserName = c.String(),
                        ModifyTime = c.DateTime(nullable: false),
                        ModifyUserId = c.String(),
                        ModifyUserName = c.String(),
                        ExternalFlag = c.Boolean(nullable: false),
                        ExternalID = c.String(),
                        ExternalCallbackUrl = c.String(),
                        ExternalStartEditUrl = c.String(),
                        ExternalEndEditUrl = c.String(),
                        ExternalFilePath = c.String(),
                        ExternalSaveOpinionUrl = c.String(),
                        LastUpdateTime = c.DateTime(),
                        IsEditing = c.Boolean(nullable: false),
                        ExtendData = c.String(),
                        SystemType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        UserId = c.String(),
                        Permission = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonalFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentID = c.Int(nullable: false),
                        CreateType = c.String(),
                        FileType = c.String(),
                        FileName = c.String(),
                        CreateUserID = c.String(),
                        CreateUserName = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyUserID = c.String(),
                        ModifyUserName = c.String(),
                        ModifyTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        UserName = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.PersonalFiles");
            DropTable("dbo.FileUsers");
            DropTable("dbo.DocumentInfoes");
            DropTable("dbo.DocumentHistories");
        }
    }
}
