using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AddressElements_ObjectGuid",
                table: "AddressElements",
                column: "ObjectGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressElements_ParentObjId",
                table: "AddressElements",
                column: "ParentObjId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AddressElements_ObjectGuid",
                table: "AddressElements");

            migrationBuilder.DropIndex(
                name: "IX_AddressElements_ParentObjId",
                table: "AddressElements");
        }
    }
}
