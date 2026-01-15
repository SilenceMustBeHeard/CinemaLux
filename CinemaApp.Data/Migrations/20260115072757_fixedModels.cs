using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CinemaMovies_CinemaMovieId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cinemas_CinemaId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_CinemaMovies_MovieId_CinemaId_ShowTimes",
                table: "CinemaMovies");

            migrationBuilder.DropColumn(
                name: "ShowTimes",
                table: "CinemaMovies");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Tickets",
                newName: "PricePerTicket");

            migrationBuilder.AlterColumn<Guid>(
                name: "CinemaId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowTime",
                table: "CinemaMovies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CinemaMovies_MovieId_CinemaId_ShowTime",
                table: "CinemaMovies",
                columns: new[] { "MovieId", "CinemaId", "ShowTime" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CinemaMovies_CinemaMovieId",
                table: "Tickets",
                column: "CinemaMovieId",
                principalTable: "CinemaMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cinemas_CinemaId",
                table: "Tickets",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CinemaMovies_CinemaMovieId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cinemas_CinemaId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_CinemaMovies_MovieId_CinemaId_ShowTime",
                table: "CinemaMovies");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShowTime",
                table: "CinemaMovies");

            migrationBuilder.RenameColumn(
                name: "PricePerTicket",
                table: "Tickets",
                newName: "Price");

            migrationBuilder.AlterColumn<Guid>(
                name: "CinemaId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShowTimes",
                table: "CinemaMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CinemaMovies_MovieId_CinemaId_ShowTimes",
                table: "CinemaMovies",
                columns: new[] { "MovieId", "CinemaId", "ShowTimes" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CinemaMovies_CinemaMovieId",
                table: "Tickets",
                column: "CinemaMovieId",
                principalTable: "CinemaMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cinemas_CinemaId",
                table: "Tickets",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
