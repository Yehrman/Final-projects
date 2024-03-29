﻿using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;


namespace CompuskillsMvcProject.Models
{
    public class EmailMessage
    {
        public void SendEmail(EmailSetup setup)
        {
           
            var message = new MimeMessage();
           
            message.From.Add(new MailboxAddress(setup.SenderName, setup.Sender));
            message.To.Add(new MailboxAddress(setup.RecieverName, setup.Reciever));
            message.Subject = setup.Subject;
       
            message.Body = message.Body = new TextPart(TextFormat.Html)
            {
                Text = setup.Content
            };
            using (var client = new SmtpClient())
            {
                
                client.Connect("cmx6.my-hosting-panel.com", 465);
                client.Authenticate(setup.Sender, setup.EmailPassword);
                client.Send(message);
                client.Disconnect(true);
            
            }
        }
    }
}