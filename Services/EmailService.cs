using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PharmaStock.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"] ?? "25"),
                    Credentials = new NetworkCredential(_configuration["Email:User"], _configuration["Email:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:User"] ?? throw new ArgumentNullException("Email:User configuration is missing")),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(destinatario);

                smtpClient.Send(mailMessage);
                _logger.LogInformation($"📧 Correo enviado a {destinatario}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error al enviar correo: {ex.Message}");
            }
        }
    }
}
