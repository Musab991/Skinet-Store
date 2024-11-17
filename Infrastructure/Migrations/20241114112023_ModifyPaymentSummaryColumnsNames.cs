using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPaymentSummaryColumnsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentSummary_Year",
                table: "Orders",
                newName: "PaymentSummary_Last4");

            migrationBuilder.RenameColumn(
                name: "PaymentSummary_Last",
                table: "Orders",
                newName: "PaymentSummary_ExpYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentSummary_Last4",
                table: "Orders",
                newName: "PaymentSummary_Year");

            migrationBuilder.RenameColumn(
                name: "PaymentSummary_ExpYear",
                table: "Orders",
                newName: "PaymentSummary_Last");
        }
    }
}
