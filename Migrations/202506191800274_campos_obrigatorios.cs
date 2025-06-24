namespace iTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campos_obrigatorios : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tarefas", "IdGestor", c => c.Int(nullable: false));
            AlterColumn("dbo.Tarefas", "IdProgramador", c => c.Int(nullable: false));
            AlterColumn("dbo.Tarefas", "IdTipoTarefa", c => c.Int(nullable: false));
            AlterColumn("dbo.Tarefas", "DataPrevistaInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tarefas", "DataPrevistaFim", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tarefas", "DataPrevistaFim", c => c.DateTime());
            AlterColumn("dbo.Tarefas", "DataPrevistaInicio", c => c.DateTime());
            AlterColumn("dbo.Tarefas", "IdTipoTarefa", c => c.Int());
            AlterColumn("dbo.Tarefas", "IdProgramador", c => c.Int());
            AlterColumn("dbo.Tarefas", "IdGestor", c => c.Int());
        }
    }
}
