using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Shopping.Common;

namespace Shopping.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Response SendMail(string toName, string toEmail, string subject, string body)
        {
            try
            {
                string from = _configuration["Mail:From"];//Aqui buca en la configuracion el correo que indicamos como el from
                string name = _configuration["Mail:Name"];//El nombre que le indicamos en la configuracion
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string password = _configuration["Mail:Password"];

                MimeMessage message = new MimeMessage();//Creamos el mensaje
                message.From.Add(new MailboxAddress(name, from));//De donde lo enviamos
                message.To.Add(new MailboxAddress(toName, toEmail));//A quien lo estamos enviando
                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())//Creamos un cliente
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(smtp, int.Parse(port), SecureSocketOptions.Auto);//Nos conectamos al cliente
                    client.Authenticate(from, password);//Nos autenticamos
                    client.Send(message);//Lo enviamos
                    client.Disconnect(true);//Y nos deconectamos
                }

                return new Response { IsSuccess = true };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex
                };
            }
        }
    }
}

   