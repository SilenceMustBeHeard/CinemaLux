using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAppuserMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMovies_AspNetUsers_AppUserId",
                table: "AppUserMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMovies_Movies_MovieId",
                table: "AppUserMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserMovies",
                table: "AppUserMovies");

            migrationBuilder.RenameTable(
                name: "AppUserMovies",
                newName: "AppUserMovie");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserMovies_MovieId",
                table: "AppUserMovie",
                newName: "IX_AppUserMovie_MovieId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "AppUserMovie",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppUserMovie",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "AppUserMovie",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserMovie",
                table: "AppUserMovie",
                columns: new[] { "AppUserId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMovie_AspNetUsers_AppUserId",
                table: "AppUserMovie",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMovie_Movies_MovieId",
                table: "AppUserMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMovie_AspNetUsers_AppUserId",
                table: "AppUserMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMovie_Movies_MovieId",
                table: "AppUserMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserMovie",
                table: "AppUserMovie");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "AppUserMovie");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppUserMovie");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "AppUserMovie");

            migrationBuilder.RenameTable(
                name: "AppUserMovie",
                newName: "AppUserMovies");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserMovie_MovieId",
                table: "AppUserMovies",
                newName: "IX_AppUserMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserMovies",
                table: "AppUserMovies",
                columns: new[] { "AppUserId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMovies_AspNetUsers_AppUserId",
                table: "AppUserMovies",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMovies_Movies_MovieId",
                table: "AppUserMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
