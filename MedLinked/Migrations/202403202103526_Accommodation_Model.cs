namespace MedLinked.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accommodation_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accommodations",
                c => new
                    {
                        AccommodationID = c.Int(nullable: false, identity: true),
                        AccommodationName = c.String(),
                        Address = c.String(),
                        Destination = c.String(),
                        Departure = c.DateTime(nullable: false),
                        Arrival = c.DateTime(nullable: false),
                        AccommodationCost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.AccommodationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accommodations");
        }
    }
}
