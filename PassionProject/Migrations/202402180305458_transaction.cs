namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.Products", "Transaction_TransactionId", c => c.Int());
            CreateIndex("dbo.Products", "Transaction_TransactionId");
            AddForeignKey("dbo.Products", "Transaction_TransactionId", "dbo.Transactions", "TransactionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Transaction_TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Transactions", new[] { "CustomerId" });
            DropIndex("dbo.Products", new[] { "Transaction_TransactionId" });
            DropColumn("dbo.Products", "Transaction_TransactionId");
            DropTable("dbo.Transactions");
        }
    }
}
