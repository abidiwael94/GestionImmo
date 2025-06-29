using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionImmo.Migrations
{
    /// <inheritdoc />
    public partial class predictionFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoolingType",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedPrice",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Furnished",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GarageSpaces",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBasement",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasElevator",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGarage",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPool",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HeatingType",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ListingDate",
                table: "Properties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LotSize",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SquareFeet",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalFloors",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearBuilt",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CoolingType",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "EstimatedPrice",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Furnished",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "GarageSpaces",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "HasBasement",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "HasElevator",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "HasGarage",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "HasPool",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "HeatingType",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ListingDate",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "LotSize",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SquareFeet",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "TotalFloors",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "YearBuilt",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Properties");
        }
    }
}
