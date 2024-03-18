namespace MedLinked.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MedicalProcedure_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalProcedures",
                c => new
                    {
                        MedicalProcedureID = c.Int(nullable: false, identity: true),
                        MedicalProcedureName = c.String(),
                        MedicalCenter = c.String(),
                        MedicalProcedureDate = c.DateTime(nullable: false),
                        MedicalProcedureCost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.MedicalProcedureID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MedicalProcedures");
        }
    }
}
