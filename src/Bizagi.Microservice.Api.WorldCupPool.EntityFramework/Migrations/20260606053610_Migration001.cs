using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Migration001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdUserCreator = table.Column<int>(type: "integer", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FlagUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IdGroup = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Groups_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    IdUser = table.Column<decimal>(type: "numeric", nullable: false),
                    IdRole = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    IdGroup = table.Column<decimal>(type: "numeric", nullable: false),
                    IdHomeTeam = table.Column<decimal>(type: "numeric", nullable: false),
                    IdAwayTeam = table.Column<decimal>(type: "numeric", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    RoundName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HomeTeamName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AwayTeamName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Groups_IdGroup",
                        column: x => x.IdGroup,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_IdAwayTeam",
                        column: x => x.IdAwayTeam,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_IdHomeTeam",
                        column: x => x.IdHomeTeam,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchResults",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    IdMatch = table.Column<decimal>(type: "numeric", nullable: false),
                    HomeGoals = table.Column<int>(type: "integer", nullable: false),
                    AwayGoals = table.Column<int>(type: "integer", nullable: false),
                    RegisteredByUserId = table.Column<decimal>(type: "numeric", nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchResults_Matches_IdMatch",
                        column: x => x.IdMatch,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    IdUser = table.Column<decimal>(type: "numeric", nullable: false),
                    IdMatch = table.Column<decimal>(type: "numeric", nullable: false),
                    HomeGoals = table.Column<int>(type: "integer", nullable: false),
                    AwayGoals = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsCalculated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CalculatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdUserCreator = table.Column<int>(type: "integer", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_Matches_IdMatch",
                        column: x => x.IdMatch,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Predictions_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreLogs",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric", nullable: false),
                    IdPrediction = table.Column<decimal>(type: "numeric", nullable: false),
                    IdMatchResult = table.Column<decimal>(type: "numeric", nullable: false),
                    PredictedHomeGoals = table.Column<int>(type: "integer", nullable: false),
                    PredictedAwayGoals = table.Column<int>(type: "integer", nullable: false),
                    RealHomeGoals = table.Column<int>(type: "integer", nullable: false),
                    RealAwayGoals = table.Column<int>(type: "integer", nullable: false),
                    PointsAssigned = table.Column<int>(type: "integer", nullable: false),
                    RuleApplied = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CalculationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreLogs_MatchResults_IdMatchResult",
                        column: x => x.IdMatchResult,
                        principalTable: "MatchResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScoreLogs_Predictions_IdPrediction",
                        column: x => x.IdPrediction,
                        principalTable: "Predictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_IdAwayTeam",
                table: "Matches",
                column: "IdAwayTeam");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_IdGroup",
                table: "Matches",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_IdHomeTeam",
                table: "Matches",
                column: "IdHomeTeam");

            migrationBuilder.CreateIndex(
                name: "IX_MatchResults_IdMatch",
                table: "MatchResults",
                column: "IdMatch",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_IdMatch",
                table: "Predictions",
                column: "IdMatch");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_IdUser_IdMatch",
                table: "Predictions",
                columns: new[] { "IdUser", "IdMatch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreLogs_IdMatchResult",
                table: "ScoreLogs",
                column: "IdMatchResult");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreLogs_IdPrediction",
                table: "ScoreLogs",
                column: "IdPrediction");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_IdGroup",
                table: "Teams",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_IdRole",
                table: "UserRoles",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_IdUser",
                table: "UserRoles",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreLogs");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "MatchResults");

            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
