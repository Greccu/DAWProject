namespace DAWProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_id = c.Int(nullable: false, identity: true),
                        Category_name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Category_id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Post_id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(),
                        Created_at = c.DateTime(nullable: false),
                        Category_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Post_id)
                .ForeignKey("dbo.Categories", t => t.Category_id, cascadeDelete: true)
                .Index(t => t.Category_id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Post_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Posts", t => t.Post_id, cascadeDelete: true)
                .Index(t => t.Post_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "Category_id", "dbo.Categories");
            DropIndex("dbo.Comments", new[] { "Post_id" });
            DropIndex("dbo.Posts", new[] { "Category_id" });
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Categories");
        }
    }
}
