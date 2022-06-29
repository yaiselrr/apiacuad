using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.OtherModels.DB
{
    public partial class TORREON_BDContext : DbContext
    {
        public TORREON_BDContext()
        {
        }

        public TORREON_BDContext(DbContextOptions<TORREON_BDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CMedidor> CMedidors { get; set; }
        public virtual DbSet<CTipoServicio> CTipoServicios { get; set; }
        public virtual DbSet<Derivadum> Derivada { get; set; }
        public virtual DbSet<EstructuraHidraulica> EstructuraHidraulicas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TORREON_BD;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<CMedidor>(entity =>
            {
                entity.HasKey(e => e.MedidorC)
                    .HasName("PK__C_MEDIDOR__25869641");

                entity.ToTable("C_MEDIDOR");

                entity.Property(e => e.MedidorC)
                    .ValueGeneratedNever()
                    .HasColumnName("MEDIDOR_C");

                entity.Property(e => e.ConsumoGlobal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CONSUMO_GLOBAL");

                entity.Property(e => e.EstadoC).HasColumnName("ESTADO_C");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ALTA");

                entity.Property(e => e.LocalizacionC).HasColumnName("LOCALIZACION_C");

                entity.Property(e => e.MarcaC).HasColumnName("MARCA_C");

                entity.Property(e => e.ModeloC).HasColumnName("MODELO_C");

                entity.Property(e => e.NoElectronico)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NO_ELECTRONICO");

                entity.Property(e => e.NoFactura)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NO_FACTURA");

                entity.Property(e => e.NoSerie)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.ProveedorMedidorC).HasColumnName("PROVEEDOR_MEDIDOR_C");

                entity.Property(e => e.SectorC).HasColumnName("SECTOR_C");
            });

            modelBuilder.Entity<CTipoServicio>(entity =>
            {
                entity.HasKey(e => e.ServicioC)
                    .HasName("PK__C_TIPO_S__FF54C54733F2FC03");

                entity.ToTable("C_TIPO_SERVICIO");

                entity.Property(e => e.ServicioC)
                    .ValueGeneratedNever()
                    .HasColumnName("SERVICIO_C");

                entity.Property(e => e.ServicioD)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SERVICIO_D");
            });

            modelBuilder.Entity<Derivadum>(entity =>
            {
                entity.HasKey(e => new { e.Inmueble, e.Derivada })
                    .HasName("PK__DERIVADA__6DEC4894");

                entity.ToTable("DERIVADA");

                entity.Property(e => e.Inmueble).HasColumnName("INMUEBLE");

                entity.Property(e => e.Derivada).HasColumnName("DERIVADA");

                entity.Property(e => e.Adicionales).HasColumnName("ADICIONALES");

                entity.Property(e => e.Administrador).HasColumnName("ADMINISTRADOR");

                entity.Property(e => e.Alta)
                    .HasColumnType("datetime")
                    .HasColumnName("ALTA");

                entity.Property(e => e.Alterna)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ALTERNA");

                entity.Property(e => e.Beneficiados).HasColumnName("BENEFICIADOS");

                entity.Property(e => e.CalificaCliente)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CALIFICA_CLIENTE");

                entity.Property(e => e.Catastro)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("CATASTRO");

                entity.Property(e => e.Contrato).HasColumnName("CONTRATO");

                entity.Property(e => e.DirEnvio).HasColumnName("DIR_ENVIO");

                entity.Property(e => e.DirFacturacion).HasColumnName("DIR_FACTURACION");

                entity.Property(e => e.Edificio)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EDIFICIO");

                entity.Property(e => e.EstadopC).HasColumnName("ESTADOP_C");

                entity.Property(e => e.FolioCenso)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("FOLIO_CENSO");

                entity.Property(e => e.FolioRuta).HasColumnName("FOLIO_RUTA");

                entity.Property(e => e.GiroC).HasColumnName("GIRO_C");

                entity.Property(e => e.Interior)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("INTERIOR");

                entity.Property(e => e.MetrosConstruidos).HasColumnName("METROS_CONSTRUIDOS");

                entity.Property(e => e.NivelC).HasColumnName("NIVEL_C");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACION");

                entity.Property(e => e.Piso)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PISO");

                entity.Property(e => e.Propietario).HasColumnName("PROPIETARIO");

                entity.Property(e => e.RutaC).HasColumnName("RUTA_C");

                entity.Property(e => e.ServicioC).HasColumnName("SERVICIO_C");

                entity.Property(e => e.SituacionC).HasColumnName("SITUACION_C");

                entity.Property(e => e.Usuario).HasColumnName("USUARIO");
            });

            modelBuilder.Entity<EstructuraHidraulica>(entity =>
            {
                entity.HasKey(e => new { e.Inmueble, e.Derivada, e.Toma })
                    .HasName("PK__ESTRUCTURA_HIDRA__123EB7A3");

                entity.ToTable("ESTRUCTURA_HIDRAULICA");

                entity.Property(e => e.Inmueble).HasColumnName("INMUEBLE");

                entity.Property(e => e.Derivada).HasColumnName("DERIVADA");

                entity.Property(e => e.Toma).HasColumnName("TOMA");

                entity.Property(e => e.ALectura).HasColumnName("A_LECTURA");

                entity.Property(e => e.AbastecimientoC).HasColumnName("ABASTECIMIENTO_C");

                entity.Property(e => e.CasoFacturacion).HasColumnName("CASO_FACTURACION");

                entity.Property(e => e.CircuitoHidraulico).HasColumnName("CIRCUITO_HIDRAULICO");

                entity.Property(e => e.CvePeriodo).HasColumnName("CVE_PERIODO");

                entity.Property(e => e.DescargaC).HasColumnName("DESCARGA_C");

                entity.Property(e => e.DiametroDescarga).HasColumnName("DIAMETRO_DESCARGA");

                entity.Property(e => e.DiametroToma).HasColumnName("DIAMETRO_TOMA");

                entity.Property(e => e.EstadotC).HasColumnName("ESTADOT_C");

                entity.Property(e => e.MLectura).HasColumnName("M_LECTURA");

                entity.Property(e => e.MaterialDescargaC).HasColumnName("MATERIAL_DESCARGA_C");

                entity.Property(e => e.MaterialTomaC).HasColumnName("MATERIAL_TOMA_C");

                entity.Property(e => e.MedidorC).HasColumnName("MEDIDOR_C");

                entity.Property(e => e.PeriodoAbsolutoC).HasColumnName("PERIODO_ABSOLUTO_C");

                entity.Property(e => e.ServicioContratadoC).HasColumnName("SERVICIO_CONTRATADO_C");

                entity.Property(e => e.UbicacionToma)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UBICACION_TOMA");

                entity.Property(e => e.ValorX).HasColumnName("VALOR_X");

                entity.Property(e => e.ValorY).HasColumnName("VALOR_Y");

                entity.Property(e => e.ValorZ).HasColumnName("VALOR_Z");

                entity.Property(e => e.Volumenacumulado)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VOLUMENACUMULADO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
