namespace MedLinked.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Doctor_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorID = c.Int(nullable: false, identity: true),
                        DoctorFirstName = c.String(),
                        DoctorLastName = c.String(),
                        DoctorEmail = c.String(),
                        DoctorPhone = c.Int(nullable: false),
                        DoctorSpecialization = c.String(),
                    })
                .PrimaryKey(t => t.DoctorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Doctors");
        }
    }
}
