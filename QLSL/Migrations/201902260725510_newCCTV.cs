namespace QLSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newCCTV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCTV", "MDT", c => c.String(maxLength: 100));
            AddColumn("dbo.CCTV", "MTD", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCTV", "MTD");
            DropColumn("dbo.CCTV", "MDT");
        }
    }
}
