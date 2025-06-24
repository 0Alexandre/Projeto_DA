namespace iTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposObrigatorios : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Programadores", "IdGestor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programadores", "IdGestor", c => c.Int());
        }
    }
}
