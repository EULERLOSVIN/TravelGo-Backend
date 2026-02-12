using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Persistence.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<DetailVehicle> DetailVehicles { get; set; }

    public virtual DbSet<DocumentVehicle> DocumentVehicles { get; set; }

    public virtual DbSet<Headquarter> Headquarters { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationState> NotificationStates { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<QueueVehicle> QueueVehicles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RouteAssignment> RouteAssignments { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatVehicle> SeatVehicles { get; set; }

    public virtual DbSet<StateAccount> StateAccounts { get; set; }

    public virtual DbSet<StateHeadquarter> StateHeadquarters { get; set; }

    public virtual DbSet<StateSeatVehicle> StateSeatVehicles { get; set; }

    public virtual DbSet<StateTrip> StateTrips { get; set; }

    public virtual DbSet<TicketState> TicketStates { get; set; }

    public virtual DbSet<TravelRoute> TravelRoutes { get; set; }

    public virtual DbSet<TravelTicket> TravelTickets { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleState> VehicleStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=servidor-sql.ccjikiaksg0w.us-east-1.rds.amazonaws.com;Database=DbTravelGo;User Id=admin;Password=martinez1234;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__DA18132C73016F04");

            entity.ToTable("Account");

            entity.Property(e => e.IdAccount).HasColumnName("idAccount");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.IdStateAccount)
                .HasDefaultValue(1)
                .HasColumnName("idStateAccount");
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .HasColumnName("password");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Person");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");

            entity.HasOne(d => d.IdStateAccountNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdStateAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_StateAccount");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.IdAssignment).HasName("PK__Assignme__89A425AA365E3276");

            entity.ToTable("Assignment");

            entity.Property(e => e.IdAssignment).HasColumnName("idAssignment");
            entity.Property(e => e.AssignmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assignmentDate");
            entity.Property(e => e.IdAccount).HasColumnName("idAccount");
            entity.Property(e => e.IdHeadquarter).HasColumnName("idHeadquarter");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Account");

            entity.HasOne(d => d.IdHeadquarterNavigation).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.IdHeadquarter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Headquarter");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.IdBilling).HasName("PK__Billing__83EAF8A22AEAA961");

            entity.ToTable("Billing");

            entity.Property(e => e.IdBilling).HasColumnName("idBilling");
            entity.Property(e => e.BillingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("billingDate");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(20)
                .HasColumnName("documentNumber");
            entity.Property(e => e.IdCompany).HasColumnName("idCompany");
            entity.Property(e => e.IdPaymentMethod).HasColumnName("idPaymentMethod");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Billing_Company");

            entity.HasOne(d => d.IdPaymentMethodNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.IdPaymentMethod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Billing_PaymentMethod");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Billing_Person");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.IdCompany).HasName("PK__Company__BBAEF00313940EAF");

            entity.ToTable("Company");

            entity.Property(e => e.IdCompany).HasColumnName("idCompany");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(50)
                .HasColumnName("businessName");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FiscalAddress)
                .HasMaxLength(50)
                .HasColumnName("fiscalAddress");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.Ruc)
                .HasMaxLength(20)
                .HasColumnName("ruc");
        });

        modelBuilder.Entity<DetailVehicle>(entity =>
        {
            entity.HasKey(e => e.IdDetailVehicle).HasName("PK__DetailVe__CD0976F5647EA070");

            entity.ToTable("DetailVehicle");

            entity.Property(e => e.IdDetailVehicle).HasColumnName("idDetailVehicle");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");
            entity.Property(e => e.SeatNumber).HasColumnName("seatNumber");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(30)
                .HasColumnName("vehicleType");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany(p => p.DetailVehicles)
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailVehicle_Vehicle");
        });

        modelBuilder.Entity<DocumentVehicle>(entity =>
        {
            entity.HasKey(e => e.IdDocumentVehicle).HasName("PK__Document__61EE4615336FC427");

            entity.ToTable("DocumentVehicle");

            entity.Property(e => e.IdDocumentVehicle).HasColumnName("idDocumentVehicle");
            entity.Property(e => e.ExpirationDate).HasColumnName("expirationDate");
            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");
            entity.Property(e => e.NumberSoat)
                .HasMaxLength(20)
                .HasColumnName("numberSoat");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany(p => p.DocumentVehicles)
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentVehicle_Vehicle");
        });

        modelBuilder.Entity<Headquarter>(entity =>
        {
            entity.HasKey(e => e.IdHeadquarter).HasName("PK__Headquar__D1396C9F6A474DFA");

            entity.ToTable("Headquarter");

            entity.Property(e => e.IdHeadquarter).HasColumnName("idHeadquarter");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .HasColumnName("department");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdCompany).HasColumnName("idCompany");
            entity.Property(e => e.IdStateHeadquarter).HasColumnName("idStateHeadquarter");
            entity.Property(e => e.IsMain).HasColumnName("isMain");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registrationDate");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Headquarters)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Headquarter_Company");

            entity.HasOne(d => d.IdStateHeadquarterNavigation).WithMany(p => p.Headquarters)
                .HasForeignKey(d => d.IdStateHeadquarter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Headquarter_StateHeadquarter");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.IdNotification).HasName("PK__Notifica__22C02321774CD52A");

            entity.ToTable("Notification");

            entity.Property(e => e.IdNotification).HasColumnName("idNotification");
            entity.Property(e => e.IdNotificationState).HasColumnName("idNotificationState");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.IdTrip).HasColumnName("idTrip");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .HasColumnName("message");

            entity.HasOne(d => d.IdNotificationStateNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.IdNotificationState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_NotificationState");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Person");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Trip");
        });

        modelBuilder.Entity<NotificationState>(entity =>
        {
            entity.HasKey(e => e.IdNotificationState).HasName("PK__Notifica__56E98973A736C9D1");

            entity.ToTable("NotificationState");

            entity.Property(e => e.IdNotificationState).HasColumnName("idNotificationState");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.IdPaymentMethod).HasName("PK__PaymentM__4568774731A28353");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.IdPaymentMethod).HasColumnName("idPaymentMethod");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.IdPerson).HasName("PK__Person__BAB33700FB6BEB16");

            entity.ToTable("Person");

            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.IdTypeDocument).HasColumnName("idTypeDocument");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.NumberIdentityDocument)
                .HasMaxLength(20)
                .HasColumnName("numberIdentityDocument");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(9)
                .HasColumnName("phoneNumber");

            entity.HasOne(d => d.IdTypeDocumentNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.IdTypeDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_IdentityDocument");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.IdPlace).HasName("PK__Place__39B84B90DB4830AB");

            entity.ToTable("Place");

            entity.Property(e => e.IdPlace).HasColumnName("idPlace");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<QueueVehicle>(entity =>
        {
            entity.HasKey(e => e.IdQueueVehicle).HasName("PK__QueueVeh__07518E363F347379");

            entity.ToTable("QueueVehicle");

            entity.Property(e => e.IdQueueVehicle).HasColumnName("idQueueVehicle");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("entryDate");
            entity.Property(e => e.IdPlace).HasColumnName("idPlace");
            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");

            entity.HasOne(d => d.IdPlaceNavigation).WithMany(p => p.QueueVehicles)
                .HasForeignKey(d => d.IdPlace)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QueueVehicle_Place");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany(p => p.QueueVehicles)
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QueueVehicle_Vehicle");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__E5045C54566FFF61");

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RouteAssignment>(entity =>
        {
            entity.HasKey(e => e.IdRouteAssignment).HasName("PK__RouteAss__1B777D2D55E29F8A");

            entity.ToTable("RouteAssignment");

            entity.Property(e => e.IdRouteAssignment).HasColumnName("idRouteAssignment");
            entity.Property(e => e.AssignmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("assignmentDate");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.IdTravelRoute).HasColumnName("idTravelRoute");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.RouteAssignments)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RouteAssignment_Person");

            entity.HasOne(d => d.IdTravelRouteNavigation).WithMany(p => p.RouteAssignments)
                .HasForeignKey(d => d.IdTravelRoute)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RouteAssignment_TravelRoute");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.IdSeat).HasName("PK__Seat__C5963E9B8065B8D3");

            entity.ToTable("Seat");

            entity.Property(e => e.IdSeat).HasColumnName("idSeat");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<SeatVehicle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SeatVehicle");

            entity.Property(e => e.IdSeat).HasColumnName("idSeat");
            entity.Property(e => e.IdSeatVehicle).HasColumnName("idSeatVehicle");
            entity.Property(e => e.IdStateSeatVehicle).HasColumnName("idStateSeatVehicle");
            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");

            entity.HasOne(d => d.IdSeatNavigation).WithMany()
                .HasForeignKey(d => d.IdSeat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatVehicle_Seat");

            entity.HasOne(d => d.IdStateSeatVehicleNavigation).WithMany()
                .HasForeignKey(d => d.IdStateSeatVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatVehicle_StateSeatVehicle");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany()
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatVehicle_Vehicle");
        });

        modelBuilder.Entity<StateAccount>(entity =>
        {
            entity.HasKey(e => e.IdStateAccount).HasName("PK__StateAcc__D19B468F2C2B2DEB");

            entity.ToTable("StateAccount");

            entity.Property(e => e.IdStateAccount).HasColumnName("idStateAccount");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StateHeadquarter>(entity =>
        {
            entity.HasKey(e => e.IdStateHeadquarter).HasName("PK__StateHea__33807AF2CEDFF001");

            entity.ToTable("StateHeadquarter");

            entity.Property(e => e.IdStateHeadquarter).HasColumnName("idStateHeadquarter");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StateSeatVehicle>(entity =>
        {
            entity.HasKey(e => e.IdStateSeatVehicle).HasName("PK__StateSea__CB5D4163A9D33BEE");

            entity.ToTable("StateSeatVehicle");

            entity.Property(e => e.IdStateSeatVehicle).HasColumnName("idStateSeatVehicle");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StateTrip>(entity =>
        {
            entity.HasKey(e => e.IdStateTrip).HasName("PK__StateTri__355FCB80AEC7F082");

            entity.ToTable("StateTrip");

            entity.Property(e => e.IdStateTrip).HasColumnName("idStateTrip");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TicketState>(entity =>
        {
            entity.HasKey(e => e.IdTicketState).HasName("PK__TicketSt__FC6BA6B36D565568");

            entity.ToTable("TicketState");

            entity.Property(e => e.IdTicketState).HasColumnName("idTicketState");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TravelRoute>(entity =>
        {
            entity.HasKey(e => e.IdTravelRoute).HasName("PK__TravelRo__98ED03AEF1CA59F2");

            entity.ToTable("TravelRoute");

            entity.Property(e => e.IdTravelRoute).HasColumnName("idTravelRoute");
            entity.Property(e => e.IdPlaceA).HasColumnName("idPlaceA");
            entity.Property(e => e.IdPlaceB).HasColumnName("idPlaceB");
            entity.Property(e => e.NameRoute)
                .HasMaxLength(50)
                .HasColumnName("nameRoute");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");

            entity.HasOne(d => d.IdPlaceANavigation).WithMany(p => p.TravelRouteIdPlaceANavigations)
                .HasForeignKey(d => d.IdPlaceA)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelRoute_PlaceA");

            entity.HasOne(d => d.IdPlaceBNavigation).WithMany(p => p.TravelRouteIdPlaceBNavigations)
                .HasForeignKey(d => d.IdPlaceB)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelRoute_PlaceB");
        });

        modelBuilder.Entity<TravelTicket>(entity =>
        {
            entity.HasKey(e => e.IdTravelTicket).HasName("PK__TravelTi__50775021A43CEC6D");

            entity.ToTable("TravelTicket");

            entity.Property(e => e.IdTravelTicket).HasColumnName("idTravelTicket");
            entity.Property(e => e.IdBilling).HasColumnName("idBilling");
            entity.Property(e => e.IdTicketState).HasColumnName("idTicketState");
            entity.Property(e => e.IdTravelRoute).HasColumnName("idTravelRoute");
            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");
            entity.Property(e => e.SeatNumber).HasColumnName("seatNumber");
            entity.Property(e => e.TravelDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("travelDate");

            entity.HasOne(d => d.IdBillingNavigation).WithMany(p => p.TravelTickets)
                .HasForeignKey(d => d.IdBilling)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelTicket_Person");

            entity.HasOne(d => d.IdTicketStateNavigation).WithMany(p => p.TravelTickets)
                .HasForeignKey(d => d.IdTicketState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelTicket_TicketState");

            entity.HasOne(d => d.IdTravelRouteNavigation).WithMany(p => p.TravelTickets)
                .HasForeignKey(d => d.IdTravelRoute)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelTicket_TravelRoute");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany(p => p.TravelTickets)
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TravelTicket_Vehicle");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("PK__Trip__B90DB49C53E3FE99");

            entity.ToTable("Trip");

            entity.Property(e => e.IdTrip).HasColumnName("idTrip");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("datetime")
                .HasColumnName("arrivalDate");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("datetime")
                .HasColumnName("departureDate");
            entity.Property(e => e.IdStateTrip).HasColumnName("idStateTrip");
            entity.Property(e => e.IdTravelTicket).HasColumnName("idTravelTicket");

            entity.HasOne(d => d.IdStateTripNavigation).WithMany(p => p.Trips)
                .HasForeignKey(d => d.IdStateTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trip_StateTrip");

            entity.HasOne(d => d.IdTravelTicketNavigation).WithMany(p => p.Trips)
                .HasForeignKey(d => d.IdTravelTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trip_TravelTicket");
        });

        modelBuilder.Entity<TypeDocument>(entity =>
        {
            entity.HasKey(e => e.IdTypeDocument).HasName("PK__TypeDocu__CF96EB7B2C6199CF");

            entity.ToTable("TypeDocument");

            entity.Property(e => e.IdTypeDocument).HasColumnName("idTypeDocument");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.IdVehicle).HasName("PK__Vehicle__B5E2475484902CD3");

            entity.ToTable("Vehicle");

            entity.Property(e => e.IdVehicle).HasColumnName("idVehicle");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.IdVehicleState).HasColumnName("idVehicleState");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.PlateNumber)
                .HasMaxLength(10)
                .HasColumnName("plateNumber");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicle_Person");

            entity.HasOne(d => d.IdVehicleStateNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.IdVehicleState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicle_VehicleState");
        });

        modelBuilder.Entity<VehicleState>(entity =>
        {
            entity.HasKey(e => e.IdVehicleState).HasName("PK__VehicleS__000F5CB0F92D849E");

            entity.ToTable("VehicleState");

            entity.Property(e => e.IdVehicleState).HasColumnName("idVehicleState");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
