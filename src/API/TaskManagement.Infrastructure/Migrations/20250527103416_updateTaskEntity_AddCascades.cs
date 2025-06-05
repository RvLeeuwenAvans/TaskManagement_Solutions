using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTaskEntity_AddCascades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_DamageClaims_DamageClaimId",
                table: "LinkedObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects");
            
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_Relations_RelationId",
                table: "LinkedObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_LinkedObjects_LinkedObjectId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_LinkedObjectId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_DamageClaims_DamageClaimId",
                table: "LinkedObjects",
                column: "DamageClaimId",
                principalTable: "DamageClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects",
                column: "InsuranceId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_Relations_RelationId",
                table: "LinkedObjects",
                column: "RelationId",
                principalTable: "Relations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_DamageClaims_DamageClaimId",
                table: "LinkedObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkedObjects_Relations_RelationId",
                table: "LinkedObjects");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_LinkedObjectId",
                table: "Tasks",
                column: "LinkedObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_DamageClaims_DamageClaimId",
                table: "LinkedObjects",
                column: "DamageClaimId",
                principalTable: "DamageClaims",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_InsurancePolicies_InsuranceId",
                table: "LinkedObjects",
                column: "InsuranceId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedObjects_Relations_RelationId",
                table: "LinkedObjects",
                column: "RelationId",
                principalTable: "Relations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_LinkedObjects_LinkedObjectId",
                table: "Tasks",
                column: "LinkedObjectId",
                principalTable: "LinkedObjects",
                principalColumn: "Id");
        }
    }
}
