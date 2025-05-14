namespace iTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Utilizadores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarefas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdGestor = c.Int(),
                        IdProgramador = c.Int(),
                        IdTipoTarefa = c.Int(),
                        OrdemExecucao = c.Int(nullable: false),
                        Descricao = c.String(),
                        DataPrevistaInicio = c.DateTime(),
                        DataPrevistaFim = c.DateTime(),
                        StoryPoints = c.Int(nullable: false),
                        DataRealInicio = c.DateTime(),
                        DataRealFim = c.DateTime(),
                        DataCriacao = c.DateTime(nullable: false),
                        EstadoAtual = c.String(),
                        Gestor_Id = c.Int(),
                        Programador_Id = c.Int(),
                        TipoTarefa_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gestores", t => t.Gestor_Id)
                .ForeignKey("dbo.Programadores", t => t.Programador_Id)
                .ForeignKey("dbo.TipoTarefas", t => t.TipoTarefa_Id)
                .Index(t => t.Gestor_Id)
                .Index(t => t.Programador_Id)
                .Index(t => t.TipoTarefa_Id);
            
            CreateTable(
                "dbo.TipoTarefas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gestores",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Departamento = c.String(),
                        GereUtilizadores = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Utilizadores", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Programadores",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Gestor_Id = c.Int(),
                        NivelExperiencia = c.String(),
                        IdGestor = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Utilizadores", t => t.Id)
                .ForeignKey("dbo.Gestores", t => t.Gestor_Id)
                .Index(t => t.Id)
                .Index(t => t.Gestor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Programadores", "Gestor_Id", "dbo.Gestores");
            DropForeignKey("dbo.Programadores", "Id", "dbo.Utilizadores");
            DropForeignKey("dbo.Gestores", "Id", "dbo.Utilizadores");
            DropForeignKey("dbo.Tarefas", "TipoTarefa_Id", "dbo.TipoTarefas");
            DropForeignKey("dbo.Tarefas", "Programador_Id", "dbo.Programadores");
            DropForeignKey("dbo.Tarefas", "Gestor_Id", "dbo.Gestores");
            DropIndex("dbo.Programadores", new[] { "Gestor_Id" });
            DropIndex("dbo.Programadores", new[] { "Id" });
            DropIndex("dbo.Gestores", new[] { "Id" });
            DropIndex("dbo.Tarefas", new[] { "TipoTarefa_Id" });
            DropIndex("dbo.Tarefas", new[] { "Programador_Id" });
            DropIndex("dbo.Tarefas", new[] { "Gestor_Id" });
            DropTable("dbo.Programadores");
            DropTable("dbo.Gestores");
            DropTable("dbo.TipoTarefas");
            DropTable("dbo.Tarefas");
            DropTable("dbo.Utilizadores");
        }
    }
}
