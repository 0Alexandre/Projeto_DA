namespace iTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrigirUtilizador : DbMigration
    {
        public override void Up()
        {
            // 1. Remove a FK da tabela Programadores que depende de Gestor_Id
            DropForeignKey("dbo.Programadores", "Gestor_Id", "dbo.Gestores");

            // 2. Remove o índice que usa Gestor_Id
            DropIndex("dbo.Programadores", new[] { "Gestor_Id" });

            // 3. Agora sim, pode remover a coluna Gestor_Id
            DropColumn("dbo.Programadores", "Gestor_Id");

            // 4. Adiciona a coluna Tipo na tabela Utilizadores
            AddColumn("dbo.Utilizadores", "Tipo", c => c.Int(nullable: false));

            // 5. Adiciona a coluna GestorId na tabela Utilizadores (nullable)
            AddColumn("dbo.Utilizadores", "GestorId", c => c.Int());

            // 6. Cria índice para GestorId
            CreateIndex("dbo.Utilizadores", "GestorId");

            // 7. Cria FK para GestorId na tabela Utilizadores apontando para Id de Utilizadores
            AddForeignKey("dbo.Utilizadores", "GestorId", "dbo.Utilizadores", "Id");
        }

        public override void Down()
        {
            // Remove FK e índice da tabela Utilizadores
            DropForeignKey("dbo.Utilizadores", "GestorId", "dbo.Utilizadores");
            DropIndex("dbo.Utilizadores", new[] { "GestorId" });

            // Remove colunas adicionadas
            DropColumn("dbo.Utilizadores", "GestorId");
            DropColumn("dbo.Utilizadores", "Tipo");

            // Recria coluna Gestor_Id na tabela Programadores
            AddColumn("dbo.Programadores", "Gestor_Id", c => c.Int());

            // Recria índice
            CreateIndex("dbo.Programadores", "Gestor_Id");

            // Recria FK
            AddForeignKey("dbo.Programadores", "Gestor_Id", "dbo.Gestores", "Id");
        }
    }
}
