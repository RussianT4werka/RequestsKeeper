using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RequestsKeeper.Model;

namespace RequestsKeeper.DB;

public partial class User502Context : DbContext
{
    public User502Context()
    {
    }

    public User502Context(DbContextOptions<User502Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<SubDivision> SubDivisions { get; set; }

    public virtual DbSet<TypeRequest> TypeRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    public virtual DbSet<VisitorsRequest> VisitorsRequests { get; set; }

    public virtual DbSet<VisitorsVisit> VisitorsVisits { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=192.168.200.35;user=user50;password=26643;database=user50_2;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_100_CS_AI_SC_UTF8");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Request");

            entity.Property(e => e.FinishDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TypeRequestId).HasColumnName("TypeRequestID");
            entity.Property(e => e.UsersId).HasColumnName("UsersID");
            entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

            entity.HasOne(d => d.Status).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_Status");

            entity.HasOne(d => d.TypeRequest).WithMany(p => p.Requests)
                .HasForeignKey(d => d.TypeRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_TypeRequest");

            entity.HasOne(d => d.Users).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_Users");

            entity.HasOne(d => d.Worker).WithMany(p => p.Requests)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_Worker");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SubDivision>(entity =>
        {
            entity.ToTable("SubDivision");
        });

        modelBuilder.Entity<TypeRequest>(entity =>
        {
            entity.ToTable("TypeRequest");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("Visit");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.GroupNumber).HasMaxLength(50);
            entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

            entity.HasOne(d => d.Worker).WithMany(p => p.Visits)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visit_Worker");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.Property(e => e.DoB).HasColumnType("date");
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.UsersId).HasColumnName("UsersID");

            entity.HasOne(d => d.Users).WithMany(p => p.Visitors)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visitors_Users");
        });

        modelBuilder.Entity<VisitorsRequest>(entity =>
        {
            entity.ToTable("VisitorsRequest");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.VisitorsId).HasColumnName("VisitorsID");

            entity.HasOne(d => d.Request).WithMany(p => p.VisitorsRequests)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_VisitorsRequest_Request");

            entity.HasOne(d => d.Visitors).WithMany(p => p.VisitorsRequests)
                .HasForeignKey(d => d.VisitorsId)
                .HasConstraintName("FK_VisitorsRequest_Visitors");
        });

        modelBuilder.Entity<VisitorsVisit>(entity =>
        {
            entity.ToTable("VisitorsVisit");

            entity.Property(e => e.VisitId).HasColumnName("VisitID");
            entity.Property(e => e.VisitorsId).HasColumnName("VisitorsID");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitorsVisits)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitorsVisit_Visit");

            entity.HasOne(d => d.Visitors).WithMany(p => p.VisitorsVisits)
                .HasForeignKey(d => d.VisitorsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitorsVisit_Visitors");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.ToTable("Worker");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.SubDivisionId).HasColumnName("SubDivisionID");

            entity.HasOne(d => d.Department).WithMany(p => p.Workers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Worker_Department");

            entity.HasOne(d => d.SubDivision).WithMany(p => p.Workers)
                .HasForeignKey(d => d.SubDivisionId)
                .HasConstraintName("FK_Worker_SubDivision");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
