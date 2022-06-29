using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.OtherModels
{
    public partial class TORREON_BDEContext : DbContext
    {
        public TORREON_BDEContext()
        {
        }

        public TORREON_BDEContext(DbContextOptions<TORREON_BDEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CConceptoacobrar> CConceptoacobrars { get; set; }
        public virtual DbSet<CFactura> CFacturas { get; set; }
        public virtual DbSet<CIndividuo> CIndividuos { get; set; }
        public virtual DbSet<CMedidor> CMedidors { get; set; }
        public virtual DbSet<CTipoServicio> CTipoServicios { get; set; }
        public virtual DbSet<Derivadum> Derivada { get; set; }
        public virtual DbSet<EstadoDeCuentum> EstadoDeCuenta { get; set; }
        public virtual DbSet<EstructuraHidraulica> EstructuraHidraulicas { get; set; }
        public virtual DbSet<RegistroFactura> RegistroFacturas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=TORREON_BD;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<CConceptoacobrar>(entity =>
            {
                entity.HasKey(e => e.ConceptocobrarC)
                    .HasName("PK__C_CONCEPTOACOBRA__58D1301D");

                entity.ToTable("C_CONCEPTOACOBRAR");

                entity.Property(e => e.ConceptocobrarC)
                    .ValueGeneratedNever()
                    .HasColumnName("CONCEPTOCOBRAR_C");

                entity.Property(e => e.Adicional).HasColumnName("ADICIONAL");

                entity.Property(e => e.Aplicado).HasColumnName("APLICADO");

                entity.Property(e => e.Arrastrado).HasColumnName("ARRASTRADO");

                entity.Property(e => e.ConceptocobrarD)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("CONCEPTOCOBRAR_D");

                entity.Property(e => e.Facturacion).HasColumnName("FACTURACION");

                entity.Property(e => e.Impuesto)
                    .HasColumnType("decimal(8, 4)")
                    .HasColumnName("IMPUESTO");

                entity.Property(e => e.NombreCorto)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_CORTO");

                entity.Property(e => e.Proceso).HasColumnName("PROCESO");

                entity.Property(e => e.TipoCalculo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CALCULO");

                entity.Property(e => e.UnidadC).HasColumnName("UNIDAD_C");

                entity.Property(e => e.VigenciaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("VIGENCIA_FIN");

                entity.Property(e => e.VigenciaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("VIGENCIA_INICIO");
            });

            modelBuilder.Entity<CFactura>(entity =>
            {
                entity.HasKey(e => e.FacturaC)
                    .IsClustered(false);

                entity.ToTable("C_FACTURA");

                entity.Property(e => e.FacturaC)
                    .ValueGeneratedNever()
                    .HasColumnName("FACTURA_C");

                entity.Property(e => e.AFactura).HasColumnName("A_FACTURA");

                entity.Property(e => e.BloqueC).HasColumnName("BLOQUE_C");

                entity.Property(e => e.CasoFacturacionC).HasColumnName("CASO_FACTURACION_C");

                entity.Property(e => e.ConsumoDescontado)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CONSUMO_DESCONTADO");

                entity.Property(e => e.ConsumoFacturado)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CONSUMO_FACTURADO");

                entity.Property(e => e.Derivada).HasColumnName("DERIVADA");

                entity.Property(e => e.EdoP).HasColumnName("EDO_P");

                entity.Property(e => e.EdoT).HasColumnName("EDO_T");

                entity.Property(e => e.Entregada).HasColumnName("ENTREGADA");

                entity.Property(e => e.EstadoC).HasColumnName("ESTADO_C");

                entity.Property(e => e.FechaCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CORTE");

                entity.Property(e => e.FechaEmision)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_EMISION");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ENTREGA");

                entity.Property(e => e.FechaEstado)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ESTADO");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_VENCIMIENTO");

                entity.Property(e => e.Inicioadeudo).HasColumnName("INICIOADEUDO");

                entity.Property(e => e.Inmueble).HasColumnName("INMUEBLE");

                entity.Property(e => e.LecturaAnt).HasColumnName("LECTURA_ANT");

                entity.Property(e => e.Lecturista).HasColumnName("LECTURISTA");

                entity.Property(e => e.MFactura).HasColumnName("M_FACTURA");

                entity.Property(e => e.MetodoAnticipo).HasColumnName("METODO_ANTICIPO");

                entity.Property(e => e.NivelC).HasColumnName("NIVEL_C");

                entity.Property(e => e.Pagada).HasColumnName("PAGADA");

                entity.Property(e => e.PeriodoAbsolutoC).HasColumnName("PERIODO_ABSOLUTO_C");

                entity.Property(e => e.PeriodosAdeudo).HasColumnName("PERIODOS_ADEUDO");

                entity.Property(e => e.RefAnticipo)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("REF_ANTICIPO");

                entity.Property(e => e.SecLectura).HasColumnName("SEC_LECTURA");

                entity.Property(e => e.ServicioC).HasColumnName("SERVICIO_C");

                entity.Property(e => e.SitP).HasColumnName("SIT_P");

                entity.Property(e => e.Toma).HasColumnName("TOMA");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UUID");
            });

            modelBuilder.Entity<CIndividuo>(entity =>
            {
                entity.HasKey(e => e.IndividuoC)
                    .HasName("PK__C_INDIVI__5434A1679B9BB26B");

                entity.ToTable("C_INDIVIDUO");

                entity.Property(e => e.IndividuoC)
                    .ValueGeneratedNever()
                    .HasColumnName("INDIVIDUO_C");

                entity.Property(e => e.Banco).HasColumnName("BANCO");

                entity.Property(e => e.Ctabancaria)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CTABANCARIA");

                entity.Property(e => e.DomComentario)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOM_COMENTARIO");

                entity.Property(e => e.DomEstado)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOM_ESTADO");

                entity.Property(e => e.DomFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("DOM_FECHA");

                entity.Property(e => e.DomRef)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DOM_REF");

                entity.Property(e => e.DomTcta)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DOM_TCTA");

                entity.Property(e => e.DomTitular)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DOM_TITULAR");

                entity.Property(e => e.EMail)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("E_MAIL");

                entity.Property(e => e.FacturaId).HasColumnName("FACTURA_ID");

                entity.Property(e => e.Foto)
                    .HasColumnType("image")
                    .HasColumnName("foto");

                entity.Property(e => e.Identificacion)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("IDENTIFICACION");

                entity.Property(e => e.IndividuoD)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("INDIVIDUO_D");

                entity.Property(e => e.Limite)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("LIMITE");

                entity.Property(e => e.Registro)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("REGISTRO");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO1");

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO2");
            });

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

            modelBuilder.Entity<EstadoDeCuentum>(entity =>
            {
                entity.HasKey(e => new { e.Inmueble, e.Derivada, e.Toma })
                    .HasName("PK__ESTADO_DE_CUENTA__70A8B9AE");

                entity.ToTable("ESTADO_DE_CUENTA");

                entity.Property(e => e.Inmueble).HasColumnName("INMUEBLE");

                entity.Property(e => e.Derivada).HasColumnName("DERIVADA");

                entity.Property(e => e.Toma).HasColumnName("TOMA");

                entity.Property(e => e.AFactura).HasColumnName("A_FACTURA");

                entity.Property(e => e.Adeudoanterior)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("ADEUDOANTERIOR");

                entity.Property(e => e.Cargosactuales)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("CARGOSACTUALES");

                entity.Property(e => e.Cargospendientes)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("CARGOSPENDIENTES");

                entity.Property(e => e.Creditosactuales)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("CREDITOSACTUALES");

                entity.Property(e => e.CvePeriodo).HasColumnName("CVE_PERIODO");

                entity.Property(e => e.Documento).HasColumnName("DOCUMENTO");

                entity.Property(e => e.MFactura).HasColumnName("M_FACTURA");

                entity.Property(e => e.Pagosactuales)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("PAGOSACTUALES");

                entity.Property(e => e.PeriodoAbsolutoC).HasColumnName("PERIODO_ABSOLUTO_C");

                entity.Property(e => e.Periodofinaladeudo).HasColumnName("PERIODOFINALADEUDO");

                entity.Property(e => e.Periodoinicialadeudo).HasColumnName("PERIODOINICIALADEUDO");

                entity.Property(e => e.Periodosdeadeudo).HasColumnName("PERIODOSDEADEUDO");

                entity.Property(e => e.Primerafacturacion)
                    .HasColumnType("datetime")
                    .HasColumnName("PRIMERAFACTURACION");

                entity.Property(e => e.Saldototal)
                    .HasColumnType("decimal(21, 4)")
                    .HasColumnName("SALDOTOTAL");

                entity.HasOne(d => d.Derivadum)
                    .WithMany(p => p.EstadoDeCuenta)
                    .HasForeignKey(d => new { d.Inmueble, d.Derivada })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ESTADO_DE_CUENTA__0A1E72EE");
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

            modelBuilder.Entity<RegistroFactura>(entity =>
            {
                entity.HasKey(e => new { e.FacturaC, e.ConceptocobrarC, e.Renglon })
                    .IsClustered(false);

                entity.ToTable("REGISTRO_FACTURA");

                entity.Property(e => e.FacturaC).HasColumnName("FACTURA_C");

                entity.Property(e => e.ConceptocobrarC).HasColumnName("CONCEPTOCOBRAR_C");

                entity.Property(e => e.Renglon).HasColumnName("RENGLON");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.ImpAnticipo)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("IMP_ANTICIPO");

                entity.Property(e => e.ImpNegociado)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("IMP_NEGOCIADO");

                entity.Property(e => e.MovAplicado).HasColumnName("MOV_APLICADO");

                entity.Property(e => e.Registro).HasColumnName("REGISTRO");

                entity.Property(e => e.SaldoActual)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("SALDO_ACTUAL");

                entity.Property(e => e.SaldoAnterior)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("SALDO_ANTERIOR");

                entity.Property(e => e.Tasa01)
                    .HasColumnType("decimal(8, 4)")
                    .HasColumnName("TASA_01");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
