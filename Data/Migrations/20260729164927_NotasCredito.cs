using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotasCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbCategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCategoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbCliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoIdentificacion = table.Column<int>(type: "int", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SegundoApellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCliente", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "tbProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    TipoImpuesto = table.Column<int>(type: "int", nullable: false),
                    PorcentajeImpuesto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DescuentoMaximo = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbProducto_tbCategoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "tbCategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpuestoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbFactura_tbCliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tbCliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbFacturaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeImpuesto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MontoImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescuentoAplicado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbFacturaDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbFacturaDetalle_tbFactura_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "tbFactura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbFacturaDetalle_tbProducto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "tbProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbNotaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaOriginalId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpuestoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbNotaCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbNotaCredito_tbCliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tbCliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbNotaCredito_tbFactura_FacturaOriginalId",
                        column: x => x.FacturaOriginalId,
                        principalTable: "tbFactura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbNotaCreditoDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false),
                    FacturaDetalleOriginalId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeImpuesto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MontoImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescuentoAplicado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbNotaCreditoDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbNotaCreditoDetalle_tbFacturaDetalle_FacturaDetalleOriginalId",
                        column: x => x.FacturaDetalleOriginalId,
                        principalTable: "tbFacturaDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbNotaCreditoDetalle_tbNotaCredito_NotaCreditoId",
                        column: x => x.NotaCreditoId,
                        principalTable: "tbNotaCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbNotaCreditoDetalle_tbProducto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "tbProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbCliente_TipoIdentificacion_NumeroIdentificacion",
                table: "tbCliente",
                columns: new[] { "TipoIdentificacion", "NumeroIdentificacion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbFactura_ClienteId",
                table: "tbFactura",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tbFacturaDetalle_FacturaId",
                table: "tbFacturaDetalle",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_tbFacturaDetalle_ProductoId",
                table: "tbFacturaDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbNotaCredito_ClienteId",
                table: "tbNotaCredito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tbNotaCredito_FacturaOriginalId",
                table: "tbNotaCredito",
                column: "FacturaOriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_tbNotaCreditoDetalle_FacturaDetalleOriginalId",
                table: "tbNotaCreditoDetalle",
                column: "FacturaDetalleOriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_tbNotaCreditoDetalle_NotaCreditoId",
                table: "tbNotaCreditoDetalle",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbNotaCreditoDetalle_ProductoId",
                table: "tbNotaCreditoDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbProducto_CategoriaId",
                table: "tbProducto",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbNotaCreditoDetalle");

            migrationBuilder.DropTable(
                name: "tbFacturaDetalle");

            migrationBuilder.DropTable(
                name: "tbNotaCredito");

            migrationBuilder.DropTable(
                name: "tbProducto");

            migrationBuilder.DropTable(
                name: "tbFactura");

            migrationBuilder.DropTable(
                name: "tbCategoria");

            migrationBuilder.DropTable(
                name: "tbCliente");
        }
    }
}
