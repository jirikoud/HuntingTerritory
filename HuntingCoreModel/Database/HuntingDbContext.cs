using HuntingCoreModel.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database
{
    public class HuntingDbContext : DbContext
    {
        private readonly ILogger<HuntingDbContext> _logger;

        public HuntingDbContext(DbContextOptions<HuntingDbContext> options, ILogger<HuntingDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<AclUser> AclUsers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<EmailInfo> EmailInfos { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MapArea> MapAreas { get; set; }
        public DbSet<MapItem> MapItems { get; set; }
        public DbSet<MapItemType> MapItemTypes { get; set; }
        public DbSet<ModelVersion> ModelVersions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<TerritoryUser> TerritoryUsers { get; set; }
        public DbSet<TerritoryUserContact> TerritoryUserContacts { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserMapPoint> UserMapPoints { get; set; }
        public DbSet<UserMapPointShare> UserMapPointShares { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
    }
}
