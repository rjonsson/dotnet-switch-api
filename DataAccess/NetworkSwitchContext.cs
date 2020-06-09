using Microsoft.EntityFrameworkCore;
using switch_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace switch_api.DataAccess
{
    public class NetworkSwitchContext : DbContext
    {
        public NetworkSwitchContext(DbContextOptions options) : base(options) { }
        public DbSet<NetworkSwitch> NetworkSwitches { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Vlan> Vlans { get; set; }
        public DbSet<PortVlan> PortVlans { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NetworkSwitch>()
                        .HasKey(swi => swi.Id);

            modelBuilder.Entity<NetworkSwitch>()
                        .Property(swi => swi.Name)
                        .IsRequired();

            modelBuilder.Entity<Port>()
                        .HasKey(port => port.Id);

            modelBuilder.Entity<Port>()
                        .HasOne(port => port.NetworkSwitch)
                        .WithMany(swi => swi.Ports)
                        .HasForeignKey(port => port.NetworkSwitchId);

            modelBuilder.Entity<Vlan>()
                        .Property(vlan => vlan.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<Vlan>()
                        .HasKey(vlan => vlan.Id);

            modelBuilder.Entity<PortVlan>()
                        .HasKey(portvlan => new { portvlan.PortId, portvlan.VlanId });

            modelBuilder.Entity<PortVlan>()
                        .HasOne(portvlan => portvlan.Port)
                        .WithMany(port => port.Vlans)
                        .HasForeignKey(portvlan => portvlan.PortId);

            modelBuilder.Entity<PortVlan>()
                        .HasOne(portvlan => portvlan.Vlan)
                        .WithMany(vlan => vlan.Ports)
                        .HasForeignKey(portvlan => portvlan.VlanId);

        }    
    }
}
