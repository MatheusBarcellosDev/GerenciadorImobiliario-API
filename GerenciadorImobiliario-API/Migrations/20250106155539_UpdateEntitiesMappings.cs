using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorImobiliario_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Client_OwnerId",
                table: "Property");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Property",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Property",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "CondominiumFee",
                table: "Property",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBalcony",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBarbecueGrill",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBuiltInWardrobes",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGarden",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPool",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Iptu",
                table: "Property",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFurnished",
                table: "Property",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBathrooms",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBedrooms",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfParkingSpaces",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RentPrice",
                table: "Property",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Property",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "TotalArea",
                table: "Property",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Property",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "UsableArea",
                table: "Property",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Property",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Stage",
                table: "PipelineStage",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LeadStatus",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientPropertyInterest",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    InterestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPropertyInterest", x => new { x.ClientId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ClientPropertyInterest_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPropertyInterest_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropertyDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyDocument_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropertyImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyImage_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "LeadStatus",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Status",
                value: "NovoLead");

            migrationBuilder.UpdateData(
                table: "LeadStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Status",
                value: "Cliente");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Stage",
                value: "EsperandoAtendimento");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Stage",
                value: "EmAtendimento");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Stage",
                value: "EmVisita");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Stage",
                value: "PropostaEnviada");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Stage",
                value: "Documentacao");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Stage",
                value: "VendaConcluida");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Stage",
                value: "VendaPerdida");

            migrationBuilder.CreateIndex(
                name: "IX_Property_UserId",
                table: "Property",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPropertyInterest_PropertyId",
                table: "ClientPropertyInterest",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocument_PropertyId",
                table: "PropertyDocument",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImage_PropertyId",
                table: "PropertyImage",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Client_OwnerId",
                table: "Property",
                column: "OwnerId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_User_UserId",
                table: "Property",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Client_OwnerId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_User_UserId",
                table: "Property");

            migrationBuilder.DropTable(
                name: "ClientPropertyInterest");

            migrationBuilder.DropTable(
                name: "PropertyDocument");

            migrationBuilder.DropTable(
                name: "PropertyImage");

            migrationBuilder.DropIndex(
                name: "IX_Property_UserId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "CondominiumFee",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasBalcony",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasBarbecueGrill",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasBuiltInWardrobes",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasGarden",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "HasPool",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Iptu",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "IsFurnished",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "NumberOfBathrooms",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "NumberOfBedrooms",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "NumberOfParkingSpaces",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "RentPrice",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "UsableArea",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Property");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Property",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Property",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Stage",
                table: "PipelineStage",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "LeadStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "LeadStatus",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LeadStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Stage",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Stage",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Stage",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Stage",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Stage",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Stage",
                value: 6);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Stage",
                value: 7);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Client_OwnerId",
                table: "Property",
                column: "OwnerId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
