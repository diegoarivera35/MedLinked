namespace MedLinked.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Patient_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        PatientFirstName = c.String(),
                        PatientLastName = c.String(),
                        PatientGender = c.String(),
                        PatientDOB = c.DateTime(nullable: false),
                        PatientNationality = c.String(),
                        PatientEmail = c.String(),
                        PatientPhone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Patients");
        }
    }
}
