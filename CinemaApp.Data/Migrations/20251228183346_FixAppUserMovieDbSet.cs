using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAppUserMovieDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameTable(
                name: "AppUserMovie",
                newName: "AppUserMovies");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserMovie_MovieId",
                table: "AppUserMovies",
                newName: "IX_AppUserMovies_MovieId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
