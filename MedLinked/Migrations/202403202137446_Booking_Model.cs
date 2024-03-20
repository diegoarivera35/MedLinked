namespace MedLinked.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Booking_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        BookingDate = c.DateTime(nullable: false),
                        GrandTotal = c.Single(nullable: false),
                        PatientID = c.Int(nullable: false),
                        DoctorID = c.Int(nullable: false),
                        MedicalProcedureID = c.Int(nullable: false),
                        AccommodationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Accommodations", t => t.AccommodationID, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorID, cascadeDelete: true)
                .ForeignKey("dbo.MedicalProcedures", t => t.MedicalProcedureID, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID)
                .Index(t => t.DoctorID)
                .Index(t => t.MedicalProcedureID)
                .Index(t => t.AccommodationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Bookings", "MedicalProcedureID", "dbo.MedicalProcedures");
            DropForeignKey("dbo.Bookings", "DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Bookings", "AccommodationID", "dbo.Accommodations");
            DropIndex("dbo.Bookings", new[] { "AccommodationID" });
            DropIndex("dbo.Bookings", new[] { "MedicalProcedureID" });
            DropIndex("dbo.Bookings", new[] { "DoctorID" });
            DropIndex("dbo.Bookings", new[] { "PatientID" });
            DropTable("dbo.Bookings");
        }
    }
}
