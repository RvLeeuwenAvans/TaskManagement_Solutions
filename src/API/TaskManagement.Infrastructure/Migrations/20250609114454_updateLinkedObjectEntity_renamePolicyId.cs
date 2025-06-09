using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateLinkedObjectEntity_renamePolicyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects");

            migrationBuilder.RenameColumn(
                name: "InsuranceId",
                table: "LinkedObjects",
                newName: "InsurancePolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkedObjects_InsuranceId",
                table: "LinkedObjects",
                newName: "IX_LinkedObjects_InsurancePolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsurancePolicyId",
                table: "LinkedObjects",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsurancePolicyId",
                table: "LinkedObjects");

            migrationBuilder.RenameColumn(
                name: "InsurancePolicyId",
                table: "LinkedObjects",
                newName: "InsuranceId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkedObjects_InsurancePolicyId",
                table: "LinkedObjects",
                newName: "IX_LinkedObjects_InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects",
                column: "InsuranceId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
