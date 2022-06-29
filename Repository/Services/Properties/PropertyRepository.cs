using API.Data;
using API.Models;
using API.Models.Dto;
using API.OtherModels.DB;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace API.Repository.Services.Properties
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly TORREON_BDContext _dbOther;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyRepository> _logger;
        private readonly UserManager<GeneralDataUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly string propertyNotificationHtmlBody = @"<table style=""max-width: 600px;padding: 10px;margin: 0 auto;border-collapse: collapse;"">
		
		
		<tr class=""mb-4"">
			<td>
				<div style=""color: #34495e;margin: 4% 10% 2%; text-align: justify;font-family: sans-serif;"">
					<h2 style=""color: #0099a8;margin: 0 0 7px;"">Estimada/o usuaria/o</h2>
					<p style=""margin: 2px; font-size: 15px;"">
						El usuario con correo @Email dio de alta el inmueble @Inmueble en el que usted es usuario principal;
					</p>
					<p style =""margin: 2px; font-size: 15px;"">
                        si no está de acuerdo con esta alta, puede editarlo en la siguiente url: @UrlLink
                    </p>
                </div>		
			</td>
		</tr>
        <tr>
			<td style=""text-align: left;padding: 0;"">
				<img src = ""09.png"" width=""20%"" style=""display: block;margin: 1.5% 3%;"">	
				<p style = ""margin: 2px; font-size: 15px;"" >
                        Para más información favor de contactarnos al 871 74 91 700
				</p>			
			</td>
		</tr>
		
		
	</table>";

        public PropertyRepository(ApplicationDBContext db, TORREON_BDContext dbOther,  IMapper mapper, ILogger<PropertyRepository> logger, UserManager<GeneralDataUser> userManager, IConfiguration configuration)
        {
            _db = db;
            _dbOther = dbOther;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateProperty(PropertyDto propertyDto)
        {
            Property property = _mapper.Map<PropertyDto, Property>(propertyDto);

            var user = await _userManager.FindByEmailAsync(propertyDto.Email);

            var propertyFind = _db.Properties.FirstOrDefault(p => p.PropertyNumber == property.PropertyNumber 
                               && p.GeneralDataUserId == user.Id);

            var userPropertyFind = _db.Properties.FirstOrDefault(p => p.PropertyNumber == property.PropertyNumber && p.IsOwner == true);
            var owner = await _userManager.FindByIdAsync(userPropertyFind.GeneralDataUserId);

            if (propertyFind != null)
            {
                return "ExistProperty";
            }

            if (propertyFind == null && ValidateInsertAllAsync(propertyDto.PropertyNumber, propertyDto.DerivativeNumber, propertyDto.TakeNumber))
            {
                _logger.LogInformation("Ejecutando la funcionalidad Adicionar Inmueble");

                var getProperty = GetDataProperty(propertyDto.PropertyNumber, propertyDto.DerivativeNumber, propertyDto.TakeNumber);

                string typeService = GetTypeService(getProperty.ServicioContratadoC);

                if (userPropertyFind == null)
                {
                    var userIsProperty = new Property
                    {
                        PropertyNumber = property.PropertyNumber,
                        GeneralDataUserId = user.Id,
                        DerivativeNumber = property.DerivativeNumber,
                        TakeNumber = property.TakeNumber,
                        MeterNumber = getProperty.MedidorC.ToString(),
                        TypeService = typeService,
                        IsOwner = true,
                        IsActive = true,
                        IsEnabled = true
                    };

                    await _db.Properties.AddAsync(userIsProperty);

                    await _db.SaveChangesAsync();
                }
                else
                {
                    var userIsNoProperty = new Property
                    {
                        PropertyNumber = property.PropertyNumber,
                        GeneralDataUserId = user.Id,
                        DerivativeNumber = property.DerivativeNumber,
                        TakeNumber = property.TakeNumber,
                        MeterNumber = getProperty.MedidorC.ToString(),
                        TypeService = typeService,
                        IsOwner = false,
                        IsActive = true,
                        IsEnabled = true
                    };

                    await _db.Properties.AddAsync(userIsNoProperty);

                    await _db.SaveChangesAsync();

                    if (userPropertyFind.IsOwner)
                    {
                        //var uriBuilder = new UriBuilder(_configuration["ReturnPaths:AuthorizeUser"]);

                        //var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                        /*query["userid"] = propertyDto.Email;

                        query["propertyid"] = userPropertyFind.PropertyNumber;*/
                        var uriBuilder = new UriBuilder(_configuration["ReturnPaths:Transfer"]) + owner.Email;

                        var htmlUrl = propertyNotificationHtmlBody.Replace("@UrlLink", uriBuilder.ToString()).Replace("@Inmueble", userPropertyFind.PropertyNumber).Replace("@Email", propertyDto.Email);                        

                        var urlString = htmlUrl;

                        //uriBuilder.Query = query.ToString();

                        //var urlString = uriBuilder.ToString();

                        var senderEmail = propertyDto.Email;

                        if (!await SendEmailAsync(senderEmail, propertyDto.Email, "Notificación de alta de inmueble.", urlString))
                        {
                            return "NoSendMail";
                        }
                    }
                }              

                return "Ok";
            }
            else
            {
                _logger.LogError("El inmueble no existe");

                return "NoExistProperty";
            }
        }

        public async Task<string> DeleteProperty(string email, string id)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Eliminar Inmueble por ID");

            var user = await _userManager.FindByEmailAsync(email);

            Property property = await _db.Properties.Where(p => p.PropertyNumber == id && p.GeneralDataUserId == user.Id).FirstOrDefaultAsync();

            if (property == null)
            {
                return "NoProperty";
            }
            else
            {
                _db.Properties.Remove(property);

                //property.IsActive = false;

                //_db.Properties.Update(property);

                await _db.SaveChangesAsync();

                return "Ok";
            }
        }

        public async Task<List<Property>> GetProperties(string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Lista de Inmuebles");

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                List<Property> list = await _db.Properties.Where(p => p.GeneralDataUserId == user.Id).ToListAsync();

                return list;
            }
            else
            {
                return null;
            }
        }

        public async Task<PropertyDto> GetPropertyById(string id, string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Inmueble por ID");

            var user = await _userManager.FindByEmailAsync(email);

            Property property = await _db.Properties.Where(p => p.PropertyNumber == id && p.GeneralDataUserId == user.Id).FirstOrDefaultAsync();

            if (property != null)
            {
                return _mapper.Map<PropertyDto>(property);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SuccessAuth(string email, string token, string isAuthenticated)
        {
            _logger.LogInformation("Ejecutando la validando la Autenticación del Usuario");

            string emailToken = "";

            if (email == "" || token == "" || isAuthenticated == "")
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            if (jwt != null && isAuthenticated == "true")
            {
                emailToken = jwt.Claims.First(claim => claim.Type == "email").Value;
            }

            if (emailToken == null && !email.Equals(emailToken))
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !user.EmailConfirmed)
            {
                return false;
            }

            if (user != null && !user.EmailConfirmed)
            {
                return false;
            }

            return true;
        }

        public async Task<string> TransferProperty(TransferPropertyDto model)
        {
            string noOwner = "no";

            var userOwner = await _userManager.FindByEmailAsync(model.Email);
            var userNoOwner = await _userManager.FindByEmailAsync(model.EmailToTransfer);

            if (userOwner == null)
            {
                return "UserOwnerNoExist";
            }

            var userTransfer = await _userManager.FindByEmailAsync(model.EmailToTransfer);

            if (userTransfer == null)
            {
                return "userTransferNoExist";
            }

            Property isProperty = _db.Properties
                            .FirstOrDefault(
                                   c => c.PropertyNumber == model.PropertyId && c.GeneralDataUserId == userOwner.Id && c.IsOwner == true);

            Property isNotProperty = _db.Properties
                            .FirstOrDefault(
                                   c => c.PropertyNumber == model.PropertyId && c.GeneralDataUserId == userTransfer.Id && c.IsOwner == false);

            if (isProperty == null)
            {
                return "PropertyNoExist";
            }
            
            if (isNotProperty == null)
            {
                return "NoPropertyNoExist";
            }

            if (isProperty != null)
            {
                _logger.LogInformation("Ejecutando la funcionalidad Traspaso de Inmueble Dejar de ser Propietario");

                isProperty.IsOwner = false;

                _db.Properties.Update(isProperty);

                await _db.SaveChangesAsync();

                noOwner = "si";

                string message = $"El Inmueble ha Sido Traspasado a la Cuenta de Usuario {userTransfer.Email} para ser el nuevo propietario";

                if (!await SendEmailAsync(_configuration["ReturnPaths:SenderEmail"], userOwner.Email, "Confirmación de Traspaso de inmueble", message))
                {
                    return "NoSendMail";
                }

            }

            if (isNotProperty != null && noOwner == "si")
            {
                _logger.LogInformation("Ejecutando la funcionalidad Traspaso de Inmueble al Nuevo Propietario");

                isNotProperty.IsOwner = true;

                _db.Properties.Update(isNotProperty);

                await _db.SaveChangesAsync();

                string message = $"El Inmueble ha Sido Traspasado a su Cuenta de Usuario {userTransfer.Email} ahora usted es el propietario";

                if (!await SendEmailAsync(_configuration["ReturnPaths:SenderEmail"], userTransfer.Email, "Confirmación de Traspaso de inmueble", message))
                {
                    return "NoSendMail";
                }

                return "Ok";
            }
            else
            {
                _logger.LogError("El Inmueble no Existe");

                return "NoProperty";
            }
        }

        public async Task<string> UpdateProperty(string email, PropertyUpdateDto propertyUpdateDto)
        {
            var user = await _userManager.FindByEmailAsync(email);

            Property IsProperty= _db.Properties
                            .FirstOrDefault(
                                   c => c.PropertyNumber == propertyUpdateDto.PropertyNumber && c.GeneralDataUserId == user.Id && c.IsActive == true && c.IsEnabled == true);


            if (IsProperty == null)
            {
                return "NoExist";
            }

            if (IsProperty != null)
            {
                _logger.LogInformation("Ejecutando la funcionalidad Actualizar Tarjeta de Crédito");

                //property.GeneralDataUserId = user.Id;
                if (IsProperty.IsOwner)
                {
                    IsProperty.PropertyNumber = propertyUpdateDto.PropertyNumber;
                    IsProperty.DerivativeNumber = propertyUpdateDto.DerivativeNumber;
                    IsProperty.TakeNumber = propertyUpdateDto.TakeNumber;
                    IsProperty.MeterNumber = propertyUpdateDto.MeterNumber;
                    IsProperty.TypeService = propertyUpdateDto.TypeService;
                }
                else
                {                    
                    IsProperty.DerivativeNumber = propertyUpdateDto.DerivativeNumber;
                    IsProperty.TakeNumber = propertyUpdateDto.TakeNumber;
                    IsProperty.MeterNumber = propertyUpdateDto.MeterNumber;
                    IsProperty.TypeService = propertyUpdateDto.TypeService;
                }

                _db.Properties.Update(IsProperty);

                await _db.SaveChangesAsync();

                return "Ok";
            }
            else
            {
                _logger.LogError("El Inmueble no Existe");

                return "NoProperty";
            }
        }

        public async Task<bool> SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Enviando Correo al Usuario");

            var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            using (var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }

        }

        public async Task<string> AuthorizeUser(string email, string email2, string propertyId)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Autorizar Uso del Inmueble");

            bool result = false;

            if (email2 == "")
            {
                return "NotEmpty";
            }

            var user = await _userManager.FindByEmailAsync(email2);

            if (user == null)
            {
                return "UserNoExist";
            }

            var userOwner = await _userManager.FindByEmailAsync(email);

            var userPropertyFind = _db.Properties.FirstOrDefault(p => p.PropertyNumber == propertyId
                                   && p.GeneralDataUserId == userOwner.Id && p.IsOwner == true);

            var userNoPropertyFind = _db.Properties.FirstOrDefault(p => p.PropertyNumber == propertyId
                                   && p.GeneralDataUserId == user.Id && p.IsOwner == false);

            if (userPropertyFind != null && userNoPropertyFind != null)
            {
                userNoPropertyFind.IsActive = true;
                userNoPropertyFind.IsEnabled = true;

                _db.Properties.Update(userPropertyFind);

                await _db.SaveChangesAsync();

                result = true;
            }

            if (result)
            {
                var userProp = new UserProperty {
                    GeneralDataUserId = userNoPropertyFind.GeneralDataUserId,
                    PropertyId = userNoPropertyFind.PropertyNumber
                };

                await _db.UsersProperties.AddAsync(userProp);

                await _db.SaveChangesAsync();

                return "Ok";
            }

            return "Error";
        }

        public async Task<string> NoAuthorizeUser(string email, string email2, string propertyId)
        {
            var user = await _userManager.FindByEmailAsync(email2);

            if (user != null)
            {
                return "UserNoExist";
            }

            var userNoPropertyFind = _db.Properties.FirstOrDefault(p => p.PropertyNumber == propertyId
                                   && p.GeneralDataUserId == user.Id && p.IsOwner == false);

            if (userNoPropertyFind != null)
            {
                var userFromDb = await _userManager.FindByIdAsync(user.Id);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

                var uriBuilder = new UriBuilder(_configuration["ReturnPaths:NoAuthorizeUser"]);

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                query["email"] = userFromDb.Email;

                query["property"] = userNoPropertyFind.PropertyNumber;

                uriBuilder.Query = query.ToString();

                var urlString = uriBuilder.ToString();

                var senderEmail = email;

                if (!await SendEmailAsync(_configuration["ReturnPaths:SenderEmail"], userFromDb.Email, $"Debe Eliminar inmueble {propertyId} el Usuario con Cuenta {email} no Autorizo su Uso", urlString))
                {
                    return "NoSendMail";
                }
                return "Ok";
            }
            return "Error";
        }

        public bool ValidateInsertAllAsync(int estructuraHidraulica, int derivadum, int toma)
        {
            var estructuraHidraulicaFind = _dbOther.EstructuraHidraulicas.Where(e => e.Inmueble == estructuraHidraulica && e.Derivada == derivadum && e.Toma == toma).FirstOrDefault();

            if (estructuraHidraulicaFind != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public EstructuraHidraulica GetDataProperty(int estructuraHidraulica, int derivadum, int toma)
        {
            var estructuraHidraulicaFind = _dbOther.EstructuraHidraulicas.Where(e => e.Inmueble == estructuraHidraulica && e.Derivada == derivadum && e.Toma == toma).FirstOrDefault();

            if (estructuraHidraulicaFind != null)
            {
                return estructuraHidraulicaFind;
            }
            else
            {
                return null;
            }
        }

        public string GetTypeService(int type)
        {
            var typeServiceFind = _dbOther.CTipoServicios.Where(t => t.ServicioC == type).FirstOrDefault();

            if (typeServiceFind != null)
            {
                return typeServiceFind.ServicioD;
            }
            else
            {
                return null;
            }
        }

        public List<BalanceDto> GetBalance(int property)
        {
            List<BalanceDto> list = new List<BalanceDto>();

            string connectionString = _configuration.GetConnectionString("OtherConnection");

            SqlConnection connection = new SqlConnection(@connectionString);

            //string query = "SELECT dbo.Properties.PropertyNumber AS 'CUENTA', dbo.Properties.GeneralDataUserId AS 'USER' FROM dbo.Properties WHERE MeterNumber = 281204";
            string query1 = "SELECT DBO.REGISTRO_FACTURA.FECHA AS 'FECHA VAL', DBO.REGISTRO_FACTURA.SALDO_ANTERIOR AS 'SALDO ANTERIOR'" + 
                ", DBO.REGISTRO_FACTURA.SALDO_ACTUAL AS 'NUEVO SALDO'" +
                ",  ((SELECT DBO.REGISTRO_FACTURA.SALDO_ANTERIOR)+(DBO.REGISTRO_FACTURA.SALDO_ACTUAL)) AS 'TOTAL' " +
                "FROM DBO.REGISTRO_FACTURA WHERE DBO.REGISTRO_FACTURA.FACTURA_C ="+ property;

            string query = "SELECT DBO.DERIVADA.INMUEBLE AS 'CUENTA'" +
                ", DBO.DERIVADA.DERIVADA AS 'DERIVADA'" +
                ", DBO.DERIVADA.ALTERNA AS 'CLAVE LOCALIZACION'" +
                ", DBO.C_INDIVIDUO.INDIVIDUO_D AS 'PROPIETARIO'" +
                ", DBO.REGISTRO_FACTURA.FACTURA_C AS 'FACTURA'" +
                ", DBO.C_FACTURA.FECHA_EMISION AS 'FECHA DE EMISION'" +
                ", DBO.C_FACTURA.FECHA_VENCIMIENTO AS 'FECHA DE VENCIMIENTO'" +
                ", DBO.REGISTRO_FACTURA.SALDO_ANTERIOR AS 'SALDO ANTERIOR'" +
                ", DBO.REGISTRO_FACTURA.SALDO_ACTUAL AS 'NUEVO SALDO'" +
                ", ((SELECT DBO.REGISTRO_FACTURA.SALDO_ANTERIOR)+(DBO.REGISTRO_FACTURA.SALDO_ACTUAL)) AS 'TOTAL'" +
                ", DBO.C_CONCEPTOACOBRAR.CONCEPTOCOBRAR_D AS 'CONCEPTO A COBRAR' " +
                "FROM DBO.DERIVADA " +
                "INNER JOIN DBO.C_INDIVIDUO ON DBO.C_INDIVIDUO.INDIVIDUO_C = DBO.DERIVADA.PROPIETARIO " +
                "INNER JOIN DBO.ESTADO_DE_CUENTA ON DBO.ESTADO_DE_CUENTA.DERIVADA = DBO.DERIVADA.INMUEBLE " +
                "INNER JOIN DBO.C_FACTURA ON DBO.C_FACTURA.FACTURA_C = DBO.ESTADO_DE_CUENTA.DOCUMENTO " +
                "INNER JOIN DBO.REGISTRO_FACTURA ON DBO.REGISTRO_FACTURA.FACTURA_C = DBO.ESTADO_DE_CUENTA.DOCUMENTO " +
                "INNER JOIN DBO.C_CONCEPTOACOBRAR ON DBO.C_CONCEPTOACOBRAR.CONCEPTOCOBRAR_C = DBO.REGISTRO_FACTURA.CONCEPTOCOBRAR_C " +
                "WHERE DBO.DERIVADA.INMUEBLE = "+ property;

            //SqlCommand command = new SqlCommand(query, connection);
            SqlCommand command = new SqlCommand(query, connection);

            var model = new BalanceDto
            {
                PropertyNumber = "",
                OwnerName = "",
                ExpiredDate = "",
                BeforeBalance = 0,
                NewBalance = 0,
                TotalBalance = 0,
                Concept = ""
            };

            try
            {
                connection.Open();

                command.ExecuteNonQuery();                

                SqlDataReader registro = command.ExecuteReader();



                while (registro.Read())
                {
                    double val = Convert.ToDouble(registro["TOTAL"].ToString());
                    val = Math.Round(val, 2);

                    model = new BalanceDto
                    {
                        PropertyNumber = registro["CUENTA"].ToString(),
                        OwnerName = registro["PROPIETARIO"].ToString(),
                        ExpiredDate = registro["FECHA DE VENCIMIENTO"].ToString().Substring(0,10),
                        BeforeBalance = float.Parse(registro["SALDO ANTERIOR"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        NewBalance = float.Parse(registro["NUEVO SALDO"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        TotalBalance = val,
                        Concept = registro["CONCEPTO A COBRAR"].ToString()
                    };

                    /*model = new BalanceDto
                    {
                        PropertyNumber = "",
                        OwnerName = "",
                        ExpiredDate = registro["FECHA VAL"].ToString().Substring(0,10),
                        BeforeBalance = float.Parse(registro["SALDO ANTERIOR"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        NewBalance = float.Parse(registro["NUEVO SALDO"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        TotalBalance = val,
                        Concept = "concepto"
                    };*/

                    list.Add(model);

                    

                    //string test = registro["SALDO ANTERIOR"].ToString();
                    //string test1 = registro["NUEVO SALDO"].ToString();
                }
                Console.WriteLine("Records showed Successfully");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return list;
        }

        public async Task<string> DeletePropertyEnd(string id)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Eliminar Inmueble por ID");

            //var user = await _userManager.FindByIdAsync(id);

            Property property = await _db.Properties.Where(p => p.PropertyNumber == id).FirstOrDefaultAsync();

            if (property == null)
            {
                return "NoProperty";
            }
            else
            {
                _db.Properties.Remove(property);

                //property.IsActive = false;

                //_db.Properties.Update(property);

                await _db.SaveChangesAsync();

                return "Ok";
            }
        }

        public async Task<List<string>> GetPropertiesNoMain(string idProperty)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Lista de Inmuebles no Principales");

            List<Property> users = await _db.Properties.Where(p => p.PropertyNumber == idProperty && p.IsOwner == false).ToListAsync();
            List<string> emails = new List<string>();

            foreach (var user in users)
            {
                var userFind = await _userManager.FindByIdAsync(user.GeneralDataUserId);
                emails.Add(userFind.Email);
            }

            if (emails != null)
            {               
                return emails;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Property>> GetPropertiesMain(string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Lista de Inmuebles Principales");

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                List<Property> list = await _db.Properties.Where(p => p.GeneralDataUserId == user.Id && p.IsOwner == true).ToListAsync();

                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
